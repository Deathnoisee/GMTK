using UnityEngine;

public class SurfController : MonoBehaviour
{
    Rigidbody2D rb;

    bool isDead = false;

    public float completionCount = 0;

    public float completionTarget = 10;

    public bool started = false;
    private bool spawnedHeart;




    public int health = 3;

    public float speed = 5f;

    void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        Move();

    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0f);
        rb.linearVelocity = movement * speed;
        if (moveHorizontal != 0)
        {
            started = true;
            if (!spawnedHeart)
            {
                CanvasManager.instance.SpawnHeart(health);
                spawnedHeart = true;
            }
        }

    }

    public void AddCompletion(float amount)
    {
        completionCount += amount;
        if (completionCount >= completionTarget)
        {
            Debug.Log("Level Complete!");

            // You can add any additional logic here for when the level is complete
        }
    }

    public void Win()
    {

        if (completionCount >= completionTarget)
        {
            Debug.Log("Level Complete!");
            CanvasManager.instance.DesactivateHearts();
            // You can add any additional logic here for when the level is complete
        }

    }
    public void Die()
    {

        health--;
        CanvasManager.instance.UpdateheartUI();
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
