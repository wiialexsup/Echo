using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sonar : MonoBehaviour
{
    [SerializeField] private float maxRadius = 0.05f;
    [SerializeField] private float waveDuration = 0.2f;
    [SerializeField] private KeyCode sonarKey = KeyCode.E;
    [SerializeField] private LayerMask revealLayer;
    [SerializeField] private AudioClip sonarClip;

    private AudioSource audioSource;
    private LineRenderer wave;
    private bool activePulse;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;

        GameObject waveObject = new GameObject("SonarWave");
        waveObject.transform.SetParent(transform, false);
        wave = waveObject.AddComponent<LineRenderer>();
        wave.useWorldSpace = true;
        wave.loop = true;
        wave.positionCount = 65;
        wave.startWidth = 0.055f;
        wave.endWidth = 0.055f;
        wave.material = new Material(Shader.Find("Sprites/Default"));
        wave.startColor = new Color(0.2f, 0.9f, 1f, 0.9f);
        wave.endColor = new Color(0.2f, 0.9f, 1f, 0.9f);
        wave.sortingOrder = 20;
        wave.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(sonarKey) && !activePulse)
            StartCoroutine(EmitPulse());
    }

    private IEnumerator EmitPulse()
    {
        activePulse = true;
        wave.enabled = true;
        if (sonarClip != null) audioSource.PlayOneShot(sonarClip);
        float elapsed = 0f;
        while (elapsed < waveDuration)
        {
            elapsed += Time.deltaTime;
            float radius = maxRadius * Mathf.Clamp01(elapsed / waveDuration);
            DrawCircle(radius);
            RevealObjects(radius);
            yield return null;
        }
        wave.enabled = false;
        activePulse = false;
    }

    private void DrawCircle(float radius)
    {
        Vector3 center = transform.position;
        for (int i = 0; i < wave.positionCount; i++)
        {
            float angle = i * Mathf.PI * 2f / (wave.positionCount - 1);
            wave.SetPosition(i, center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f));
        }
    }

    private void RevealObjects(float radius)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, revealLayer);
        foreach (Collider2D hit in hits)
        {
            SonarReveal reveal = hit.GetComponentInParent<SonarReveal>();
            if (reveal != null) reveal.Reveal();
        }
    }
}
