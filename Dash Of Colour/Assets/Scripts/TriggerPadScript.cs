using System;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerPadScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 moveDir = Vector3.right;
    public float moveSpeed = 8.0f;
    private float moveLimit = 0.0f;
    private float movedDist = 0.0f;
    public GameObject block;
    bool triggered = false; //Has the pad been triggered or not
    Vector3 initialPos;
    void Start()
    {
        moveLimit = this.transform.localScale.z;
       
        initialPos = block.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(triggered)
        {
            Vector3 curPosition = block.gameObject.transform.position;
            movedDist += moveSpeed * Time.deltaTime * moveDir.x;
            

            Vector3 dist = moveSpeed * Time.deltaTime * moveDir;
            block.gameObject.transform.Translate(dist);
            Debug.Log("Current moveLimit: " + moveLimit);

            if (movedDist >= moveLimit)
            {
                moveDir *= -1;
                
            }
            if (movedDist < 0.0)
            {
                block.gameObject.transform.position = initialPos;
                moveDir *= -1;
                movedDist = 0.0f;
                triggered = false;
               
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name=="Player_Car"|| other.name == "Car_Opp_1")
        {
            triggered = true;
        }
    }
}
