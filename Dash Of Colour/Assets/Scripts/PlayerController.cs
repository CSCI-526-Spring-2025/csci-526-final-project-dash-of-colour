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
    private float origSpeed = 9.0f;
    private float speedMultiplier = 1f;
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
    private Vector3 curRotation;

    public ParticleSystem trailParticles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        origSpeed = speed;

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
        curRotation = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (ExitResetterScript.isFalling || BouncyPlatform.isBouncing)
        {
            if (isFocusMode)
                ExitFocusMode(); // Force exit focus if falling
            if (trailParticles.isPlaying)
                trailParticles.Stop();
            return; // Skip input while falling
        }
        if (LevelData.validLevels.Contains(SceneManager.GetActiveScene().name))
            if (!GameManager.instance.gameStarted) return; // Stop movement before countdown ends
        float moveAmount = Input.GetAxisRaw("Vertical");
        float turnAmount = Input.GetAxisRaw("Horizontal");// * rotationSpeed * Time.fixedDeltaTime;
        Vector3 movement = (Vector3.left * moveAmount * speed * speedMultiplier * Time.fixedDeltaTime) + (Vector3.forward * turnAmount * speed * speedMultiplier * Time.fixedDeltaTime);
        if (moveAmount != 0.0f||turnAmount!=0.0f)
        {
            if (isFocusMode)
                ExitFocusMode();
            playerRB.transform.rotation = Quaternion.Slerp(playerRB.transform.rotation, Quaternion.LookRotation(movement, Vector3.up), 0.1f);
            if (!trailParticles.isPlaying)
                trailParticles.Play();
        }
        else
        {
            if (!isFocusMode)
                EnterFocusMode();
            if (trailParticles.isPlaying)
                trailParticles.Stop();
        }

        playerRB.AddForce(movement, ForceMode.VelocityChange);
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
                if(speed<=origSpeed)
                    StartCoroutine(StickTemporarily(1.0f, 1.1f));
            }
        }
        else if (collision.gameObject.CompareTag("Slightly_Bouncy"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                playerRB.AddForce(-1*contact.normal * slightBounceForce, ForceMode.Impulse);
                if (speed >= origSpeed)
                    StartCoroutine(StickTemporarily(1.0f, 0.25f));
            }
        }
        /*
        else if (collision.gameObject.CompareTag("Slightly_Bouncy"))
        {
             foreach (ContactPoint contact in collision.contacts)
             {
                 playerRB.linearVelocity = Vector3.zero;
                 playerRB.angularVelocity = Vector3.zero;

                 StartCoroutine(StickTemporarily(0.7f));
             }
         }
        */
    }
    private IEnumerator StickTemporarily(float duration, float speedChange)
    {
        speed *= speedChange;
        yield return new WaitForSeconds(duration);
        speed = origSpeed;
    }
    void EnterFocusMode()
    {
        if (!isFocusMode)
        {
            Time.timeScale = 0.05f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;

            // Apply post-processing effects
            targetIntensity = 0.55f;
            targetSmoothness = 0.15f;

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

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

}
