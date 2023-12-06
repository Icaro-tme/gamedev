using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header ("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;

    public Rigidbody rb;

    public float rotationSpeed = 1f;

    public void Start()
    {
        // Configura o estado do cursor para travado
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        // Calcula a direção da visão do jogador em relação à câmera
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // Obtém as entradas de movimento horizontal e vertical do jogador
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calcula a direção do input do jogador em relação à orientação da câmera
        Vector3 inputDir = orientation.forward * vertical + orientation.right * horizontal;

        // Verifica se o jogador está fornecendo algum input de movimento
        if(inputDir != Vector3.zero)
        {
            // Interpola suavemente a rotação do jogador na direção do input
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
