using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using Spine.Unity;
using Spine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/
        public Collider2D collider2d;
        /*internal new*/
        public AudioSource audioSource;
        public Health health;

        [SerializeField] private Vector3 mouse_pos;
        [SerializeField] private Vector3 object_pos;
        private float angle;
        public Transform arrow;
        private bool prepareForFire;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        internal SkeletonAnimation m_spineAni;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            m_spineAni = GetComponent<SkeletonAnimation>();
            //spriteRenderer = GetComponent<SpriteRenderer>();
            //animator = GetComponent<Animator>();

            m_spineAni.AnimationState.Complete += HandleEvent;
        }

        void HandleEvent(TrackEntry trackEntry)
        {
            // Play some sound if the event named "footstep" fired.
            if (trackEntry.Animation.Name == "attack2")
            {

                Debug.Log("Play a footstep sound!");
                prepareForFire = false;
            }
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = Input.GetAxis("Horizontal");
                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
                else if (Input.GetButtonUp("Jump"))
                {
                    //stopJump = true;
                    //Schedule<PlayerStopJump>().player = this;
                }
            }
            else
            {
                move.x = 0;
            }

            if (Input.GetKeyDown(KeyCode.F) && !prepareForFire)
            {
                prepareForFire = true;
                arrow.gameObject.SetActive(true);
                m_spineAni.state.SetAnimation(0, "attack", true);
            }
            else if (prepareForFire && Input.GetKeyUp(KeyCode.F))
            {
                arrow.gameObject.SetActive(false);
                m_spineAni.state.SetAnimation(0, "attack2", false);

            }


            mouse_pos = Input.mousePosition;
            object_pos = Camera.main.WorldToScreenPoint(arrow.position);
            mouse_pos.x = mouse_pos.x - object_pos.x;
            mouse_pos.y = mouse_pos.y - object_pos.y;
            angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;

            //var x = Mathf.

            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);




            UpdateJumpState();
            base.Update();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    m_spineAni.state.SetAnimation(0, "Jump 1", false);
                    m_spineAni.state.AddAnimation(0, "jump 2", false, 0f);
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                m_spineAni.skeleton.ScaleX = 1;
            else if (move.x < -0.01f)
                m_spineAni.skeleton.ScaleX = -1;


            if (health.IsAlive)
            {
                if (velocity == Vector2.zero && m_spineAni.state.GetCurrent(0).Animation.Name != "idle" && !prepareForFire)
                {
                    m_spineAni.state.SetAnimation(0, "idle", true);
                }
                else if (velocity.x != 0 && IsGrounded && m_spineAni.state.GetCurrent(0).Animation.Name != "move" && !prepareForFire && !jump)
                {
                    m_spineAni.state.TimeScale = 2;
                    m_spineAni.state.SetAnimation(0, "move", true);
                }
            }

            //animator.SetBool("grounded", IsGrounded);
            //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}