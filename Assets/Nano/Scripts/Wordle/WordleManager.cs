using SmallHedge.SoundManager;
using UnityEngine;

public class WordleManager : MonoBehaviour
{
    [System.Serializable]
    public class WordStage
    {
        public Transform wordParent; // e.g. Word1, Word2, Word3 GameObject
        public string[] wordList;
        [HideInInspector] public WordlTile[] tiles;
        [HideInInspector] public int wordLength;
    }

    public WordStage[] stages; // exactly 3 entries: Word1, Word2, Word3
    public int attemptsPerStage = 5;

    public string targetWord;
    public string finalResult = "";

    private int currentStageIndex = 0;
    private int currentAttempt = 0;
    private bool[] lockedLetters;
    private bool waitingForNewGuess = false;
    private bool gameOver = false;

    


    public Panel panel;
    public InputField emailInput;
    public InputField nextInput;

    private WordlTile[] currentTiles;
    private int currentWordLength;

    

    void Start()
    {
        // Cache each stage's tiles from its own parent, and deactivate all parents initially
        foreach (WordStage stage in stages)
        {
            stage.tiles = stage.wordParent.GetComponentsInChildren<WordlTile>(true);
            stage.wordLength = stage.tiles.Length;
            stage.wordParent.gameObject.SetActive(false);
        }

        StartStage(0);
    }

    void StartStage(int stageIndex)
    {
        CanvasManager.instance.DesactivateHearts();
        Debug.Log("Destroyed hearts");
        CanvasManager.instance.SpawnHeart(attemptsPerStage);
        CanvasManager.instance.ClearUsedLetters();

        currentStageIndex = stageIndex;
        currentAttempt = 0;
        waitingForNewGuess = false;

        WordStage stage = stages[currentStageIndex];
        currentTiles = stage.tiles;
        currentWordLength = stage.wordLength;
        lockedLetters = new bool[currentWordLength];

        targetWord = stage.wordList[Random.Range(0, stage.wordList.Length)].ToUpper();
        Debug.Log("Stage " + (currentStageIndex + 1) + " Target Word: " + targetWord);

        // Deactivate all word parents, activate only the current one
        foreach (WordStage s in stages)
        {
            s.wordParent.gameObject.SetActive(s == stage);
        }

        foreach (WordlTile tile in currentTiles)
        {
            tile.Clear();
        }
    }

    void Update()
    {
        if (gameOver)
            return;

        foreach (char c in Input.inputString)
        {
            SoundManager.PlaySound(SoundType.type);
            if (c == '\b')
            {
                Backspace();
            }
            else if (c == '\n' || c == '\r')
            {
                if (waitingForNewGuess)
                {
                    waitingForNewGuess = false;
                    for (int i = 0; i < currentWordLength; i++)
                    {
                        if (!lockedLetters[i])
                        {
                            currentTiles[i].Clear();
                        }
                    }
                }
                SubmitGuess();
            }
            else if (char.IsLetter(c))
            {
                AddLetter(char.ToUpper(c));
            }
        }
    }

    void AddLetter(char letter)
    {
        if (waitingForNewGuess)
        {
            waitingForNewGuess = false;

            for (int i = 0; i < currentWordLength; i++)
            {
                if (!lockedLetters[i])
                {
                    currentTiles[i].Clear();
                }
            }
        }

        for (int i = 0; i < currentWordLength; i++)
        {
            if (lockedLetters[i])
                continue;

            if (string.IsNullOrEmpty(currentTiles[i].letterText.text))
            {
                currentTiles[i].SetLetter(letter);
                return;
            }
        }
    }

    void Backspace()
    {
        for (int i = currentWordLength - 1; i >= 0; i--)
        {
            if (lockedLetters[i])
                continue;

            if (!string.IsNullOrEmpty(currentTiles[i].letterText.text))
            {
                currentTiles[i].Clear();
                return;
            }
        }
    }

    string GetCurrentGuess()
    {
        string guess = "";

        for (int i = 0; i < currentWordLength; i++)
        {
            if (string.IsNullOrEmpty(currentTiles[i].letterText.text))
                return "";

            guess += currentTiles[i].letterText.text.ToUpper();
        }

        return guess;
    }

    void SubmitGuess()
    {
        if (currentAttempt >= attemptsPerStage)
        {
            Debug.Log("No more attempts left on this stage!");
            gameOver = true;
            return;
        }

        string guess = GetCurrentGuess();

        if (guess.Length != currentWordLength)
            return;

        EvaluateGuess(guess);

        if (guess == targetWord)
        {
            SoundManager.PlaySound(SoundType.right);
            Debug.Log("Stage " + (currentStageIndex + 1) + " solved!");

            // Build "word1_word2_word3" format
            finalResult = string.IsNullOrEmpty(finalResult)
                ? targetWord.ToLower()
                : finalResult + "_" + targetWord.ToLower();

            if (currentStageIndex >= stages.Length - 1)
            {
                finalResult = finalResult + "@GMTK.com";
                emailInput.inputText = finalResult;
                emailInput.displayText.text = finalResult;
                emailInput.displayText.fontSize = 8;
                emailInput.displayText.alpha = 1f;
                nextInput.gameObject.GetComponent<Button>().isActive = false;

                Debug.Log("All stages complete! Final result: " + finalResult);
                CanvasManager.instance.DesactivateHearts();
                CanvasManager.instance.ClearUsedLetters();
                panel.PlayPopOutSequence();
                gameOver = true;
            }
            else
            {
                StartStage(currentStageIndex + 1);
            }
            return;
        }
        else
        {
            SoundManager.PlaySound(SoundType.error);
            CameraShake.instance.ShakeMedium();
        }

       

        currentAttempt++;

        if (currentAttempt >= attemptsPerStage)
        {
            Debug.Log("No more attempts left! Word was: " + targetWord);
            gameOver = true;
            return;
        }

        waitingForNewGuess = true;
    }

    void EvaluateGuess(string guess)
    {
        bool[] targetUsed = new bool[currentWordLength];

        for (int i = 0; i < currentWordLength; i++)
        {
            if (guess[i] == targetWord[i])
            {
                
                currentTiles[i].SetState(WordlTile.TileState.Correct);
                lockedLetters[i] = true;
                currentTiles[i].rightLetter = true;
                targetUsed[i] = true;
            }
        }

        for (int i = 0; i < currentWordLength; i++)
        {
            if (guess[i] == targetWord[i])
                continue;

            bool found = false;

            for (int j = 0; j < currentWordLength; j++)
            {
                if (!targetUsed[j] && guess[i] == targetWord[j])
                {
                    found = true;
                    targetUsed[j] = true;
                    break;
                }
            }

            CanvasManager.instance.UpdateheartUI();

            if (found)
            {
                currentTiles[i].SetState(WordlTile.TileState.Present);
                CanvasManager.instance.AddPresentLetter(guess[i]);
            }
            else
            {
                currentTiles[i].SetState(WordlTile.TileState.Absent);
                CanvasManager.instance.AddAbsentLetter(guess[i]);
            }
        }
    }
}