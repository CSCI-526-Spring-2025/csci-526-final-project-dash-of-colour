using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class AINavCar : MonoBehaviour
{
    public float speed = 4.5f; //Player linear speed    //Test value = 0.3
    public float rotationSpeed = 30.0f; //Player rotation speed
    public float bounceForce = 5.0f;
    public float slightBounceForce = 3.5f;
    private Rigidbody carRB;
    public GameObject finishGoal;

    private NavMeshAgent agent; // 新增：导航代理组件引用

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        carRB = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>(); // 新增：获取 NavMeshAgent

        if (finishGoal != null) // 新增：设置导航目标点
        {
            agent.SetDestination(finishGoal.transform.position);
        }

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (LevelData.validLevels.Contains(SceneManager.GetActiveScene().name))
            if (!GameManager.instance.gameStarted) return; // Stop movement before countdown ends
        carRB.AddForce(transform.forward * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);*/
        if (LevelData.validLevels.Contains(SceneManager.GetActiveScene().name))
        {
            if (!GameManager.instance.gameStarted)
            {
                agent.isStopped = true; // 新增：游戏未开始时暂停寻路
                return;
            }
            else
            {
                agent.isStopped = false; // 新增：游戏开始恢复导航

                // 新增：不断刷新目标位置（如果终点是动态的）
                if (finishGoal != null)
                    agent.SetDestination(finishGoal.transform.position);
            }
        }
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
