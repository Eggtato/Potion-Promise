using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class cutscenecontroller : MonoBehaviour
{
    public VideoPlayer vid;
    public GameObject cover;

    void Start()
    {
        cover.SetActive(true);
        vid.loopPointReached += EndReached;
    }

    void FixedUpdate(){
        if(vid.isPrepared){
            cover.SetActive(false);
        }
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        if(progressController.Instance != null){
            progressController.Instance.progressThroughGame();
        }
    }
}
