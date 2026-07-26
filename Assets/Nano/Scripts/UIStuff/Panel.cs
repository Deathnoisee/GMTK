using DG.Tweening;
using UnityEngine;
using TMPro;
using SmallHedge.SoundManager;

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
    private Transform[] allAnimatedElements; // sprites AND TextMeshPro objects
    private Vector3[] childOriginalScales;

    void Awake()
    {
        backgroundOriginalScale = background.localScale;
        parent = this.transform.parent.gameObject;

        // Grab every SpriteRenderer AND TextMeshPro nested anywhere under contentParent
        SpriteRenderer[] sprites = contentParent.GetComponentsInChildren<SpriteRenderer>(true);
        TextMeshPro[] texts = contentParent.GetComponentsInChildren<TextMeshPro>(true);

        allAnimatedElements = new Transform[sprites.Length + texts.Length];

        int index = 0;
        for (int i = 0; i < sprites.Length; i++)
        {
            allAnimatedElements[index] = sprites[i].transform;
            index++;
        }
        for (int i = 0; i < texts.Length; i++)
        {
            allAnimatedElements[index] = texts[i].transform;
            index++;
        }

        childOriginalScales = new Vector3[allAnimatedElements.Length];
        for (int i = 0; i < allAnimatedElements.Length; i++)
        {
            childOriginalScales[i] = allAnimatedElements[i].localScale;
        }
    }

    void OnEnable()
    {
        PlayPopSequence();
    }

    private void OnDisable()
    {
        PlayPopOutSequence();
    }

    public void PlayPopSequence()
    {
        // Reset everything to zero first
        background.localScale = Vector3.zero;

        for (int i = 0; i < allAnimatedElements.Length; i++)
        {
            if (allAnimatedElements[i] != null)
            {
                allAnimatedElements[i].localScale = Vector3.zero;
            }
        }

        Sequence seq = DOTween.Sequence();

        // Background pops in first
        seq.Append(background.DOScale(backgroundOriginalScale, backgroundPopDuration).SetEase(popEase));
        seq.InsertCallback(0f, () => SoundManager.PlaySound(SoundType.popIn));

        // Then each element pops in with a staggered overlap
        float startTime = backgroundPopDuration;
        for (int i = 0; i < allAnimatedElements.Length; i++)
        {
            if (allAnimatedElements[i] == null) continue;

            Transform t = allAnimatedElements[i];
            Vector3 targetScale = childOriginalScales[i];

            seq.Insert(startTime, t.DOScale(targetScale, childPopDuration).SetEase(popEase));

            // InsertCallback fires the sound exactly when this element STARTS popping,
            // instead of PlaySound running immediately when the loop builds the sequence.
            seq.InsertCallback(startTime, () => SoundManager.PlaySound(SoundType.popIn));

            startTime += staggerDelay;
        }
    }

    public void PlayPopOutSequence(System.Action onComplete = null)
    {
        Sequence seq = DOTween.Sequence();

        // Elements pop out first, in reverse order, staggered
        float startTime = 0f;
        for (int i = allAnimatedElements.Length - 1; i >= 0; i--)
        {
            if (allAnimatedElements[i] == null) continue;

            Transform t = allAnimatedElements[i];

            seq.Insert(startTime, t.DOScale(Vector3.zero, childPopDuration).SetEase(popOutEase));
            seq.InsertCallback(startTime, () => SoundManager.PlaySound(SoundType.popOut));

            startTime += staggerDelay;
        }

        // Background pops out last, after elements have started/finished
        seq.Insert(startTime, background.DOScale(Vector3.zero, backgroundPopDuration).SetEase(popOutEase));
        seq.InsertCallback(startTime, () => SoundManager.PlaySound(SoundType.popOut));

        seq.OnComplete(() =>
        {
            onComplete?.Invoke();
            parent.SetActive(false);
            gameObject.SetActive(false);
        });
    }
}