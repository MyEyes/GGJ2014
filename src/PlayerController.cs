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

        public PlayerController (Player player)
        {
            this.player = player;
            LoadPlayerAnim();

            idleTimer = new Timer (new Random ().Next (3000, 8000));
            idleTimer.Elapsed += (s, a) =>
            {
                player.AnimState.AddAnimation (0, "idle2", false, 0);
                player.AnimState.AddAnimation (0, "idle", true, 0);
                idleTimer.Interval = new Random ().Next (3000, 8000);
            };

            states = new Dictionary<PlayerState, Tuple<Action, Action>>
            {
                {PlayerState.None,          new Tuple<Action, Action> (null, null)},
                {PlayerState.Idle,          new Tuple<Action, Action> (IdleStart, IdleUpdate)},
                {PlayerState.Walk,          new Tuple<Action, Action> (WalkStart, WalkUpdate)},
                {PlayerState.Interact,      new Tuple<Action, Action> (null, null)},
                {PlayerState.Jump,          new Tuple<Action, Action> (JumpStart, JumpUpdate)}
            };

            ChangeState (PlayerState.Idle);
        }

        private void JumpStart()
        {
            idleTimer.Stop ();
            player.Jump();
            player.UpdateAnim ("jump", Look, false);
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

        private void WalkStart()
        {
            idleTimer.Stop ();
        }
        
        private void WalkUpdate()
        {
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
            player.UpdateAnim ("walk", Look);
        }

        private void IdleStart()
        {
            idleTimer.Start();
            player.LookDir = Look;
            player.UpdateAnim ("idle", player.LookDir);
            Console.WriteLine ("Idle start");
        }

        private void IdleUpdate()
        {
            if (IsWalking)
                ChangeState (PlayerState.Walk);
        }

        public void Update()
        {
            Vector2 moveDir = Vector2.Zero;
            bool useInteract = false;
            bool useJump = false;

            currPadState = GamePad.GetState (PlayerIndex.One);

            if (isButtonDown (currPadState.DPad.Left))  moveDir.X = -1;
            if (isButtonDown (currPadState.DPad.Right)) moveDir.X = 1;
            if (isButtonDown (currPadState.DPad.Up))    useInteract = true;
            if (isButtonDown (currPadState.Buttons.A))  useJump = true;

            IsInteracting = useInteract;
            IsJumping = useJump && player.OnGround;
            IsWalking = moveDir != Vector2.Zero;
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
        Interact
    }
}
