using System.Collections;
using System.Collections.Generic;
using Eggtato.Utility;
using UnityEngine;

public class DayProgressionManager : MonoBehaviour
{
    public GameObject endGame;

    private GameDataManager gameDataManager;

    private void Awake()
    {
        gameDataManager = GameDataManager.Instance;
    }

    private void OnEnable()
    {
        gameDataManager.OnCurrentDayChanged += ProgressCurrentDay;
    }

    private void OnDisable()
    {
        gameDataManager.OnCurrentDayChanged -= ProgressCurrentDay;
    }

    public void ProgressCurrentDay(int currentDay)
    {
        // if (InventoryMaterialUI.Instance != null)
        // {
        //     InventoryMaterialUI.Instance.bookbtn.SetActive(true);
        // }

        switch (currentDay)
        {
            case 1:
                GameSceneManager.Instance.LoadVisualNovelScene(); //VisualNovel
                break;
            case 2:
                GameSceneManager.Instance.LoadVisualNovelScene(); //VisualNovel
                break;
            case 3:
                GameSceneManager.Instance.LoadShopScene();
                break;
            case 4:
                GameSceneManager.Instance.LoadVisualNovelScene(); //VisualNovel
                break;
            case 5:
                GameSceneManager.Instance.LoadGatheringScene();
                break;
            case 6:
                GameSceneManager.Instance.LoadShopScene();
                break;
            case 7:
                GameSceneManager.Instance.LoadGatheringScene();
                break;
            case 8:
                GameSceneManager.Instance.LoadShopScene();
                break;
            case 9:
                GameSceneManager.Instance.LoadGatheringScene();
                break;
            case 10:
                GameSceneManager.Instance.LoadShopScene();
                break;
            case 11:
                GameSceneManager.Instance.LoadVisualNovelScene(); //VisualNovel
                break;
            case 12:
                GameSceneManager.Instance.LoadVisualNovelScene(); //VisualNovel
                break;
            case 13:
                GameSceneManager.Instance.LoadVisualNovelScene(); //VisualNovel
                break;
            case 14:
                GameSceneManager.Instance.LoadGatheringScene();
                break;
            case 15:
                endGame.SetActive(true);
                Time.timeScale = 1;
                // Destroy(InventoryMaterialUI.Instance.gameObject);
                // Destroy(NPCManager.Instance.gameObject);
                break;
        }
    }
}
