using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
  
    [Space]
    public GameObject MsgPopGameObject;
    [Space]
    public int current_Msg;
    [Space]
    public GameObject[] MsgTexts;

    public bool Enabled;


    private void Awake()
    {
        Enabled = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        // Initialize Tutorial Panel
        StartCoroutine(TutorialMessage());

        if(MsgPopGameObject != null)
        {
            MsgPopGameObject.SetActive(false);
        }

        // Dislay the first Message
        MsgTexts[current_Msg].SetActive(true);
    }

    void Update()
    {
        // MOBILE
        if (Enabled == true)
        {
            
            if (Application.platform == RuntimePlatform.Android)
            {
                Touch touch = Input.GetTouch(0);

                if (Input.touchCount > 0)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        if (current_Msg < MsgTexts.Length - 1)
                        {
                            NextMessage();
                        }
                        else // If there are no more messages, then continue
                        {
                            Continue();
                        }
                    }
                }
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                // EDITOR/PC
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (current_Msg < MsgTexts.Length - 1)
                    {
                        NextMessage();
                    }
                    else // If there are no more messages, then continue
                    {
                        Continue();
                    }
                }
            }
        }
    }

    IEnumerator TutorialMessage(float MsgPopTime = 1.5f)
    {
        // Wait for 1.5 seconds
        yield return new WaitForSeconds(MsgPopTime);

        // Stop Time
        Time.timeScale = 0.0f;

        // Activate the Message Panel
        MsgPopGameObject.SetActive(true);
    }

    void NextMessage()
    {
        // Disable the Current Message
        MsgTexts[current_Msg].SetActive(false);
        // Go to the Next Index of the Message
        current_Msg += 1;
        // Enable this Message
        MsgTexts[current_Msg].SetActive(true);
    }

    void Continue()
    {
        Time.timeScale = 1.0f;

        MsgPopGameObject.SetActive(false);

        this.gameObject.SetActive(false);
        
        // Disabe this Prompt
        Enabled = false;

        // Disable this Script
        this.enabled = false;
    }
       
}
