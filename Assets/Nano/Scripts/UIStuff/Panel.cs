using DG.Tweening;
using UnityEngine;

public class Panel : MonoBehaviour
{
    public Transform background;
    public Transform contentParent; // holds all the child objects to stagger in

    public GameObject parent;

    public float backgroundPopDuration = 0.3f;
    public float childPopDuration = 0.25f;
    public float staggerDelay = 0.08f; // gap between each child popping in
    public Ease popEase = Ease.OutBack;
    public Ease popOutEase = Ease.InBack;

    private Vector3 backgroundOriginalScale;
    private Vector3[] childOriginalScales;

    void Awake()
    {
        backgroundOriginalScale = background.localScale;
        parent = this.transform.parent.gameObject;

        childOriginalScales = new Vector3[contentParent.childCount];
        for (int i = 0; i < contentParent.childCount; i++)
        {
            childOriginalScales[i] = contentParent.GetChild(i).localScale;
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

        for (int i = 0; i < contentParent.childCount; i++)
        {
            Transform child = contentParent.GetChild(i);
            if (child.GetComponent<SpriteRenderer>() != null)
            {
                child.localScale = Vector3.zero;
            }
        }

        Sequence seq = DOTween.Sequence();

        // Background pops in first
        seq.Append(background.DOScale(backgroundOriginalScale, backgroundPopDuration).SetEase(popEase));

        // Then each child pops in with a staggered overlap
        float startTime = backgroundPopDuration;
        for (int i = 0; i < contentParent.childCount; i++)
        {
            Transform child = contentParent.GetChild(i);
            if (child.GetComponent<SpriteRenderer>() != null)
            {
                Vector3 targetScale = childOriginalScales[i];

                seq.Insert(startTime, child.DOScale(targetScale, childPopDuration).SetEase(popEase));
                startTime += staggerDelay;
            }
        }
    }

    public void PlayPopOutSequence(System.Action onComplete = null)
    {
        Sequence seq = DOTween.Sequence();

        // Children pop out first, in reverse order, staggered
        float startTime = 0f;
        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            Transform child = contentParent.GetChild(i);
            if (child.GetComponent<SpriteRenderer>() != null)
            {
                seq.Insert(startTime, child.DOScale(Vector3.zero, childPopDuration).SetEase(popOutEase));
                startTime += staggerDelay;
            }
        }

        // Background pops out last, after children have started/finished
        seq.Insert(startTime, background.DOScale(Vector3.zero, backgroundPopDuration).SetEase(popOutEase));

        seq.OnComplete(() =>
        {
            onComplete?.Invoke();
            gameObject.SetActive(false);
        });
    }
}