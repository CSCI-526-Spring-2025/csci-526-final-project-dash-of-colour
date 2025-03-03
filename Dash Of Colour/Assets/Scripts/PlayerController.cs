using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 9.0f; //Player linear speed    //Test value = 0.6
    public float rotationSpeed = 30.0f; //Player rotation speed
    public float bounceForce = 5.0f;
    public float slightBounceForce = 3.5f;
    private Rigidbody playerRB;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveAmount = Input.GetAxis("Vertical");
        float turnAmount = Input.GetAxis("Horizontal") * rotationSpeed * Time.fixedDeltaTime;
        if (moveAmount != 0.0f||turnAmount!=0.0f)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
        else
        {
            Time.timeScale = 0.05f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }

        Vector3 movement = transform.forward * moveAmount * speed * Time.fixedDeltaTime;
        playerRB.AddForce(transform.forward * moveAmount * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        
        // Rotate player based on horizontal input.
        
        Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
        playerRB.MoveRotation(playerRB.rotation * turnRotation);
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

}
