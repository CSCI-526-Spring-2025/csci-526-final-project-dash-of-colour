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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();

        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Exit_reset"))
        {
            ResetPlayer();
        }
    }

    void ResetPlayer()
    {
        
        transform.position = startPosition;
        transform.rotation = startRotation;
        
        if (playerRB != null)
        {
            playerRB.linearVelocity = Vector3.zero;
            playerRB.angularVelocity = Vector3.zero;
        }

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
