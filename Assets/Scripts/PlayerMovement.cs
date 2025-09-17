using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundChecker;
    public LayerMask groundLayer;

    private float horizontal;
    private float speed = 3f;
    private float jumpingPower = 4f;
    private bool isFacingRight = true;
    private Animator animator;
    private int maxJumps = 2;
    private int jumpCount;
    private bool wasGrounded;

    void Start()
    {
        animator = GetComponent<Animator>();
        jumpCount = maxJumps;
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);

        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }

        if (animator != null)
        {
            if (Mathf.Abs(horizontal) > 0.01f) // Если есть движение по X
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }
        }

        if (animator != null)
        {
            animator.SetBool("isJumping", !IsGrounded());
        }

        if (animator != null)
        {
            animator.SetBool("isRunning", Mathf.Abs(horizontal) > 0.01f);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && jumpCount > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            jumpCount--;
        }

        if (context.canceled && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        bool grounded = Physics2D.OverlapCircle(groundChecker.position, 0.2f, groundLayer);

        // Сбрасываем прыжки, только если:
        // 1. Мы на земле
        // 2. Ранее не были на земле (переход из воздуха)
        if (grounded && !wasGrounded)
        {
            jumpCount = maxJumps;
        }

        // Обновляем wasGrounded для следующего кадра
        wasGrounded = grounded;

        return grounded;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }
}