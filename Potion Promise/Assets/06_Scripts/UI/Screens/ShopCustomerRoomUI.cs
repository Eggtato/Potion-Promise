using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Febucci.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCustomerRoomUI : BaseUI
{
    [Serializable]
    public class CustomerLine
    {
        public Transform Transform;
        [HideInInspector] public bool IsOccupied;
        [HideInInspector] public ShopCustomerImageUI ShopCustomerImageUI;
        public Color32 CustomerColor;
    }

    [Header("Project Reference")]
    [SerializeField] private GameSettingSO gameSettingSO;

    [Header("NPC UI")]
    [SerializeField] private CanvasGroup npcPanel;
    [SerializeField] private List<CustomerLine> customerLines = new();
    [SerializeField] private CanvasGroup orderPanel;
    [SerializeField] private TMP_Text orderText;
    [SerializeField] private Button rejectButton;
    [SerializeField] private Transform shopCustomerParent;
    [SerializeField] private ShopCustomerImageUI shopCustomerImageUI;

    [Header("Counter")]
    [SerializeField] private string openString = "OPEN";
    [SerializeField] private string closeString = "CLOSE";
    [SerializeField] private Button openShopButton;

    [Header("Animation")]
    [SerializeField] private float orderAppearDelay = 0.5f;
    [SerializeField] private float customerMoveDuration = 1f;
    [SerializeField] private float customerMoveDelay = 0.5f;
    [SerializeField] private float exitMoveOffsetX = 50f;
    [SerializeField] private Ease moveEase;
    [SerializeField] private Ease fadeEase;
    [SerializeField] private Ease orderEase;

    private bool isCustomerMoving;
    private ShopCustomerOrderData currentOrder;
    private ShopCustomerManager manager;
    private readonly Queue<Dictionary<ShopCustomerData, ShopCustomerOrderData>> customerQueue = new();

    public Queue<Dictionary<ShopCustomerData, ShopCustomerOrderData>> CustomerQueue => customerQueue;
    public bool IsCustomerMoving => isCustomerMoving;

    private void Start()
    {
        npcPanel.gameObject.SetActive(false);
        shopCustomerImageUI.gameObject.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        playerEventSO.Event.OnCustomerRoomOpened += Show;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        playerEventSO.Event.OnCustomerRoomOpened -= Show;
    }

    public void Initialize(ShopCustomerManager customerManager)
    {
        manager = customerManager;

        rejectButton.onClick.RemoveAllListeners();
        rejectButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickSound();
            StartCoroutine(HandleReject());
        });

        openShopButton.onClick.RemoveAllListeners();
        openShopButton.onClick.AddListener(() =>
        {
            playerEventSO.Event.OnOpenShopButtonClicked?.Invoke();
            openShopButton.GetComponentInChildren<TMP_Text>().text = closeString;
        });
    }

    // Initializes a new customer and places them in the first available line
    public void InitializeCustomer(ShopCustomerData data, ShopCustomerOrderData order)
    {
        foreach (var line in customerLines)
        {
            if (line.IsOccupied) continue; // Skip if the line is already occupied

            // Instantiate customer UI object and parent it correctly
            var customer = Instantiate(shopCustomerImageUI, shopCustomerParent);
            customer.gameObject.SetActive(true);

            // Initialize the customer with provided data and order
            customer.InitilizeCustomer(data, order);
            customer.transform.SetAsFirstSibling(); // Set to render in front of other customers
            customer.GetComponent<Image>().color = line.CustomerColor; // Assign color for the line

            // Fade-in animation for the customer UI
            customer.CanvasGroup.alpha = 0f;
            customer.transform.position = line.Transform.position;
            customer.CanvasGroup.DOFade(1f, gameSettingSO.CustomerFadeDuration).SetEase(fadeEase);
            customer.CanvasGroup.blocksRaycasts = false;

            // Mark the line as occupied and assign the customer to it
            line.IsOccupied = true;
            line.ShopCustomerImageUI = customer;

            // Enqueue the customer into the customer queue
            customerQueue.Enqueue(new() { { data, order } });

            // If this is the only customer in queue, display their order immediately
            if (customerQueue.Count == 1)
            {
                currentOrder = order;
                orderText.text = order.OrderDescription;
                customer.CanvasGroup.blocksRaycasts = true;
                SetupCustomerVisual(customer.GetComponent<Image>(), data.Sprite);
            }
            break; // Stop after placing the customer in the first available spot
        }
    }

    // Moves all customers forward in line and removes the one at the front
    public IEnumerator ShiftCustomers()
    {
        isCustomerMoving = true;

        // Fade out UI panels for the current customer
        FadeOutUI(rejectButton.GetComponent<CanvasGroup>(), orderPanel);

        // Remove the first customer in line if present
        if (customerLines[0].IsOccupied)
        {
            var first = customerLines[0].ShopCustomerImageUI;
            // Move and fade out animation before destroying the object
            first.GetComponent<RectTransform>().DOLocalMoveX(exitMoveOffsetX, gameSettingSO.CustomerFadeDuration).SetEase(fadeEase);
            first.CanvasGroup.DOFade(0, gameSettingSO.CustomerFadeDuration).SetEase(fadeEase).OnComplete(() =>
            {
                Destroy(first.gameObject);
                customerLines[0].IsOccupied = false;
                customerLines[0].ShopCustomerImageUI = null;
            });

            // Wait for the animation to finish
            yield return new WaitForSeconds(gameSettingSO.CustomerFadeDuration);
        }

        // Shift each remaining customer forward in line
        for (int i = 1; i < customerLines.Count; i++)
        {
            if (!customerLines[i].IsOccupied) continue;

            var current = customerLines[i];
            var target = customerLines[i - 1];

            // Animate movement and color transition to the new line
            current.ShopCustomerImageUI.transform.DOMove(target.Transform.position, customerMoveDuration).SetEase(moveEase);
            current.ShopCustomerImageUI.GetComponent<Image>().DOColor(target.CustomerColor, customerMoveDuration).SetEase(moveEase);

            // Reassign customer references
            target.IsOccupied = true;
            target.ShopCustomerImageUI = current.ShopCustomerImageUI;

            current.IsOccupied = false;
            current.ShopCustomerImageUI = null;

            // Allow interaction only for the new front customer
            target.ShopCustomerImageUI.CanvasGroup.blocksRaycasts = (i - 1 == 0);

            // Wait briefly before moving the next customer
            yield return new WaitForSeconds(customerMoveDelay);
        }

        // Update the order panel to reflect the next customer in queue
        if (customerQueue.Count > 0)
        {
            var next = customerQueue.Peek().First();
            currentOrder = next.Value;
            orderText.text = currentOrder.OrderDescription;

            var front = customerLines[0].ShopCustomerImageUI;
            if (front != null)
            {
                SetupCustomerVisual(front.GetComponent<Image>(), next.Key.Sprite);
            }
        }

        isCustomerMoving = false;

        // If another spawn was queued during shifting, spawn it now
        if (manager.IsSpawnQueued)
        {
            manager.SpawnQueuedCustomer();
        }
    }

    // Updates the UI and visuals for the current customer at the front
    private void SetupCustomerVisual(Image image, Sprite sprite)
    {
        image.sprite = sprite;

        // Show NPC-related panels and order/reject UI
        npcPanel.gameObject.SetActive(true);
        rejectButton.GetComponent<CanvasGroup>().DOFade(0, 0).SetEase(orderEase);
        rejectButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
        orderPanel.DOFade(0, 0);

        rejectButton.gameObject.SetActive(true);
        orderPanel.gameObject.SetActive(true);

        // Fade in order panel with delay
        orderPanel.DOFade(1f, gameSettingSO.CustomerFadeDuration).SetDelay(orderAppearDelay).SetEase(orderEase).OnComplete(() =>
        {
            // Start typewriter effect for order text
            var typewriter = orderText.GetComponent<TypewriterByCharacter>();
            typewriter.StartShowingText();
            typewriter.onTextShowed.RemoveAllListeners();
            typewriter.onTextShowed.AddListener(() =>
            {
                // Once text finishes, fade in the reject button and make it interactable
                rejectButton.GetComponent<CanvasGroup>().DOFade(1f, gameSettingSO.CustomerFadeDuration / 3).SetEase(orderEase);
                rejectButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
            });
        });
    }

    private void FadeOutUI(params CanvasGroup[] groups)
    {
        foreach (var group in groups)
        {
            group.DOFade(0f, gameSettingSO.FadeInAnimation).OnComplete(() => group.gameObject.SetActive(false));
        }
    }

    public IEnumerator HandleReject()
    {
        StartCoroutine(PlayOrderResponse(currentOrder.DeclineDescription, manager.RejectOrder));
        yield return null;
    }

    public IEnumerator HandleCorrectOrder() => PlayOrderResponse(currentOrder.CorrectOrderDescription, manager.RejectOrder);
    public IEnumerator HandleIncorrectOrder() => PlayOrderResponse(currentOrder.InCorrectOrderDescription, manager.RejectOrder);

    private IEnumerator PlayOrderResponse(string message, Action callback = null)
    {
        orderText.text = message;
        yield return null;

        var typewriter = orderText.GetComponent<TypewriterByCharacter>();
        typewriter.StartShowingText();

        FadeOutUI(rejectButton.GetComponent<CanvasGroup>());
        rejectButton.GetComponent<CanvasGroup>().blocksRaycasts = false;

        typewriter.onTextShowed.RemoveAllListeners();
        typewriter.onTextShowed.AddListener(() =>
        {
            FadeOutUI(orderPanel);
            callback?.Invoke();
        });
    }
}