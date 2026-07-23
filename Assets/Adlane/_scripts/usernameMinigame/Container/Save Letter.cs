using UnityEngine;
using TMPro;
public class SaveLetter : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    private GameObject letterPrefab;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Letter"))
        {
            letterPrefab = collision.gameObject;
            SaveLetterData();
        }
    }
    private void SaveLetterData()
    {
        string letterText = letterPrefab.GetComponentInChildren<TMP_Text>().text;
        text.text += letterText;
        Debug.Log("Letter saved: " + letterText);
        Destroy(letterPrefab);
    }
}
