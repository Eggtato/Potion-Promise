using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class visualnovelcontroller : MonoBehaviour
{
    public TMP_Text nametxt, dialogtxt;
    public GameObject logdialog, logobject, normalspeed, autospeed;
    public Transform logcontainer;
    public Button skipbtn;
    public Image leftimage, rightimage, highlightimage;
    public List<dialogclass> conversationlist = new List<dialogclass>();
    public List<dialogclass> conversationlist1 = new List<dialogclass>();
    public List<dialogclass> conversationlist2 = new List<dialogclass>();
    public List<dialogclass> conversationlist3 = new List<dialogclass>();
    bool skip = false, isauto = false;
    public AudioSource audi;
    public SpriteRenderer sr;
    public GameObject[] CutsceneObject;
    public GameObject skipCutsceneBtn, cover;

    public AudioSource typingAudio;

    private void Awake()
    {
        skipbtn.onClick.AddListener(TaskOnClick);
    }

    void Start()
    {
        // if(InventoryMaterialUI.Instance != null){
        //     InventoryMaterialUI.Instance.bookbtn.SetActive(false);
        // }
        switch (GameDataManager.Instance.CurrentDay)
        {
            case 1:
                CutsceneObject[0].SetActive(true);
                skipCutsceneBtn.SetActive(true);
                break;
            case 2:
                StartCoroutine(conversationrotation(conversationlist));
                cover.SetActive(false);
                break;
            case 4:
                StartCoroutine(conversationrotation(conversationlist1));
                cover.SetActive(false);
                break;
            case 11:
                StartCoroutine(conversationrotation(conversationlist2));
                cover.SetActive(false);
                break;
            case 12:
                CutsceneObject[1].SetActive(true);
                skipCutsceneBtn.SetActive(true);
                break;
            case 13:
                StartCoroutine(conversationrotation(conversationlist3));
                cover.SetActive(false);
                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("x"))
        {
            SkipVisualNovel();
        }

        if (Input.GetKeyDown("c"))
        {
            if (logobject.activeSelf)
            {
                logobject.SetActive(false);
            }
            else
            {
                logobject.SetActive(true);
            }
        }
    }

    public void setauto()
    {
        if (isauto == true)
        {
            isauto = false;
            normalspeed.SetActive(true);
            autospeed.SetActive(false);
            return;
        }
        else
        {
            isauto = true;
            autospeed.SetActive(true);
            normalspeed.SetActive(false);
            return;
        }
    }

    IEnumerator conversationrotation(List<dialogclass> a)
    {
        foreach (dialogclass dc in a)
        {
            nametxt.text = dc.name;
            if (dc.background != null)
            {
                sr.sprite = dc.background;
            }
            foreach (lineclass line in dc.dialoglist)
            {

                // Instantiate(logdialog, logcontainer).GetComponent<logdialogcontroller>().init(dc.name, line.dialog);

                if (line.audioClip != null)
                {
                    audi.PlayOneShot(line.audioClip, 1);
                }

                if (line.highlight != null)
                {
                    highlightimage.transform.parent.gameObject.SetActive(true);
                    highlightimage.sprite = line.highlight;
                }
                else
                {
                    highlightimage.transform.parent.gameObject.SetActive(false);
                }

                if (line.isleft == true)
                {
                    if (!leftimage.gameObject.activeSelf)
                    {
                        leftimage.gameObject.SetActive(true);
                    }
                    leftimage.sprite = line.sprite;
                    leftimage.color = new Color32(255, 255, 255, 255);
                    rightimage.color = new Color32(110, 110, 110, 255);
                }
                else
                {
                    if (!rightimage.gameObject.activeSelf)
                    {
                        rightimage.gameObject.SetActive(true);
                    }
                    rightimage.sprite = line.sprite;
                    rightimage.color = new Color32(255, 255, 255, 255);
                    leftimage.color = new Color32(110, 110, 110, 255);
                }

                dialogtxt.text = "";

                foreach (char c in line.dialog)
                {
                    dialogtxt.text += c;
                    if (!typingAudio.isPlaying)
                    {
                        typingAudio.Play();
                    }
                    if (skip == true)
                    {
                        dialogtxt.text = line.dialog;
                        break;
                    }
                    yield return new WaitForSeconds(0.02f);
                }
                skip = false;

                while (skip == false && isauto == false)
                {
                    yield return new WaitForSeconds(0.02f);
                }
                skip = false;
                if (isauto == true)
                {
                    Debug.Log(0.5f + (line.dialog.Length * 0.005f));
                    yield return new WaitForSeconds(0.5f + (line.dialog.Length * 0.02f));
                }
            }
        }
        HandleVisualNovelEnd();
    }

    private void TaskOnClick()
    {
        skip = true;
    }

    public void SkipVisualNovel()
    {
        HandleVisualNovelEnd();
    }

    private void HandleVisualNovelEnd()
    {
        // if(InventoryMaterialUI.Instance != null){
        //     InventoryMaterialUI.Instance.bookbtn.SetActive(true);
        // }

        // if (DayProgressionManager.Instance != null)
        // {
        //     DayProgressionManager.Instance.ProgressCurrentDay();
        // }
        // else
        // {
        //     SceneManager.LoadScene("2DGameplay");
        // }
    }

}


[System.Serializable]
public class dialogclass
{
    public string name;
    public Sprite background;
    public List<lineclass> dialoglist = new List<lineclass>();
}

[System.Serializable]
public class lineclass
{
    public string dialog;
    public bool isleft = true;
    public Sprite sprite, highlight;
    public AudioClip audioClip;
}