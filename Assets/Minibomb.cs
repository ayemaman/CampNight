using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minibomb : Enemy
{
    [SerializeField]
    private Direction direction;
    public float ExplosionRadius;
    private Rigidbody2D rb;
    private CircleCollider2D col;
    public float up;
    public float left;
    public float right;
    public float DelayTimer = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        col.enabled = true;
        switch (direction)
        {
            case Direction.Left:
                rb.AddForce(new Vector2(left, up), ForceMode2D.Impulse);
                break;
            case Direction.Center:
                rb.AddForce(new Vector2(0, up), ForceMode2D.Impulse);
                break;
            case Direction.Right:
                rb.AddForce(new Vector2(right, up), ForceMode2D.Impulse);
                break;

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (DelayTimer > 0)
        {
            DelayTimer -= Time.deltaTime;
        }
        else
        {
            col.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < -0.4)
        {
            rb.gravityScale = 0.5f;
        }
    }


    public enum Direction
    {
        Left, Center, Right
    }
    public void SetDirection(Direction direction)
    {
        this.direction = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if(!collision.collider.CompareTag("Bombardier") && !collision.collider.CompareTag("BombardierBomb") && !collision.collider.CompareTag("Minibomb"))
        {

            Collider2D[] collisions = Physics2D.OverlapCircleAll(collision.contacts[0].point, ExplosionRadius);
            foreach (Collider2D col in collisions)
            {
                if (col.CompareTag("ENEMY"))
                {
                  
                    GameMaster.HitEnemy(col.gameObject, 5);
                }
                else if (col.CompareTag("Player"))
                {
                    GameMaster.DamagePlayer(1);
                }
            }
            //TODO SPAWN EXPLOSION PREFAB
         
            AudioManager.instance.PlaySound("knock_boom");
            Destroy(this.gameObject);


        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, ExplosionRadius);
    }
}
