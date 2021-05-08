using UnityEngine.UI;
using UnityEngine;

// 10/23/2020 - this script manages the combo system of the game where the player does consecutive hits to gain an added bonus score
public class ComboSystem : MonoBehaviour
{
    [Header("Score Properties")]
    [Space]
    public Text scr_text;
    public int  scr_currentAmount;
    [Space]
    [Header("Combo Properties")]
    [Space]
    public Text cmbo_text;
    public  int cmbo_amount;
    [Space]
    [Header("Timer Properties")]
    [Space]
    public Image tmr_image;
    public float tmr_currentAmount;
    public  bool tmr_enabler;

    private float tmr_deductionSpeed = 1.0f;
    private float tmr_duplicateAmount;
    private float tmr_duplicateTimeAmount;

    public void Start()
    {
        // Assigned the value of the current timer as reference for origin
        tmr_duplicateAmount = tmr_currentAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
        // Assigned the current amount to sync asyn via the image
        tmr_image.fillAmount = tmr_currentAmount/tmr_duplicateAmount;

        // Assign the current combo amount to the text property to visualize the accumulated non missed hits.
        cmbo_text.text = "x" + cmbo_amount.ToString();
        
        // Assign the current score increment amount depending on the amount of combos stacked.
        scr_text.text = "+" + scr_currentAmount.ToString();

        // Enable the timer of the game
        if(tmr_enabler == true)
        {
            // Countdown of the Combo Timer is initialized
            tmr_currentAmount -= Time.deltaTime * tmr_deductionSpeed;

            // Enable both the text and the cmbo visuals when the combo amount is greater than 1
            if(cmbo_amount > 1)
            {
              cmbo_text.enabled = true;
              tmr_image.enabled = true;
              scr_text.enabled = true;
            }
             
            if(tmr_currentAmount <= 0)
            {
                scoreToMainScore();
                reset();
                tmr_enabler = false;
            }
        }

        else
        {
            // Disable both the text and the cmbo pertaining to it
            tmr_image.enabled = false;
            cmbo_text.enabled = false;
            scr_text.enabled = false;
        }
    }

    private void scoreToMainScore()
    {
        this.gameObject.GetComponent<Score>().AddScore(scr_currentAmount);
    }

    // resets the whole system
    private void reset()
    {
        cmbo_amount = 0;
        tmr_currentAmount = 0.0f;
        scr_currentAmount = 0;
        tmr_deductionSpeed = 1;
    }

    // Called when an object is hit to reset the time.
    public void hitDetect()
    {
        // Enable timer countdown.
        tmr_enabler = true;
        // Reset the Current timer amount.
        tmr_currentAmount = tmr_duplicateAmount;
        // Increase the speed of the timer amount.
        tmr_deductionSpeed += 0.05f;
        // Add another combo
        cmbo_amount += 1; 
        // Add score based on the score multiplier
        scr_currentAmount += 2;
    }
}
