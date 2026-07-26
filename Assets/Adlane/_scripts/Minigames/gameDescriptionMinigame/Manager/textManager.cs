using System;
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
    private string lastCheckedText = "";
    public string[] targetWords;
    public string[] typedWords;

    // nano
    public void startGame()
    {
        if (customInputField != null)
            customInputField.gameObject.SetActive(true);

        if (collectedText != null)
            collectedText.text = "";

        if (promptText != null)
            promptText.text = prompt;

        lastCheckedText = "";
    }
    public void SetPrompt(string newPrompt)
    {
        prompt = newPrompt;
        if (promptText != null)
            promptText.text = prompt;
    }

    private void Update()
    {
        if (customInputField == null || customInputField.displayText == null)
            return;

        string typedText = customInputField.displayText.text;
        typedText = typedText.Replace("_", "").Trim();

        if (collectedText != null)
            collectedText.text = typedText;

        if (typedText == lastCheckedText)
            return;

        lastCheckedText = typedText;
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
        if (health <= 0)
        {
            Debug.Log("Game Over");
            customInputField.gameObject.SetActive(false);
            return;
        }
        Debug.Log("Mistake made");
        health--;
    }
    // ak taerff lfilm nano
    private void Win()
    {
        if (health <= 0)
        {
            Debug.Log("Game Over");
            return;
        }
        Debug.Log("You are a human!");
    }
}