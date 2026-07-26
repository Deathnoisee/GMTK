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

    [Header("Arrow Fill")]
    [SerializeField] private Transform arrowFill;
    [SerializeField] private float pulseStart = 0.8f;
    [SerializeField] private float pulseSpeed = 12f;
    [SerializeField] private float pulseAmount = 0.15f;
    [SerializeField] private LayerMask scaleLayerMask; // optional: scale other objects on this layer too

    [Header("Sentence Manager")]
    [SerializeField] private sentenceManager sentenceManager;

    private bool isHolding;
    private float charge;
    private Vector3 fullFillScale;
    private Vector3 fullFillPosition;
    private Vector3 arrowFillOriginalScale;
    private Vector3 arrowOriginalScale; // full scale for the whole Arrow object
    private float amount;
    private float angle;
    private GameObject throwable;
    private float initialAngle;

    void Start()
    {
        if (arrowFill != null)
        {
            fullFillScale = arrowFill.localScale;       // full/100% fill
            fullFillPosition = arrowFill.localPosition; // position at full
            // fallback if inspector accidentally set scale to zero
            if (fullFillScale == Vector3.zero) fullFillScale = Vector3.one;
            arrowFillOriginalScale = fullFillScale;
        }

        if (Arrow != null)
        {
            arrowOriginalScale = Arrow.transform.localScale;
            initialAngle = Arrow.transform.rotation.eulerAngles.z;
        }
    }

    void Update()
    {
        if (!isHolding || arrowFill == null) return;
        if (throwable != null) return;

        charge = Mathf.Min(charge + chargeSpeed * Time.deltaTime, maxCharge);
        float t = charge / maxCharge;

        SetFill01(t);

        amount = Mathf.PingPong(Time.time * sweepSpeed, 1f);
        angle = Mathf.Lerp(-sweepAngle * 0.5f, sweepAngle * 0.5f, amount);

        if (Arrow != null)
            Arrow.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void SetFill01(float t)
    {
        if (arrowFill == null) return;

        t = Mathf.Clamp01(t);

        float pulse = 1f;
        if (t >= pulseStart)
            pulse = 1f + Mathf.PingPong(Time.time * pulseSpeed, pulseAmount);

        // Uniformly scale on all axes based on t (with optional pulsing).
        // Do NOT modify localPosition — user provided custom pivot handles anchoring.
        float scaleFactor = Mathf.Max(0.0001f, t) * pulse;
        Vector3 baseFill = arrowFillOriginalScale != Vector3.zero ? arrowFillOriginalScale : fullFillScale;
        arrowFill.localScale = baseFill * scaleFactor;

        // Optionally scale the whole Arrow uniformly if it's on the configured layer.
        if (scaleLayerMask != 0 && Arrow != null)
        {
            if ((scaleLayerMask.value & (1 << Arrow.layer)) != 0)
            {
                Vector3 baseArrow = arrowOriginalScale != Vector3.zero ? arrowOriginalScale : Vector3.one;
                Arrow.transform.localScale = baseArrow * scaleFactor;
            }
        }
    }

    void OnMouseDown()
    {

        isHolding = true;
        charge = 0f;
        SetFill01(0f);
    }

    void OnMouseUp()
    {
        isHolding = false;
        if (throwable != null) return;

        SetFill01(0f);

        if (ThrowablePrefab != null)
        {
            Vector2 dir = Quaternion.Euler(0f, 0f, angle) * baseDirection.normalized;

            Vector3 spawnPos = transform.position;
            if (spawnPoint != null)
                spawnPos = spawnPoint.position;
            else if (Arrow != null)
                spawnPos = Arrow.transform.position + (Vector3)(dir * spawnOffset);

            throwable = Instantiate(ThrowablePrefab, spawnPos, Quaternion.identity);
            throwable.GetComponent<Death>().sentenceManager = sentenceManager;

            Rigidbody2D rb = throwable.GetComponent<Rigidbody2D>();

            if (Arrow != null)
                Arrow.transform.rotation = Quaternion.Euler(0f, 0f, initialAngle);

            if (rb != null)
                rb.AddForce(dir * charge, ForceMode2D.Impulse);
        }
    }
}