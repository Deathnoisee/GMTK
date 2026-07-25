using UnityEngine;

public class SurfController : MonoBehaviour
{
    Rigidbody2D rb;

    bool isDead = false;




    public int health = 3;

    public float speed = 5f;

    void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        float moveHorizontal = Input.GetAxis("Horizontal");


        Vector2 movement = new Vector2(moveHorizontal, 0f);
        rb.linearVelocity = movement * speed;
    }


    public void Die()
    {
        health--;
        if (health <= 0)
        {
            rb.linearVelocity = Vector2.zero;
            isDead = true;
            Debug.Log("Game Over");

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Die();
            Destroy(collision.gameObject);
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}
