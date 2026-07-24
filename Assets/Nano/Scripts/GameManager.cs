using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


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
        button.GetComponent<SwitchButton>().CurrentGame.SetActive(false);

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


    // Update is called once per frame
    void Update()
    {
        OnClick();
    }
}
