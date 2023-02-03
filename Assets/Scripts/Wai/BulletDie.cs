using System.Threading.Tasks;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class BulletDie : MonoBehaviour
{
    enum BulletType {Straight, Bounce, Junkrat }
    [SerializeField] BulletType BT;
    [SerializeField] PhysicsMaterial2D Mat;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //enemy.minusHp
            Destroy(gameObject);
        }
        if (BT == BulletType.Bounce)
            return;
        else if (collision.gameObject.tag == "Wall")
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //enemy.minusHp
            Destroy(gameObject);
        }
    }

}
