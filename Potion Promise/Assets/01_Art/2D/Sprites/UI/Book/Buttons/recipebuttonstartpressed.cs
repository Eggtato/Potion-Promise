using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recipebuttonstartpressed : MonoBehaviour
{
    public Animator anim;
    void Start()
    {
        anim.Play("Pressed");
    }
}
