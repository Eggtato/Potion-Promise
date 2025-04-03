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

    [SerializeField] private RewardManager rewardManager;
    public GameObject rewardMaterialShow;
    [SerializeField] private List<GameObject> rewardMaterialShowList = new List<GameObject>();
    public Transform rewardGrid;

    public Animator anim;
    private bool canTakeMaterial = true;

    [SerializeField] private List<InventoryMaterialSlotUI> InventoryMaterialSlotUIList = new List<InventoryMaterialSlotUI>();
    public List<InventoryMaterialImageUI> InventoryMaterialImageUIList = new List<InventoryMaterialImageUI>();

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
                a.GetComponent<MaterialShowData>().init(materialObject, item);
                materialShowList.Add(a);
                break;
            }
        }
    }

    private void RemoveMaterialShow(GameObject materialObject)
    {
        foreach (GameObject a in materialShowList)
        {
            if (a.GetComponent<MaterialShowData>().getMaterialObject() == materialObject)
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

        StartCoroutine(TakingItemAnim());
            
        GameObject materialObject = materialShowList[0];
        MaterialShowData materialShowData = materialObject.GetComponent<MaterialShowData>();
        MaterialType gatheredMaterialType = materialShowData.getMaterialData().MaterialType;

        InventoryMaterialSlotUI ims = InventoryMaterialSlotUIList.Find(item => item.obtainedMaterialData.MaterialType == gatheredMaterialType);

        //add material
        if (ims != null)
        {
            ims.AddQuantity();
        }
        else
        {
            GameObject a = Instantiate(rewardMaterialShow, rewardGrid);
            ims = a.GetComponent<InventoryMaterialSlotUI>();
            ObtainedMaterialData omd = new ObtainedMaterialData();
            omd.Quantity = 1;
            omd.MaterialType = gatheredMaterialType;
            InventoryMaterialImageUIList.Add(ims.InitializeInGathering(omd, materialDatabaseSO.MaterialDataList.Find(item => item.MaterialType == gatheredMaterialType), rewardManager, gameAssetSO));
            InventoryMaterialSlotUIList.Add(ims);
        }

        Destroy(materialShowData.getMaterialObject());

        materialShowList.Remove(materialObject);
        Destroy(materialObject);

        inventoryCount--;
    }

    private IEnumerator TakingItemAnim()
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
