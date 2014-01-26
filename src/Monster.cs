using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Otherworld.Utilities;
using Vest.graphics;
using Vest.levels;

namespace Vest
{
    public class Monster
        : PhysicsSpineGameObject
    {
        private const float PATROL_SPEED = 1f;
        private const float CHASE_SPEED = 2f;
        private const float SEE_DISTANCE = 150;
        LookDir look = LookDir.Right;
        private float velocity = 1;
        private float leftLimit;
        private float rightLimit;
        private MonsterState state;
        private Timer idleTimer;

        public Monster (Vector2 position, float leftLimit, float rightLimit)
            : base(position, Player.STAND_COLLISION)
        {
            this.leftLimit = leftLimit;
            this.rightLimit = rightLimit;
            LoadSkeleton ("content/enemy/mob.json", "enemy");

            idleTimer = new Timer (new Random ().Next (100, 200));
            idleTimer.Elapsed += (s, a) =>
            {
                idleTimer.Stop ();
                SetAnim ("idle2", look, false);
                TaskHelper.SetDelay (3000, () => {
                    look = look == LookDir.Left
                        ? LookDir.Right
                        : LookDir.Left;
                    velocity *= -1;
                    state = MonsterState.Patrol;
                });
            };

            state = MonsterState.Patrol;
        }

        public override void Update (float dt)
        {
            checkChase();

            if (state == MonsterState.Patrol)
                walkPatrol();
            if (state == MonsterState.Chase)
                chasePlayer();
            
            base.Update (dt);
        }

        private void walkPatrol()
        {
            Move (new Vector2 (velocity * PATROL_SPEED, 0));
            SetAnim ("walk", look, true);

            if (look == LookDir.Right && position.X >= rightLimit)
            {
                state = MonsterState.Idle;
                SetAnim ("idle", look, false);
                idleTimer.Start();
            }
            else if (look == LookDir.Left && position.X <= leftLimit)
            {
                state = MonsterState.Idle;
                SetAnim ("idle", look, false);
                idleTimer.Start();
            }
        }

        private void chasePlayer()
        {
            velocity = (level.player.position.X - position.X) < 0 ? -1 : 1;
            look = velocity > 0 ? LookDir.Right : LookDir.Left;

            Move (new Vector2 (CHASE_SPEED * velocity, 0));
            SetAnim ("walk", look, true);

            float distance = Vector2.Distance (level.player.position, this.position);

            if (level.player.Collides (this))
            {
                level.RestorePosition();
                state = MonsterState.Patrol;
            }
            if (look == LookDir.Right && (position.X >= rightLimit || distance >= SEE_DISTANCE))
            {
                state = MonsterState.Patrol;
            }
            else if (look == LookDir.Left && (position.X <= leftLimit || distance >= SEE_DISTANCE))
            {
                state = MonsterState.Idle;
            }
        }

        private void checkChase()
        {
            return;
            float distance = Vector2.Distance (level.player.position, this.position);

            if (level.player != null && distance <= SEE_DISTANCE && !level.player.IsHiding && level.State == LevelState.Evil &&
                (
                    look == LookDir.Right && level.player.position.X < rightLimit ||
                    look == LookDir.Left && level.player.position.X < rightLimit)
                )
            {
                idleTimer.Stop ();
                state = MonsterState.Chase;
            }
        }
    }

    public enum MonsterState
    {
        Idle,
        Patrol,
        Chase
    }
}
