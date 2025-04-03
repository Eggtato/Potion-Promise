using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardManager : MonoBehaviour
{
    [SerializeField] private GameObject rewardScreen;
    [SerializeField] private TMP_Text rewardTxt;
    [SerializeField] private GameObject toNextSceneBtn;
    [SerializeField] private GameObject confirmMaterialBtn;
    [SerializeField] private GameObject confirmPortalPanel;

    [SerializeField] private PlayerMaterialDetection playerMaterialDetection;
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private GameDataManager gameDataManager;

    private MaterialData materialDataSelected = null;


    public void PassedOut()
    {
        toNextSceneBtn.SetActive(false);
        confirmMaterialBtn.SetActive(true);
        rewardTxt.text = "Passed Out!";

        UnselectAll();

        rewardScreen.SetActive(true);
        playerMovement.SetInRewardScreen();
    }

    public void SetMaterialSelected(MaterialData materialDataSelected)
    {
        this.materialDataSelected = materialDataSelected;
    }

    public void UnselectAll()
    {
        foreach (InventoryMaterialImageUI a in playerMaterialDetection.InventoryMaterialImageUIList)
        {
            a.Unselect();
        }

        materialDataSelected = null;
    }

    public void ResetMaterialDataSelected()
    {
        materialDataSelected = null;
    }

    public void ConfirmItemSelected()
    {
        if (playerMaterialDetection.InventoryMaterialImageUIList.Count == 0)
        {
            ToNextScene();
        }

        if (materialDataSelected == null) return;

        gameDataManager.AddObtainedMaterial(materialDataSelected);

        ToNextScene();
    }

    public void GoBackThroughPortal()
    {
        confirmPortalPanel.SetActive(true);
        playerMovement.SetInRewardScreen();
    }

    public void CancelGoBackThroughPortal()
    {
        confirmPortalPanel.SetActive(false);
        playerMovement.SetInRewardScreen();
    }

    public void ConfirmGoBackThroughPortal()
    {
        confirmPortalPanel.SetActive(false);
        toNextSceneBtn.SetActive(true);
        confirmMaterialBtn.SetActive(false);

        foreach (InventoryMaterialImageUI a in playerMaterialDetection.InventoryMaterialImageUIList)
        {
            gameDataManager.AddObtainedMaterial(a.MaterialData);
        }

        rewardScreen.SetActive(true);
        playerMovement.SetInRewardScreen();
    }

    public void ToNextScene()
    {
        Debug.Log("Gathering Done");
        playerEventSO.Event.OnGoToNextScene?.Invoke();
    }
}
