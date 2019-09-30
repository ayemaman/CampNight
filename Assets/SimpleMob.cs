using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMob : Enemy
{

    private Transform playerPos;
    public float speed;
    private BoxCollider2D bc;
    private Rigidbody2D rb;
    private bool left;
   

    private void Start()
    {
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
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stats.canMove)
        {
            if (left)
            {
                rb.velocity = Vector2.right * speed * Time.deltaTime;
            }
            else
            {
                rb.velocity = Vector2.left * speed * Time.deltaTime;
            }
            CheckIfDestroy();
        }
        else
        {
            rb.velocity = Vector2.zero;
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
