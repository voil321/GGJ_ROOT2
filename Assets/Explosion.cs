using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Collider2D> triggerObject;

    void Start()
    {
        Destroy(gameObject, 1f);
        triggerObject = new List<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (!triggerObject.Contains(collision))
        {
            var target = collision.gameObject.GetComponent<BossHandler>();

            if (target == null)
                return;

            target.Hurt(0);

            triggerObject.Add(collision);

            Debug.Log("triiger!!");
        }
    }



}
