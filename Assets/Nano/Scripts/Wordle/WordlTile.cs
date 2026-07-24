using TMPro;
using UnityEngine;

public class WordlTile : MonoBehaviour
{
    private SpriteRenderer background;
    public TextMeshPro letterText;

    public bool rightLetter = false;

    public enum TileState { Empty, Correct, Present, Absent }


    void Start()
    {
        letterText = GetComponentInChildren<TextMeshPro>();
        background = GetComponent<SpriteRenderer>();
        Clear();
    }
    public void SetLetter(char letter)
    {
        letterText.text = letter.ToString().ToUpper();
    }

    public void Clear()
    {
        letterText.text = "";
        rightLetter = false;
        SetState(TileState.Empty);
    }

    public void SetState(TileState state)
    {
        switch (state)
        {
            case TileState.Correct:
                background.color = new Color(0.42f, 0.65f, 0.39f); // green
                break;
            case TileState.Present:
                background.color = new Color(0.79f, 0.68f, 0.29f); // yellow
                break;
            case TileState.Absent:
                background.color = new Color(0.47f, 0.49f, 0.49f); // gray
                break;
            case TileState.Empty:
                background.color = Color.white;
                break;
        }
    }
}
