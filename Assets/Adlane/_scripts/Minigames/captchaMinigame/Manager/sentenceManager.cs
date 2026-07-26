using TMPro;
using UnityEngine;

public class sentenceManager : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private TMP_Text sentenceText;

    [Header("Levels")]
    [SerializeField] private SentenceLevel[] levels;

    private int currentLevel = 0;

    private void Start()
    {
        BeginLevel(0);
    }
    public void StartGame()
    {
        currentLevel = 0;
        BeginLevel(currentLevel);
    }

    public void BeginLevel(int level)
    {
        currentLevel = level;

        if (levels == null || levels.Length == 0 || currentLevel < 0 || currentLevel >= levels.Length)
            return;

        if (sentenceText != null)
            sentenceText.text = levels[currentLevel].prompt;

    }

    public bool IsCurrentAnswerYes()
    {
        if (levels == null || currentLevel < 0 || currentLevel >= levels.Length)
            return false;

        return levels[currentLevel].correctIsYes;
    }

    public void NextLevel()
    {
        currentLevel++;

        if (currentLevel < levels.Length)
            BeginLevel(currentLevel);
        else
            Win();
    }

    private void Win()
    {
        sentenceText.text = "You are a human!";
    }
}
[System.Serializable]
public class SentenceLevel
{
    [TextArea] public string prompt;
    public bool correctIsYes;
}
