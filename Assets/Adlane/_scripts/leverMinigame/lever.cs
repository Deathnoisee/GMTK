using UnityEngine;

public class lever : MonoBehaviour
{
    public bool isTrue = false;

    public void changeColor()
    {
        if (isTrue)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
            Debug.Log("True color green");
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            Debug.Log("False color red");
        }
    }
}