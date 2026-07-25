using DG.Tweening;
using UnityEngine;

public class Panel : MonoBehaviour
{
    public Transform background;
    public Transform contentParent; // holds all the child objects (at any depth) to stagger in

    public GameObject parent;

    public float backgroundPopDuration = 0.3f;
    public float childPopDuration = 0.25f;
    public float staggerDelay = 0.08f; // gap between each child popping in
    public Ease popEase = Ease.OutBack;
    public Ease popOutEase = Ease.InBack;

    private Vector3 backgroundOriginalScale;
    private SpriteRenderer[] allSprites;
    private Vector3[] childOriginalScales;

    void Awake()
    {
        backgroundOriginalScale = background.localScale;
        parent = this.transform.parent.gameObject;

        // Grab every SpriteRenderer nested anywhere under contentParent, not just direct children
        allSprites = contentParent.GetComponentsInChildren<SpriteRenderer>(true);

        childOriginalScales = new Vector3[allSprites.Length];
        for (int i = 0; i < allSprites.Length; i++)
        {
            childOriginalScales[i] = allSprites[i].transform.localScale;
        }
    }

    void OnEnable()
    {
        PlayPopSequence();
    }

    public void PlayPopSequence()
    {
        // Reset everything to zero first
        background.localScale = Vector3.zero;

        for (int i = 0; i < allSprites.Length; i++)
        {
            if (allSprites[i] != null)
            {
                allSprites[i].transform.localScale = Vector3.zero;
            }
        }

        Sequence seq = DOTween.Sequence();

        // Background pops in first
        seq.Append(background.DOScale(backgroundOriginalScale, backgroundPopDuration).SetEase(popEase));

        // Then each sprite pops in with a staggered overlap
        float startTime = backgroundPopDuration;
        for (int i = 0; i < allSprites.Length; i++)
        {
            if (allSprites[i] == null) continue;

            Transform t = allSprites[i].transform;
            Vector3 targetScale = childOriginalScales[i];

            seq.Insert(startTime, t.DOScale(targetScale, childPopDuration).SetEase(popEase));
            startTime += staggerDelay;
        }
    }

    public void PlayPopOutSequence(System.Action onComplete = null)
    {
        Sequence seq = DOTween.Sequence();

        // Sprites pop out first, in reverse order, staggered
        float startTime = 0f;
        for (int i = allSprites.Length - 1; i >= 0; i--)
        {
            if (allSprites[i] == null) continue;

            Transform t = allSprites[i].transform;

            seq.Insert(startTime, t.DOScale(Vector3.zero, childPopDuration).SetEase(popOutEase));
            startTime += staggerDelay;
        }

        // Background pops out last, after sprites have started/finished
        seq.Insert(startTime, background.DOScale(Vector3.zero, backgroundPopDuration).SetEase(popOutEase));

        seq.OnComplete(() =>
        {
            onComplete?.Invoke();
            gameObject.SetActive(false);
        });
    }
}