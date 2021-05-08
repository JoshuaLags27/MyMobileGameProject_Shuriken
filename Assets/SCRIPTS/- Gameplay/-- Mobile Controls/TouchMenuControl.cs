using UnityEngine;

public class TouchMenuControl : MenuFunctions
{
    // Update is called once per frame
    void Update()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if(Input.touchCount > 0)
            {
                if (Input.touchCount == 1)
                {
                    /***********************/
                    // TAPPED FINGER
                    /***********************/
                    if (touch.phase == TouchPhase.Began)
                    {
                        menuControl_Down(hitReg, areaReg, touchPosition);
                    }
                    /***********************/
                    // HELD FINGER
                    /***********************/
                    if (touch.phase == TouchPhase.Moved)
                    {
                        menuControl_Hold(touchPosition);
                    }
                    /***********************/
                    // LIFT FINGER
                    /***********************/
                    if (touch.phase == TouchPhase.Ended)
                    {
                        menuControl_Up(touchPosition);
                    }
                }
            }
        }
    }
}
