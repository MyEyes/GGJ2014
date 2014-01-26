using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Otherworld.Utilities;
using Spine;

namespace Vest
{
    public class PlayerController
    {
        private const float MOVE_SPEED = 3;
        private const float CROUCH_SPEED = 2;
        private const float JUMP_SPEED = 3;
        private const float RUN_SPEED = 10;
        
        private readonly Player player;
        private readonly Timer idleTimer;
        
        public PlayerState State { get; set; }
        public PlayerState LastState { get; private set; }
        private readonly Dictionary<PlayerState, Tuple<Action, Action>> states;

        private GamePadState currPadState;
        private GamePadState prevPadState;

        private LookDir Look = LookDir.Right;
        public Vector2 MoveDir;
        public bool IsInteracting;
        public bool IsJumping;
        public bool IsWalking;
        public bool IsRunning;
        public bool IsCrawling;
        public bool IsStanding;
        public bool IsHiding;

        public Vector2 MoveToX;
        public bool Script = false;
        public PlayerController (Player player)
        {
            this.player = player;
            player.Controller = this;
            LoadPlayerAnim();

            idleTimer = new Timer (new Random ().Next (3000, 8000));
            idleTimer.Elapsed += (s, a) =>
            {
                if (State == PlayerState.CrawlIdle)
                {
                    player.AnimState.AddAnimation (0, "crawl_idle2", false, 0);
                    player.AnimState.AddAnimation (0, "crawl_idle", true, 0);
                }
                else if (State == PlayerState.Idle)
                {
                    player.AnimState.AddAnimation (0, "idle2", false, 0);
                    player.AnimState.AddAnimation (0, "idle", true, 0);
                }
                idleTimer.Interval = new Random ().Next (3000, 8000);
            };

            states = new Dictionary<PlayerState, Tuple<Action, Action>>
            {
                {PlayerState.None,          new Tuple<Action, Action> (null, null)},
                {PlayerState.Idle,          new Tuple<Action, Action> (IdleStart, IdleUpdate)},
                {PlayerState.Walk,          new Tuple<Action, Action> (WalkStart, WalkUpdate)},
                {PlayerState.Run,           new Tuple<Action, Action> (RunStart, RunUpdate)},
                {PlayerState.CrawlStart,    new Tuple<Action, Action> (CrawlStart, null)},
                {PlayerState.CrawlEnd,      new Tuple<Action, Action> (CrawlEnd, null)},
                {PlayerState.CrawlIdle,     new Tuple<Action, Action> (CrawlIdleStart, CrawlIdleUpdate)},
                {PlayerState.CrawlWalk,     new Tuple<Action, Action> (null, CrawlWalkUpdate)},
                {PlayerState.Interact,      new Tuple<Action, Action> (InteractStart, null)},
                {PlayerState.Jump,          new Tuple<Action, Action> (JumpStart, JumpUpdate)},
                {PlayerState.Back,          new Tuple<Action, Action> (BackStart, null)},
                {PlayerState.Swallow,       new Tuple<Action, Action> (SwallowStart, null)},
                {PlayerState.Freakout,      new Tuple<Action, Action> (FreakoutStart, null)}
            };

            ChangeState (PlayerState.Idle);
            player.SetCollision (Player.STAND_COLLISION);
            player.Depth = Player.PLAYER_DEPTH;

            player.AnimState.Event += (s, e) => { if (e.Event.Data.Name == "step1")SoundHelper.PlaySound("sounds/step1",0.1f); };
            player.AnimState.Event += (s, e) => { if (e.Event.Data.Name == "step2")SoundHelper.PlaySound("sounds/step2",0.1f); };
        }

        private void InteractStart()
        {
            Elevator elevator = player.level.GetObjects<Elevator>().FirstOrDefault((e) => e.Enabled && player.Collides(e));
            if (elevator != null)
            {
                elevator.Interact(player);
                return;
            }

            Pill pill = player.level.GetObjects<Pill>().FirstOrDefault((e) => true);
            if (pill != null && pill.Collides(player))
            {
                pill.Activate(player);
                return;
            }
            ChangeState(PlayerState.Idle);
        }

        private void JumpStart()
        {
            idleTimer.Stop();
            player.Jump();
            player.SetAnim ("jump", Look, false);
        }

        private void BackStart()
        {
            idleTimer.Stop();
            player.SetAnim("back", Look, false);
        }

        private void SwallowStart()
        {
            idleTimer.Stop();
            player.SetAnim("swallow", Look, false);
        }

        private void FreakoutStart()
        {
            idleTimer.Stop();
            player.SetAnim("freakout", Look, false);
        }

        private void JumpUpdate()
        {
            if (player.OnGround)
            {
                ChangeState (LastState);
                return;
            }
            
            player.Move (MoveDir * JUMP_SPEED);
        }

        private void CrawlStart()
        {
            idleTimer.Stop();
            player.SetAnim ("crawl_down", Look, false);
            player.Depth = Player.PLAYER_HIDE_DEPTH;

            player.DisableInput++;
            TaskHelper.SetDelay (300, () => {
                player.DisableInput--;
                ChangeState (PlayerState.CrawlIdle);
            });
        }

        private bool CanPlayerStand()
        {
            player.SetCollision (Player.STAND_COLLISION);
            if (player.level.IsColliding (player))
            {
                player.SetCollision (Player.CROUCH_COLLISION);
                return false;
            }

            return true;
        }

        private void CrawlEnd()
        {
            if (!CanPlayerStand())
            {
                ChangeState (LastState);
                return;
            }

            idleTimer.Stop();
            player.SetAnim ("crawl_up", Look, false);
            player.Depth = Player.PLAYER_DEPTH;
            UpdateIsHiding();

            player.DisableInput++;
            TaskHelper.SetDelay (300, () =>
            {
                player.DisableInput--;
                player.SetCollision (Player.STAND_COLLISION);
                ChangeState (PlayerState.Idle);
            });
        }

        private void CrawlIdleStart()
        {
            idleTimer.Start();
            UpdateIsHiding();
            player.SetCollision (Player.CROUCH_COLLISION);
        }

        private void CrawlIdleUpdate()
        {
            if (IsStanding)
            {
                ChangeState (PlayerState.CrawlEnd);
                return;
            }
            if (IsWalking)
            {
                ChangeState (PlayerState.CrawlWalk);
                return;
            }

            player.SetAnim ("crawl_idle", Look, true);
        }

        private void CrawlWalkUpdate()
        {
            if (IsStanding)
            {
                ChangeState (PlayerState.CrawlEnd);
                return;
            }
            if (!IsWalking)
                ChangeState (PlayerState.CrawlIdle);

            player.Move (MoveDir * CROUCH_SPEED);
            player.SetAnim ("crawl_walk", Look);
            player.SetCollision (Player.CROUCH_COLLISION);
            UpdateIsHiding();
        }

        private void RunStart()
        {
            idleTimer.Stop ();
        }

        private void RunUpdate()
        {
            if (IsCrawling)
            {
                ChangeState (PlayerState.CrawlStart);
                return;
            }
            if (IsWalking)
            {
                ChangeState (PlayerState.Walk);
                return;
            }
            if (!IsRunning)
            {
                ChangeState (PlayerState.Idle);
                return;
            }
            if (IsJumping)
            {
                ChangeState (PlayerState.Jump);
                return;
            }

            player.Move (MoveDir * RUN_SPEED);
            player.SetAnim ("run", Look);
        }

        private void WalkStart()
        {
            idleTimer.Stop ();
        }
        
        private void WalkUpdate()
        {
            if (IsCrawling)
            {
                ChangeState (PlayerState.CrawlStart);
                return;
            }
            if (IsRunning)
            {
                ChangeState (PlayerState.Run);
                return;
            }
            if (!IsWalking)
            {
                ChangeState (PlayerState.Idle);
                return;
            }
            if (IsJumping)
            {
                ChangeState (PlayerState.Jump);
                return;
            }

            player.Move (MoveDir * MOVE_SPEED);
            player.SetAnim ("walk", Look);
        }

        private void IdleStart()
        {
            idleTimer.Start();
            player.LookDir = Look;
            player.SetAnim ("idle", player.LookDir);
        }

        private void IdleUpdate()
        {
            if (IsCrawling)
                ChangeState(PlayerState.CrawlStart);
            else if (IsWalking)
                ChangeState(PlayerState.Walk);
            else if (IsRunning)
                ChangeState(PlayerState.Run);
            else if (IsJumping)
                ChangeState(PlayerState.Jump);
            else if (IsInteracting)
                ChangeState(PlayerState.Interact);
        }

        public void Update()
        {
            Vector2 moveDir = Vector2.Zero;
            bool useInteract = false;
            bool useJump = false;
            bool useCrawl = false;
            bool useStand = false;
            bool useRun = false;

            currPadState = GamePad.GetState(PlayerIndex.One);


            if (player.DisableInput == 0)
            {
                if (isButtonDown(currPadState.DPad.Left)) moveDir.X = -1;
                if (isButtonDown(currPadState.DPad.Right)) moveDir.X = 1;
                if (isButtonDown(currPadState.DPad.Down)) useCrawl = true;
                if (isButtonDown(currPadState.DPad.Up)) useStand = true;
                if (isButtonDown(currPadState.Buttons.A)) useJump = true;
                if (isButtonDown(currPadState.Buttons.X)) useInteract = true;
                if (isButtonDown(currPadState.Buttons.Y)) useRun = true;
            }
            else if(Script)
            {
                moveDir.X = Math.Sign(MoveToX.X - player.position.X);
                moveDir.Y = 0;
            }

            IsInteracting = useInteract;
            IsJumping = useJump && player.OnGround;
            IsWalking = moveDir != Vector2.Zero && !useRun;
            IsRunning = moveDir != Vector2.Zero && useRun;
            IsCrawling = useCrawl;
            IsStanding = useStand;
            MoveDir = moveDir;

            if (IsWalking)
                Look = moveDir.X < 0 ? LookDir.Left : LookDir.Right;

            prevPadState = currPadState;

            UpdateCurrentState();

        }

        private void UpdateCurrentState()
        {
            Tuple<Action, Action> stateFuncs;

            if (states.TryGetValue (State, out stateFuncs) && stateFuncs.Item2 != null)
                stateFuncs.Item2 ();
        }

        private void UpdateIsHiding()
        {
            var table = player.level
                .GetObjects<Table>()
                .OrderBy (t => Vector2.Distance (player.position, t.position))
                .FirstOrDefault ();

            bool isBeingCovered = table != null && table.IsCovering (player);

            bool isCrawling = State == PlayerState.CrawlStart
                || State == PlayerState.CrawlIdle
                || State == PlayerState.CrawlWalk;

            player.IsHiding = IsHiding = isBeingCovered && isCrawling;
        }

        private void LoadPlayerAnim()
        {
            player.LoadSkeleton ("content/player/vest.json", "player/");
            player.AnimState.SetAnimation (0, "idle", true);
            player.AnimData.SetMix ("idle", "run", 0.3f);
            player.AnimData.SetMix ("idle2", "run", 0.3f);
            player.AnimData.SetMix ("run", "idle", 0.3f);
            player.AnimData.SetMix ("run", "idle2", 0.3f);
            player.AnimData.SetMix ("run", "jump", 0.2f);
            player.AnimData.SetMix ("run", "walk", 0.2f);
            player.AnimData.SetMix ("walk", "run", 0.2f);
            player.AnimData.SetMix ("idle", "jump", 0.2f);
            player.AnimData.SetMix ("jump", "run", 0.3f);
            player.AnimData.SetMix ("jump", "idle", 0.3f);
            
            player.AnimData.SetMix ("idle", "crawl_down", 0.5f);
            player.AnimData.SetMix ("idle2", "crawl_down", 0.5f);
            player.AnimData.SetMix ("crawl_down", "crawl_idle", 0.5f);
            player.AnimData.SetMix ("crawl_idle", "crawl_walk", 0.5f);
            player.AnimData.SetMix ("crawl_down", "crawl_walk", 0.5f);
            player.AnimData.SetMix ("crawl_down", "crawl_up", 0.5f);
            player.AnimData.SetMix ("crawl_walk", "crawl_up", 0.5f);
            player.AnimData.SetMix ("crawl_up", "idle", 0.5f);
            player.AnimData.SetMix ("crawl_up", "idle", 0.5f);
        }

        public void ChangeState(PlayerState newState)
        {
            Tuple<Action, Action> stateFuncs;

            // Check if we have any registered start/update functions
            // for the igven state
            if (!states.TryGetValue (newState, out stateFuncs))
                throw new Exception ("No state found for " + newState);

            LastState = State;
            State = newState;

            Console.WriteLine ("State Change: {0} -> {1}", LastState, State);

            if (State != LastState && stateFuncs.Item1 != null)
            {
                // Call the state's registered `start` function
                stateFuncs.Item1 ();
            }
        }

        private bool isButtonDown (ButtonState bState)
        {
            return bState == ButtonState.Pressed;
        }
    }

    [Flags]
    public enum PlayerState
    {
        None,
        Idle,
        Jump,
        Walk,
        Run,
        Interact,
        CrawlStart,
        CrawlEnd,
        CrawlIdle,
        CrawlWalk,
        Back,
        Swallow,
        Freakout
    }
}
