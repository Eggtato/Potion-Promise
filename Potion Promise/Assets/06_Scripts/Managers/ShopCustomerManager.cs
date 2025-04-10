using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

public class ShopCustomerManager : MonoBehaviour
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private GameSettingSO gameSettingSO;
    [SerializeField] private ShopCustomerDatabaseSO shopCustomerDatabaseSO;

    [Header("Scene Reference")]
    [SerializeField] private ShopCustomerRoomUI shopCustomerRoomUI;
    [SerializeField] private int maxCustomerLine = 3;
    [SerializeField] private float maxCustomerSpawnTime = 3f;

    private float customerSpawnTimer;
    private bool isShopOpened;
    private bool isSpawnQueued;
    private int customerSpawnedToday = 0;
    private ShopCustomerOrderData currentOrder;

    public bool IsSpawnQueued => isSpawnQueued;

    private void OnEnable()
    {
        playerEventSO.Event.OnOpenShopButtonClicked += HandleShopOpened;
        playerEventSO.Event.OnPotionDroppedOnCustomer += ProcessPotionDrop;
    }

    private void OnDisable()
    {
        playerEventSO.Event.OnOpenShopButtonClicked -= HandleShopOpened;
        playerEventSO.Event.OnPotionDroppedOnCustomer -= ProcessPotionDrop;
    }

    private void Start()
    {
        shopCustomerRoomUI.Initialize(this);
        customerSpawnTimer = maxCustomerSpawnTime;
        ResetCustomerCount();
    }

    private void Update()
    {
        if (!ShouldSpawnCustomer()) return;

        customerSpawnTimer -= Time.deltaTime;
        if (customerSpawnTimer <= 0f)
        {
            if (shopCustomerRoomUI.IsCustomerMoving)
            {
                isSpawnQueued = true;
            }
            else
            {
                SpawnCustomer();
            }

            customerSpawnTimer = maxCustomerSpawnTime;
        }
    }

    private bool ShouldSpawnCustomer()
    {
        return isShopOpened && !isSpawnQueued && shopCustomerRoomUI.CustomerQueue.Count < maxCustomerLine;
    }

    private void HandleShopOpened()
    {
        isShopOpened = true;
        SpawnCustomer(); // spawn one immediately
    }

    private void SpawnCustomer()
    {
        if (!CanSpawnCustomer()) return;

        var randomCustomer = shopCustomerDatabaseSO.GetRandomCustomer();
        var randomCustomerOrder = randomCustomer.GetRandomOrder();

        shopCustomerRoomUI.InitializeCustomer(randomCustomer, randomCustomerOrder);
        customerSpawnedToday++; // <-- Track the number of spawned customers
    }


    public void SpawnQueuedCustomer()
    {
        if (CanSpawnCustomer())
        {
            SpawnCustomer();
        }
        isSpawnQueued = false;
    }

    public void FinishOrder()
    {
        if (shopCustomerRoomUI.CustomerQueue.Count == 0) return;

        shopCustomerRoomUI.CustomerQueue.Dequeue();

        if (CanSpawnCustomer())
        {
            isSpawnQueued = true;
        }

        StartCoroutine(shopCustomerRoomUI.ShiftCustomers());
    }

    private bool CanSpawnCustomer()
    {
        return !shopCustomerRoomUI.IsCustomerMoving &&
               shopCustomerRoomUI.CustomerQueue.Count < maxCustomerLine &&
               customerSpawnedToday < gameSettingSO.ShopCustomerLimitPerDay;
    }


    public void ProcessPotionDrop(PotionData potionData)
    {
        currentOrder = shopCustomerRoomUI.CustomerQueue.Peek().First().Value;

        if (currentOrder == null)
        {
            Debug.LogWarning("No current order set.");
            return;
        }

        bool isCorrectOrder = currentOrder.OrderedPotion == potionData.PotionType;

        if (isCorrectOrder)
        {
            GameLevelManager.Instance.AddEarnedCoin(potionData.Price);
            StartCoroutine(shopCustomerRoomUI.HandleCorrectOrder());
        }
        else
        {
            StartCoroutine(shopCustomerRoomUI.HandleIncorrectOrder());
        }
    }

    public void ResetCustomerCount()
    {
        customerSpawnedToday = 0;
    }

}
