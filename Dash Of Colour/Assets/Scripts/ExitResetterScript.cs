using UnityEngine;
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

    private float orig_Speed = 9.0f; //The original speed of the player car
    public float blinkDuration = 2f; 
    public float blinkInterval = 0.2f; 
    public static bool isFalling = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Exit_reset"))
        {
            ResetPlayer(); // Resetting the player with tag comparision logic
        }
    }

    void ResetPlayer()
    {
        StartCoroutine(FallThenReset());
    }

    IEnumerator FallThenReset()
    {
        if (playerRB.CompareTag("Player"))
        {
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
        }

        // Now reset the position
        transform.position = CheckPointData.currCheckPoint;
        transform.rotation = startRotation;

        if (playerRB.CompareTag("Player")&&playerRB != null)
        {
            playerRB.linearVelocity = Vector3.zero;
            playerRB.angularVelocity = Vector3.zero;
            player_controls.speed = orig_Speed; //Return to original speed when giving control back to the player

            playerRB.useGravity = false; // Re-disable gravity
            playerRB.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation; 
        }
        isFalling = false;
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
