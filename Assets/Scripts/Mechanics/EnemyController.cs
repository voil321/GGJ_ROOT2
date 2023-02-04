using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// A simple controller for enemies. Provides movement control over a patrol path.
    /// </summary>
    //[RequireComponent(typeof(AnimationController), typeof(Collider2D))]
    public class EnemyController : MonoBehaviour
    {
        public PatrolPath path;
        public AudioClip ouch;

        internal PatrolPath.Mover mover;
        internal AnimationController control;
        internal Collider2D _collider;
        internal AudioSource _audio;
        SpriteRenderer spriteRenderer;

        float duration;
        float startTime;
        bool attacked;

        public Bounds Bounds => _collider.bounds;

        void Awake()
        {
            control = GetComponent<AnimationController>();
            _collider = GetComponent<Collider2D>();
            _audio = GetComponent<AudioSource>();
            //spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SpwanAndDestroy()
        {
            Destroy(gameObject, 2f);
        }

        public void Spwan()
        {
            attacked = false;
            transform.localScale = new Vector2(0.3f, 0.1f);
            var target = transform.position.y + (1 - transform.localScale.y) / 2;

            //var p = Mathf.InverseLerp(0.1f, 1f, Mathf.PingPong(Time.time - startTime, duration));

            var seq = DOTween.Sequence();

            seq.AppendInterval(.5f);
            seq.AppendCallback(() =>
            {
                transform.DOScaleY(1f, 0.15f);
                transform.DOMoveY(target, 0.15f).OnComplete(() => { attacked = true; });
                SpwanAndDestroy();
            });


        }



        //void OnCollisionEnter2D(Collision2D collision)
        //{
        //    var player = collision.gameObject.GetComponent<PlayerController>();
        //    if (player != null)
        //    {
        //        var ev = Schedule<PlayerEnemyCollision>();
        //        ev.player = player;
        //        ev.enemy = this;
        //    }
        //}

        private void OnTriggerStay2D(Collider2D collision)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null && attacked)
            {
                var ev = Schedule<PlayerEnemyCollision>();
                ev.player = player;
                ev.enemy = this;
            }
        }

        void Update()
        {
            if (path != null)
            {
                if (mover == null) mover = path.CreateMover(control.maxSpeed * 0.5f);
                control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                Spwan();
            }
        }

    }
}