using UnityEngine;
using System.Collections;
public class BouncyPlatform : MonoBehaviour
{
    private float targetYPosition1 = -10f + 0.22f;
    private float targetYPosition2 = 10.22f;
    private float targetYPosition3 = 1.5f;
    public float moveDuration1 = 1f;
    public float moveDuration2 = 1f;
    public float moveDuration3 = 1f;

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
            if (gameObject.tag == "Not_Bouncy")
            {
                StartCoroutine(BounceEffect());
                StartCoroutine(SmoothMoveToY(collision.gameObject, collision.gameObject.transform, targetYPosition1, moveDuration1));
            }
            else if (gameObject.tag == "Bouncy")
            {
                StartCoroutine(BounceEffect());
                StartCoroutine(SmoothMoveToY(collision.gameObject, collision.gameObject.transform, targetYPosition2, moveDuration2));
            }
            else if (CompareTag("Slightly_Bouncy"))
            {
                StartCoroutine(BounceEffect());
                StartCoroutine(MoveUpAndDownY(collision.gameObject, collision.transform, targetYPosition3, 0.23f, moveDuration3));
            }
        }
    }

    IEnumerator SmoothMoveToY(GameObject player, Transform target, float yPosition, float duration)
    {
        isBouncing = true;
        Vector3 startPos = target.position;
        Vector3 endPos = new Vector3(startPos.x - 1.5f, yPosition, startPos.z);
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

    IEnumerator MoveUpAndDownY(GameObject player, Transform target, float peakY, float returnY, float duration)
    {
        isBouncing = true;
        Vector3 startPos = target.position;
        Vector3 peakPos = new Vector3(startPos.x - 3f, peakY, startPos.z);
        Vector3 endPos = new Vector3(startPos.x - 6f, returnY, startPos.z);
        float halfDuration = duration / 2f;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Move up
        float elapsed = 0f;
        while (elapsed < halfDuration)
        {
            float t = elapsed / halfDuration;
            target.position = Vector3.Lerp(startPos, peakPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.position = peakPos;

        // yield return new WaitForSeconds(0.2f);
        // Move down
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            float t = elapsed / halfDuration;
            target.position = Vector3.Lerp(peakPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.position = endPos;

        if (rb != null)
        {
            rb.useGravity = true;
            rb.freezeRotation = false;
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
