using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenController : MonoBehaviour
{
    public Object barrel; // Reference to the barrel object
    public float movementSpeed = 2.0f; // Speed at which the chicken moves
    public float detectionRange = 5.0f; // Range at which the chicken detects the barrel

    private Vector3 targetPosition;
    private bool movingTowardsBarrel = false;
    // Find the barrel by its name

    void Start()
    {
        SetRandomTarget();
    }

    void Update()
    {
        if (!movingTowardsBarrel)
        {
            MoveTowardsTarget();
        }
        else
        {
            LookAtBarrelAndMove();
        }
    }

    void SetRandomTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * detectionRange;
        targetPosition = new Vector3(randomCircle.x, 0, randomCircle.y) + barrel.position;
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

        if (Vector3.Distance(transform.position, barrel.position) > detectionRange)
        {
            movingTowardsBarrel = true;
        }
    }

    void LookAtBarrelAndMove()
    {
        transform.LookAt(barrel.position);
        float step = movementSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, barrel.position, step);

        if (Vector3.Distance(transform.position, barrel.position) < 0.1f)
        {
            movingTowardsBarrel = false;
        }
    }
}
