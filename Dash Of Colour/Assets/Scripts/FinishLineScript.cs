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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winPage.SetActive(false);
        winPosText = winPosObj.GetComponent<TextMeshProUGUI>();
        carsFinished = new bool[2] { false, false };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
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
            //Logic to correctly determine the finish states of the player car.
            switch(position)
            { 
                //Each cases decribing the finish status of the player in the race.
                case 1: winPosText.text = "Congratulations! \nYou came 1st!";
                        break;
                case 2: winPosText.text = "Congratulations! \nYou came 2nd!";
                        break;
                case 3: winPosText.text = "Congratulations! \nYou came 3rd!";
                        break;
                default: winPosText.text = "Congratulations! \nYou finished the race!";
                         break;
            }
            gameDone = true;
            winPage.SetActive(true);
            AnalyticsManager.Instance.LevelComplete();
        }
    }
}
