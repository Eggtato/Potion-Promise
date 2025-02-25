using UnityEngine;
using System.Collections;

public class PlayerSpellController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Camera Camera;
    [SerializeField] private Transform Body;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject magicWand;
    [SerializeField] private GameObject[] effectGameObjects;

    private bool canSpell = true;
    private RaycastHit hit;
    private Ray ray;
    public LayerMask layerMask;

    void Start()
    {
        magicWand.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canSpell)
        {
            StartCoroutine(castingSpellAnim());
        }
    }

    IEnumerator castingSpellAnim()
    {
        canSpell = false;

        playerMovement.canMove = false;

        effectGameObjects[0].SetActive(true);

        magicWand.SetActive(true);

        ray = Camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Vector3 b = (hit.point - Body.position).normalized;
            Body.LookAt(new Vector3(hit.point.x, Body.position.y, hit.point.z));
        }

        anim.SetBool("castingspell", true);
        yield return new WaitForSeconds(0.8f);
        anim.SetBool("castingspell", false);

        magicWand.SetActive(false);

        playerMovement.canMove = true;

        yield return new WaitForSeconds(0.5f);

        canSpell = true;
    }
}
