using SmallHedge.SoundManager;
using TMPro;
using UnityEngine;

public class sentenceManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text sentenceText;

    [Header("Levels")]
    [SerializeField] private SentenceLevel[] levels;

    [Header("Win Flow")]
    public Panel myPanel;        // pops out when this minigame is completed
    public GameObject nextLevel; // activated once myPanel finishes popping out

    private int currentLevel = 0;
    private bool isDead = false;
    private bool completed = false;

    private void Start()
    {
        BeginLevel(0);
    }

    public void StartGame()
    {
        currentLevel = 0;
        isDead = false;
        completed = false;
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

    public void AnswerSelected(bool userSaidYes)
    {
        if (isDead || completed) return;

        bool correct = (userSaidYes == IsCurrentAnswerYes());

        if (correct)
        {
            SoundManager.PlaySound(sound: SoundType.right);
            NextLevel();
        }
        else
        {
            SoundManager.PlaySound(sound: SoundType.error);
            Lose();
        }
    }

    public void NextLevel()
    {
        currentLevel++;
        if (currentLevel < levels.Length)
            BeginLevel(currentLevel);
        else
            Win();
    }

    private void Lose()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Wrong answer — Game Over");

        if (CameraShake.instance != null)
            CameraShake.instance.ShakeSmall();

        GameManager.instance.TriggerLose();
    }

    private void Win()
    {
        if (completed) return;
        completed = true;

        Debug.Log("Win() called");

        if (sentenceText != null)
            sentenceText.text = "You are a human!";

        if (myPanel == null)
        {
            Debug.LogWarning("sentenceManager: myPanel is not assigned! Skipping pop-out and triggering win directly.");
            FinishWin();
            return;
        }

        myPanel.PlayPopOutSequence(() =>
        {
            Debug.Log("PlayPopOutSequence callback fired");
            FinishWin();
        });
    }

    private void FinishWin()
    {
        if (nextLevel != null)
        {
            nextLevel.SetActive(true);
        }

        Debug.Log("Calling GameManager.instance.TriggerWin()");
        GameManager.instance.TriggerWin();
    }
}

[System.Serializable]
public class SentenceLevel
{
    [TextArea] public string prompt;
    public bool correctIsYes;
}