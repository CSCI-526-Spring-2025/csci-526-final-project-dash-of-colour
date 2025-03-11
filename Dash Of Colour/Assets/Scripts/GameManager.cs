using System.Diagnostics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance
    public bool gameStarted = false; // Track game start

    private void Awake()
    {
        instance = this;
    }

    public void StartGame()
    {
        gameStarted = true; // Allow all objects to move
    }
}