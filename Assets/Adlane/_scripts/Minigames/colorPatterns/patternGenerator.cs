using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class patternGenerator : MonoBehaviour
{
    [Header("Ring Segments")]
    [SerializeField] private SpriteRenderer[] ringSegments; // 0 red 1 green 2 blue 3 yellow
    private List<int> patternSequence = new List<int>();
    [Header("Flash Options")]
    [SerializeField] private Color flashColor;
    [SerializeField] private float flashDuration;
    [Header("other Options")]
    [SerializeField] private int maxRounds = 5;
    [SerializeField] private TMP_Text ButtonText;
    private int currentRound = 0;
    private int currentInputIndex = 0;
    public bool canClick = false;

    // Hnaya tebda Nano
    public void StartPattern()
    {
        currentRound = 0;
        patternSequence.Clear();
        canClick = false;
        ButtonText.text = "Restart?";
        NextPattern();
    }
    private void NextPattern()
    {
        currentRound++;
        if (currentRound > maxRounds)
        {
            WinGame();
            return;
        }

        currentInputIndex = 0;
        patternSequence.Add(Random.Range(0, ringSegments.Length));
        StartCoroutine(FlashPattern());
    }
    private IEnumerator FlashPattern()
    {
        canClick = false;
        foreach (int index in patternSequence)
        {
            StartCoroutine(FlashSegment(index));
            yield return new WaitForSeconds(flashDuration);
            Debug.Log("Flashed segment: " + index);
        }
        canClick = true;
    }

    private IEnumerator FlashSegment(int index, bool isInput = false)
    {
        Color originalColor = ringSegments[index].color;

        float halfTime = flashDuration * 0.5f;

        float elapsed = 0f;

        if (isInput)
        {
            halfTime *= 0.15f; // Reduce the flash duration for input flashes
        }
        while (elapsed < halfTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfTime;
            ringSegments[index].color = Color.Lerp(originalColor, flashColor, t);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < halfTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfTime;
            ringSegments[index].color = Color.Lerp(flashColor, originalColor, t);
            yield return null;
        }

        ringSegments[index].color = originalColor;
        if (isInput)
        {
            yield return new WaitForSeconds(0.4f);
        }
    }
    public void CheckInput(int inputIndex)
    {
        if (!canClick) return;

        if (inputIndex != patternSequence[currentInputIndex])
        {
            Debug.Log("Wrong input! Game Over.");
            canClick = false;
            ButtonText.text = "Restart?";
            return;
        }

        currentInputIndex++;
        if (currentInputIndex >= patternSequence.Count)
        {
            canClick = false;
            Debug.Log("Correct sequence! Proceeding to next round.");
            NextPattern();
        }

    }


    public void Click(int segmentIndex)
    {
        if (!canClick) return;
        StartCoroutine(ClickAndCheck(segmentIndex));
    }

    private IEnumerator ClickAndCheck(int segmentIndex)
    {
        yield return FlashSegment(segmentIndex, isInput: true);
        CheckInput(segmentIndex);
    }
    // Hawlik l output from this minigame nano
    public void WinGame()
    {
        Debug.Log("You won the game!");
        ButtonText.text = "You Won!";
        canClick = false;
    }
}
