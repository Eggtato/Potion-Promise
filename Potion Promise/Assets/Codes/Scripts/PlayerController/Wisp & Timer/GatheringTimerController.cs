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
    [SerializeField] private FischlWorks_FogWar.csFogWar csFog;

    private void Start()
    {
        StartCoroutine(SanityTimer());
    }

    private IEnumerator SanityTimer()
    {
        bool lowSanity = false;
        float thresholdSanityLow = sanityAmount / 3;

        while (csFog.fogRevealers[0].sightRange > 0)
        {
            csFog.fogRevealers[0].sightRange -= 2;

            if (csFog.fogRevealers[0].sightRange <= 5 && !lowSanity)
            {
                lowSanity = true;
                //play low sanity sound
            }

            yield return new WaitForSeconds(15f);
        }

        ShowRewardScreen();
    }

    public void ShowRewardScreen()
    {
        rewardScreen.SetActive(true);
    }
}
