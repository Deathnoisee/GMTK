using UnityEngine;

public class yesOrNo : MonoBehaviour
{
    [SerializeField] private bool trueHolder = false;
    [SerializeField] private sentenceManager sentenceManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Checkbox")) return;

        bool correct = (sentenceManager != null && sentenceManager.IsCurrentAnswerYes() == trueHolder);

        if (correct)
        {
            Debug.Log("You Win");
            sentenceManager.NextLevel();
        }
        else
        {
            Debug.Log("You Lose");
            CameraShake.instance.ShakeMedium();
        }

        Destroy(collision.gameObject);
    }
}
