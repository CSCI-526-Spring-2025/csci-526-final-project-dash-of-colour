using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //Loading respective scenes based on selections
    public void playTutorial()
    {
        SceneManager.LoadScene("Tutorial Level");
    }
    public void playLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void playLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }
    public void playLevel3()
    {
        SceneManager.LoadScene("Level 3");
    }
    public void playLevel4()
    {
        SceneManager.LoadScene("Level 4");
    }
}
