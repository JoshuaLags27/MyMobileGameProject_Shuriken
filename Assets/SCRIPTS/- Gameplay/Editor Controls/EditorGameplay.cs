using UnityEngine;


public class EditorGameplay : GameplayFunctions
{

    void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            CurrentCooldownRegistry = CurrentCooldown;
            // Start the game with the Already set cooldown.
            CurrentCooldown = 0;
        }
    }

    void LateUpdate()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
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
            // FOR THE APPLICATION TO RUN ON THE UNITY EDITOR
            //---------------------
            
            // The Update of the Cursors Mouse Position
            cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                //---------------------
                // LMG - PRESSED
                //---------------------
                if (Input.GetKeyDown(KeyCode.Mouse0) && !IsPointerOverUIObject())
                {
                    inputPressed(cursorPosition);
                }

            if (EnabledThrowEffect == true)
            {
                //---------------------
                // LMB - HOLD
                //---------------------
                if (Input.GetKey(KeyCode.Mouse0) && !IsPointerOverUIObject())
                {
                    inputHold(cursorPosition);
                }

                //---------------------
                // LMG - RELEASED
                //---------------------
                if (Input.GetKeyUp(KeyCode.Mouse0) && !IsPointerOverUIObject())
                {
                    inputReleased(cursorPosition);
                }
            }
        }
    }
}
