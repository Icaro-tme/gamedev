using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Objeto alvo da câmera (o personagem)
    public float distance = 5.0f; // Distância da câmera ao personagem
    public float heightOffset = 1.5f; // Offset de altura para onde a câmera olha
    public float sensitivity = 2.0f; // Sensibilidade do mouse para rotação

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position; // Calcula o deslocamento inicial
    }

    void LateUpdate()
    {
        // Movimenta a câmera para seguir o personagem
        transform.position = target.position + offset;

        // Rotaciona a câmera com base na posição do mouse
        float horizontalInput = Input.GetAxis("Mouse X") * sensitivity;
        Quaternion rotation = Quaternion.Euler(0, horizontalInput, 0);
        offset = rotation * offset;

        // Mantém a distância fixa do personagem
        transform.position = target.position + offset;

        // Ajusta o ponto para onde a câmera está olhando para cima do personagem
        Vector3 lookAtPosition = target.position + Vector3.up * heightOffset;
        transform.LookAt(lookAtPosition);
    }
}
