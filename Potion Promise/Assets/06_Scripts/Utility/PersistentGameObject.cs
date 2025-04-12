using Eggtato.Utility;
using UnityEngine;

public class PersistentGameObject : MonoBehaviour
{
    private void Awake()
    {
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }
}
