using Unity.VisualScripting;
using UnityEngine;

public class lever : MonoBehaviour
{
    public bool isTrue = false;
    [SerializeField] private Sprite leverOn;
    [SerializeField] private Sprite leverOff;
    [SerializeField] private Sprite LightOn;
    [SerializeField] private Sprite LightOff;
    [SerializeField] private GameObject light;

    public void changeColor()
    {
        if (isTrue)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = leverOn;
            light.GetComponent<SpriteRenderer>().sprite = LightOn;
            Debug.Log("True color green");
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = leverOff;
            light.GetComponent<SpriteRenderer>().sprite = LightOff;
            Debug.Log("False color red");
        }
    }
}