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

    [SerializeField] private PlayerEventSO playerEventSO;

    private void Start()
    {
        StartCoroutine(SanityTimer());
    }

    private IEnumerator SanityTimer()
    {
        bool lowSanity = false;
        float thresholdSanityLow = sanityAmount / 3;
        float countDown = sanityAmount / csFog.fogRevealers[0].sightRange;
        int minus = 2;

        while (countDown > 0)
        {
            csFog.fogRevealers[0].sightRange -= minus;
            countDown -= minus;

            if (countDown <= thresholdSanityLow && !lowSanity)
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

    public void ToNextScene()
    {
        playerEventSO.Event.OnGoToNextScene?.Invoke();
    }
}
