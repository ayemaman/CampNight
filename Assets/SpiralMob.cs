using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralMob : Enemy
{
    private SpriteRenderer sr;
    private Transform playerPos;
    private Animator animator;
    private BoxCollider2D bc;
    private Rigidbody2D rb;
    private bool left;
    public float angle, radius = 10;
    public float angleSpeed = 2;
    public float radialSpeed = 0.5f;
    public float speed = 0;
   

    // Start is called before the first frame update
    private void Start()
    {

        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerPos == null)
        {
            Debug.Log("Players position is not set");
        }
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        if (transform.position.x < playerPos.position.x)
        {
            left = true;
        }
        else
        {
            left = false;
            //speed *= -1;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        float bufferTime = Time.time;
        float x = Mathf.Sin(bufferTime);
        float y = Mathf.Cos(bufferTime);

        if (x > 0.3)
        {
            x *= 2;
        }
        if (!left)
        {
            x *= -1;
        }
        
        rb.velocity = new Vector2(x, y) * speed ;
        Debug.Log(rb.velocity);
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

/*
  float angle, radius = 10;
float angleSpeed = 2;
float radialSpeed = 0.5f;
 
void Update() {
    angle += Time.deltaTime * angleSpeed;
    radius -= Time.deltaTime * radialSpeed;
 
    float x = radius * Mathf.Cos(Mathf.Deg2Rad*angle);
    float z = radius * Mathf.Sin(Mathf.Deg2Rad*angle);
    float y = transform.position.y;
 
    transform.position = new Vector3(x, y, z);
}
 * */
