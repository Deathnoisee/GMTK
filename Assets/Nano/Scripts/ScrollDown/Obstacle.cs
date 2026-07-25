using TMPro;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 5f;

    public int count;

    public TextMeshProUGUI countText;

    void Update()
    {
        if (this.transform.position.y > 6f)
        {
            Destroy(this.gameObject);
        }
        this.transform.position += Vector3.up * speed * Time.deltaTime;
    }



}
