using System.Globalization;
using TMPro;
using UnityEngine;

public class calculations : MonoBehaviour
{
    private TMP_Text currentValueText;

    public void SetText(TMP_Text text)
    {
        currentValueText = text;
    }

    private int Value
    {
        get
        {
            if (currentValueText == null)
                return 0;

            if (!int.TryParse(currentValueText.text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
                return 0;

            return value;
        }
        set
        {
            if (currentValueText != null)
                currentValueText.text = value.ToString(CultureInfo.InvariantCulture);
        }
    }

    public void Add() => Value += 3;
    public void Subtract() => Value -= 2;
    public void Multiply() => Value *= 3;
    public void Divide() => Value = Value == 0 ? 0 : Value / 2;
}
