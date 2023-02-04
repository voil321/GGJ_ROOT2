using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Platformer.Mechanics;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Image> imageList;
    public static UIController instance;

    public GameObject bulletSelect;
    public GameObject messageBox;
    public TMP_Text messageText;

    public PlayerController player;

    [SerializeField] bool isMessage;

    public string[] message;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartEvent();
        }
    }

    public void SetBullet(int index)
    {
        for (int i = 0; i < imageList.Count; i++)
        {
            imageList[i].color = (i == index) ? new Color(0.78f, 0.42f, 0.42f) : Color.white;
        }
    }

    public void StartEvent()
    {
        StartCoroutine(MessageStart());
    }

    public IEnumerator MessageStart()
    {
        Debug.Log("Run");

        if (isMessage)
            yield break;

        player.controlEnabled = false;
        isMessage = true;
        int index = 0;
        messageBox.gameObject.SetActive(true);
        messageText.text = message[index];

        while (index < message.Length)
        {
            yield return new WaitForSeconds(0.2f);

            yield return new WaitUntil(() => Input.anyKeyDown);

            index++;

            if (index < message.Length)
                messageText.text = message[index];
        }
        messageBox.gameObject.SetActive(false);
        player.controlEnabled = true;
        bulletSelect.SetActive(true);

    }

}
