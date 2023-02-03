using Platformer.Mechanics;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandler : KinematicObject
{
    // Start is called before the first frame update
    public Collider2D collider2d;
    public float maxSpeed;
    public Vector2 move;
    public PatrolPath patrolPath;
    internal SkeletonAnimation m_spineAni;

    void Awake()
    {
        m_spineAni = GetComponent<SkeletonAnimation>();
    }
    protected override void ComputeVelocity()
    {
        if (patrolPath.startPosition.x > transform.position.x)
        {
            move.x = 1;
            m_spineAni.skeleton.ScaleX = -1;

        }
        else if (patrolPath.endPosition.x < transform.position.x)
        {
            move.x = -1;
            m_spineAni.skeleton.ScaleX = 1;
        }

        //animator.SetBool("grounded", IsGrounded);
        //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;


    }
}
