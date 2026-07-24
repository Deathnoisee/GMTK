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




        OnClick();
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
    void OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 realMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            Collider2D hit = Physics2D.OverlapPoint(realMousePos);
            Debug.Log("Hit object: " + (hit != null ? hit.name : "none"));

            if (hit != null && hit.CompareTag("Button"))
            {
                Debug.Log("Hit button");
                Button button = hit.GetComponent<Button>();
                button?.onClick.Invoke();
            }
        }

    }

    void StartLabyrinth()
    {
        isDead = false;
        transform.position = startPosition;

        // Recalculate offset so current cursor position becomes the new reference point
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        offset = startPosition - mousePosition;
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