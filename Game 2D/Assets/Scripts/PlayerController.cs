using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour   
{

    public float speed;
    public int jumpForce;
    public int health;
    public Transform groundCheck;

    private bool invunerable = false;
    private bool grounded = false;
    private bool jumping = false;
    private bool facingRight = true;

    private SpriteRenderer sprite;
    private Rigidbody2D rigidbody2D;
    private Animator animation;
    private Transform transf;

    public float fireRate;
    public Transform spawnShot;
    public GameObject shotPrefab;
    private float nextShot = 0f;

        // Start is called before the first frame update
    void Start()    
    {
        sprite = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        transf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()    
    {

        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

        if(Input.GetButtonDown("Jump") && grounded)
        {
            jumping = true;
        }

        SetAnimations();

        if(Input.GetButton("Fire1") && grounded && Time.time > nextShot)
        {
            Shot();
        }
    }

    void FixedUpdate()    
    {
        
        float move = Input.GetAxis("Horizontal");
        rigidbody2D.velocity = new Vector2(move * speed, rigidbody2D.velocity.y);

        if ((move < 0f && facingRight) || (move > 0f && !facingRight))
        {
            Flip();
        }

        if(jumping)
        {
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
            jumping = false;
        }
    }

    void Flip()    
    {
        facingRight = !facingRight;
        transf.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

    }

    void SetAnimations()   
    {
        animation.SetFloat("VelY", rigidbody2D.velocity.y);
        animation.SetBool("Player_Jump", !grounded);
        animation.SetBool("Player_Run", rigidbody2D.velocity.x != 0f && grounded);

    }

    void Shot()  
    {
        animation.SetTrigger("Player_Shot");
        nextShot = Time.time + fireRate;

        GameObject cloneShot = Instantiate(shotPrefab, spawnShot.position, spawnShot.rotation);

        if(!facingRight)
        {
            cloneShot.transform.eulerAngles = new Vector3(180, 0, 180);
        }
    }

    IEnumerator DamageEffect()
    {
        for (float i = 0f; i < 1f; i += 0.1f)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        invunerable = false;

    }

    public void DamagePlayer()
    {
        if (!invunerable)
        {
            invunerable = true;
            health--;
            StartCoroutine(DamageEffect());

            if (health <= 0)
            {

                Debug.Log("Morreu");
                Invoke("ReloadFase", 1f);
                gameObject.SetActive(false);

            }
        }
    }

    public void DamageWater()
    {
        Debug.Log("Morreu");
        health = 0;
        Invoke("ReloadFase", 1f);
        gameObject.SetActive(false);

    }

    void ReloadFase()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);

    }
}