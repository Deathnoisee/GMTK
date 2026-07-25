using UnityEngine;

public class SurfController : MonoBehaviour
{
    Rigidbody2D rb;
    bool isDead = false;
    public float completionCount = 0;
    public float completionTarget = 10;
    public float completionDuration = 60f; // total seconds to reach completionTarget
    public bool started = false;
    public bool completed = false;
    private bool spawnedHeart;
    public int health = 3;
    public float speed = 5f;

    void Update()
    {
        if (isDead || completed || !started)
        {
            return;
        }

        // Increase completion at a constant rate so it reaches completionTarget after completionDuration seconds
        float ratePerSecond = completionTarget / completionDuration;
        AddCompletion(ratePerSecond * Time.deltaTime);
    }

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
            completionCount = completionTarget;
            Win();
        }
    }

    public void Win()
    {
        if (completed) return; // avoid running this more than once

        completed = true;
        Debug.Log("Level Complete!");
        CanvasManager.instance.DesactivateHearts();
        // You can add any additional logic here for when the level is complete
    }

    public void Die()
    {
        health--;
        CameraShake.instance.ShakeSmall();
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