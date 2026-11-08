using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private float hoverScale;
    [SerializeField] private float defaultScale;
    [SerializeField] private float dragAlpha;
    [SerializeField] private float defaultAlpha;

    private Transform originalParent;
    private int originalSiblingIndex;
    private CanvasGroup canvasGroup;
    private Card card;
    private bool isPlayed;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        card = GetComponent<Card>();
        isPlayed = false;
    }

    public CardInstance GetCardInstance()
    {
        return card.CurrentInstance;
    }

    public void MarkAsPlayed()
    {
        isPlayed = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * defaultScale;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        transform.SetParent(originalParent.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = dragAlpha;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isPlayed)
        {
            return;
        }

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = defaultAlpha;
        transform.SetParent(originalParent);
        transform.SetSiblingIndex(originalSiblingIndex);
    }
}