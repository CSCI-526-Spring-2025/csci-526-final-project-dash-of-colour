using UnityEngine;

public class BouncyPlatform : MonoBehaviour
{
    private float targetYPosition1 = 5.22f;
    private float targetYPosition2 = 10.22f;
    public float moveDuration1 = 1f;
    public float moveDuration2 = 2f;

    public float bounceScale = 1.2f;
    public float bounceDuration = 0.3f;

    private Vector3 originalScale;
    public static bool isBouncing = false;

    void Start()
    {
        originalScale = transform.localScale;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Renderer rend = GetComponent<Renderer>();
            if (gameObject.tag == "Slightly_Bouncy")
            {
                StartCoroutine(BounceEffect());
                StartCoroutine(SmoothMoveToY(collision.gameObject, collision.gameObject.transform, targetYPosition1, moveDuration1));
            }
            else if (gameObject.tag == "Bouncy")
            {
                StartCoroutine(BounceEffect());
                StartCoroutine(SmoothMoveToY(collision.gameObject, collision.gameObject.transform, targetYPosition2, moveDuration2));
            }
        }
    }

    System.Collections.IEnumerator SmoothMoveToY(GameObject player, Transform target, float yPosition, float duration)
    {
        isBouncing = true;
        Vector3 startPos = target.position;
        Vector3 endPos = new Vector3(startPos.x - 1f, yPosition, startPos.z);
        float elapsed = 0f;
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero; // Cancel any existing motion
            rb.angularVelocity = Vector3.zero;
        }

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            target.position = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.position = endPos;

        // Restore gravity and allow rotation again (if needed)
        if (rb != null)
        {
            rb.useGravity = true;
            rb.freezeRotation = false; // Or keep it true if you always want rotation locked
        }
        
        isBouncing = false;
    }
    System.Collections.IEnumerator BounceEffect()
    {
        // isBouncing = true;
        Vector3 squashed = new Vector3(originalScale.x, originalScale.y * bounceScale, originalScale.z );
        transform.localScale = squashed;
        yield return new WaitForSeconds(bounceDuration);
        transform.localScale = originalScale;
        // isBouncing = false;
    }

}
