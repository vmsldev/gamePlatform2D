using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rigidbody2;
    public int damage;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        OctopusController octopus = hitInfo.GetComponent<OctopusController>();
        if(octopus != null)
        {
            octopus.TakeDamage(damage);
        }

        CrabController crab = hitInfo.GetComponent<CrabController>();
        if (crab != null)
        {
            crab.TakeDamage(damage);
        }
        Debug.Log(hitInfo.name);
        Destroy(gameObject);
    }
}
