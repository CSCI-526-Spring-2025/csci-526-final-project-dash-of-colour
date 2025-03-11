using UnityEngine;

public class ObstaclesMoving : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.back;
    public float speed = 2.0f;
    public float moveDistance = 2.4f;
    private Vector3 startPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.gameStarted) return;
        float pingPongValue = Mathf.PingPong(Time.time * speed, moveDistance * 2) - moveDistance;
        transform.position = startPosition + moveDirection.normalized * pingPongValue;
    }
}
