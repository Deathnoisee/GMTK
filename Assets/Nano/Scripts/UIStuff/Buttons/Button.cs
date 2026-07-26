using UnityEngine;
using UnityEngine.Events;
public class Button : MonoBehaviour
{
    public UnityEvent onClick;
    public bool isActive;
    public bool specialButton =false;

    private void Update()
    {
        if (!specialButton)
        {
            
            if (!isActive)
            {
                this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
            else
            {
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
      
    }


}
