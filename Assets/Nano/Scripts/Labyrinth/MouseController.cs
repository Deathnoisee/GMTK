using UnityEngine;

public class MouseController : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector3 startPosition;
    private Vector3 offset;


    public GameObject nextLevel;
    public LayerMask wallLayerMask;



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
            Vector3 targetPos = mousePosition + offset;
            MoveWithWallCheck(targetPos);
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

    void NextLevel()
    {
        Debug.Log("Next Level");
        Lab = false;
        nextLevel.SetActive(true);
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {

            Die();
        }
        if (collision.CompareTag("Finisher"))
        {

            NextLevel();
            Debug.Log("Finished");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall"))
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
    void MoveWithWallCheck(Vector3 targetPos)
    {
        Vector3 currentPos = transform.position;
        Vector3 direction = targetPos - currentPos;
        float distance = direction.magnitude;

        if (distance > 0.001f)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPos, direction.normalized, distance, wallLayerMask);

            if (hit.collider != null && hit.collider.CompareTag("Wall"))
            {
                Debug.Log("Wall in path — blocked/died");
                Die();
                return;
            }
        }

        transform.position = targetPos;
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