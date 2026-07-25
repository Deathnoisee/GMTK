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

    public void Add() => Value = Mathf.Min(Value + 3, 999);
    public void Subtract() => Value = Mathf.Max(Value - 2, 000);
    public void Multiply() => Value = Mathf.Min(Value * 3, 999);
    public void Divide() => Value = Value == 0 ? 0 : Mathf.Max(Value / 2, 000);
}
