using UnityEngine;
using System.Collections;
using UnityEngine;

public class PlayerSpellController : MonoBehaviour
{
    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(castingSpellAnim());
        }
    }

    IEnumerator castingSpellAnim()
    {
        anim.SetBool("castingspell", true);
        yield return new WaitForSeconds(1);
        anim.SetBool("castingspell", false);
    }
}
