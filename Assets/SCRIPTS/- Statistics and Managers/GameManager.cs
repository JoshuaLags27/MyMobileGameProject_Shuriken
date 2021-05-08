using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
 
    public GameObject ResultsScreen; // This will display FinalScoreCounter UI referenced to the base score

    [Header("Score Properties")]
    [Space]
    public GameObject FinalScoreObj;
    public Text FinalScoreText;     // The Final Score that's to be displayed upon depletion of life;
    public int currentScore;
    [Space]
    public int scoreMilestoneIndex;
    [Space]
    public int[] scoreMilestone;
    
    EnemySpawner_Falling es_Falling;
    Score score;

    [Space]
    [Header("Combo Properties")]
    [Space]

    public GameObject MaxComboGameObject;
    public Text MaximumCombosText;   // The Maximum Combo the player had achieved
    public int currentMaxCombo;

    [Space]
    [Header("Accu Properties")]

    public float shurikensThrown; // amount of shurikensThrown
    public float shurikensHit;    // amount of hit Shurikens
    [Space]
    public GameObject AccuRatioGameObject;
    public Text AccuRatioText;
    public float AccuRatio;  // The hit Accuracy amount of the player;

    ComboSystem comboSystem;

    [Space]
    public GameObject RetryButton;
    public GameObject MenuButton;
    [Space]
    [Header(" Enemy Object Properties")]

    public float EnemiesSpeed;
    public float EnemiesSpeedIncreaseAmount;


    // Start is called at the start of the frame
    private void Start()
    {
        // Disable the Win and Lose Screen in game
        ResultsScreen.SetActive(false);

        // Disable the RetryButton from the ResultScreen.
        RetryButton.SetActive(false);

        // Disable the Accuracy GameObject at the start
        AccuRatioGameObject.SetActive(false);

        // Disable the MenuButton from the ResultsScreen.
        MenuButton.SetActive(false);

        // Disable the FinaSCoreObj at the Start
        FinalScoreObj.SetActive(false);

        // Disable the MaxComboGameObject at the Start
        MaxComboGameObject.SetActive(false);
    }

    void Update()
    {

        if (GameObject.FindObjectOfType<Score>())
        {
            score = GameObject.FindObjectOfType<Score>();

            currentScore = score.Base_Score;
        }
        else
        {
            Debug.Log("No Score Script Present");
        }
       
        // Find the 'ComboSystem' class and call it's 'cmbpo_amount' variable that holds the score.
        comboSystem = GameObject.FindObjectOfType<ComboSystem>();

        // Calculate the Accuracy Ratio in the game
        AccuRatio = (shurikensHit / shurikensThrown) * 100;

        if(AccuRatio >= 100f)
        {
            AccuRatio = 100f;
        }

        // Update the Maximum Combo
        if (comboSystem.cmbo_amount > currentMaxCombo)
        {
            currentMaxCombo = comboSystem.cmbo_amount;
        }

        // Update the the Life Counter
        if (score.LifeAmount <= 0)
        {
            // Disable the Spawning
            GameObject.Find("Spawners").GetComponent<EnemySpawner_Falling>().enabled = false;
            
            StartCoroutine(FinalResultScreen());
        }

        // Secret Goal
        if(score.Base_Score >= 1000)
        {
            StartCoroutine(FinalResultScreen());
        }

        // For updating the diffiulty of the game

        // DifficultyEscalate();

    }


    IEnumerator FinalResultScreen(float resultsDisplayInterval = 0.5f, float resultsUIButtons = 1.5f)
    {
      
        // Enable the ResultsScreen
        ResultsScreen.SetActive(true);

        // Stop Time
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(resultsDisplayInterval);
        FinalScoreObj.SetActive(true);
        yield return new WaitForSecondsRealtime(resultsDisplayInterval);

        //  Updates the Base score to the Final Score to be displayed when life is gone.
        FinalScoreText.text = score.Base_Score.ToString();


        yield return new WaitForSecondsRealtime(resultsDisplayInterval);
        MaxComboGameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(resultsDisplayInterval);

        // Update the Combo Amount to the Final Score
        MaximumCombosText.text = currentMaxCombo.ToString();

        yield return new WaitForSecondsRealtime(resultsDisplayInterval);
        AccuRatioGameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(resultsDisplayInterval);

        AccuRatio = Mathf.Round(AccuRatio * 10.0f) * 0.1f; // To Display the Value in the Tenths Place

        AccuRatioText.text = AccuRatio.ToString() + "%";

        yield return new WaitForSecondsRealtime(resultsUIButtons);
        
        RetryButton.SetActive(true);
        MenuButton.SetActive(true);
    }

    // This Function is for increasing the challenge in the demo default game
    void DifficultyEscalate()
    {
        es_Falling = GameObject.FindObjectOfType<EnemySpawner_Falling>();
        
        // Update the Difficuty 
        if (score.Base_Score >= scoreMilestone[scoreMilestoneIndex])
        {
            // Update the Enemy prefab to increase the speed of the falling enemies
            EnemiesSpeed += EnemiesSpeedIncreaseAmount;

            // Reduce the Time the Shuriken will Spawn
            es_Falling.CurrentTime -= 0.075f; 

            // Increment to the next milstone
            scoreMilestoneIndex += 1;
        }
        

        foreach(Enemy_Movement allPresentShurikens in GameObject.FindObjectsOfType<Enemy_Movement>())
        {
            if(allPresentShurikens.GetComponent<Enemy_Movement>() != null)
            {
                allPresentShurikens.GetComponent<Enemy_Movement>().speed = EnemiesSpeed;
            }
        }
        
    }
    
    public void FinalResultsRetry()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Play");
    }

    public void FinalResultsMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }

    // This Method will be called when the player has no life remaining
    IEnumerator TransitionToMainMenu()
    {
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }


}
