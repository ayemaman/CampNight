using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingMob : Enemy
{
    private SpriteRenderer sr;
    private Transform playerPos;
    public float speed;
    private BoxCollider2D bc;
    private Rigidbody2D rb;
    private bool left;
    public float xForce;
    public float yForce;
    public float timeBetweenJumps;
    private float _timeBetweenJumps;
    private float gravityScaleBuffer;

    // Start is called before the first frame update
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerPos == null)
        {
            Debug.Log("Players position is not set");
        }
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        gravityScaleBuffer = rb.gravityScale;
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
    void FixedUpdate()
    {
        if (_flashTimer > 0)
        {
            sr.enabled = !sr.enabled;
            _flashTimer -= Time.deltaTime * 3;
            rb.velocity = Vector2.zero;
        }
        else
        {
            sr.enabled = true;
            if (stats.canMove)
            {

                rb.gravityScale = gravityScaleBuffer;
                _timeBetweenJumps -= Time.deltaTime;
                if (_timeBetweenJumps < 0)
                {
                    if (left)
                    {
                        rb.AddForce(new Vector2(xForce, yForce), ForceMode2D.Impulse);

                    }
                    else
                    {
                        rb.AddForce(new Vector2(-xForce, yForce), ForceMode2D.Impulse);

                    }
                    _timeBetweenJumps = timeBetweenJumps;

                }
                CheckIfDestroy();
            }
            else
            {
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
            }
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
