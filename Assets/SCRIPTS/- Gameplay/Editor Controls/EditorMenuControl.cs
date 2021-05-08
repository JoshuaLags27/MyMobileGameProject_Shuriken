using UnityEngine;

public class EditorMenuControl : MenuFunctions
{
    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            /***********************/
            // LMB PRESSED
            /***********************/
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                menuControl_Down(hitReg, areaReg, mousePosition);
            }
            /***********************/
            // LMB HELD
            /***********************/
            if (Input.GetKey(KeyCode.Mouse0))
            {
                menuControl_Hold(mousePosition);
            }
            /***********************/
            // LMB LIFTED UP
            /***********************/
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                menuControl_Up(mousePosition);
            }
        }
    }
}
