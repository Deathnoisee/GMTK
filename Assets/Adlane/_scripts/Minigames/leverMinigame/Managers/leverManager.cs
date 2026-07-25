using UnityEngine;
using System.Collections.Generic;
using TMPro;
public class leverManager : MonoBehaviour
{
    [SerializeField] private List<lever> levers = new List<lever>();
    [SerializeField] private TMP_Text ButtonText;

    // Call this method to start the game nano
    public void StartGame()
    {
        Init();
    }

    private void Init()
    {
        if (levers == null || levers.Count == 0) return;

        foreach (lever l in levers)
        {
            if (l == null) continue;
            l.gameObject.SetActive(true);
            l.isTrue = false;
            l.GetComponent<PolygonCollider2D>().enabled = true;
            ButtonText.text = "Reset";
            l.changeColor();
        }

        int randomIndex = Random.Range(0, levers.Count);
        if (levers[randomIndex] != null)
        {
            levers[randomIndex].isTrue = true;
            levers[randomIndex].changeColor();
        }
    }

    public void SwitchLever(int index)
    {
        if (index < 0 || index >= levers.Count) return;

        Toggle(index);
        Toggle(index - 1);
        Toggle(index + 1);

        if (CheckWinCondition()) WinGame();
    }
    private void Toggle(int i)
    {
        if (i < 0 || i >= levers.Count) return;
        if (levers[i] == null) return;
        levers[i].isTrue = !levers[i].isTrue;
        levers[i].changeColor();
    }

    private bool CheckWinCondition()
    {
        foreach (lever l in levers)
        {
            if (l == null || !l.isTrue) return false;
        }
        return true;
    }
    // extend this method lel win nano
    private void WinGame()
    {
        Debug.Log("You win the lever minigame!");
        ButtonText.text = "You Win!";
        foreach (lever l in levers)
        {
            if (l == null) continue;
            l.GetComponent<PolygonCollider2D>().enabled = false;
        }
    }

}
