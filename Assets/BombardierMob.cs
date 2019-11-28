using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombardierMob : Enemy
{

    private SpriteRenderer sr;
    private Transform playerPos;
    private Animator animator;
    private GameObject bomb;
    
    public float speed;
    public bool dropped = false;
    public bool done = false;

    private bool left;

    private float timerToFixRifle = 0.15f;
    private float _TimerToFixRifle;




    // Start is called before the first frame update
    private void Start()
    {
        bomb = transform.GetChild(0).gameObject;
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerPos == null)
        {
            Debug.Log("Players position is not set");
        }
       
        
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
            Scaler = transform.GetChild(0).transform.localScale;
            Scaler.x *= -1;
            transform.GetChild(0).transform.localScale = Scaler;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (left)
        {
            transform.Translate(Vector2.right*speed);
        }
        else {
            transform.Translate(Vector2.left*speed);
        }

        if (Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(playerPos.position.x, 0)) < 0.2)
        {
            if (!dropped)
            {
                dropped = true;
                _TimerToFixRifle = timerToFixRifle;
                if (bomb.transform.parent != null)
                {
                    bomb.transform.parent = null;
                }
            }
        }

        if (dropped && !done)
        {
            if (_TimerToFixRifle > 0)
            {
                _TimerToFixRifle = -Time.deltaTime;
            }
            else
            {
                Rigidbody2D rbBomb = bomb.GetComponent<Rigidbody2D>();
                rbBomb.bodyType = RigidbodyType2D.Dynamic;
                rbBomb.gravityScale = 0.5f;
                bomb.GetComponent<BombardierBomb>().growSize();
                bomb.GetComponent<CircleCollider2D>().enabled = true;
                done = true;
            }
        }
        CheckIfDestroy();
    }


    public void ReleaseBomb()
    {
        if (!dropped)
        {
            Debug.Log("Releasing");
            dropped = true;
            bomb.transform.parent = null;
            bomb.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            bomb.GetComponent<CircleCollider2D>().enabled = true;
            bomb.GetComponent<BombardierBomb>().growSize();
        }
    }
}
