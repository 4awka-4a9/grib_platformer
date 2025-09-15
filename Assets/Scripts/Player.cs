using UnityEngine;

public class Player : MonoBehaviour
{
    //Основные параметры
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float dashForce = 15f;
    public float dashCooldown = 1f;
    //Все остальные переменные
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canDash = true;
    private float moveInput;

    //Методы
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Считываем управление
        moveInput = Input.GetAxisRaw("Horizontal");
        //Прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        //Рывок
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, -1, -1);
        }
    }
    //Корутина рывка
    private System.Collections.IEnumerator Dash()
    {
        canDash = false
        //Направление рывка
        float dashDirection = transform.localScale.x;
        yield return new WaitForSeconds(dashCooldown);
    }
}
