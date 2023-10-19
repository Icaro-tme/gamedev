using System.Collections;
using UnityEngine;

public class TreeFalling : MonoBehaviour, IInteractable
{
    public float fallForce = 10f; // Ajuste para controlar a força da queda.
    public float fallDuration = 5f; // Tempo em segundos antes da árvore desaparecer.
    public float initialHeight = 1f; // A altura inicial antes da queda.

    private Rigidbody rb;
    private bool isFalling = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Inicialmente, o Rigidbody é cinemático para evitar que ele caia.

        // Adicione um MeshCollider ao GameObject.
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.convex = true; // Defina o colisor como convexo.
    }

    public void Interact()
    {
        if (!isFalling)
        {
            StartCoroutine(FallTree());
        }
    }

    private IEnumerator FallTree()
    {
        isFalling = true;

        // Desative o MeshCollider enquanto a árvore está caindo.
        GetComponent<Collider>().enabled = false;

        // Mova a árvore ligeiramente para cima.
        Vector3 initialPosition = transform.position;
        transform.position = new Vector3(initialPosition.x, initialPosition.y + initialHeight, initialPosition.z);

        // Ative o Rigidbody para permitir que a árvore caia.
        rb.isKinematic = false;

        // Aplique uma força à árvore para fazê-la cair ligeiramente para o lado.
        Vector3 fallDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        rb.AddForce(fallDirection.normalized * fallForce, ForceMode.Impulse);

        // Aguarde a duração especificada.
        yield return new WaitForSeconds(fallDuration);

        // Desative o GameObject para fazer a árvore desaparecer.
        gameObject.SetActive(false);
    }
}
