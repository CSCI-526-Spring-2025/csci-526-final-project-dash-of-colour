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

    // private void UpdateCheckPoint() {
    //     CheckPointData.currCheckPoint = new Vector3(transform.position.x, transform.position.y, 0f);
    // }
    private void UpdateCheckPoint()
    {
        Vector3 newCheckpoint = new Vector3(transform.position.x, transform.position.y, 0f);
        CheckPointData.currCheckPoint = newCheckpoint;
        
        // Increment checkpoint usage in the static dictionary
        if (CheckPointData.checkpointUsage.ContainsKey(newCheckpoint))
        {
            CheckPointData.checkpointUsage[newCheckpoint]++;
        }
        else
        {
            CheckPointData.checkpointUsage[newCheckpoint] = 1;
        }
        
        Debug.Log($"Checkpoint updated to: {newCheckpoint}. Used {CheckPointData.checkpointUsage[newCheckpoint]} times.");
    }
}
