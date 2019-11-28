using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmberallaMob : Enemy
{
    private SpriteRenderer sr;
    private Transform playerPos;
    private Animator animator;
    private BoxCollider2D bc;
    private Rigidbody2D rb;
   
    private bool left;
    public float speed;
    public float upForce;
    public float testTimer;
    private float _testTimer;



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

    /*
     * void Update() {
     
         Vector3 direction = (target.transform.position - transform.position).normalized;
           rigidbody.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
     
     }
     * */

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("fly up");
            FlyUP();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_testTimer > 0)
        {
            _testTimer -= Time.deltaTime;
        }
        else
        {

            //Debug.Log(currentState);
            // Debug.Log(rb.velocity);
            if (_flashTimer > 0)
            {
                sr.enabled = !sr.enabled;
                _flashTimer -= Time.deltaTime * 3;
            }
            else
            {
                sr.enabled = true;
            }

            if (stats.canMove)
            {
                //movement();
                //Debug.Log("moving");
                Vector3 direction = (playerPos.position - transform.position).normalized;
                rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
                CheckIfDestroy();
            }
            else
            {
                Debug.Log("cant moving");
                rb.velocity = Vector2.zero;
            }
        }
        
    }

    public void FlyUP()
    {
        _testTimer = testTimer;
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(Vector2.up*upForce, ForceMode2D.Force);

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
