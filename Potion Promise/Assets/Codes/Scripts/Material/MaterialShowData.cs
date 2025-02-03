using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MaterialShowData : MonoBehaviour
{
    GameObject materialObject;
    MaterialData materialData;

    public Image icon;
    public TMP_Text nametxt;

    public void init(GameObject materialObject, MaterialData materialData)
    {
        this.materialObject = materialObject;
        this.materialData = materialData;

        icon.sprite = materialData.Sprite;
        nametxt.text = materialData.Name;
    }

    public GameObject getMaterialObject()
    {
        return materialObject;
    }

    public MaterialData getMaterialData()
    {
        return materialData;
    }
}
