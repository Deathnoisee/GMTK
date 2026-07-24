using UnityEngine;

public class yesOrNo : MonoBehaviour
{
    [SerializeField] private bool trueHolder = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (trueHolder && collision.gameObject.CompareTag("Checkbox"))
        {
            Debug.Log("You Win");
            Destroy(collision.gameObject);
        }
        else if (!trueHolder && collision.gameObject.CompareTag("Checkbox"))
        {
            Debug.Log("You Lose");
            Destroy(collision.gameObject);
        }
    }

}
