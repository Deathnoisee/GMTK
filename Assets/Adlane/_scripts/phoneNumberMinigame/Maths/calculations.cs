using UnityEngine;
using TMPro;
public class calculations : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputA;
    public void Add()
    {
        float a = float.Parse(inputA.text);
        inputA.text = (a + 3).ToString();
    }

    public void Subtract()
    {
        float a = float.Parse(inputA.text);
        inputA.text = (a - 2).ToString();
    }
    public void Multiply()
    {
        float a = float.Parse(inputA.text);
        inputA.text = (a * 3.5f).ToString();
    }
    public void Divide()
    {
        float a = float.Parse(inputA.text);
        if (a != 0)
        {
            inputA.text = (a / 7).ToString();
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
        float a = float.Parse(inputA.text);
        if (a != 0)
        {
            inputA.text = (a % 5).ToString();
        }
        else
        {
            inputA.text = "0";
            return;
        }
    }
    public void exponential()
    {
        float a = float.Parse(inputA.text);
        inputA.text = (Mathf.Exp(a)).ToString();
    }
    public void SquareRoot()
    {
        float a = float.Parse(inputA.text);
        inputA.text = (Mathf.Sqrt(a)).ToString();
    }
    public void floor()
    {
        float a = float.Parse(inputA.text);
        inputA.text = (Mathf.FloorToInt(a)).ToString();
    }
}
