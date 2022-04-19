using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector2 velocity;
    private Transform player;

    public float smoothTimeX;
    public float smoothTimeY;


    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.Find("Player").GetComponent<Transform>();

    }

    void FixedUpdate()
    {

        if (player != null) {
            float posX = Mathf.SmoothDamp(transform.position.x, player.position.x, ref velocity.x, smoothTimeX);
            float posY = Mathf.SmoothDamp(transform.position.y, player.position.y, ref velocity.y, smoothTimeY);

            transform.position = new Vector3(posX, posY, transform.position.z);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
