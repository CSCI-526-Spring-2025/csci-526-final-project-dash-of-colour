using UnityEngine;

public class CheckPointUpdater : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            UpdateCheckPoint();
        }
    }

    private void UpdateCheckPoint() {
        CheckPointData.currCheckPoint = new Vector3(transform.position.x, transform.position.y, 0f);
    }
}
