using System.Collections;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    [SerializeField] private namousManager namousManager;

    [Header("Spawn Timing")]
    [SerializeField] private float spawnDuration = 30f;
    [SerializeField] private float minSpawnInterval = 0.25f;
    [SerializeField] private float maxSpawnInterval = 2f;

    [Header("Burst Size")]
    [SerializeField] private int minBurstCount = 2;
    [SerializeField] private int maxBurstCount = 5;

    [Header("Curve")]
    [SerializeField] private AnimationCurve spawnRateCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private bool gameStarted = false;

    public void startMinigame()
    {
        if (gameStarted) return;
        gameStarted = true;
        Debug.Log("Starting spawn loop");
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        Debug.Log("Starting spawn loop inside coroutine");
        float elapsed = 0f;

        while (namousManager != null && namousManager.RemainingNamous > 0)
        {
            float t = Mathf.Clamp01(elapsed / Mathf.Max(0.01f, spawnDuration));
            float curveValue = Mathf.Clamp01(spawnRateCurve.Evaluate(t));

            int burstCount = Mathf.Clamp(
                Mathf.RoundToInt(Mathf.Lerp(minBurstCount, maxBurstCount, curveValue)),
                1,
                maxBurstCount
            );

            float waitTime = Mathf.Lerp(maxSpawnInterval, minSpawnInterval, curveValue);

            for (int i = 0; i < burstCount && namousManager.RemainingNamous > 0; i++)
            {
                namousManager.TryGetNamous();
            }

            yield return new WaitForSeconds(waitTime);
            elapsed += waitTime;
        }
        gameStarted = false;
    }
}