using UnityEngine;


public class Death : MonoBehaviour
{
    public sentenceManager sentenceManager;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}