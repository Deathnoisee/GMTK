using System.Collections;
using TMPro;
using UnityEngine;

public class PasswordManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text passwordInput;
    [SerializeField] private TMP_Text passwordStrengthText;
    [SerializeField] private Color incorrectColor = Color.red;
    [SerializeField] private Color correctColor = Color.green;
    [Header("Requirements")]
    [SerializeField] private string requiredWord = "Count Down";
    [SerializeField] private float checkInterval = 0.15f;


    private void Start()
    {
        //remove later
        StartCheckingPassword();
    }
    public void StartCheckingPassword()
    {
        StartCoroutine(CheckPasswordRoutine());
    }

    private IEnumerator CheckPasswordRoutine()
    {
        while (true)
        {
            string text = passwordInput != null ? passwordInput.text : string.Empty;

            if (IsCurrentStepValid(text))
            {
                SetText("Password accepted.", correctColor);
                yield break;
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    private bool IsCurrentStepValid(string text)
    {

        if (!(text.Length >= 3))
        {
            SetText("Password must be at least 3 characters long.", incorrectColor);
            return false;
        }
        if (!HasUppercase(text) || !HasLowercase(text))
        {
            SetText("Password must contain both uppercase and lowercase letters.", incorrectColor);
            return false;
        }
        if (!(CountDigits(text) >= 3))
        {
            SetText("Password must contain at least 3 digits.", incorrectColor);
            return false;
        }
        if (!HasSpecialCharacter(text))
        {
            SetText("Password must contain a special character.", incorrectColor);
            return false;
        }
        if (!HasSpace(text))
        {
            SetText("Password must contain at least 1 space.", incorrectColor);
            return false;
        }
        if (!ContainsRequiredWord(text, requiredWord))
        {
            SetText($"Password must include the word '{requiredWord}'.", incorrectColor);
            return false;
        }
        if (!ContainsRequiredWord(text, "F"))
        {
            SetText("Press F to Pay Respect.", incorrectColor);
            return false;
        }
        if (!ContainsRequiredWord(text, "2017"))
        {
            SetText("Password must include the first gmtk game jam year", incorrectColor);
            return false;
        }
        if (!ContainsRequiredWord(text, "aa"))
        {
            SetText("Password Must include conscutive a's", incorrectColor);
            return false;
        }
        return true;
    }

    // private string GetCurrentInstruction()
    // {
    //     switch (currentStep)
    //     {
    //         case 0: return "enter at least 8 characters.";
    //         case 1: return "must contains both uppercase and lowercase letters.";
    //         case 2: return "must at least have 3 numbers.";
    //         case 3: return "must contain a special character.";
    //         case 4: return "must have at least 1 space.";
    //         case 5: return $"must include the word '{requiredWord}'.";
    //         case 6: return "Press F to pay respect.";
    //         case 7: return "";
    //         default: return "Done.";
    //     }
    // }



    private void SetText(string message, Color color)
    {
        if (passwordStrengthText == null) return;

        passwordStrengthText.text = message;
        passwordStrengthText.color = color;
    }

    private int CountDigits(string text)
    {
        int count = 0;

        foreach (char c in text)
        {
            if (char.IsDigit(c)) count++;
        }

        return count;
    }

    private bool HasSpecialCharacter(string text)
    {
        foreach (char c in text)
        {
            if (!char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c)) return true;
        }
        return false;
    }

    private bool HasSpace(string text)
    {
        foreach (char c in text)
        {
            if (char.IsWhiteSpace(c)) return true;
        }
        return false;
    }

    private bool HasUppercase(string text)
    {
        foreach (char c in text)
        {
            if (char.IsUpper(c)) return true;
        }

        return false;
    }

    private bool HasLowercase(string text)
    {
        foreach (char c in text)
        {
            if (char.IsLower(c)) return true;
        }

        return false;
    }

    private bool ContainsRequiredWord(string text, string word)
    {
        return text.IndexOf(word, System.StringComparison.OrdinalIgnoreCase) >= 0;
    }
}
