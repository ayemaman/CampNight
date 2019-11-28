using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDutchMob : Enemy
{
    private SpriteRenderer sr;
    private Transform playerPos;
    private Animator animator;
    private BoxCollider2D bc;
    private Rigidbody2D rb;
    public float speed;
    

    private bool left;
    




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
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if ((left &&(transform.position.x < GameMaster.gm.spawnPoint1.position.x)) || (!left&&(transform.position.x > GameMaster.gm.spawnPoint2.position.x)))
        {
            left = !left;
            Debug.Log(" playerPos.position.y:" + playerPos.position.y+" transform.pos.y: "+transform.position.y);
            if (transform.position.y - 0.5f < playerPos.position.y)
            {
                Debug.Log("smaller!");
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 2f);
            }
        }

        if (left)
        {
            rb.velocity = Vector2.left * speed * Time.deltaTime;
        }
        else
        {
            rb.velocity = Vector2.right * speed * Time.deltaTime;
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
