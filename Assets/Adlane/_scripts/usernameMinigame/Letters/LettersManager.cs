using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LettersManager : MonoBehaviour
{
    [Header("Letter Settings")]
    [SerializeField] private List<GameObject> lettersPrefab = new List<GameObject>();

    [Header("Spawning Settings")]
    [SerializeField] private BoxCollider2D spawnArea;
    [SerializeField] private float spawnInterval = 2.5f;
    [SerializeField] private int maxAlive = 5;
    public bool spawnLetters = true;

    private float spawnTimer = 0f;

    private void Start()
    {
        spawnTimer = 0f;
    }

    private void Update()
    {
        if (!spawnLetters || lettersPrefab.Count == 0 || spawnArea == null)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            int currentAlive = GameObject.FindGameObjectsWithTag("Letter").Length;
            if (currentAlive < maxAlive)
            {
                SpawnLetter();
            }
            spawnTimer = 0f;
        }
    }

    private void SpawnLetter()
    {
        if (lettersPrefab.Count == 0 || spawnArea == null)
            return;

        GameObject prefab = lettersPrefab[Random.Range(0, lettersPrefab.Count)];
        Vector3 spawnPosition = GetRandomPointInBox(spawnArea);
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        Instantiate(prefab, spawnPosition, randomRotation);
    }

    private Vector3 GetRandomPointInBox(BoxCollider2D box)
    {
        Vector2 localCenter = box.offset;
        Vector2 halfSize = box.size * 0.5f;

        float x = Random.Range(-halfSize.x, halfSize.x);
        float y = Random.Range(-halfSize.y, halfSize.y);

        Vector3 localPoint = new Vector3(localCenter.x + x, localCenter.y + y, 0f);
        return box.transform.TransformPoint(localPoint);
    }


}
