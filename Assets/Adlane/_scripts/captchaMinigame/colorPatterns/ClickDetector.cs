using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    [SerializeField] private patternGenerator patternGenerator;
    [SerializeField] private int segmentIndex;

    private void OnMouseDown()
    {
        if (!patternGenerator.canClick) return;
        patternGenerator.CheckInput(segmentIndex);
    }
}
