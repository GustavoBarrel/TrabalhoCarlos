using UnityEngine;
using TMPro;
using System;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;         // Velocidade de movimento do personagem
    public float jumpForce = 5f;     // Força do pulo
    public float dashDistance = 3f;  // Distância máxima do dash
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool inwall;
    private bool isDashing;

    private Vector3 dashTarget;      // Posição final do dash

    public TextMeshProUGUI counterText;  // Referência ao TextMeshProUGUI para o contador
    private float groundTimeCounter = 0f;  // Tempo em que o jogador está no chão

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (moveInput > 0)
            transform.localScale = new Vector3(1f, 1f, 1f);   // Virar para a direita
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);  // Virar para a esquerda

        isGrounded = Physics2D.OverlapCircle(transform.position, 0.7f, LayerMask.GetMask("Ground"));
        inwall = Physics2D.OverlapCircle(transform.position, 0.7f, LayerMask.GetMask("wall"));

        if (inwall)
        {
            Debug.Log("em contato com a parede");

        }
        if (inwall && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("wtf");
            Jump();
        }

        if (Input.GetMouseButtonDown(0) && !inwall && !isDashing && !isGrounded)
        {
            StartDash();
        }

        UpdateAnimations(moveInput);
        UpdateCounter();

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetMouseButtonDown(0) && !isGrounded && !isDashing && !inwall)
            StartDash();

        // Movimento para a posição final do dash enquanto estiver em dash
        if (isDashing)
        {
            Vector3 currentPosition = transform.position;
            Vector3 newPosition = Vector3.MoveTowards(currentPosition, dashTarget, 100f * Time.deltaTime);
            rb.MovePosition(newPosition);

            if (Vector3.Distance(currentPosition, dashTarget) < 0.1f)
                EndDash();
        }
    }

    void UpdateAnimations(float moveInput)
    {
        if (isGrounded)
        {
            animator.SetBool("walking", Mathf.Abs(moveInput) > 0.1f);
            animator.SetBool("jumping", false);
        }
        else
        {
            animator.SetBool("walking", false);
            animator.SetBool("jumping", true);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetBool("jumping", true);
    }

    void StartDash()
    {
        animator.SetBool("DashAnimation", true);

        // Determina a direção do movimento (direita ou esquerda)
        float moveDirection = transform.localScale.x;

        // Calcula a posição final do dash
        dashTarget = transform.position + new Vector3(moveDirection * dashDistance, 0f, 0f);

        // Define que o personagem está realizando o dash
        isDashing = true;
    }

    void EndDash()
    {
        animator.SetBool("DashAnimation", false);
        isDashing = false;
    }

    void UpdateCounter()
    {
        if (isGrounded)
        {
            groundTimeCounter += Time.deltaTime;
            counterText.text = groundTimeCounter.ToString("00.00");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Se o personagem colidir com algo enquanto está realizando o dash, termine o dash
        if (isDashing)
        {
            EndDash();
        }
    }
}
