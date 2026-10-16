
using UnityEngine;

public class PlatformPassenger : MonoBehaviour
{
    private Transform originalParent;
    private Transform currentPlatform;

    private void Awake()
    {
        originalParent = transform.parent;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AttachToPlatform(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        AttachToPlatform(collision);
    }

    private void AttachToPlatform(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("MovingPlatform"))
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
            {
                if (currentPlatform != collision.transform)
                {
                    currentPlatform = collision.transform;
                    transform.SetParent(currentPlatform, true);
                }

                return;
            }
        }
    }

    public void Detach()
    {
        if (currentPlatform == null)
            return;

        transform.SetParent(originalParent, true);
        currentPlatform = null;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (currentPlatform == collision.transform)
            Detach();
    }
}
