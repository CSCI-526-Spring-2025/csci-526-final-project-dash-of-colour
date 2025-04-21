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
    public float blinkDuration = 2f; 
    public float blinkInterval = 0.2f; 
    public static bool isFalling = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();

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
        if(playerRB.CompareTag("Player"))
            isFalling = true;

        if (playerRB != null)
        {
            playerRB.useGravity = true; 
            playerRB.constraints = RigidbodyConstraints.None; // Remove Y-axis freeze
        }

        yield return new WaitForSeconds(1f); // wait duration

        // Now reset the position
        transform.position = CheckPointData.currCheckPoint;
        transform.rotation = startRotation;

        if (playerRB != null)
        {
            playerRB.linearVelocity = Vector3.zero;
            playerRB.angularVelocity = Vector3.zero;

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
