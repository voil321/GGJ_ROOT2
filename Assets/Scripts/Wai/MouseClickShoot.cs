using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Platformer.Mechanics;

public class MouseClickShoot : MonoBehaviour
{
    [SerializeField] private GameObject[] bullet;
    [SerializeField] private Transform shootPos;
    [SerializeField] [Range(0,50)] private float shootSpeed;
    [SerializeField] [Range(0, 20)] private int BulletDestroyTimer=0;
    [SerializeField] private float shootTimer;
    private bool isShooting;
    private float Lscale, angle, modifiedAngle;
    public PlayerController Pl;

    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetMouseButton(1))//aim down sight
        {
            //spawn aim down sight on Input.mousePosition
            Lscale = Pl.m_spineAni.skeleton.ScaleX;
            angle = Mathf.Atan2(Camera.main.ScreenToWorldPoint(Input.mousePosition).y -
                               gameObject.transform.position.y,
                               Camera.main.ScreenToWorldPoint(Input.mousePosition).x -
                               gameObject.transform.position.x) * Mathf.Rad2Deg;
            // check is the character facing right & the angle between mouse pos and the character center
            if (Lscale > 0)
            { // testing vector
                /*gameObject.transform.localScale = new Vector3(1, 1, 1);*/
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(angle, -60f, 60f));
                if (Input.GetMouseButtonDown(0) && !isShooting)//shoot
                    StartCoroutine(Shoot(Mathf.Clamp(Lscale, -1f, 1f), Mathf.Clamp(angle, -60f, 60f)));
            }
            // face left
            if (Lscale < 0)
            {
                if (angle < 0)
                    modifiedAngle = angle + 360;
                else
                    modifiedAngle = angle;
                /*gameObject.transform.localScale = new Vector3(-1, 1, 1);*/
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(modifiedAngle, 120f, 240f));
                if (Input.GetMouseButtonDown(0) && !isShooting)//shoot
                    StartCoroutine(Shoot(Mathf.Clamp(Lscale, -1f, 1f), Mathf.Clamp(modifiedAngle, 120f, 240f)));
            }
        }


    }

    IEnumerator Shoot(float scale, float angle)
    {

        isShooting = true;
        GameObject newBullet = Instantiate(bullet[Pl.bulletIndex], shootPos.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(scale, Mathf.Tan(angle * Mathf.PI / 180) * Mathf.Clamp(Lscale, -1f, 1f)).normalized * shootSpeed;
        newBullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        newBullet.transform.localScale = new Vector2(newBullet.transform.localScale.x * scale, newBullet.transform.localScale.y);
        Timer(BulletDestroyTimer, newBullet);
        yield return new WaitForSeconds(shootTimer);
        isShooting = false;
    }

    async void Timer(int Second, GameObject ObjToDestroy)
    {
        await Task.Delay(Second * 1000);
        Destroy(ObjToDestroy);
    }
}
