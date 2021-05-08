using UnityEngine;
using UnityEngine.UI;


// This script is the component of the players progress
public class Score : MonoBehaviour
{
    public int Base_Score;          // The Current score amount of the game
    public int LifeAmount;          // The Life amount the player has, if Life amount goes to zero game over 
    [Space]
    public Text TextScore;          // The Score UI to be Referenced by text to the Base_Score variable
    public Text LifeCounter;        // The Life UI to be referenced by text to the LifeAmount varible

    [Space]
    public int ScoreMilestoneCurrent; // (Endless Mode) this integer value serves as a mil
    [Space]
    public float EnemySpeedCurrent;
    [Space]
    public float EnemySpawnIntervalCurrent;
    //public float EnemySpawnIntervalUpdate;

    GameObject[] AllEnemies;

    private void Awake()
    {
        // Set the Time to RealTime When Starting the game
        Time.timeScale = 1.0f;
    }


    // Update is called once per frame
    void Update()
    {
        // Updates the Base Score of the game in the UI
        TextScore.text = "Score: " + Base_Score.ToString();

        // Update the Amount of Life the Player has
        LifeCounter.text = "Life:" + LifeAmount.ToString();
        
    }
    
    // This AddScore Method will add a single score to the player
    public void AddScore(int amount)
    {
        Base_Score += amount;
    }
}
