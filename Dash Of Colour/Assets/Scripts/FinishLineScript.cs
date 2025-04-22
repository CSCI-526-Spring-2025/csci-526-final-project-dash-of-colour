using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishLineScript : MonoBehaviour
{
    private int position = 1;//Assume the player is in 1st position
    bool gameDone = false;
    bool[] carsFinished;
    public GameObject winPage;
    public GameObject winPosObj;
    TextMeshProUGUI winPosText;
    public Leaderboard leaderboard;
    string sceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winPage.SetActive(false);
        winPosText = winPosObj.GetComponent<TextMeshProUGUI>();
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
            // Determine finishing position text
            string positionText = sceneName switch
            {
                "Tutorial Level" => "Congratulations! \nYou finished the tutorial!",
                _ => "Congratulations! You finished the level!"
            };

            // Get final time from TimerController
            string currentFinalTime = TimerController.instance.GetFinalTime();

            // Set win page text
            if (sceneName.Equals("Tutorial Level"))
            {
                winPosText.text = $"{positionText}";
            }
            else
            {
                winPosText.text = $"{positionText}\n{currentFinalTime}";

                string levelName = SceneManager.GetActiveScene().name;
                float finalTime = TimerController.instance.GetFinalTimeAsFloat(); // convert to float
                string dummyName = DummyNameGenerator.GenerateName();

                leaderboard.SubmitScore(levelName, dummyName, finalTime);

                leaderboard.FetchTopScores(levelName, top5 =>
                {
                    string board = "\nLeader Board (Top 5):\n";
                    foreach (var entry in top5)
                        board += $"{entry.player_name} - {entry.time_seconds:F2}s\n";
                    winPosText.text += "\n" + board;
                });

            }

            // Displaying the winning banner
            gameDone = true;
            winPage.SetActive(true);

            AnalyticsManager.Instance.LevelComplete();
        }
        TimerController.instance.PlayerWon();
    }
}
