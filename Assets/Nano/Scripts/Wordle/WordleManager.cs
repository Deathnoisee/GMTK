using UnityEngine;

public class WordleManager : MonoBehaviour
{
    public WordlTile[] tiles;
    public int wordLength = 5;

    public string[] wordList;
    public string targetWord;

    public int attempts = 6;


    private bool waitingForNewGuess = false;

    [SerializeField]
    private int currentAttempt = 0;
    private bool[] lockedLetters;
    private bool gameOver = false;

    void Start()
    {
        tiles = GetComponentsInChildren<WordlTile>();

        lockedLetters = new bool[wordLength];

        PickRandomWord();

        Debug.Log("Target Word: " + targetWord);
    }

    void PickRandomWord()
    {
        targetWord = wordList[Random.Range(0, wordList.Length)].ToUpper();
    }

    void Update()
    {
        if (gameOver)
            return;

        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {
                Backspace();
            }
            else if (c == '\n' || c == '\r')
            {
                if (waitingForNewGuess)
                {
                    waitingForNewGuess = false;
                    for (int i = 0; i < wordLength; i++)
                    {
                        if (!lockedLetters[i])
                        {
                            tiles[i].Clear();
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

            for (int i = 0; i < wordLength; i++)
            {
                if (!lockedLetters[i])
                {
                    tiles[i].Clear();
                }
            }
        }
        for (int i = 0; i < wordLength; i++)
        {
            if (lockedLetters[i])
                continue;

            if (string.IsNullOrEmpty(tiles[i].letterText.text))
            {
                tiles[i].SetLetter(letter);
                return;
            }
        }
    }

    void Backspace()
    {
        for (int i = wordLength - 1; i >= 0; i--)
        {
            if (lockedLetters[i])
                continue;

            if (!string.IsNullOrEmpty(tiles[i].letterText.text))
            {
                tiles[i].Clear();
                return;
            }
        }
    }

    string GetCurrentGuess()
    {
        string guess = "";

        for (int i = 0; i < wordLength; i++)
        {
            if (string.IsNullOrEmpty(tiles[i].letterText.text))
                return "";

            guess += tiles[i].letterText.text.ToUpper();
        }

        return guess;
    }

    void SubmitGuess()
    {
        if (currentAttempt >= attempts)
        {
            Debug.Log("No more attempts left!");
            gameOver = true;
            return;
        }
        string guess = GetCurrentGuess();

        if (guess.Length != wordLength)
            return;

        EvaluateGuess(guess);

        if (guess == targetWord)
        {
            Debug.Log("You win!");
            gameOver = true;
            return;
        }
        currentAttempt++;

        waitingForNewGuess = true;
    }

    void EvaluateGuess(string guess)
    {
        bool[] targetUsed = new bool[wordLength];

        // First pass: Correct letters
        for (int i = 0; i < wordLength; i++)
        {
            if (guess[i] == targetWord[i])
            {
                tiles[i].SetState(WordlTile.TileState.Correct);
                lockedLetters[i] = true;
                tiles[i].rightLetter = true;
                targetUsed[i] = true;
            }
        }

        // Second pass: Present / Absent
        for (int i = 0; i < wordLength; i++)
        {
            // Skip already-correct letters
            if (guess[i] == targetWord[i])
                continue;

            bool found = false;

            for (int j = 0; j < wordLength; j++)
            {
                if (!targetUsed[j] && guess[i] == targetWord[j])
                {
                    found = true;
                    targetUsed[j] = true;
                    break;
                }
            }

            if (found)
                tiles[i].SetState(WordlTile.TileState.Present);
            else
                tiles[i].SetState(WordlTile.TileState.Absent);
        }
    }
}