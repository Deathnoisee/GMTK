using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    public float popDuration = 0.3f;
    public Ease popEase = Ease.OutBack;

    private Vector3 originalScale;

    public Sprite deadSprite;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    void OnEnable()
    {
        PlayPopIn();
    }

    public void ChangeSprite()
    {
        this.gameObject.GetComponent<Image>().sprite = deadSprite;
    }

    public void PlayPopIn()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(originalScale, popDuration).SetEase(popEase);
        Debug.Log("Heart popped in!");
    }

    public void PlayPopOut(System.Action onComplete = null)
    {
        transform.DOScale(Vector3.zero, popDuration).SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
                Destroy(gameObject);
            });
    }
}