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

    private void Start()
    {
        BeginStep(0);
    }

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

    public void Win()
    {
        Debug.Log("Phone number complete!");

        if (calculations != null)
            calculations.enabled = false;
    }
}
