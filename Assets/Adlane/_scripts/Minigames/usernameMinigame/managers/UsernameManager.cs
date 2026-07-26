using SmallHedge.SoundManager;
using System;
using System.Collections;
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

    [SerializeField] private TMP_Text TutorialText;
    [SerializeField] private float tutorialDisplayTime = 2f;
    [SerializeField] private float fadeSpeed = 2f;

    [Header("Rounds")]
    [SerializeField] private LetterRound[] rounds;
    [SerializeField] private LettersManager lettersManager;

    public InputField usernameField;

    public InputField nextInputField;

    public Panel mypanel;


    [Header("Health")]
    [SerializeField] private int maxHearts = 3;

    [Header("Random")]
    [SerializeField] private bool randomizeRoundOrder = true;

    private readonly List<int> roundOrder = new List<int>();
    private HashSet<char> allowedLetters = new HashSet<char>();

    private int currentRoundIndex = 0;
    private int heartsLeft;

    private string collectedLetters = "";
    private bool isTutorialActive = false;
    // Button to start the game, 3yetlha mena nano
    public void BeginGame()
    {
        heartsLeft = maxHearts;
        BuildRoundOrder();

        collectedLetters = "";
        BeginRound(0);
        CanvasManager.instance.SpawnHeart(heartsLeft);
    }


    private IEnumerator showTutorial()
    {
        if (TutorialText == null)
            yield break;

        TutorialText.gameObject.SetActive(true);

        Color c = TutorialText.color;
        c.a = 0f;
        TutorialText.color = c;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            c.a = Mathf.Lerp(0f, 1f, t);
            TutorialText.color = c;
            yield return null;
        }

        yield return new WaitForSeconds(tutorialDisplayTime);

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            c.a = Mathf.Lerp(1f, 0f, t);
            TutorialText.color = c;
            yield return null;
        }

        TutorialText.gameObject.SetActive(false);
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

        if (index == 0)
        {
            StartCoroutine(BeginRoundAfterTutorial(index));
        }
        else
        {
            SetupRound(index);
        }
    }
    private IEnumerator BeginRoundAfterTutorial(int index)
    {
        isTutorialActive = true;
        yield return StartCoroutine(showTutorial());
        isTutorialActive = false;

        SetupRound(index);

        if (lettersManager != null)
            lettersManager.StartSpawning();
    }
    private void SetupRound(int index)
    {
        allowedLetters.Clear();

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

        
    }
    public bool TryCollectLetter(string value)
    {
        if (isTutorialActive)
            return false;
        if (string.IsNullOrEmpty(value))
            return false;

        char c = char.ToUpperInvariant(value[0]);

        if (!allowedLetters.Contains(c))
        {
            TakeDamage();
            return false;
        }

        SoundManager.PlaySound(SoundType.type);
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
        CameraShake.instance.ShakeMedium();
        SoundManager.PlaySound(SoundType.error);
        CanvasManager.instance.UpdateheartUI();

        if (heartsLeft <= 0)
            LoseGame();
    }

   

    private void WinGame()
    {
        if (lettersManager != null)
            lettersManager.StopSpawning();
        CanvasManager.instance.DesactivateHearts();
        returnToMenu();

        Debug.Log("Username minigame complete!");
    }

    private void LoseGame()
    {
        if (lettersManager != null)
            lettersManager.StopSpawning();

        GameManager.instance.TriggerLose();

      
    }
    public void RestartGame()
    {
        if (lettersManager != null)
            lettersManager.StopSpawning();

        BeginGame();
    }

    // extend this function ela 7sab wch tes7a9 fel menu nano
    public void returnToMenu()
    {

        usernameField.inputText = collectedLetters;
        usernameField.displayText.text = collectedLetters;
        usernameField.displayText.alpha = 1f;
        nextInputField.gameObject.GetComponent<Button>().isActive = false;
        mypanel.PlayPopOutSequence();
    }

}