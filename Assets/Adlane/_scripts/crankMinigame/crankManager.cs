using System.Collections;
using UnityEngine;

public class crankManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject crankedObject;

    [Header("Crank Settings")]
    [SerializeField] private int crankCount = 20;
    [SerializeField] private float crankDuration = 0.15f;
    [SerializeField] private float crankSpinDegrees = 360f;

    [Header("Load Bar Settings")]
    [SerializeField] private float loadPerCrank = 0.05f;
    [SerializeField] private Vector3 loadBarGrowAxis = new Vector3(0f, 1f, 0f);
    [SerializeField] private float maxLoadProgress = 1f;

    private int currentCrankCount = 0;
    private bool canCrank = true;

    private Vector3 loadBarBaseScale;
    private float loadProgress = 0f;

    private void Awake()
    {
        if (crankedObject != null) loadBarBaseScale = crankedObject.transform.localScale;
    }

    public void crank(GameObject crankSprite)
    {
        if (!canCrank || crankSprite == null) return;
        StartCoroutine(RotateCrank(crankSprite.transform));
    }

    private IEnumerator RotateCrank(Transform crankTransform)
    {
        canCrank = false;
        currentCrankCount++;

        float startAngle = crankTransform.localEulerAngles.z;
        float elapsed = 0f;

        while (elapsed < crankDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / crankDuration);

            // Ease with overshoot, then settle
            float easedT = EaseOutBack(t, 1.35f);

            // Use an unwrapped angle so a full spin is visible
            float angle = startAngle - (crankSpinDegrees * easedT);
            crankTransform.localRotation = Quaternion.Euler(0f, 0f, angle);

            // Fill the bar based on crank speed
            loadProgress = Mathf.Clamp(loadProgress + (loadPerCrank * Time.deltaTime / crankDuration), 0f, maxLoadProgress);
            ApplyLoadBar(loadProgress / maxLoadProgress);

            yield return null;
        }

        crankTransform.localRotation = Quaternion.Euler(0f, 0f, startAngle - crankSpinDegrees);
        canCrank = true;

        if (currentCrankCount >= crankCount)
        {
            Debug.Log("Crank complete!");
            var sr = crankTransform.GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = Color.green;
            canCrank = false;
            yield return null;
        }

    }

    private void ApplyLoadBar(float normalizedValue)
    {
        if (crankedObject == null) return;

        Vector3 added = loadBarGrowAxis.normalized * normalizedValue;
        crankedObject.transform.localScale = loadBarBaseScale + added;
    }

    private float EaseOutBack(float t, float overshoot)
    {
        t -= 1f;
        return 1f + (t * t * ((overshoot + 1f) * t + overshoot));
    }
}
