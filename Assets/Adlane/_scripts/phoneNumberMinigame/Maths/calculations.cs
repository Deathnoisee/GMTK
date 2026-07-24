using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class calculations : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputA;
    [SerializeField] private GameObject AdvancedMaths;

    private static double ParseInput(string input)
    {
        return double.Parse(input, CultureInfo.InvariantCulture);
    }

    private static string FormatOutput(double value)
    {
        return value.ToString("0.##############", CultureInfo.InvariantCulture);
    }

    public void Add()
    {
        double a = ParseInput(inputA.text);
        inputA.text = FormatOutput(a + 3);
    }

    public void Subtract()
    {
        double a = ParseInput(inputA.text);
        inputA.text = FormatOutput(a - 2);
    }
    public void Multiply()
    {
        double a = ParseInput(inputA.text);
        inputA.text = FormatOutput(a * 3.5d);
    }
    public void Divide()
    {
        double a = ParseInput(inputA.text);
        if (a != 0)
        {
            inputA.text = FormatOutput(a / 7);
            return;
        }
        else
        {
            inputA.text = "0";
            return;
        }
    }
    public void Modulus()
    {
        double a = ParseInput(inputA.text);
        if (a != 0)
        {
            inputA.text = FormatOutput(a % 5);
        }
        else
        {
            inputA.text = "0";
            return;
        }
    }
    public void exponential()
    {
        double a = ParseInput(inputA.text);
        inputA.text = FormatOutput(System.Math.Exp(a));
    }
    public void SquareRoot()
    {
        double a = ParseInput(inputA.text);
        inputA.text = FormatOutput(System.Math.Sqrt(a));
    }
    public void floor()
    {
        double a = ParseInput(inputA.text);
        inputA.text = FormatOutput(System.Math.Floor(a));
    }
    public void toggleAdvancedMaths()
    {
        AdvancedMaths.SetActive(!AdvancedMaths.activeSelf);
    }
}
