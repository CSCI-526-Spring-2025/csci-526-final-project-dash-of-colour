using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 9.0f; //Player linear speed    //Test value = 0.6
    public float rotationSpeed = 30.0f; //Player rotation speed
    public float bounceForce = 10.0f;
    public float slightBounceForce = 3.5f;

    public Volume postProcessingVolume;
    private Vignette vignette;
    public float transitionSpeed = 25.0f;  // Speed of vignette transition

    private float targetIntensity = 0f; // Target vignette intensity
    private float targetSmoothness = 0f;

    private Rigidbody playerRB;
    private bool isFocusMode = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        TimerController.instance.BeginTimer();

        if (postProcessingVolume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = 0f;
            vignette.smoothness.value = 1f;
            Debug.Log("Vignette effect found and initialized.");
        }
        else
        {
            Debug.LogError("Vignette effect not found in the post-processing volume profile.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Level 1")
            if (!GameManager.instance.gameStarted) return; // Stop movement before countdown ends
        float moveAmount = Input.GetAxis("Vertical");
        float turnAmount = Input.GetAxis("Horizontal") * rotationSpeed * Time.fixedDeltaTime;
        if (moveAmount != 0.0f||turnAmount!=0.0f)
        {
            if (isFocusMode)
                ExitFocusMode();
        }
        else
        {
            if (!isFocusMode)
                EnterFocusMode();
        }

        Vector3 movement = transform.forward * moveAmount * speed * Time.fixedDeltaTime;
        playerRB.AddForce(movement, ForceMode.VelocityChange);
        //playerRB.MovePosition(playerRB.position + movement);

        // Rotate player based on horizontal input.
        Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
        playerRB.MoveRotation(playerRB.rotation * turnRotation);

    }

    void FixedUpdate()
    {
        UpdateVignetteEffect();
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Logic for adding the innovative collision mechanics of the game
        if (collision.gameObject.CompareTag("Bouncy"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                playerRB.AddForce(contact.normal * bounceForce, ForceMode.Impulse);
            }
        }
        else if (collision.gameObject.CompareTag("Slightly_Bouncy"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                playerRB.AddForce(contact.normal * slightBounceForce, ForceMode.Impulse);
            }
        }
    }

    void EnterFocusMode()
    {
        if (!isFocusMode)
        {
            Time.timeScale = 0.05f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;

            // Apply post-processing effects
            targetIntensity = 0.6f;
            targetSmoothness = 0.5f;

            transitionSpeed = 25.0f;
            isFocusMode = true;
        }
    }

    void ExitFocusMode()
    {
        if (isFocusMode)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;

            // Disable post-processing effects
            targetIntensity = 0f;  // No vignette when focus mode ends
            targetSmoothness = 1f;
            transitionSpeed = 1.25f;
            isFocusMode = false;
        }
    }

    void UpdateVignetteEffect()
    {
        if (vignette != null)
        {
            // Smoothly transition vignette intensity and smoothness
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetIntensity, Time.deltaTime * transitionSpeed);
            vignette.smoothness.value = Mathf.Lerp(vignette.smoothness.value, targetSmoothness, Time.deltaTime * transitionSpeed);
        }
    }

}
