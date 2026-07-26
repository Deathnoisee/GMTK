using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class LettersManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UsernameManager usernameManager;
    [SerializeField] private GameObject letterPrefab;
    [SerializeField] private Transform lettersParent;
    [SerializeField] private BoxCollider2D spawnArea;

    [Header("Spawn Settings")]
    [SerializeField] private float minSpawnDelay = 0.6f;
    [SerializeField] private float maxSpawnDelay = 1.5f;
    [SerializeField, Range(0f, 1f)] private float validLetterChance = 0.75f;
    [SerializeField] private int maxAliveLetters = 5;
    [SerializeField] private float spawnAboveBoundsOffset = 0.5f;

    private List<GameObject> aliveLetters = new List<GameObject>();
    private Coroutine spawnRoutine;

    private void Start()
    {
        
    }

    public void StartSpawning()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        spawnRoutine = null;
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            CleanupDeadLetters();

            if (aliveLetters.Count < maxAliveLetters)
                SpawnOneLetter();

            float wait = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(wait);
        }
    }

    private void SpawnOneLetter()
    {
        if (usernameManager == null || letterPrefab == null || spawnArea == null)
            return;

        bool wantValid = Random.value < validLetterChance;
        if (!usernameManager.TryGetSpawnLetter(wantValid, out char letter))
            return;

        Bounds b = spawnArea.bounds;
        float x = Random.Range(b.min.x, b.max.x);
        float y = b.max.y + spawnAboveBoundsOffset;
        Vector3 spawnPos = new Vector3(x, y, 0f);

        GameObject obj = Instantiate(letterPrefab, spawnPos, Quaternion.identity, lettersParent);

        TMP_Text text = obj.GetComponentInChildren<TMP_Text>();
        if (text != null)
            text.text = letter.ToString();

        // Pop-in animation
        Vector3 targetScale = obj.transform.localScale;
        obj.transform.localScale = Vector3.zero;
        obj.transform.DOScale(targetScale, 0.3f).SetEase(Ease.OutBack);

        aliveLetters.Add(obj);
    }

    private void CleanupDeadLetters()
    {
        aliveLetters.RemoveAll(item => item == null);
    }
}
