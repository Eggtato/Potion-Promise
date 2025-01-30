using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueBackgroundImageManager : MonoBehaviour
{
    [SerializeField] private Image portalBackgroundImage;
    [SerializeField] private Fader fader;

    public void ShowPortalBackground()
    {
        fader.Show(() =>
        {
            portalBackgroundImage.gameObject.SetActive(true);
        });
    }
}
