using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormMob : Enemy
{
    private SpriteRenderer sr;
    private Transform playerPos;
    private Animator animator;
    private BoxCollider2D bc;
    private Rigidbody2D rb;
    private WormState currentState = WormState.Stop;
    private WormState lastState = WormState.Stop;
    private bool left;
    public float stopTimer;
    private float _stopTimer;
    public float slowPush;
    public float fastPush;
    private bool moves = false;
    public float slowMoveTimer;
    private float _slowMoveTimer;
    public float fastMovetimer;
    private float _fastMoveTimer;

    private bool animPrepareInitiated = false;
    private bool animStopInitiated = false;
    



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
        _fastMoveTimer = fastMovetimer;
        _slowMoveTimer = slowMoveTimer;
        _stopTimer = stopTimer;
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

        //Debug.Log(currentState);
       // Debug.Log(rb.velocity);
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
                movement();
                CheckIfDestroy();
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void movement()
    {
        if (currentState == WormState.Stop)
        {
           
            rb.velocity = Vector2.zero;
            if (_stopTimer > 0)
            {
                _stopTimer -= Time.deltaTime;
            }
            else
            {
                if (lastState == WormState.FastPush)
                {
                    currentState = WormState.SlowPush;
                    lastState = WormState.SlowPush;
                    Debug.Log("JUMP/from update");
                    animator.SetTrigger("Jump");
                    animPrepareInitiated = false;
                }
                else
                {
                    currentState = WormState.FastPush;
                    lastState = WormState.FastPush;
                    Debug.Log("JUMP/from update");
                    animator.SetTrigger("Jump");
                    animPrepareInitiated = false;
                }
                _stopTimer = stopTimer;
            }

            
        }
        else if (currentState == WormState.SlowPush)
        {
            Debug.Log("SLOW"+rb.velocity);
            if (left)
            {
                if (_slowMoveTimer > 0)
                {
                    _slowMoveTimer -= Time.deltaTime;
                    rb.velocity = Vector2.right * slowPush;
                }
                else
                {
                    currentState = WormState.Stop;
                    _slowMoveTimer = slowMoveTimer;
                }

            }
            else
            {

                if (_slowMoveTimer > 0)
                {
                    _slowMoveTimer -= Time.deltaTime;
                    rb.velocity = Vector2.left * slowPush;
                }
                else
                {
                    currentState = WormState.Stop;
                    _slowMoveTimer = slowMoveTimer;
                }

            }

        }
        else if (currentState == WormState.FastPush)
        {
            Debug.Log("FAST"+rb.velocity);
            if (left)
            {


                if (_fastMoveTimer > 0)
                {
                    _fastMoveTimer -= Time.deltaTime;
                    rb.velocity = Vector2.right * fastPush;
                }
                else
                {
                    currentState = WormState.Stop;
                    _fastMoveTimer = fastMovetimer;
                }


            }
            else
            {

                if (_fastMoveTimer > 0)
                {
                    _fastMoveTimer -= Time.deltaTime;
                }
                else
                {
                    currentState = WormState.Stop;
                    rb.velocity = Vector2.left * fastPush;
                    _fastMoveTimer = fastMovetimer;
                }

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

    private enum WormState
    {
        Stop,
        SlowPush,
        FastPush
    }

    public void JumpFinished()
    {
        Debug.Log("STOP/from function");
        animator.SetTrigger("Stop");
    }
    public void StopFinished()
    {
        Debug.Log("PREPARE/from function");
        animator.SetTrigger("Prepare");
    }
    public void PrepareFinished()
    {
        if (Random.Range(0, 2) == 0)
        {
            currentState = WormState.SlowPush;
        }
        else
        {
            currentState = WormState.FastPush;
        }
        Debug.Log("JUMP/from function");
        animator.SetTrigger("Jump");
    }
}
