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

    public GameObject root;
    public PlayerController player;
    public Collider2D bossArea;

    void Awake()
    {
        m_spineAni = GetComponent<SkeletonAnimation>();

        StartCoroutine(Attack());
    }
    protected override void ComputeVelocity()
    {

        if (patrolPath != null)
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

    public IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            yield return new WaitUntil(() => player.IsGrounded);

            m_spineAni.state.SetAnimation(0, "Jump 1", false);
            m_spineAni.state.AddAnimation(0, "Jump 2", false, 0f);
            m_spineAni.state.AddAnimation(0, "Jump 3", false, 0f);
            m_spineAni.state.AddAnimation(0, "idle", true, 0f);

            SpwanRoot();

            yield return new WaitForSeconds(0.7f);

            yield return new WaitUntil(() => player.IsGrounded);

            SpwanRoot();

            yield return new WaitForSeconds(0.7f);

            yield return new WaitUntil(() => player.IsGrounded);

            SpwanRoot();

        }
    }

    public void Hurt(int bulletIndex) {

        m_spineAni.state.SetAnimation(0, "Jump 3", false);
        m_spineAni.state.AddAnimation(0, "idle", true, 0f);

        Debug.Log("hurt!!!!!!!!!");
    }

    void SpwanRoot()
    {
        if (bossArea.IsTouching(player.gameObject.GetComponent<Collider2D>()))
        {
            var aclone = Instantiate(root);

            aclone.transform.position = player.transform.position;

            aclone.GetComponentInChildren<EnemyController>().Spwan();
        }
    }


}
