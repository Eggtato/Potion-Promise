using UnityEngine;
using System.Collections;

public class PlayerSpellController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform body;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject magicWand;
    [SerializeField] private GameObject[] effectGameObjects;

    private bool canSpell = true;
    private RaycastHit hit;
    private Ray ray;
    [SerializeField] private LayerMask groundMask;

    private void Start()
    {
        magicWand.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && canSpell)
        {
            StartCoroutine(CastingSpellRoutine());
        }
    }

    IEnumerator CastingSpellRoutine()
    {
        canSpell = false;

        playerMovement.canMove = false;

        effectGameObjects[0].SetActive(true);

        magicWand.SetActive(true);

        ray = camera.ScreenPointToRay(Input.mousePosition);

        animator.SetBool("castingspell", true);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {
            Vector3 b = (hit.point - body.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(b);
            rotation.x = 0;
            rotation.z = 0;

            //Debug.Log(rotation.y - body.rotation.eulerAngles.y);

            while (rotation.y - body.rotation.eulerAngles.y > 0.5f)
            {
                body.rotation = Quaternion.Lerp(body.rotation, rotation, 0.5f);
                yield return new WaitForSeconds(0.02f);
            }
        }

        yield return new WaitForSeconds(0.8f);

        animator.SetBool("castingspell", false);

        yield return new WaitForSeconds(0.2f);

        magicWand.SetActive(false);

        playerMovement.canMove = true;

        yield return new WaitForSeconds(0.5f);

        canSpell = true;
    }
}
