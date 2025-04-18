using UnityEngine;
using UnityEngine.UI;

public class ShopCloseConfirmationUI : BaseUI
{
    [SerializeField] private Button okayButton;

    private void Awake()
    {
        okayButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickSound();
            Hide();
        });
    }

    private void Start()
    {
        InstantHide();
    }
}
