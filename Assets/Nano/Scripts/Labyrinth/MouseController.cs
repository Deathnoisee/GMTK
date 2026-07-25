using UnityEngine;

public class MouseController : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector3 startPosition;
    private Vector3 offset;

    [System.Serializable]
    public class Labyrinth
    {
        public GameObject labyrinthObject; // e.g. Lab1, Lab2, Lab3 parent
        public Transform startPoint;       // where the mouse should start in this labyrinth
    }

    public Labyrinth[] labyrinths;
    public GameObject nextLevel; // what to show after ALL labyrinths are done (e.g. a win screen)

    public LayerMask wallLayerMask;

    private int currentLabyrinthIndex = 0;

    public bool Lab = false;

    bool isDead = false;

    void Start()
    {
        if (labyrinths.Length > 0)
        {
            EnterLabyrinth(0);
        }
    }

    void EnterLabyrinth(int index)
    {
        currentLabyrinthIndex = index;
        Lab = true;
        isDead = false;

        // Deactivate all labyrinths, activate only the current one
        for (int i = 0; i < labyrinths.Length; i++)
        {
            labyrinths[i].labyrinthObject.SetActive(i == index);
        }

        startPosition = labyrinths[index].startPoint.position;
        transform.position = startPosition;

        Cursor.visible = false;

        // Recalculate offset so the CURRENT real cursor position maps to this labyrinth's start point
        // (same trick as Die(), so the rodent doesn't jump based on where the real cursor happens to be)
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        offset = startPosition - mousePosition;
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

        if (currentLabyrinthIndex < labyrinths.Length - 1)
        {
            // Move to the next labyrinth in the list
            EnterLabyrinth(currentLabyrinthIndex + 1);
        }
        else
        {
            // No more labyrinths — finished the whole sequence
            Debug.Log("All labyrinths complete!");
            Lab = false;
            Cursor.visible = true;

            if (nextLevel != null)
            {
                nextLevel.SetActive(true);
            }

            this.gameObject.transform.parent.gameObject.SetActive(false);
        }
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

    void Die()
    {
        transform.position = startPosition;

        // Recalculate offset so current cursor position becomes the new reference point
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        offset = startPosition - mousePosition;
    }
}