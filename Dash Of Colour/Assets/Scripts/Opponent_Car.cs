using System;
using System.Collections;
using UnityEngine;

public class Opponent_Car : MonoBehaviour
{
    public float speed = 4.5f; //Player linear speed    //Test value = 0.3
    public float rotationSpeed = 30.0f; //Player rotation speed
    public float bounceForce = 5.0f;
    public float slightBounceForce = 3.5f;
    private Rigidbody carRB;
    public GameObject finishGoal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        carRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.gameStarted) return; // Stop movement before countdown ends
        carRB.AddForce(transform.forward * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Logic for adding the innovative collision mechanics of the game.
        if (collision.gameObject.CompareTag("Bouncy"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                carRB.AddForce(contact.normal * bounceForce, ForceMode.Impulse);
            }
        }
        else if (collision.gameObject.CompareTag("Slightly_Bouncy"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                carRB.AddForce(contact.normal * slightBounceForce, ForceMode.Impulse);
            }
        }
        else if (collision.gameObject.CompareTag("Player")) // if the other object is the player, bounce force should depend on the tag of the opponent car itself
        {
            if (gameObject.CompareTag("Bouncy"))
            {
                foreach (ContactPoint contact in collision.contacts)
                {
                    carRB.AddForce(contact.normal * bounceForce, ForceMode.Impulse);
                }
            }
            else if (gameObject.CompareTag("Slightly_Bouncy"))
            {
                foreach (ContactPoint contact in collision.contacts)
                {
                    carRB.AddForce(contact.normal * slightBounceForce, ForceMode.Impulse);
                }
            }
        }
    }
    
}
