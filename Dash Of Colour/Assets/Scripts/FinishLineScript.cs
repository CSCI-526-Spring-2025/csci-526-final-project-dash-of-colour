using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinishLineScript : MonoBehaviour
{
    private int position = 1;//Assume the player is in 1st position
    bool gameDone = false;
    bool[] carsFinished;
    public GameObject winPage;
    public GameObject winPosObj;
    public GameObject leaderboardObj;
    TextMeshProUGUI winPosText;
    TextMeshProUGUI leaderboardText;
    public GameObject pauseMenu;
    private bool isPaused = false;
    public Leaderboard leaderboard;
    string sceneName;
    private bool gameFinish = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winPage.SetActive(false);
        pauseMenu.SetActive(false);
        winPosText = winPosObj.GetComponent<TextMeshProUGUI>();
        leaderboardText = leaderboardObj.GetComponent<TextMeshProUGUI>();
        carsFinished = new bool[2] { false, false };
        sceneName = SceneManager.GetActiveScene().name;
        
        if (leaderboard == null)
        {
            leaderboard = FindObjectOfType<Leaderboard>();
            if (leaderboard == null)
            {
                Debug.LogError("Leaderboard not found in scene!");
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            AnalyticsManager.Instance.IncrementResetCount();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
            // Debug.LogError("Esc Button pressed");
        }

    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f; // Pause the game
            TimerController.instance.PauseTimer();
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
            TimerController.instance.ResumeTimer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Logic to correctly determine the finish states of the opponent cars
        if (other.gameObject.name == "Car_Opp_1" && !gameDone && !carsFinished[0])
        { 
            position++;
            carsFinished[0] = true;
        }
        else if (other.gameObject.name == "Car_Opp_2" && !gameDone && !carsFinished[1])
        {
            position++;
            carsFinished[1] = true;
        }
        else if (other.gameObject.name=="Player_Car")//The player has passed the finish line
        {
            if(!gameFinish)
            {
                StartCoroutine(HandlePlayerFinish());
            }
            
            gameFinish = true;


        }
        TimerController.instance.PlayerWon();
    }

    private IEnumerator HandlePlayerFinish()
    {
        string positionText = sceneName switch
        {
            "Tutorial Level" => "Congratulations! \nYou finished the tutorial!",
            _ => "Congratulations! You finished the level!"
        };

        string curretnFinalTime = TimerController.instance.GetFinalTime();

        if (sceneName.Equals("Tutorial Level"))
        {
            winPosText.text = $"{positionText}";
        }
        else
        {
            string levelName = SceneManager.GetActiveScene().name;
            float finalTime = TimerController.instance.GetFinalTimeAsFloat();
            string dummyName = DummyNameGenerator.GenerateName();

            winPosText.text = $"{positionText}\n{curretnFinalTime}";

            leaderboard.SubmitScore(levelName, dummyName, finalTime);

            // === Fetch Current Position ===
            bool fetchCurrentDone = false;
            int playerPosition = -1;

            leaderboard.FetchCurrentPosition(levelName, finalTime, pos =>
            {
                playerPosition = pos;
                fetchCurrentDone = true;
            });

            yield return new WaitUntil(() => fetchCurrentDone);

            if (playerPosition != -1)
            {
                string posText = $"\nYour position: {playerPosition} ({dummyName})";
                winPosText.text += posText;
            }
            else
            {
                string posText = "\nCould not predict your position!";
                winPosText.text += posText;
            }

            // === Fetch Top Scores ===
            bool fetchTopDone = false;

            leaderboard.FetchTopScores(levelName, top5 =>
            {
                string board = "";
                foreach (var entry in top5)
                    board += $"{entry.player_name} - {entry.time_seconds:F2}s\n";
                leaderboardText.text += "\n" + board;
                fetchTopDone = true;
            });

            yield return new WaitUntil(() => fetchTopDone);
          
        }

        // Now safe to show the winning page
        gameDone = true;
        winPage.SetActive(true);

        AnalyticsManager.Instance.LevelComplete();
    }

}
