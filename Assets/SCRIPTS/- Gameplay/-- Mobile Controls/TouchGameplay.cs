using UnityEngine;

public class TouchGameplay : GameplayFunctions
{
   
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            CurrentCooldownRegistry = CurrentCooldown;
            // Start the game with the Already set cooldown.
            CurrentCooldown = 0;
        }
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        { 
            // Shuriken Cooldown
            CooldownCircle.fillAmount = CurrentCooldown / CurrentCooldownRegistry;

            // Shuriken Cooldown timer
            if(Cooled_down == false)
            {
                // Shuriken Cooldown Deduction Timer
                CurrentCooldown -= Time.deltaTime;

                if(CurrentCooldown <= 0)
                {
                    // Disable the Logo Transluscent
                    LogoTransluscentEnabled(false);

                    // Enable the Cooldown UI
                    CooldownCircle.enabled = false;

                    // Lock the Cooldown in exactly zero
                    CurrentCooldown = 0;

                    // Enable the Shuriken to be spawned
                    Cooled_down = true;
                }
            }

            //---------------------
            // FOR THE APPLICATION TO RUN ON THE ANDROID PLATFORM
            //---------------------

            // Variable of the First Touch
            Touch touch = Input.GetTouch(0);

            // The Update of the Fingers touch position in space
            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (Input.touchCount > 0)
            {
                    //---------------------
                    // FINGER - PRESSED
                    //---------------------
                    if (touch.phase == TouchPhase.Began && !IsPointerOverUIObject())
                    {
                        inputPressed(touchPosition);
                    }

                if (EnabledThrowEffect == true)
                {
                    //---------------------
                    // FINGER - HOLD
                    //---------------------
                    if (touch.phase == TouchPhase.Moved && !IsPointerOverUIObject())
                    {
                        inputHold(touchPosition);
                    }

                    //---------------------
                    // FINGER - RELEASED
                    //---------------------
                    if (touch.phase == TouchPhase.Ended && !IsPointerOverUIObject())
                    {
                        inputReleased(touchPosition);
                    }
                }
            }
        }
    }
}
