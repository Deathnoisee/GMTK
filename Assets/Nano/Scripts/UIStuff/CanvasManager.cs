using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;
    public GameObject heart;

    public GameObject HearthGroup;

    public TextMeshProUGUI timerText;

    [Header("Used Letter Tracking (Canvas UI)")]
    public GameObject usedLettersGroup;     // single GameObject with Horizontal/Grid Layout Group
    public GameObject absentTilePrefab;     // gray tile prefab, with a TextMeshProUGUI child
    public GameObject presentTilePrefab;    // yellow tile prefab, with a TextMeshProUGUI child

    private HashSet<char> shownAbsentLetters = new HashSet<char>();
    private HashSet<char> shownPresentLetters = new HashSet<char>();

    public void UpdateheartUI()
    {
        int childCount = HearthGroup.transform.childCount;
        int lastIndex = childCount - 1;

        if (lastIndex < 0)
        {
            Debug.Log("No hearts left to remove");
            return;
        }

        HearthGroup.transform.GetChild(lastIndex).gameObject.GetComponent<Heart>().ChangeSprite();
    }

    public void GameOverPanel()
    {
        Debug.Log("Game Over! Show Game Over Panel.");
    }

    public void RestartGame()
    {
        Debug.Log("Restarting Game...");
    }

    public void DesactivateHearts()
    {
        for (int i = HearthGroup.transform.childCount - 1; i >= 0; i--)
        {
            Debug.Log("destroyed heart");
            Destroy(HearthGroup.transform.GetChild(i).gameObject);
        }
    }

    public void UpdateTimerUI(float time)
    {
        if (time < 0) time = 0;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }

    public void SpawnHeart(int health)
    {
        for (int i = 0; i < health; i++)
        {
            GameObject newHeart = Instantiate(heart, HearthGroup.transform);
            newHeart.transform.SetSiblingIndex(0);
            newHeart.SetActive(true);
        }
    }

    // ---- Used letter tracking (single group, different prefab per state) ----

    public void AddAbsentLetter(char letter)
    {
        if (shownAbsentLetters.Contains(letter)) return;
        shownAbsentLetters.Add(letter);
        SpawnLetterTile(letter, absentTilePrefab);
    }

    public void AddPresentLetter(char letter)
    {
        if (shownPresentLetters.Contains(letter)) return;
        shownPresentLetters.Add(letter);
        SpawnLetterTile(letter, presentTilePrefab);
    }

    private void SpawnLetterTile(char letter, GameObject prefab)
    {
        GameObject tile = Instantiate(prefab, usedLettersGroup.transform);
        TextMeshProUGUI text = tile.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = letter.ToString();
        }
        tile.SetActive(true);
    }

    public void ClearUsedLetters()
    {
        shownAbsentLetters.Clear();
        shownPresentLetters.Clear();

        for (int i = usedLettersGroup.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(usedLettersGroup.transform.GetChild(i).gameObject);
        }
    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {

    }
}