using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GatheringTimerController : MonoBehaviour
{
    [SerializeField] private float sanityAmount = 120f;
    [SerializeField] private TMP_Text sanityTxt;
    [SerializeField] private Image sanityBar;
    [SerializeField] private GameObject rewardScreen;

    private void Start()
    {
        StartCoroutine(SanityTimer());
    }

    private IEnumerator SanityTimer()
    {
        float kecepatanImage = (1 / sanityAmount) * 0.02f;
        float kecepatanTxt = 1 * 0.02f;
        bool lowSanity = false;
        float thresholdSanityLow = sanityAmount / 3;
        while (sanityBar.fillAmount > 0)
        {
            sanityBar.fillAmount -= kecepatanImage;
            sanityTxt.text = sanityAmount.ToString("###");
            sanityAmount -= kecepatanTxt;
            if (sanityAmount < thresholdSanityLow && lowSanity != true)
            {
                lowSanity = true;
                //play low sanity sound
            }
            yield return new WaitForSeconds(0.02f);
        }

        ShowRewardScreen();
    }

    public void ShowRewardScreen()
    {
        rewardScreen.SetActive(true);
    }
}
