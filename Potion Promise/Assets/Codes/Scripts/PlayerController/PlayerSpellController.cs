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

        anim.SetBool("castingspell", true);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Vector3 b = (hit.point - Body.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(b);
            rotation.x = 0;
            rotation.z = 0;

            while (rotation.y - Body.rotation.eulerAngles.y > 20)
            {
                Debug.Log(rotation.y - Body.rotation.eulerAngles.y);
                Body.rotation = Quaternion.Lerp(Body.rotation, rotation, 0.5f);
                yield return new WaitForSeconds(0.02f);
            }
        }

        yield return new WaitForSeconds(0.8f);

        anim.SetBool("castingspell", false);

        yield return new WaitForSeconds(0.2f);

        magicWand.SetActive(false);

        playerMovement.canMove = true;

        yield return new WaitForSeconds(0.5f);

        canSpell = true;
    }
}
