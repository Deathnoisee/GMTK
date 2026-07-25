using UnityEngine;

public class ContainerMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 boundaryXnY = new Vector2(6.8f, 0f);


    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (transform.position.x < -boundaryXnY.x) return;
            MoveLeft();
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (transform.position.x > boundaryXnY.x) return;
            MoveRight();
        }
    }

    private void MoveLeft()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }
    private void MoveRight()
    {
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    }
}
