using SmallHedge.SoundManager;
using TMPro;
using UnityEngine;

public class textManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private TMP_Text collectedText;
    [SerializeField] private InputField customInputField;
    public int health = 3;

    [Header("Prompt")]
    [TextArea]
    [SerializeField] private string prompt;

    [Header("Win Flow")]
    public Panel myPanel;        // pops out when this minigame is completed
    public GameObject nextLevel; // activated once myPanel finishes popping out

    private string lastCheckedText = "";
    public string[] targetWords;
    public string[] typedWords;

    private bool isDead = false;
    private bool completed = false;

    private void Start()
    {
        completed=false;
        startGame();
    }

    public void startGame()
    {
        isDead = false;
        completed = false;

        if (customInputField != null)
            customInputField.gameObject.SetActive(true);
        if (collectedText != null)
            collectedText.text = "";
        if (promptText != null)
            promptText.text = prompt;

        lastCheckedText = "";

        CanvasManager.instance.SpawnHeart(health);
    }

    public void SetPrompt(string newPrompt)
    {
        prompt = newPrompt;
        if (promptText != null)
            promptText.text = prompt;
    }

    private void Update()
    {
        if (isDead || completed) return;
        if (customInputField == null || customInputField.displayText == null)
            return;

        string typedText = customInputField.displayText.text;
        typedText = typedText.Replace("_", "").Trim();

        if (collectedText != null)
            collectedText.text = typedText;

        if (typedText == lastCheckedText)
            return;

       
        bool textShrank = typedText.Length < lastCheckedText.Length;
        lastCheckedText = typedText;

        if (textShrank)
            return;

        CompareText(typedText);
    }

    private void CompareText(string typedText)
    {
        if (typedText.Length > prompt.Length)
        {
            damage();
            return;
        }

        for (int i = 0; i < typedText.Length; i++)
        {
            if (typedText[i] != prompt[i])
            {
                damage();
                return;
            }
        }

        if (typedText == prompt)
        {
            Win();
        }
    }

    private void damage()
    {
        if (isDead) return;

        CanvasManager.instance.UpdateheartUI();
        SoundManager.PlaySound(SoundType.error);
        health--;

        if (CameraShake.instance != null)
            CameraShake.instance.ShakeMedium();

        Debug.Log("Mistake made");

        if (health <= 0)
        {
            isDead = true;
            Debug.Log("Game Over");

            if (customInputField != null)
                customInputField.gameObject.SetActive(false);

            GameManager.instance.TriggerLose();
        }
    }

    private void Win()
    {
        if (completed) return;
        completed = true;
        SoundManager.PlaySound(SoundType.right);

        Debug.Log("You are a human!");

        if (customInputField != null)
            customInputField.gameObject.SetActive(false);

        CanvasManager.instance.DesactivateHearts();

        myPanel.PlayPopOutSequence(() =>
        {
            if (nextLevel != null)
            {
                nextLevel.SetActive(true);
            }
            else
            {

                GameManager.instance.TriggerWin();
            }

        });
    }
}