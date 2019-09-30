using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMob : Enemy
{
    private Transform playerPos;
    private Rigidbody2D rb;
    private bool left;
    public float speed;             
    public float amplitude;     //  How high (or low) each wave would be - around 8 worked best
    public float frequency;     // How many waves would run in the given time (2 to 4 was used)
    private float startTime;
    private Vector3 directionLeft;
    private Vector3 directionRight;
    private Vector3 orthogonal;
    private Vector3 orthogonal2;
    private float gravityScaleBuffer;
    

    // Start is called before the first frame update
    void Start()
    {
        
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        gravityScaleBuffer = rb.gravityScale;
        startTime = Time.time;
        directionLeft = (GameMaster.gm.spawnPoint1.position - transform.position).normalized;    // Which way to head
        directionRight= (GameMaster.gm.spawnPoint2.position - transform.position).normalized;
        orthogonal = new Vector3(-directionRight.z, directionRight.x, 0);               //  Which wave to catch!
        orthogonal2 = new Vector3(directionLeft.z, -directionLeft.x, 0);
        if (transform.position.x < playerPos.position.x)
        {
            left = true;
        }
        else
        {
            left = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stats.canMove)
        {
           
            rb.gravityScale=gravityScaleBuffer;
            float t = Time.time - startTime;
            if (!left)
            {
                rb.velocity = directionLeft * speed + orthogonal2 * amplitude * Mathf.Sin(frequency * t);
            }
            else
            {
                rb.velocity = directionRight * speed + orthogonal * amplitude * Mathf.Sin(frequency * t);
            }
            CheckIfDestroy();
        }
        else
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameMaster.DamagePlayer(stats.dmg);
            GameMaster.HitEnemy(this.gameObject, 9001);
        }
    }
}


