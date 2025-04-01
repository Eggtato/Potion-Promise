using UnityEngine;
using UnityEngine.UI;

public class CreditUI : MonoBehaviour
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;

    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button linkTreeButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            GameSceneManager.Instance.LoadMainMenuScene();
        });
        linkTreeButton.onClick.AddListener(() =>
        {
            // Link Tree URL
        });
    }
}
