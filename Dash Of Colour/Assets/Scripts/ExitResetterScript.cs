using UnityEngine;

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class ExitResetterScript : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody playerRB;
    private Renderer playerRenderer;
    public PlayerController player_controls;

    public Leaderboard leaderboard;
    string sceneName;

    private float orig_Speed = 9.0f; //The original speed of the player car
    public float blinkDuration = 2f; 
    public float blinkInterval = 0.2f; 
    public static bool isFalling = false;
    private bool isResetting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();
        player_controls = GetComponent<PlayerController>();
        orig_Speed = player_controls.speed;
        startPosition = transform.position;
        startRotation = transform.rotation;

        // Initial check point is the start point
        CheckPointData.currCheckPoint = new Vector3(startPosition.x, startPosition.y, 0);

        // // leaderboard = FindObjectOfType<Leaderboard>();
        // if (leaderboard == null)
        // {
        //     Debug.LogError("Leaderboard component not found in scene!");
        // }

        
        // if (leaderboard == null)
        // {
        //     leaderboard = FindObjectOfType<Leaderboard>();
        //     if (leaderboard == null)
        //     {
        //         Debug.LogError("Leaderboard not found in scene!");
        //     }
        // }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isResetting && other.CompareTag("Exit_reset"))
        {
            isResetting = true;
            ResetPlayer(); // Resetting the player with tag comparision logic
        }
    }

    void ResetPlayer()
    {
        if (leaderboard == null)
        {
            leaderboard = FindObjectOfType<Leaderboard>();
            if (leaderboard == null)
            {
                Debug.LogError("Leaderboard not found when trying to reset player!");
            }
        }
        StartCoroutine(FallThenReset());
    }

    IEnumerator FallThenReset()
    {
        if (playerRB.CompareTag("Player"))
        {
            // Capture death data before reset
            Vector3 deathPosition = transform.position;
            string currentLevel = SceneManager.GetActiveScene().name;

            // Submit death to leaderboard if available
            if (leaderboard != null)
            {
                leaderboard.SubmitDeath(currentLevel, deathPosition);
                Debug.Log($"Death recorded in {currentLevel} at {deathPosition}");
            }

            isFalling = true;
            player_controls.speed = 0.0f;   //Stop the player from controlling the car during the fall

            Vector3 pushForce = (playerRB.transform.position - transform.position)*10.0f;// Add a bit of a force so it looks like the car clearly goes over the edge
            playerRB.AddForce(pushForce, ForceMode.Impulse);
            playerRB.AddTorque(playerRB.transform.right*0.5f, ForceMode.Impulse);
        }

        if (playerRB.CompareTag("Player")&&playerRB != null)
        {
            playerRB.useGravity = true; 
            playerRB.constraints = RigidbodyConstraints.None; // Remove Y-axis freeze
            yield return new WaitForSeconds(1f); // wait duration

            // Now reset the position
            transform.position = CheckPointData.currCheckPoint;
            transform.rotation = startRotation;

            playerRB.linearVelocity = Vector3.zero;
            playerRB.angularVelocity = Vector3.zero;
            player_controls.speed = orig_Speed; //Return to original speed when giving control back to the player

            playerRB.useGravity = false; // Re-disable gravity
            playerRB.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

            isFalling = false;
        }

        isResetting = false;
        StartCoroutine(BlinkEffect());
    }

    IEnumerator BlinkEffect()
    {
        float elapsedTime = 0f;
        bool isVisible = true;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        while (elapsedTime < blinkDuration)
        {
            foreach (Renderer renderer in renderers)
            {
                if (renderer != null)
                    renderer.enabled = isVisible;
            }

            isVisible = !isVisible;

            yield return new WaitForSecondsRealtime(blinkInterval);
            elapsedTime += blinkInterval;
        }

        
        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
                renderer.enabled = true;
        }
    }
}
