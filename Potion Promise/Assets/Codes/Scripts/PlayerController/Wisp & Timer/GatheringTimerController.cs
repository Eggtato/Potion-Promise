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

    private IEnumerator SanityTimerRoutine()
    {
        bool lowSanity = false;
        float thresholdSanityLow = sanityAmount / 3;
        float countDown = sanityAmount / csFog.fogRevealers[0].sightRange;
        int minus = 2;
        float startSightVEffect = 12;

        while (countDown > 0)
        {
            if (!this.enabled)
            {
                yield return new WaitForSeconds(0.02f);
                continue;
            }

            yield return new WaitForSeconds(16f);
            csFog.fogRevealers[0].sightRange -= minus;
            countDown -= minus;

            fogEffect.SetFloat("SightRange", startSightVEffect);
            startSightVEffect -= minus;

            if (countDown <= thresholdSanityLow && !lowSanity)
            {
                lowSanity = true;
                //play low sanity sound
            }
        }

        rewardManager.PassOut();
    }
}
