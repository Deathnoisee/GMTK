using UnityEngine;

public class namousa : MonoBehaviour
{
    [Header("Namousa Settings")]
    [SerializeField] private float speed = 2f;
    public float health = 10f;
    [Header("References")]
    public namousManager namousManager;
    public GameObject AttackObject;
    private void Update()
    {
        if (AttackObject != null) MoveTowardsPlayer();
        if (health <= 0)
        {
            namousManager.ReleaseNamous(gameObject);
        }
    }

    private void MoveTowardsPlayer()
    {

        Vector3 direction = (AttackObject.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

    }
    public void TakeDamage(float damage)
    {
        Debug.Log($"Namousa took {damage} damage!, health: {health - damage}");
        health -= Mathf.Max(0f, damage);
    }
}
