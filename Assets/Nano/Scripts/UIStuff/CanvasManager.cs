using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{

    public static CanvasManager instance;
    public GameObject heart;



    public GameObject HearthGroup;

    public TextMeshProUGUI timerText;

    public void UpdateheartUI()
    {
        int childCount = HearthGroup.transform.childCount;
        int lastIndex = childCount - 1;

        if (lastIndex < 0)
        {
            Debug.Log("No hearts left to remove");
            return;
        }

        Destroy(HearthGroup.transform.GetChild(lastIndex).gameObject);
    }

    public void GameOverPanel()
    {
        // Implement your game over panel logic here
        Debug.Log("Game Over! Show Game Over Panel.");
    }

    public void RestartGame()
    {
        // Implement your restart game logic here
        Debug.Log("Restarting Game...");
    }

    public void DesactivateHearts()
    {
        for (int i = HearthGroup.transform.childCount - 1; i >= 0; i--)
        {
            Debug.Log("destroyed heart");
            Destroy(HearthGroup.transform.GetChild(i).gameObject);
        }
    }



    public void UpdateTimerUI(float time)
    {
        if (time < 0) time = 0;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }


    public void SpawnHeart(int health)
    {
        for (int i = 0; i < health; i++)
        {
            GameObject newHeart = Instantiate(heart, HearthGroup.transform);
            newHeart.transform.SetSiblingIndex(0);
            newHeart.SetActive(true);
        }
    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
