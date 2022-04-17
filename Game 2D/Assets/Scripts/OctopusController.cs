using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusController : MonoBehaviour
{
    public float speed;
    public int health;
    public Transform groundCheck;
    public Transform skyCheck;

    private bool grounded = false;
    private bool touchedSky = false;

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
        touchedSky = Physics2D.Linecast(transform.position, skyCheck.position, 1 << LayerMask.NameToLayer("Sky"));

        
    }

    void FixedUpdate()
    {
        if (grounded)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, speed);
        }

        if (touchedSky)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -speed);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Colidiu com o Player");
        }

        if(collision.CompareTag("Shot"))
        {
            Debug.Log("Sofreu dano");
        }
    }

    IEnumerator DamageEffect()
    {
        float actualSpeed = speed;
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(DamageEffect());

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
