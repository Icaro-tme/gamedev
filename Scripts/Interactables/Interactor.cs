using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface para objetos interagíveis
interface IInteractable
{
    void Interact(); // Método que define a interação
}

public class Interactor : MonoBehaviour
{
    public SphereCollider interactionZone; // Referência ao Sphere Collider para interação

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interactionZone != null)
            {
                // Encontra todos os colliders na esfera de interação
                Collider[] colliders = Physics.OverlapSphere(interactionZone.transform.position, interactionZone.radius);

                // Itera sobre os colliders encontrados
                foreach (var collider in colliders)
                {
                    // Verifica se o collider tem um componente que implementa IInteractable
                    if (collider.TryGetComponent(out IInteractable interactObj))
                    {
                        interactObj.Interact(); // Chama o método de interação do objeto
                    }
                }
            }
        }
    }
}
