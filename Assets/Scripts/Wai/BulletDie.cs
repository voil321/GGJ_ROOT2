using System.Threading.Tasks;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class BulletDie : MonoBehaviour
{
    enum BulletType { Straight, Bounce, Junkrat }
    [SerializeField] BulletType BT;
    [SerializeField] PhysicsMaterial2D Mat;
    
    Vector3 Diff;
    bool CollisionEnter = true;
    int bounceCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        switch (BT)
        {
            case BulletType.Straight:
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                break;
            case BulletType.Bounce:
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                gameObject.GetComponent<Collider2D>().isTrigger = false;
                gameObject.GetComponent<Collider2D>().sharedMaterial = Mat;
                break;
            case BulletType.Junkrat:
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                break;
        }
    }

    // Update is called once per frame



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!CollisionEnter)
            return;
        if (collision.gameObject.tag == "Enemy")
        {
            //enemy.minusHp
            Diff = gameObject.transform.position - collision.gameObject.transform.position;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            gameObject.layer = LayerMask.NameToLayer("TouchedBullet");
            gameObject.tag = "CanExplode";
            StickEnemy(collision);
        }
        if(collision.gameObject.tag == "CanExplode")
        {
            
        }
        else if (BT == BulletType.Bounce)
        {
            if(bounceCounter++>=1)
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            return;
        }
            
        else if (collision.gameObject.tag == "Wall")
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            if (BT == BulletType.Junkrat)
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    async void StickEnemy(Collision2D C)
    {
        CollisionEnter = false;
        while (true)
        {
            gameObject.transform.position = Diff + C.gameObject.transform.position;
            await Task.Delay(50);
        }
    }
}
