using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private float hoverScale = 1.15f;
    [SerializeField] private float defaultScale = 1f;
    [SerializeField] private float dragAlpha = 0.7f;
    [SerializeField] private float defaultAlpha = 1f;
    [SerializeField] private float animationSpeed = 15f;

    public bool IsHovered { get; private set; }
    public bool IsDragging { get; private set; }

    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Card card;
    private HandFanLayout handLayout;
    private bool isPlayed;

    private Vector2 targetAnchoredPosition;
    private Quaternion targetRotation;
    private Vector3 targetScale;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        card = GetComponent<Card>();
        isPlayed = false;

        targetRotation = Quaternion.identity;
        targetScale = Vector3.one * defaultScale;
    }

    public void InitializeLayout(HandFanLayout layout)
    {
        handLayout = layout;
    }

    public void SetTargetTransform(Vector2 anchoredPosition, Quaternion rotation, bool isHoveredCard)
    {
        targetAnchoredPosition = anchoredPosition;
        targetRotation = rotation;

        float scaleMultiplier = isHoveredCard ? hoverScale : defaultScale;
        targetScale = Vector3.one * scaleMultiplier;
    }

    private void Update()
    {
        if (isPlayed || IsDragging)
        {
            return;
        }

        float interpolationStep = Time.deltaTime * animationSpeed;
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetAnchoredPosition, interpolationStep);
        rectTransform.localRotation = Quaternion.Lerp(rectTransform.localRotation, targetRotation, interpolationStep);
        rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, targetScale, interpolationStep);
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
        if (IsDragging || isPlayed)
        {
            return;
        }

        IsHovered = true;

        if (handLayout != null)
        {
            handLayout.UpdateLayout();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsDragging || isPlayed)
        {
            return;
        }

        IsHovered = false;

        if (handLayout != null)
        {
            handLayout.UpdateLayout();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        IsDragging = true;
        IsHovered = false;

        originalParent = transform.parent;
        transform.SetParent(originalParent.root);

        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one * hoverScale;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = dragAlpha;

        if (handLayout != null)
        {
            handLayout.UpdateLayout();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsDragging = false;

        if (isPlayed)
        {
            return;
        }

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = defaultAlpha;
        transform.SetParent(originalParent);

        if (handLayout != null)
        {
            handLayout.UpdateLayout();
        }
    }
}