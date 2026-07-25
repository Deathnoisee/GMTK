using TMPro;
using UnityEngine;

public class InputField : MonoBehaviour
{
    public TextMeshPro displayText; // drag a TextMeshPro (3D, not UI) object here
    public string inputText = "";



    public bool isLevelInput = true; // Flag to check if the level is in input mode

    bool isCompleted = false; // Flag to check if the level is completed

    void Start()
    {
        displayText = GetComponentInChildren<TextMeshPro>();

    }
    void Update()
    {
        if (!isLevelInput)
        {
            foreach (char c in Input.inputString)
            {
                if (c == '\b')
                {
                    if (inputText.Length > 0)
                        inputText = inputText.Substring(0, inputText.Length - 1);
                }
                else if (c == '\n' || c == '\r')
                {
                    Submit();
                }
                else
                {
                    inputText += c;
                }
            }
            displayText.text = inputText + "_"; // fake cursor blink
        }



    }

    void Submit()
    {
        Debug.Log("Submitted: " + inputText);
    }
}
