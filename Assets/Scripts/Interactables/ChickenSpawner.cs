using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSpawner : MonoBehaviour, IInteractable
{
    public GameObject chickenPrefab; // Referência para o prefab do frango
    public Transform coop; // Referência para o objeto do galinheiro
    public int maxChickenCount = 5; // Número máximo de galinhas para criar
    private int currentChickenCount = 0; // Contagem atual de galinhas criadas

    public void Interact()
    {
        SpawnChicken(); // Quando interagido, chame a função para criar um frango
    }

    void Start()
    {
        // Esta função é chamada no início do jogo, mas está vazia aqui
    }

    void SpawnChicken()
    {
        if (currentChickenCount < maxChickenCount)
        {
            Vector3 spawnPosition = coop.position + coop.forward * 3.0f; // Posiciona o frango à frente do galinheiro
            Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(0, 360), 0); // Rotação aleatória

            GameObject newChicken = Instantiate(chickenPrefab, spawnPosition, spawnRotation); // Cria um novo frango
            currentChickenCount++; // Incrementa a contagem de galinhas criadas
        }
    }
}
