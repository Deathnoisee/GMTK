using TMPro;
using UnityEngine;

public class numManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text[] stepValueTexts = new TMP_Text[3];
    [SerializeField] private calculations calculations;

    [Header("Step Values")]
    [SerializeField] private int[] startValues = { 326, 214, 102 };
    [SerializeField] private int[] targetValues = { 987, 654, 321 };

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color correctColor = Color.green;

    private int currentStep = 0;
    private bool isComplete = false;


    public InputField phoneInput;
    public Panel myPanel;
    public Button RegisterButton;

    private void Start()
    {
        BeginStep(0);
    }

    // Call this method to start the minigame nano EBDA B BeginStep(0) !!
    public void BeginStep(int step)
    {
        currentStep = step;

        if (currentStep < 0 || currentStep >= startValues.Length)
            return;

        if (stepValueTexts[currentStep] != null)
        {
            stepValueTexts[currentStep].text = startValues[currentStep].ToString();
            stepValueTexts[currentStep].color = normalColor;

            if (calculations != null)
                calculations.SetText(stepValueTexts[currentStep]);
        }
    }

    public void CheckStep()
    {
        Debug.Log("check");

        if (currentStep < 0 || currentStep >= targetValues.Length)
            return;

        if (stepValueTexts[currentStep] == null)
            return;

        if (!int.TryParse(stepValueTexts[currentStep].text, out int value))
            return;

        if (value == targetValues[currentStep])
        {
            stepValueTexts[currentStep].color = correctColor;
            currentStep++;

            if (currentStep < targetValues.Length)
                BeginStep(currentStep);
            else
                Win();
        }
    }

    // Call this method to handle win condition nano
    private void Win()
    {
        Debug.Log("Phone number complete!");
        isComplete = true;
        string final = "987-654-321";
        phoneInput.inputText = final;
        phoneInput.displayText.text = final;
        phoneInput.displayText.alpha = 1f;
        RegisterButton.isActive = false;
        myPanel.PlayPopOutSequence();

        if (calculations != null)
            calculations.enabled = false;
    }

    // Hook this up to a Restart button's OnClick — resets ONLY the current step
    public void RestartCurrentStep()
    {
        Debug.Log("Restarting current step: " + currentStep);
        BeginStep(currentStep);
    }

    // Hook this up to a Restart button's OnClick
    public void Restart()
    {
        Debug.Log("Restarting numManager minigame");

        isComplete = false;
        currentStep = 0;

        // Reset every step's text/color back to its starting state, not just the current one
        for (int i = 0; i < stepValueTexts.Length; i++)
        {
            if (stepValueTexts[i] != null && i < startValues.Length)
            {
                stepValueTexts[i].text = startValues[i].ToString();
                stepValueTexts[i].color = Color.black;
            }
        }
        stepValueTexts[currentStep].color=normalColor;

        if (calculations != null)
            calculations.enabled = true;

        BeginStep(0);
    }
}