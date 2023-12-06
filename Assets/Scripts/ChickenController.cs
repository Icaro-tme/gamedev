using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenController : MonoBehaviour
{
    public Transform coop; // Reference to the coop object
    public float movementSpeed = 2.0f; // Speed at which the chicken moves
    public float detectionRange = 5.0f; // Range at which the chicken detects the coop

    private Vector3 targetPosition;

    void Start()
    {
        SetRandomTarget();
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    void SetRandomTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * detectionRange;
        targetPosition = new Vector3(randomCircle.x, 0, randomCircle.y) + coop.position;
        targetPosition.y = transform.position.y;
    }

    void MoveTowardsTarget()
    {
        float step = movementSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetRandomTarget();
        }
    }
}
