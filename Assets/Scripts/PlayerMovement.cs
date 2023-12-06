using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintMultiplier = 1.5f; // Multiplicador de velocidade de corrida

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    bool isSprinting; // Indica se o jogador está atualmente correndo

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift; // Tecla de corrida

    [Header("Ground Check")]
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    // Adicionado: Componente Animator
    public Animator playerAnimator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        // Adicionado: Obter componente Animator do objeto filho
        playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        UpdateGrounded();
        MyInput();
        SpeedControl();

        // Lidar com arrasto
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        // Adicionado: Definir parâmetros de animação
        SetAnimationParameters();
    }

    private void SetAnimationParameters()
    {
        // Definir 1 se estiver correndo, 0.5 se estiver andando, 0.25 se estiver parado
        playerAnimator.SetFloat("Speed_f", isSprinting ? 1f : (verticalInput != 0f ? 0.4f : 0.24f));
        playerAnimator.SetBool("Jump_b", !grounded);
    }

    private void UpdateGrounded()
    {
        // Lançar uma esfera para verificar o solo
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

        // Verificar corrida
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

        // Quando pular
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection.Normalize(); // Normalizar a direção
    }

    private void MovePlayer()
    {
        // No chão
        if (grounded)
            rb.AddForce(moveDirection * GetCurrentSpeed() * 10f, ForceMode.Force);

        // No ar
        else if (!grounded)
            rb.AddForce(moveDirection * GetCurrentSpeed() * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limitar a velocidade, se necessário
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
        // Resetar velocidade no eixo y
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
