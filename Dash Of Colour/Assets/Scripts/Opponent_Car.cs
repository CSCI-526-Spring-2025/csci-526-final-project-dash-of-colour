using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Opponent_Car : MonoBehaviour
{
    public float speed = 4.5f; //Player linear speed    //Test value = 0.3
    public float rotationSpeed = 30.0f; //Player rotation speed
    public float bounceForce = 5.0f;
    public float slightBounceForce = 3.5f;
    public float detectionDistance = 2f;
    public float avoidStrength = 3f;
    public float avoidDuration = 1.0f;

    private Rigidbody carRB;
    public GameObject finalGoal;

    private bool isAvoiding = false;
    private float avoidTimer = 0f;
    private Vector3 lastAvoidDir = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        carRB = GetComponent<Rigidbody>();
        

    }

    // Update is called once per frame
    void Update()
    {

        if (LevelData.validLevels.Contains(SceneManager.GetActiveScene().name))
        
            if (!GameManager.instance.gameStarted) return; // Stop movement before countdown ends
                                                           //carRB.AddForce(transform.forward * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
            if (finalGoal == null) return;


            Vector3 toTarget = finalGoal.transform.position - transform.position;
            toTarget.y = 0;
            Vector3 moveDir = toTarget.normalized;
            RaycastHit hit;
            
            Vector3 finalDir = moveDir;


            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out hit, detectionDistance))
            {
                if (!hit.collider.isTrigger && (hit.collider.CompareTag("Slightly_Bouncy") || hit.collider.CompareTag("Bouncy")))
                {
                Vector3 avoidDir = Vector3.Cross(Vector3.up, transform.forward).normalized * avoidStrength;
                lastAvoidDir = avoidDir;
                isAvoiding = true;
                avoidTimer = avoidDuration;
                }
            }

            if (isAvoiding)
            {
                avoidTimer -= Time.deltaTime;
                if (avoidTimer <= 0f)
                {
                    isAvoiding = false;
                    lastAvoidDir = Vector3.zero;
                }

                finalDir = (moveDir + lastAvoidDir).normalized;
            }


        if (finalDir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(finalDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }


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
