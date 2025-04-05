using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MaterialShowData : MonoBehaviour
{
    public GameObject materialObject { get; private set; }
    public MaterialData materialData { get; private set; }

    public Image icon;
    public TMP_Text nametxt;

    public void Initialize(GameObject materialObject, MaterialData materialData)
    {
        this.materialObject = materialObject;
        this.materialData = materialData;

        icon.sprite = materialData.Sprite;
        nametxt.text = materialData.Name;
    }
}
