using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    private Vector3 startPosition;

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
    bool isPaused = false;

    private Coroutine pauseRoutine;

    // NEW: offset between raw mouse world position and the object's "virtual" position
    private Vector3 mouseOffset = Vector3.zero;

    void Start()
    {
        if (labyrinths.Length > 0)
        {
            EnterLabyrinth(0);
        }
    }

    Vector3 GetRawMouseWorldPos()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        mouseScreenPos.z = -Camera.main.transform.position.z;

        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        worldMousePos.z = 0f;

        return worldMousePos;
    }

    // Recalculates the offset so that "raw mouse pos + offset" equals targetPos right now
    void RealignTo(Vector3 targetPos)
    {
        Vector3 rawMouse = GetRawMouseWorldPos();
        mouseOffset = targetPos - rawMouse;
        transform.position = targetPos;
    }

    void EnterLabyrinth(int index)
    {
        currentLabyrinthIndex = index;
        Lab = true;
        isDead = false;

        for (int i = 0; i < labyrinths.Length; i++)
            labyrinths[i].labyrinthObject.SetActive(i == index);

        startPosition = labyrinths[index].startPoint.position;

        Cursor.visible = false;

        transform.position = startPosition; // just snap visually, no offset calc yet
        RestartPause(0.5f, startPosition);
    }

    void Update()
    {
        if (Lab)
        {
            Cursor.visible = false;

            if (isDead || isPaused)
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
            EnterLabyrinth(currentLabyrinthIndex + 1);
        }
        else
        {
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

    void HandleMouseMovement()
    {
        transform.position = GetRawMouseWorldPos() + mouseOffset;
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Transform spawn = labyrinths[currentLabyrinthIndex].startPoint;
        transform.position = spawn.position; // just snap visually, no offset calc yet

        RestartPause(0.5f, spawn.position);
    }

    void RestartPause(float seconds, Vector3 realignPos)
    {
        if (pauseRoutine != null)
            StopCoroutine(pauseRoutine);

        pauseRoutine = StartCoroutine(PauseMovement(seconds, realignPos));
    }

    IEnumerator PauseMovement(float seconds, Vector3 realignPos)
    {
        isPaused = true;
        isDead = true; // make sure death-triggered pauses also block during EnterLabyrinth's pause

        yield return new WaitForSeconds(seconds);

        RealignTo(realignPos); // NOW calculate the offset, using current mouse position
        isPaused = false;
        isDead = false;
    }
}