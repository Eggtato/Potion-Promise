using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaterialDetection : MonoBehaviour
{
    [SerializeField] private List<GameObject> materialShowList = new List<GameObject>();

    public GameObject materialShowPrefab;

    public Transform gridShow;

    [SerializeField] private MaterialDatabaseSO materialDatabaseSO;
    public MaterialDatabaseSO MaterialDatabaseSO => materialDatabaseSO;

    [SerializeField] private GameAssetSO gameAssetSO; 
    public GameAssetSO GameAssetSO => gameAssetSO;

    [SerializeField] private RewardManagerUI rewardManager;
    public GameObject rewardMaterialShow;
    [SerializeField] private List<GameObject> rewardMaterialShowList = new List<GameObject>();
    public Transform rewardGrid;

    public Animator anim;
    private bool canTakeMaterial = true;

    public List<InventoryMaterialSlotUIGathering> inventoryMaterialSlotUIGatheringList = new List<InventoryMaterialSlotUIGathering>();

    [SerializeField] private int inventoryCount = 20;

    private void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            GetMaterial();
        }
    }

    private void AddMaterialShow(GameObject materialObject)
    {
        GameObject a = Instantiate(materialShowPrefab, gridShow);

        MaterialType materialType = materialObject.GetComponent<MaterialObjectData>().materialType;

        foreach (var item in materialDatabaseSO.MaterialDataList)
        {
            if (item.MaterialType == materialType)
            {
                a.GetComponent<MaterialShowData>().Initialize(materialObject, item);
                materialShowList.Add(a);
                break;
            }
        }
    }

    private void RemoveMaterialShow(GameObject materialObject)
    {
        foreach (GameObject a in materialShowList)
        {
            if (a.GetComponent<MaterialShowData>().materialObject == materialObject)
            {
                materialShowList.Remove(a);
                Destroy(a);
                return;
            }
        }
    }

    private void GetMaterial()
    {
        if (materialShowList.Count <= 0 || !canTakeMaterial || inventoryCount <= 0 ) return;

        StartCoroutine(TakeItemRoutine());
            
        GameObject materialObject = materialShowList[0];
        MaterialShowData materialShowData = materialObject.GetComponent<MaterialShowData>();
        MaterialType gatheredMaterialType = materialShowData.materialData.MaterialType;

        InventoryMaterialSlotUIGathering inventoryMaterialSlot = inventoryMaterialSlotUIGatheringList.Find(item => item.obtainedMaterialData.MaterialType == gatheredMaterialType);

        //add material
        if (inventoryMaterialSlot != null)
        {
            inventoryMaterialSlot.AddQuantity();
        }
        else
        {
            GameObject a = Instantiate(rewardMaterialShow, rewardGrid);
            inventoryMaterialSlot = a.GetComponent<InventoryMaterialSlotUIGathering>();
            ObtainedMaterialData obtainedMaterialData = new ObtainedMaterialData();
            obtainedMaterialData.Quantity = 1;
            obtainedMaterialData.MaterialType = gatheredMaterialType;
            inventoryMaterialSlot.Initialize(obtainedMaterialData, materialDatabaseSO.MaterialDataList.Find(item => item.MaterialType == gatheredMaterialType), rewardManager, gameAssetSO);
            inventoryMaterialSlotUIGatheringList.Add(inventoryMaterialSlot);
        }

        Destroy(materialShowData.materialObject);

        materialShowList.Remove(materialObject);
        Destroy(materialObject);

        inventoryCount--;
    }

    private IEnumerator TakeItemRoutine()
    {
        canTakeMaterial = false;
        anim.SetBool("takingitem", true);
        yield return new WaitForSeconds(1.05f);
        anim.SetBool("takingitem", false);
        canTakeMaterial = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "material")
        {
            AddMaterialShow(collision.gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "material")
        {
            RemoveMaterialShow(collision.gameObject);
        }
    }
}
