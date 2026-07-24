using TMPro;
using System.Collections.Generic;
using UnityEngine;


public class equationGenerator : MonoBehaviour
{
    [SerializeField] private List<equation> equations = new List<equation>();
    [SerializeField] private equation currentEquation;
    [SerializeField] private TextMeshPro equationText;

    public void StartGame()
    {
        Init();
    }

    private void Init()
    {
        if (equations == null || equations.Count == 0) return;

        int randomIndex = Random.Range(0, equations.Count);
        currentEquation = equations[randomIndex];

        if (equationText != null)
            equationText.text = currentEquation.equationText;
    }

    public bool CheckAnswer(int answer)
    {
        if (currentEquation == null) return false;
        return currentEquation.answer == answer;
    }
}