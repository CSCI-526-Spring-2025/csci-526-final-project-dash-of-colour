using UnityEngine;

public class ObstaclesMovingPro : MonoBehaviour
{
    public float speed = 3f;
    public Vector3[] directions = { Vector3.forward, Vector3.left, Vector3.back, Vector3.right };
    private int currentDirectionIndex = 0;
    public float distance = 1f; 
    private float movedDistance = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.Translate(directions[currentDirectionIndex] * step);
        movedDistance += step;
        if (movedDistance >= distance)
        {
            movedDistance = 0f;
            currentDirectionIndex = (currentDirectionIndex + 1) % directions.Length;
        }
    }
}
