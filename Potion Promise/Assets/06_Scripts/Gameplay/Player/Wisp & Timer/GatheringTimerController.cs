using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.VFX;

public class GatheringTimerController : MonoBehaviour
{
    [SerializeField] private float sanityAmount = 120f;
    [SerializeField] private TMP_Text sanityTxt;
    [SerializeField] private Image sanityBar;
    [SerializeField] private FischlWorks_FogWar.csFogWar csFog;
    [SerializeField] private VisualEffect fogEffect;

    [SerializeField] private RewardManagerUI rewardManager;

    private void Start()
    {
        StartCoroutine(SanityTimerRoutine());
    }

    IEnumerator SanityTimerRoutine()
    {
        float countDownImage = (1 / sanityAmount) * 0.02f;
        float countDownTxt = 1 * 0.02f;
        bool lowSanity = false;
        float thresholdSanityLow = sanityAmount / 3;
        float effectSightRange = 16;
        float countDownEffect = 1 / effectSightRange * 0.02f;

        while (sanityBar.fillAmount > 0)
        {
            sanityBar.fillAmount -= countDownImage;
            sanityTxt.text = sanityAmount.ToString("###");
            sanityAmount -= countDownTxt;
            if (sanityAmount < thresholdSanityLow && lowSanity != true)
            {
                lowSanity = true;
            }

            effectSightRange -= countDownEffect;
            csFog.fogRevealers[0].sightRange = (int)effectSightRange;
            fogEffect.SetFloat("SightRange", effectSightRange);

            yield return new WaitForSeconds(0.02f);
        }

        rewardManager.PassOut();
    }
}
