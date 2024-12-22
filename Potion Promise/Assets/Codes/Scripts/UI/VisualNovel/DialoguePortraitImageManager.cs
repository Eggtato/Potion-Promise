using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialoguePortraitImageManager : MonoBehaviour
{
    [SerializeField] private Image portraitImage;
    [SerializeField] private Color deactivateColor;
    [SerializeField] private bool isSetInactiveOnStart;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Active()
    {
        portraitImage.DOColor(Color.white, 0.2f);
    }

    public void Deactive()
    {
        portraitImage.DOColor(deactivateColor, 0.2f);
    }
}
