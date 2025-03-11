using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu screen");  // Make sure "Main Menu" is the exact name of your main menu scene!
    }
}
