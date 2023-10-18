using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSpawner : MonoBehaviour, IInteractable
{
    public GameObject chickenPrefab; // Reference to the chicken prefab
    public Transform coop; // Reference to the coop object
    public int maxChickenCount = 5; // Maximum number of chickens to spawn
    private int currentChickenCount = 0;

    public void Interact(){
        SpawnChicken();
    }

    void Start()
    {
        SpawnChicken();
    }

    void SpawnChicken()
    {
        if (currentChickenCount < maxChickenCount)
        {
            Vector3 spawnPosition = coop.position + coop.forward * 2.0f; // Place the chicken in front of the coop
            Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(0, 360), 0); // Random rotation

            GameObject newChicken = Instantiate(chickenPrefab, spawnPosition, spawnRotation);
            currentChickenCount++;

            // Optionally, you can set the chicken's coop reference to this coop object
            ChickenController chickenController = newChicken.GetComponent<ChickenController>();
            if (chickenController != null)
            {
                chickenController.coop = coop;
            }
        }
    }
}
