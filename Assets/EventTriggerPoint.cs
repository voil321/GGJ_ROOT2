using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerPoint : MonoBehaviour
{
    // Start is called before the first frame update


    private void OnTriggerStay2D(Collider2D collision)
    {
        
        var player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            UIController.instance.StartEvent();
        }
    }

}
