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

    [Header("Debug")]
    public bool debugLogging = true;

    private Vector3 backgroundOriginalScale;
    private Transform[] allAnimatedElements; // sprites AND TextMeshPro objects
    private TextMeshPro[] elementTextComponents; // null entry = not a text element (e.g. a sprite)
    private Vector3[] childOriginalScales;
    private bool isPoppingOut = false; // prevents OnDisable from re-triggering pop-out mid-sequence

    void Awake()
    {
        backgroundOriginalScale = background.localScale;
        parent = this.transform.parent.gameObject;

        // Grab every SpriteRenderer AND TextMeshPro nested anywhere under contentParent
        SpriteRenderer[] sprites = contentParent.GetComponentsInChildren<SpriteRenderer>(true);
        TextMeshPro[] texts = contentParent.GetComponentsInChildren<TextMeshPro>(true);

        allAnimatedElements = new Transform[sprites.Length + texts.Length];
        elementTextComponents = new TextMeshPro[sprites.Length + texts.Length];

        int index = 0;
        for (int i = 0; i < sprites.Length; i++)
        {
            allAnimatedElements[index] = sprites[i].transform;
            elementTextComponents[index] = null; // not a text element
            index++;
        }
        for (int i = 0; i < texts.Length; i++)
        {
            allAnimatedElements[index] = texts[i].transform;
            elementTextComponents[index] = texts[i];
            index++;
        }

        childOriginalScales = new Vector3[allAnimatedElements.Length];
        for (int i = 0; i < allAnimatedElements.Length; i++)
        {
            childOriginalScales[i] = allAnimatedElements[i].localScale;
        }

        if (debugLogging)
        {
            Debug.Log($"[Panel:{name}] Awake — found {allAnimatedElements.Length} total elements " +
                      $"({sprites.Length} sprites, {texts.Length} text objects) under '{contentParent.name}'");
        }
    }

    private bool ShouldSkip(int index)
    {
        if (allAnimatedElements[index] == null)
        {
            if (debugLogging) Debug.Log($"[Panel:{name}] Skipping index {index} — element is null/destroyed");
            return true;
        }

        // Skip objects that are currently deactivated
        if (!allAnimatedElements[index].gameObject.activeInHierarchy)
        {
            if (debugLogging) Debug.Log($"[Panel:{name}] Skipping '{allAnimatedElements[index].name}' — deactivated");
            return true;
        }

        // Skip TextMeshPro elements that currently have no text
        TextMeshPro tmp = elementTextComponents[index];
        if (tmp != null && string.IsNullOrEmpty(tmp.text))
        {
            if (debugLogging) Debug.Log($"[Panel:{name}] Skipping '{allAnimatedElements[index].name}' — empty text");
            return true;
        }

        return false;
    }

    void OnEnable()
    {
        PlayPopSequence();
    }

    private void OnDisable()
    {
        if (isPoppingOut)
        {
            // We're disabling ourselves as part of our own pop-out sequence's OnComplete —
            // don't re-trigger another pop-out, that's the loop causing double logs/calls.
            return;
        }

        PlayPopOutSequence();
    }

    public void PlayPopSequence()
    {
        if (debugLogging) Debug.Log($"[Panel:{name}] --- Starting PlayPopSequence (pop IN) ---");

        // Reset everything to zero first
        background.localScale = Vector3.zero;

        for (int i = 0; i < allAnimatedElements.Length; i++)
        {
            if (!ShouldSkip(i))
            {
                allAnimatedElements[i].localScale = Vector3.zero;
            }
        }

        Sequence seq = DOTween.Sequence();

        // Background pops in first
        seq.Append(background.DOScale(backgroundOriginalScale, backgroundPopDuration).SetEase(popEase));
        seq.InsertCallback(0f, () =>
        {
            SoundManager.PlaySound(SoundType.popIn);
            if (debugLogging) Debug.Log($"[Panel:{name}] Background popping IN at t=0");
        });

        // Then each element pops in with a staggered overlap
        float startTime = backgroundPopDuration;
        int includedCount = 0;
        for (int i = 0; i < allAnimatedElements.Length; i++)
        {
            if (ShouldSkip(i)) continue; // skip empty text elements (and null/destroyed ones)

            Transform t = allAnimatedElements[i];
            Vector3 targetScale = childOriginalScales[i];
            float logTime = startTime;
            string elementName = t.name;

            seq.Insert(startTime, t.DOScale(targetScale, childPopDuration).SetEase(popEase));

            // InsertCallback fires the sound exactly when this element STARTS popping,
            // instead of PlaySound running immediately when the loop builds the sequence.
            seq.InsertCallback(startTime, () =>
            {
                SoundManager.PlaySound(SoundType.popIn);
                if (debugLogging) Debug.Log($"[Panel:{name}] '{elementName}' popping IN at t={logTime:F2}s");
            });

            startTime += staggerDelay;
            includedCount++;
        }

        if (debugLogging) Debug.Log($"[Panel:{name}] PlayPopSequence scheduled {includedCount} elements to pop in");
    }

    public void PlayPopOutSequence(System.Action onComplete = null)
    {
        if (isPoppingOut)
        {
            if (debugLogging) Debug.Log($"[Panel:{name}] PlayPopOutSequence called again while already popping out — invoking onComplete immediately since it's already handled");
            onComplete?.Invoke();
            return;
        }

        isPoppingOut = true;

        if (debugLogging) Debug.Log($"[Panel:{name}] --- Starting PlayPopOutSequence (pop OUT) ---");

        Sequence seq = DOTween.Sequence();

        // Elements pop out first, in reverse order, staggered
        float startTime = 0f;
        int includedCount = 0;
        for (int i = allAnimatedElements.Length - 1; i >= 0; i--)
        {
            if (ShouldSkip(i)) continue;

            Transform t = allAnimatedElements[i];
            float logTime = startTime;
            string elementName = t.name;

            seq.Insert(startTime, t.DOScale(Vector3.zero, childPopDuration).SetEase(popOutEase));
            seq.InsertCallback(startTime, () =>
            {
                SoundManager.PlaySound(SoundType.popOut);
                if (debugLogging) Debug.Log($"[Panel:{name}] '{elementName}' popping OUT at t={logTime:F2}s");
            });

            startTime += staggerDelay;
            includedCount++;
        }

        // Background pops out last, after elements have started/finished
        float finalTime = startTime;
        seq.Insert(startTime, background.DOScale(Vector3.zero, backgroundPopDuration).SetEase(popOutEase));
        seq.InsertCallback(startTime, () =>
        {
            SoundManager.PlaySound(SoundType.popOut);
            if (debugLogging) Debug.Log($"[Panel:{name}] Background popping OUT at t={finalTime:F2}s");
        });

        if (debugLogging) Debug.Log($"[Panel:{name}] PlayPopOutSequence scheduled {includedCount} elements to pop out");

        seq.OnComplete(() =>
        {
            if (debugLogging) Debug.Log($"[Panel:{name}] PlayPopOutSequence COMPLETE — deactivating panel");
            onComplete?.Invoke();
            parent.SetActive(false);
            gameObject.SetActive(false); // OnDisable will fire here, but isPoppingOut guards against re-triggering
            isPoppingOut = false; // reset for next time this panel is shown/hidden again
        });
    }
}