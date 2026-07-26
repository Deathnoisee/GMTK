using UnityEngine;

public class ContainerMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 boundaryXnY = new Vector2(6.8f, 0f);


    public UsernameManager usernameManager;

    private Rigidbody2D rb;
    public bool started;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        usernameManager.BeginGame();
    }
    private void FixedUpdate()
    {
        Move();
    }


    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0f);
        rb.linearVelocity = movement * moveSpeed;
        if (moveHorizontal != 0 && !started)
        {
            started = true;
          
        }
    }

   
}
