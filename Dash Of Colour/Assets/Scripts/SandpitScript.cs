using UnityEngine;

public class SandpitScript : MonoBehaviour
{
    public float slowdownFactor = 0.2f; 

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.SetSpeedMultiplier(slowdownFactor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.SetSpeedMultiplier(1f); // Reset to normal speed
        }
    }
}
