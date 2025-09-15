using UnityEngine;

public class Player : MonoBehaviour
{
    //Основные параметры
    public float speed = 5f;
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
    }
}
