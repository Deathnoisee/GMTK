using SmallHedge.SoundManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Timer")]
    public float startingTime = 300f; // 5:00 in seconds
    public float timeRemaining;
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
    public GameObject Gamejam;

    [Header("Win / Lose")]
    public GameObject winPanelObject;  // has a Panel component, auto pops in via OnEnable
    public GameObject losePanelObject; // has a Panel component, auto pops in via OnEnable

    private bool win = false;
    private bool resultHandled = false; // prevents this from firing more than once

    public void Awake()
    {
        instance = this;
    }

    public bool gameOver = false;

    void Start()
    {
        RestartGame();
        SetDefaultCursor();
    }

    public void OnTimerEnd()
    {
        Debug.Log("Timer has ended!");
        gameOver = true;
        TriggerLose();
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

            if (hit != null && hit.CompareTag("Button"))
            {
                Button button = hit.GetComponent<Button>();
                if (button != null)
                {
                    if (button.isActive == false)
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
        Gamejam.SetActive(true);
    }

    public void RestartGame()
    {
        timeRemaining = startingTime; // now always matches whatever Start() used
        timerRunning = true;
        gameOver = false;
        resultHandled = false;
    }

    // Call this from wherever your win condition is detected (e.g. a minigame's Win())
    public void TriggerWin()
    {
       Debug.Log("TriggerWin called");
        if (resultHandled) return;
        resultHandled = true;
        win = true;
        gameOver = true;
        SoundManager.StopMusic();

        CloseActivePanelsThen(() =>
        {
            if (winPanelObject != null)
            {
                winPanelObject.SetActive(true);
            }
        });
    }

    public void TriggerLose()
    {
        if (resultHandled) return;
        resultHandled = true;
        win = false;
        gameOver = true;
        SoundManager.StopMusic();

        CloseActivePanelsThen(() =>
        {
            if (losePanelObject != null)
            {
                losePanelObject.SetActive(true);
            }
        });
    }

    /// <summary>
    /// Finds every currently active Panel in the scene, pops them all out simultaneously,
    /// then invokes onAllClosed once every single one has finished its pop-out animation.
    /// </summary>
    private void CloseActivePanelsThen(System.Action onAllClosed)
    {
        Panel[] activePanels = FindObjectsByType<Panel>(FindObjectsInactive.Exclude);

        System.Collections.Generic.List<Panel> toClose = new System.Collections.Generic.List<Panel>();
        foreach (Panel p in activePanels)
        {
            if (p.gameObject.activeInHierarchy)
            {
                toClose.Add(p);
            }
        }

        if (toClose.Count == 0)
        {
            onAllClosed?.Invoke();
            return;
        }

        int remaining = toClose.Count;

        foreach (Panel p in toClose)
        {
            p.PlayPopOutSequence(() =>
            {
                remaining--;
                if (remaining <= 0)
                {
                    onAllClosed?.Invoke();
                }
            });
        }
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