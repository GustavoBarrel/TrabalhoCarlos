using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1f;         // Velocidade de movimento do personagem
    public float jumpForce = 5f;     // Força do pulo
    public float dashDistance = 3f;  // Distância máxima do dash
    public float dashSpeed = 100f;   // Velocidade do dash (ajuste conforme necessário)

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool inwall;
    private bool isDashing;
    private bool hasDashed;          // Indica se o personagem já deu dash no ar

    private Vector3 dashTarget;      // Posição final do dash

    public TextMeshProUGUI counterText;  // Referência ao TextMeshProUGUI para o contador
    private float groundTimeCounter = 0f;  // Tempo em que o jogador está no chão

    private bool isOnPlatform = false;  // Indica se o jogador está em cima da plataforma

    public bool IsOnPlatform
    {
        get { return isOnPlatform; }
        set { isOnPlatform = value; }
    }

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
            Jump();
        }

        if (Input.GetMouseButtonDown(0) && !inwall && !isDashing && !isGrounded && !hasDashed)
        {
            StartDash();
        }

        UpdateAnimations(moveInput);
        UpdateCounter();

        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            hasDashed = false;  // Permite dash novamente quando tocar no chão
        }

        // Movimento para a posição final do dash enquanto estiver em dash
        if (isDashing)
        {
            Vector3 currentPosition = transform.position;
            Vector3 newPosition = Vector3.MoveTowards(currentPosition, dashTarget, dashSpeed * Time.deltaTime);
            rb.MovePosition(newPosition);

            if (Vector3.Distance(currentPosition, dashTarget) < 0.1f)
            {
                EndDash();
            }
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
        hasDashed = true;  // Indica que o personagem já deu dash no ar
    }

    void EndDash()
    {
        animator.SetBool("DashAnimation", false);
        isDashing = false;
    }

    void UpdateCounter()
    {
        if (isGrounded || isOnPlatform)
        {
            groundTimeCounter += Time.deltaTime;
            int seconds = Mathf.FloorToInt(groundTimeCounter);
            int milliseconds = Mathf.FloorToInt((groundTimeCounter - seconds) * 100);
            counterText.text = string.Format("{0:00}.{1:00}", seconds, milliseconds);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing)
        {
            EndDash();
        }

        // Se o personagem colidir com uma parede enquanto está no ar, redefina a capacidade de dash
        if (collision.gameObject.layer == LayerMask.NameToLayer("wall") && !isGrounded)
        {
            hasDashed = false;
        }
    }
}
