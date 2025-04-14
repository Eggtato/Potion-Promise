using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject rewardScreen;
    [SerializeField] private TMP_Text rewardTxt;
    [SerializeField] private GameObject toNextSceneBtn;
    [SerializeField] private GameObject confirmMaterialBtn;
    [SerializeField] private GameObject confirmPortalPanel;

    [SerializeField] private PlayerMaterialDetection playerMaterialDetection;
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private PlayerEventSO playerEventSO;

    private MaterialData materialDataSelected = null;


    public void PassOut()
    {
        toNextSceneBtn.SetActive(true);
        // confirmMaterialBtn.SetActive(true);
        rewardTxt.text = "Passed Out!";

        // foreach (InventoryMaterialSlotUIGathering a in playerMaterialDetection.inventoryMaterialSlotUIGatheringList)
        // {
        //     a.inventoryMaterialImageUIGathering.canBeSelected = true;
        // }

        // UnselectAll();

        rewardScreen.SetActive(true);
        playerMovement.SetInRewardScreen();
        playerEventSO.Event.OnDayEnd?.Invoke();
    }

    public void SetMaterialSelected(MaterialData materialDataSelected)
    {
        this.materialDataSelected = materialDataSelected;
    }

    public void UnselectAll()
    {
        foreach (InventoryMaterialSlotUIGathering a in playerMaterialDetection.inventoryMaterialSlotUIGatheringList)
        {
            a.inventoryMaterialImageUIGathering.Unselect();
        }

        materialDataSelected = null;
    }

    public void ResetMaterialDataSelected()
    {
        materialDataSelected = null;
    }

    public void ConfirmItemSelected()
    {
        if (playerMaterialDetection.inventoryMaterialSlotUIGatheringList.Count == 0)
        {
            ToNextScene();
        }

        if (materialDataSelected == null) return;

        playerEventSO.Event.OnDayEnd?.Invoke();

        ToNextScene();
    }

    public void ReturnToHome()
    {
        confirmPortalPanel.SetActive(true);
        playerMovement.SetInRewardScreen();
    }

    public void CancelReturnToHome()
    {
        confirmPortalPanel.SetActive(false);
        playerMovement.SetInRewardScreen();
    }

    public void ConfirmReturnToHome()
    {
        confirmPortalPanel.SetActive(false);
        toNextSceneBtn.SetActive(true);
        // confirmMaterialBtn.SetActive(false);

        rewardScreen.SetActive(true);
        playerMovement.SetInRewardScreen();
        playerEventSO.Event.OnDayEnd?.Invoke();
    }

    public void ToNextScene()
    {
        Debug.Log("Gathering Done");
        playerEventSO.Event.OnGoToNextScene?.Invoke();
    }
}
