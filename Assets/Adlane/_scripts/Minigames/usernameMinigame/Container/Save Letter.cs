using UnityEngine;
using TMPro;

public class SaveLetter : MonoBehaviour
{
    [SerializeField] private UsernameManager usernameManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Letter"))
            return;

        TMP_Text letterText = collision.gameObject.GetComponentInChildren<TMP_Text>();
        if (letterText == null)
            return;

        string value = letterText.text;

        if (usernameManager != null)
            usernameManager.TryCollectLetter(value);

        Destroy(collision.gameObject);
    }
}
