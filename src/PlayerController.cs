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

        private readonly Player player;
        private readonly Timer idleTimer;
        
        public PlayerState State { get; private set; }
        public PlayerState LastState { get; private set; }
        private readonly Dictionary<PlayerState, Tuple<Action, Action>> states;

        private GamePadState currPadState;
        private GamePadState prevPadState;

        private LookDir Look = LookDir.Right;
        public Vector2 MoveDir;
        public bool IsInteracting;
        public bool IsJumping;
        public bool IsWalking;
        public bool IsCrawling;
        public bool IsStanding; 

        public PlayerController (Player player)
        {
            this.player = player;
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
                {PlayerState.CrawlStart,     new Tuple<Action, Action> (CrawlStart, null)},
                {PlayerState.CrawlEnd,     new Tuple<Action, Action> (CrawlEnd, null)},
                {PlayerState.CrawlIdle,     new Tuple<Action, Action> (CrawlIdleStart, CrawlIdleUpdate)},
                {PlayerState.CrawlWalk,     new Tuple<Action, Action> (null, CrawlWalkUpdate)},
                {PlayerState.Interact,      new Tuple<Action, Action> (InteractStart, null)},
                {PlayerState.Jump,          new Tuple<Action, Action> (JumpStart, JumpUpdate)}
            };

            ChangeState (PlayerState.Idle);
            player.SetCollision (Player.STAND_COLLISION);
        }

        private void InteractStart()
        {
            var table = player.level
                .GetObjects<Table>()
                .OrderBy (t => Vector2.Distance (player.position, t.position))
                .FirstOrDefault();

            if (table == null)
            {
                ChangeState (LastState);
                return;
            }

            player.position = table.position;
            player.SetAnim ("hide", Look, false);
        }

        private void JumpStart()
        {
            idleTimer.Stop ();
            player.Jump();
            player.SetAnim ("jump", Look, false);
        }

        private void JumpUpdate()
        {
            if (player.OnGround)
            {
                ChangeState (LastState);
                return;
            }
            
            player.Move (MoveDir * MOVE_SPEED);
        }

        private void CrawlStart()
        {
            idleTimer.Stop();
            player.SetAnim ("crawl_down", Look, false);

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

            player.Move (MoveDir * MOVE_SPEED);
            player.SetAnim ("crawl_walk", Look);
            player.SetCollision (Player.CROUCH_COLLISION);
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
                ChangeState (PlayerState.CrawlStart);
            else if (IsWalking)
                ChangeState (PlayerState.Walk);
            else if (IsJumping)
                ChangeState (PlayerState.Jump);
        }

        public void Update()
        {
            Vector2 moveDir = Vector2.Zero;
            bool useInteract = false;
            bool useJump = false;
            bool useCrawl = false;
            bool useStand = false;

            currPadState = GamePad.GetState(PlayerIndex.One);


            if (player.DisableInput == 0)
            {
                if (isButtonDown(currPadState.DPad.Left)) moveDir.X = -1;
                if (isButtonDown(currPadState.DPad.Right)) moveDir.X = 1;
                if (isButtonDown(currPadState.DPad.Down)) useCrawl = true;
                if (isButtonDown(currPadState.DPad.Up)) useStand = true;
                if (isButtonDown(currPadState.Buttons.A)) useJump = true;
                if (isButtonDown(currPadState.Buttons.X)) useInteract = true;
            }

            IsInteracting = useInteract;
            IsJumping = useJump && player.OnGround;
            IsWalking = moveDir != Vector2.Zero;
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

        private void LoadPlayerAnim()
        {
            player.LoadSkeleton ("content/player/vest.json", "player/");
            player.AnimState.SetAnimation (0, "idle", true);
            player.AnimData.SetMix ("idle", "run", 0.3f);
            player.AnimData.SetMix ("idle2", "run", 0.3f);
            player.AnimData.SetMix ("run", "idle", 0.3f);
            player.AnimData.SetMix ("run", "idle2", 0.3f);
            player.AnimData.SetMix ("run", "jump", 0.2f);
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

        protected void ChangeState(PlayerState newState)
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
    }
}
