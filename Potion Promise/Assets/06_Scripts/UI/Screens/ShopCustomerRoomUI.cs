using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
    [SerializeField] private float customerMoveDuration = 1f;
    [SerializeField] private float customerMoveDelayBetween = 0.5f;
    [SerializeField] private float delayTime = 3f;
    [SerializeField] private Ease customerMoveEase;

    private bool isCustomerMoving;
    private ShopCustomerOrderData currentCustomerOrderData;
    private ShopCustomerManager shopCustomerManager;
    private readonly Queue<Dictionary<ShopCustomerData, ShopCustomerOrderData>> shopCustomerDatas = new();

    public Queue<Dictionary<ShopCustomerData, ShopCustomerOrderData>> ShopCustomerDatas => shopCustomerDatas;
    public bool IsCustomerMoving => isCustomerMoving;

    private void Start()
    {
        npcPanel.gameObject.SetActive(false);
        shopCustomerImageUI.gameObject.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        playerEventSO.Event.OnCustomerRoomOpened += HandleCustomerRoomOpened;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        playerEventSO.Event.OnCustomerRoomOpened -= HandleCustomerRoomOpened;
    }

    public void Initialize(ShopCustomerManager manager)
    {
        shopCustomerManager = manager;

        rejectButton.onClick.RemoveAllListeners();
        rejectButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickSound();
            shopCustomerManager.RejectOrder();
        });

        openShopButton.onClick.RemoveAllListeners();
        openShopButton.onClick.AddListener(() =>
        {
            playerEventSO.Event.OnOpenShopButtonClicked?.Invoke();
            openShopButton.GetComponentInChildren<TMP_Text>().text = closeString;
        });
    }

    public void InitializeCustomer(ShopCustomerData customerData, ShopCustomerOrderData orderData)
    {
        foreach (var line in customerLines)
        {
            if (!line.IsOccupied)
            {
                var newCustomer = Instantiate(shopCustomerImageUI, shopCustomerParent);
                newCustomer.gameObject.SetActive(true);
                newCustomer.InitilizeCustomer(customerData, orderData);
                newCustomer.transform.SetAsFirstSibling();
                newCustomer.GetComponent<Image>().color = line.CustomerColor;

                newCustomer.CanvasGroup.alpha = 0f;
                newCustomer.transform.position = line.Transform.position;
                newCustomer.CanvasGroup.DOFade(1f, gameSettingSO.FadeInAnimation);

                line.IsOccupied = true;
                line.ShopCustomerImageUI = newCustomer;

                shopCustomerDatas.Enqueue(new Dictionary<ShopCustomerData, ShopCustomerOrderData>
                {
                    { customerData, orderData }
                });

                if (shopCustomerDatas.Count == 1)
                {
                    currentCustomerOrderData = orderData;
                    orderText.text = orderData.OrderDescription;
                    SetupNewNPC(newCustomer.GetComponent<Image>(), customerData.Sprite);
                }

                break;
            }
        }
    }

    public IEnumerator ShiftCustomers()
    {
        isCustomerMoving = true;

        FadeOutUI(rejectButton.GetComponent<CanvasGroup>(), orderPanel);

        if (customerLines[0].IsOccupied)
        {
            Destroy(customerLines[0].ShopCustomerImageUI.gameObject);
            customerLines[0].IsOccupied = false;
            customerLines[0].ShopCustomerImageUI = null;
        }

        for (int i = 1; i < customerLines.Count; i++)
        {
            if (customerLines[i].IsOccupied)
            {
                var current = customerLines[i];
                var target = customerLines[i - 1];

                current.ShopCustomerImageUI.transform.DOMove(target.Transform.position, customerMoveDuration).SetEase(customerMoveEase);
                current.ShopCustomerImageUI.GetComponent<Image>().DOColor(target.CustomerColor, customerMoveDuration).SetEase(customerMoveEase);

                target.IsOccupied = true;
                target.ShopCustomerImageUI = current.ShopCustomerImageUI;

                current.IsOccupied = false;
                current.ShopCustomerImageUI = null;

                yield return new WaitForSeconds(customerMoveDelayBetween);
            }
        }

        if (shopCustomerDatas.Count > 0)
        {
            var next = shopCustomerDatas.Peek().First();
            currentCustomerOrderData = next.Value;
            orderText.text = next.Value.OrderDescription;

            var frontCustomer = customerLines[0].ShopCustomerImageUI;
            if (frontCustomer != null)
            {
                SetupNewNPC(frontCustomer.GetComponent<Image>(), next.Key.Sprite);
            }
        }

        isCustomerMoving = false;

        if (shopCustomerManager.IsSpawnQueued)
        {
            shopCustomerManager.SpawnQueuedCustomer();
        }
    }

    private void FadeOutUI(params CanvasGroup[] groups)
    {
        foreach (var group in groups)
        {
            group.DOFade(0f, gameSettingSO.FadeInAnimation).OnComplete(() =>
            {
                group.gameObject.SetActive(false);
            });
        }
    }

    private void SetupNewNPC(Image image, Sprite sprite)
    {
        image.sprite = sprite;

        npcPanel.gameObject.SetActive(true);
        rejectButton.gameObject.SetActive(true);
        orderPanel.gameObject.SetActive(true);

        rejectButton.GetComponent<CanvasGroup>().DOFade(1f, gameSettingSO.FadeInAnimation);
        orderPanel.DOFade(1f, gameSettingSO.FadeInAnimation);
    }

    public void HandleCorrectCustomerOrder()
    {
        orderText.text = currentCustomerOrderData.CorrectOrderDescription;
        rejectButton.gameObject.SetActive(false);

        npcPanel.DOFade(0f, 0.2f).SetDelay(delayTime).OnComplete(() =>
        {
            npcPanel.gameObject.SetActive(false);
            ShopCustomerDatas.Dequeue();
            StartCoroutine(ShiftCustomers());
        });
    }

    public void HandleIncorrectCustomerOrder()
    {
        orderText.text = currentCustomerOrderData.InCorrectOrderDescription;
        rejectButton.gameObject.SetActive(false);

        npcPanel.DOFade(0f, 0.2f).SetDelay(2f).OnComplete(() =>
        {
            npcPanel.gameObject.SetActive(false);
            ShopCustomerDatas.Dequeue();
            StartCoroutine(ShiftCustomers());
        });
    }

    private void HandleCustomerRoomOpened()
    {
        Show();
    }
}
