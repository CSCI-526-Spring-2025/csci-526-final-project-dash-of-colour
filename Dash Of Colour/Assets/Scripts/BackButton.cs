using UnityEngine;
using UnityEngine.SceneManagement;

//code to implement a button which helps user navigate to main menu after levels and tutorials.
public class BackButton : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu screen");  //On-click, go back to the main menu screen named "Menu screen" in our case.
    }
}
