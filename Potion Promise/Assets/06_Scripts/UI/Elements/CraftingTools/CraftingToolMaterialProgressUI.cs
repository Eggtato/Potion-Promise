using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class CraftingToolMaterialProgressUI : MonoBehaviour
{
    [SerializeField] private Image progressBar;

    private void Start()
    {
        progressBar.fillAmount = 0;
    }

    public void UpdateProgressBar(float value)
    {
        progressBar.fillAmount = value;
    }
}
