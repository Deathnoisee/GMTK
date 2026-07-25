using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class LetterRound
{
    [TextArea] public string prompt;
    public string allowedLetters;
}
public class UsernameManager : MonoBehaviour
{


    [Header("UI")]
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private TMP_Text collectedText;
    [SerializeField] private TMP_Text heartsText;

    [Header("Rounds")]
    [SerializeField] private LetterRound[] rounds;

    [Header("Health")]
    [SerializeField] private int maxHearts = 3;

    [Header("Random")]
    [SerializeField] private bool randomizeRoundOrder = true;

    private readonly List<int> roundOrder = new List<int>();
    private HashSet<char> allowedLetters = new HashSet<char>();

    private int currentRoundIndex = 0;
    private int heartsLeft;
    private int correctLettersThisRound = 0;

    private string collectedLetters = "";

    private void Start()
    {
        BeginGame();
    }

    public void BeginGame()
    {
        heartsLeft = maxHearts;
        BuildRoundOrder();
        collectedLetters = "";
        BeginRound(0);
        UpdateHeartsText();
    }

    private void BuildRoundOrder()
    {
        roundOrder.Clear();

        for (int i = 0; i < rounds.Length; i++)
            roundOrder.Add(i);

        if (!randomizeRoundOrder)
            return;

        for (int i = 0; i < roundOrder.Count; i++)
        {
            int swapIndex = UnityEngine.Random.Range(i, roundOrder.Count);
            (roundOrder[i], roundOrder[swapIndex]) = (roundOrder[swapIndex], roundOrder[i]);
        }
    }

    public void BeginRound(int index)
    {
        if (rounds == null || rounds.Length == 0)
            return;

        if (index < 0 || index >= roundOrder.Count)
        {
            WinGame();
            return;
        }

        currentRoundIndex = index;
        allowedLetters.Clear();
        correctLettersThisRound = 0;

        LetterRound round = rounds[roundOrder[currentRoundIndex]];
        string allowed = round.allowedLetters.ToUpperInvariant();

        foreach (char c in allowed)
        {
            if (!char.IsWhiteSpace(c))
                allowedLetters.Add(c);
        }

        if (promptText != null)
            promptText.text = round.prompt;

        if (collectedText != null)
            collectedText.text = collectedLetters;

        UpdateHeartsText();
    }

    public bool TryCollectLetter(string value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        char c = char.ToUpperInvariant(value[0]);

        if (!allowedLetters.Contains(c))
        {
            TakeDamage();
            return false;
        }

        collectedLetters += c;
        if (collectedText != null)
            collectedText.text = collectedLetters;

        NextRound();

        return true;
    }

    public bool TryGetSpawnLetter(bool wantValid, out char letter)
    {
        letter = default;

        if (rounds == null || roundOrder.Count == 0 || currentRoundIndex < 0 || currentRoundIndex >= roundOrder.Count)
            return false;

        HashSet<char> allowed = BuildAllowedSet();

        if (wantValid)
        {
            if (allowed.Count == 0)
                return false;

            letter = PickRandomFromSet(allowed);
            return true;
        }

        List<char> badLetters = new List<char>();
        for (char c = 'A'; c <= 'Z'; c++)
        {
            if (!allowed.Contains(c))
                badLetters.Add(c);
        }

        if (badLetters.Count == 0)
            return false;

        letter = badLetters[UnityEngine.Random.Range(0, badLetters.Count)];
        return true;
    }

    private HashSet<char> BuildAllowedSet()
    {
        HashSet<char> set = new HashSet<char>();

        LetterRound round = rounds[roundOrder[currentRoundIndex]];
        string allowed = round.allowedLetters.ToUpperInvariant();

        foreach (char c in allowed)
        {
            if (!char.IsWhiteSpace(c))
                set.Add(c);
        }

        return set;
    }

    private char PickRandomFromSet(HashSet<char> set)
    {
        int index = UnityEngine.Random.Range(0, set.Count);
        int i = 0;

        foreach (char c in set)
        {
            if (i == index)
                return c;
            i++;
        }

        return default;
    }

    private void NextRound()
    {
        BeginRound(currentRoundIndex + 1);
    }

    private void TakeDamage()
    {
        heartsLeft--;
        UpdateHeartsText();

        if (heartsLeft <= 0)
            LoseGame();
    }

    private void UpdateHeartsText()
    {
        if (heartsText != null)
            heartsText.text = $"Hearts: {heartsLeft}/{maxHearts}";
    }

    private void WinGame()
    {
        Debug.Log("Username minigame complete!");
    }

    private void LoseGame()
    {
        if (collectedText != null)
            collectedText.text = "You are Dead Restart the game";
    }
}