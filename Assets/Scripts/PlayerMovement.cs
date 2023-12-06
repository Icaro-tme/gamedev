using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintMultiplier = 1.5f; // Sprint speed multiplier

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    bool isSprinting; // Track if the player is currently sprinting

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift; // Sprint key

    [Header("Ground Check")]
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    // Added: Animator component
    public Animator playerAnimator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        // Added: Get Animator component from the child object
        playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        UpdateGrounded();
        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        // Added: Set animation parameters
        SetAnimationParameters();
    }

    private void SetAnimationParameters()
    {
        // set 1 if sprinting, 0.5 if walking, 0.25 if idle
        playerAnimator.SetFloat("Speed_f", isSprinting ? 1f : (verticalInput != 0f ? 0.4f : 0.24f));
        playerAnimator.SetBool("Jump_b", !grounded);
    }

    private void UpdateGrounded()
    {
        // Cast a sphere to check for ground
        grounded = Physics.CheckSphere(transform.position, groundCheckRadius, whatIsGround);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Sprinting check
        if (Input.GetKeyDown(sprintKey))
        {
            isSprinting = true;
            Sprint();
        }
        else if (Input.GetKeyUp(sprintKey))
        {
            isSprinting = false;
            ResetSprint();
        }

        // when to jump
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection.Normalize(); // Normalize the direction
    }

    private void MovePlayer()
    {
        // on ground
        if (grounded)
            rb.AddForce(moveDirection * GetCurrentSpeed() * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection * GetCurrentSpeed() * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > GetCurrentSpeed())
        {
            Vector3 limitedVel = flatVel.normalized * GetCurrentSpeed();
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Sprint()
    {
        moveSpeed *= sprintMultiplier;
    }

    private void ResetSprint()
    {
        moveSpeed /= sprintMultiplier;
    }

    private float GetCurrentSpeed()
    {
        return isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
