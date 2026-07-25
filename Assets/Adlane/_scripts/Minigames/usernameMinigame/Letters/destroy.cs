using UnityEngine;

public class destroy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Letter"))
        {
            Destroy(collision.gameObject);
        }
    }
}