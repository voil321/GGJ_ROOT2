using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Image> imageList;
    public static UIController instance;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetBullet(int index)
    {
        for (int i = 0; i < imageList.Count; i++)
        {
            imageList[i].color = (i == index) ? new Color(0.78f, 0.42f, 0.42f) : Color.white;
        }
    }


}
