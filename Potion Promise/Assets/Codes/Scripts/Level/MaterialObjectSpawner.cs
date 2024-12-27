using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialObjectSpawner : MonoBehaviour
{
    public Transform[] spawnPositions;
    public GameObject[] materialObjects;

    private List<int> spawnPositionPicked = new List<int>();

    void Start()
    {
        StartCoroutine(spawnMaterialObjects());
    }

    IEnumerator spawnMaterialObjects()
    {
        int spawnPositionSize = spawnPositions.Length;
        spawnPositionSize--;
        for (int i = 0; i< spawnPositionSize; i++)
        {
            int rnd = Random.Range(0, spawnPositions.Length);
            while (spawnPositionPicked.Contains(rnd))
            {
                rnd = Random.Range(0, spawnPositions.Length);
                yield return new WaitForSeconds(0.02f);
            }
            Instantiate(materialObjects[Random.Range(0, materialObjects.Length)], spawnPositions[rnd].position, Quaternion.Euler(0, Random.Range(0, 360), 0));
            spawnPositionPicked.Add(rnd);
        }
    }
}
