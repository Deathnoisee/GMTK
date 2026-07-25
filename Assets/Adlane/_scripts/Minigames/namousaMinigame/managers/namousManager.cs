using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class namousManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject namousObject;
    [SerializeField] private GameObject AttackObject;
    [SerializeField] private List<GameObject> spawnAreas = new List<GameObject>();
    [SerializeField] private Transform namousParent;

    [Header("Pool Settings")]
    [SerializeField] private int poolSize = 5;

    [Header("Namous Settings")]
    [SerializeField] private int namousTotal = 20;

    private Stack<GameObject> namousPool = new Stack<GameObject>();

    public int RemainingNamous => namousTotal;

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        if (namousObject == null) return;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(namousObject, GetRandomPointInBounds(), Quaternion.identity, namousParent);
            obj.SetActive(false);
            obj.GetComponent<namousa>().namousManager = this;
            obj.GetComponent<namousa>().AttackObject = AttackObject;
            namousPool.Push(obj);
        }
    }

    public bool TryGetNamous()
    {
        if (namousTotal <= 0)
            return false;

        if (namousPool.Count <= 0)
        {
            Debug.LogWarning("Namous pool is empty!");
            return false;
        }

        namousTotal--;

        GameObject obj = namousPool.Pop();
        obj.GetComponent<namousa>().health = 10f;
        obj.transform.position = GetRandomPointInBounds();
        obj.SetActive(true);
        return true;
    }

    public void ReleaseNamous(GameObject obj)
    {
        if (obj == null) return;
        obj.SetActive(false);
        namousPool.Push(obj);
    }

    private Vector2 GetRandomPointInBounds()
    {
        if (spawnAreas.Count == 0) return Vector2.zero;

        int randomIndex = Random.Range(0, spawnAreas.Count);
        GameObject selectedArea = spawnAreas[randomIndex];
        BoxCollider2D box = selectedArea.GetComponent<BoxCollider2D>();

        float x = Random.Range(box.bounds.min.x, box.bounds.max.x);
        float y = Random.Range(box.bounds.min.y, box.bounds.max.y);
        return new Vector2(x, y);
    }

    // Extend this method nano to handle win condition when all namous are defeated
    private void win()
    {
        Debug.Log("You win!");
    }
}
