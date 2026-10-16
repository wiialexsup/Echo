using System.Collections;
using UnityEngine;

public class SonarReveal : MonoBehaviour
{
    [SerializeField] private float visibleTime = 1.1f;
    private SpriteRenderer[] renderers;
    private Coroutine hideRoutine;

    private void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>(true);
        SetVisible(false);
    }

    public void Reveal()
    {
        SetVisible(true);
        if (hideRoutine != null) StopCoroutine(hideRoutine);
        hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(visibleTime);
        SetVisible(false);
        hideRoutine = null;
    }

    private void SetVisible(bool visible)
    {
        if (renderers == null) return;
        foreach (SpriteRenderer sr in renderers)
            if (sr != null) sr.enabled = visible;
    }
}
