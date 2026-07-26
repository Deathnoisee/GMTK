using SmallHedge.SoundManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timeRemaining = 300f; // 5:00 in seconds
    private bool timerRunning = true;
    public bool inMinigame = false;
    public static GameManager instance;

    [Header("Custom Cursor")]
    public Texture2D defaultCursorTexture;
    public Texture2D hoverCursorTexture;
    public Vector2 cursorHotspot = Vector2.zero;

    private bool isHoveringButton = false;


    public bool GameOver;


    public Panel userPanel;

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

    void Start()
    {
        SetDefaultCursor();
    }

    public void OnTimerEnd()
    {
        Debug.Log("Timer has ended!");
        gameOver = true;
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

    void HandleCursorHover()
    {
        Vector3 realMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(realMousePos);

        bool nowHovering = hit != null && hit.CompareTag("Button");

        // Only swap the texture when the hover state actually changes,
        // instead of calling SetCursor every single frame
        if (nowHovering && !isHoveringButton)
        {
            isHoveringButton = true;
            SetHoverCursor();
        }
        else if (!nowHovering && isHoveringButton)
        {
            isHoveringButton = false;
            SetDefaultCursor();
        }
    }

    void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursorTexture, cursorHotspot, CursorMode.Auto);
    }

    void SetHoverCursor()
    {
        Cursor.SetCursor(hoverCursorTexture, cursorHotspot, CursorMode.Auto);
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
                if (button != null)
                {
                    if (button.isActive == false )
                    {
                        button?.onClick.Invoke();
                        SoundManager.PlaySound(SoundType.click);
                        if (!button.specialButton)
                        {
                            
                            button.isActive = true;
                        }
                     
                    }
                }
            }
        }
    }

    public void Register()
    {
        userPanel.PlayPopOutSequence();
    }

    public void RestartGame()
    {
        timeRemaining = 300f;
        timerRunning = true;
        gameOver = false;
    }

    void Update()
    {
        HandleCursorHover();

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