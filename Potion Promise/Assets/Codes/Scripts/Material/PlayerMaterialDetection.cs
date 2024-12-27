using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaterialDetection : MonoBehaviour
{
    public List<GameObject> materialShowList = new List<GameObject>();

    public GameObject materialShowPrefab;

    public Transform gridShow;

    [SerializeField] private MaterialDatabaseSO materialDatabaseSO;
    public MaterialDatabaseSO MaterialDatabaseSO => materialDatabaseSO;

    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            getMaterial();
        }
    }

    public void AddMaterialShow(GameObject materialObject)
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

    public void RemoveMaterialShow(GameObject materialObject)
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

    public void getMaterial()
    {
        if (materialShowList.Count <= 0) return;

        GameObject materialObject = materialShowList[0];
        MaterialShowData materialShowData = materialObject.GetComponent<MaterialShowData>();

        //add material

        materialShowList.Remove(materialObject);
        Destroy(materialObject);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "material")
        {
            AddMaterialShow(collision.gameObject);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "material")
        {
            RemoveMaterialShow(collision.gameObject);
        }
    }
}
