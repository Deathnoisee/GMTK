using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float timeRemaining = 300f; // 5:00 in seconds
    private bool timerRunning = true;


    public bool inMinigame = false;

    public static GameManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public bool gameOver = false;

    public void OnTimerEnd()
    {
        Debug.Log("Timer has ended!");
        gameOver = true;
        // Add any additional logic you want to execute when the timer ends
    }
    void Start()
    {

    }

    public void TestClcick()
    {
        Debug.Log("Clicked");
    }
    public void SwictchGame(Button button)
    {
        button.GetComponent<SwitchButton>().NextGame.SetActive(true);
        if (button.GetComponent<SwitchButton>().CurrentGame != null)
        {
            button.GetComponent<SwitchButton>().CurrentGame.SetActive(false);
        }


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

    public void RestartGame()
    {
        // Reset the game state here
        timeRemaining = 300f; // Reset timer to 5:00
        timerRunning = true;
        gameOver = false;

        // Add any additional logic to reset the game state, such as resetting player health, score, etc.
    }
    // Update is called once per frame
    void Update()
    {

        if (!timerRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            timerRunning = false;
            OnTimerEnd();
        }

        CanvasManager.instance.UpdateTimerUI(timeRemaining);
        OnClick();
    }
}
