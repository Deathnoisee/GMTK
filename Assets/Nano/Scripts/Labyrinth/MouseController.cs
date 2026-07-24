using UnityEngine;

public class MouseController : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector3 startPosition;
    private Vector3 offset;

    public bool Lab = false;

    bool isDead = false;

    void Start()
    {
        if (Lab)
        {
            Cursor.visible = false;
            startPosition = transform.position;

            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            offset = startPosition - mousePosition;
        }

    }

    void Update()
    {
        if (Lab)
        {
            if (isDead)
            {
                return;
            }
            HandleMouseMovement();
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {

            Die();
        }
    }
    void HandleMouseMovement()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        transform.position = mousePosition + offset;
    }


    void Die()
    {
        transform.position = startPosition;

        // Recalculate offset so current cursor position becomes the new reference point
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        offset = startPosition - mousePosition;
    }
}