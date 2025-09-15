using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float horisontalMove = 0f;
    public float speed = 1f;

    void Start()
    {
        rb.GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        horisontalMove = Input.GetAxisRaw("Horisontal") * speed;
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = new Vector2(horisontalMove * 10f, rb.linearVelocity.y);
        rb.linearVelocity = targetVelocity;
    }
}
