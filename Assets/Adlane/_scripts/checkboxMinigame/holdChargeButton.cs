using UnityEngine;

public class HoldChargeButton : MonoBehaviour
{
    [Header("Charge Settings")]
    [SerializeField] private float maxCharge = 5f;
    [SerializeField] private float chargeSpeed = 1f;
    [SerializeField] private GameObject ThrowablePrefab;

    [Header("Direction Sweep")]
    [SerializeField] private float sweepSpeed = 2f;
    [SerializeField] private float sweepAngle = 90f;
    [SerializeField] private Vector2 baseDirection = Vector2.down;
    [SerializeField] private GameObject Arrow;

    [Header("Spawn")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnOffset = 0.4f;

    [Header("Bar")]
    [SerializeField] private Transform chargeBar;
    [SerializeField] private float pulseStart = 0.8f;
    [SerializeField] private float pulseSpeed = 12f;
    [SerializeField] private float pulseAmount = 0.15f;

    private bool isHolding;
    private float charge;
    private Vector3 originalBarScale;
    private Vector3 originalBarPosition;
    private float amount;
    private float angle;
    private GameObject throwable;
    private float initialAngle;
    void Start()
    {
        if (chargeBar != null)
        {
            originalBarScale = chargeBar.localScale;
            originalBarPosition = chargeBar.localPosition;
        }

        if (Arrow != null)
            initialAngle = Arrow.transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        if (!isHolding || chargeBar == null) return;
        if (throwable != null) return;

        charge = Mathf.Min(charge + chargeSpeed * Time.deltaTime, maxCharge);
        float t = charge / maxCharge;

        float pulse = 1f;
        if (t >= pulseStart)
            pulse = 1f + Mathf.PingPong(Time.time * pulseSpeed, pulseAmount);

        float newHeight = originalBarScale.y + t;
        chargeBar.localScale = new Vector3(originalBarScale.x * pulse, newHeight, originalBarScale.z * pulse);

        chargeBar.localPosition = new Vector3(
            originalBarPosition.x,
            originalBarPosition.y + (newHeight - originalBarScale.y) * 0.5f,
            originalBarPosition.z
        );

        amount = Mathf.PingPong(Time.time * sweepSpeed, 1f);
        angle = Mathf.Lerp(-sweepAngle * 0.5f, sweepAngle * 0.5f, amount);

        if (Arrow != null)
            Arrow.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void OnMouseDown()
    {
        isHolding = true;
        charge = 0f;
    }

    void OnMouseUp()
    {
        isHolding = false;
        if (throwable != null) return;
        if (chargeBar != null)
        {
            chargeBar.localScale = originalBarScale;
            chargeBar.localPosition = originalBarPosition;
        }

        if (Arrow != null)

            if (ThrowablePrefab != null)
            {
                Vector2 dir = Quaternion.Euler(0f, 0f, angle) * baseDirection.normalized;

                Vector3 spawnPos = transform.position;
                if (spawnPoint != null)
                    spawnPos = spawnPoint.position;
                else if (Arrow != null)
                    spawnPos = Arrow.transform.position + (Vector3)(dir * spawnOffset);

                throwable = Instantiate(ThrowablePrefab, spawnPos, Quaternion.identity);
                Rigidbody2D rb = throwable.GetComponent<Rigidbody2D>();

                Arrow.transform.rotation = Quaternion.Euler(0f, 0f, initialAngle);

                if (rb != null) rb.AddForce(dir * charge, ForceMode2D.Impulse);
            }
    }
}