using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour
{
    public float speed;
    public int health;
    public Transform groundCheck;
    public Transform wallCheck;

    private bool grounded = false;
    private bool touchWall = false;
    private bool facingRight = false;

    private SpriteRenderer sprite;
    private Rigidbody2D rigidbody2D;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        rigidbody2D.velocity = transform.position * speed;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        touchWall = Physics2D.Linecast(transform.position, wallCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (touchWall)
        {
            Flip();
        }

    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        speed *= -1;
    }

    void FixedUpdate()
    {
        if (grounded)
        {
            rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Colidiu com o Player");
        }

        if (collision.CompareTag("Shot"))
        {
            Debug.Log("Sofreu dano");
        }
    }

    IEnumerator DamageEffect()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(DamageEffect());

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
