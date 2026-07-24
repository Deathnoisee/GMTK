using System.Collections;
using UnityEngine;

public class crankManager : MonoBehaviour
{
    [SerializeField] private int crankCount = 20;
    [SerializeField] private float crankDuration = 0.15f;

    private int currentCrankCount = 0;
    private bool canCrank = true;

    public void crank(GameObject crankSprite)
    {
        if (!canCrank) return;
        StartCoroutine(RotateCrank(crankSprite.transform));
    }

    private IEnumerator RotateCrank(Transform crankTransform)
    {
        canCrank = false;
        currentCrankCount++;

        Quaternion startRotation = crankTransform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, -360f + startRotation.eulerAngles.z);

        float elapsed = 0f;
        while (elapsed < crankDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / crankDuration);

            crankTransform.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        crankTransform.localRotation = targetRotation;

        if (currentCrankCount >= crankCount)
        {
            Debug.Log("Crank complete!");
            crankTransform.GetComponent<SpriteRenderer>().color = Color.green;
        }

        canCrank = true;
    }
}
