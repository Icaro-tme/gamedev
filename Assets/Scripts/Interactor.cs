using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    public SphereCollider interactionZone; // Reference to the Sphere Collider for interaction

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interactionZone != null)
            {
                Collider[] colliders = Physics.OverlapSphere(interactionZone.transform.position, interactionZone.radius);

                foreach (var collider in colliders)
                {
                    if (collider.TryGetComponent(out IInteractable interactObj))
                    {
                        interactObj.Interact();
                    }
                }
            }
        }
    }
}


