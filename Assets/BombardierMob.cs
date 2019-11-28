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
    private bool dropped = false;

    private bool left;





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

        if(Vector2.Distance(new Vector2(transform.position.x,0),new Vector2(playerPos.position.x, 0))<0.2){
            if (!dropped)
            {
                dropped = true;
                if (bomb.transform.parent != null)
                {
                    bomb.transform.parent = null;
                }
            
                bomb.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                bomb.GetComponent<BombardierBomb>().growSize();
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
