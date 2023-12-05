using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Velocidade de movimento do personagem

    void Update()
    {
        // Input para movimentação
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calcula a direção com base na câmera
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 desiredMoveDirection = cameraForward * verticalInput + cameraRight * horizontalInput;

        // Move o personagem na direção desejada
        transform.Translate(desiredMoveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Se houver input de movimento, faz o personagem olhar na direção correta
        if (desiredMoveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), 0.15f);
        }
    }
}
