using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombardierBomb : Enemy
{
    private float y;
    private float diff = 0;
    private bool grow;
    public GameObject minibombs;
    private Vector3 startingScale;
    // Start is called before the first frame update
    void Start()
    {
        y = transform.position.y;
        startingScale = transform.localScale;
        Physics2D.IgnoreLayerCollision(9, 9, true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (grow)
        {
            transform.localScale += new Vector3(0.02f, 0.02f, transform.localScale.z);
            
        }
    }

    public void growSize()
    {
        grow = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name != "Bombardier")
        {

            Debug.Log("Collison: " + collision.collider.name);
            grow = false;
            
            if (collision.collider.CompareTag("Player"))
            {

                GameMaster.DamagePlayer(stats.dmg);
                GameMaster.HitEnemy(this.gameObject, 9001);
            }
            else {
                
                Multiply();
            }
        }
    }

    public void Multiply()
    {
        float ratio=transform.localScale.x / startingScale.x;
        GameObject bomb1= Instantiate(minibombs, transform.position, Quaternion.identity);
        bomb1.GetComponent<Minibomb>().SetDirection(Minibomb.Direction.Left);
        bomb1.transform.localScale = new Vector3(bomb1.transform.localScale.x * ratio, bomb1.transform.localScale.y * ratio, 0);
        bomb1.GetComponent<CircleCollider2D>().enabled = true;
        GameObject bomb2 = Instantiate(minibombs, transform.position, Quaternion.identity);
        bomb2.GetComponent<Minibomb>().SetDirection(Minibomb.Direction.Center);
        bomb2.transform.localScale = new Vector3(bomb2.transform.localScale.x * ratio, bomb2.transform.localScale.y * ratio, 0);
        bomb2.GetComponent<CircleCollider2D>().enabled = true;
        GameObject bomb3 = Instantiate(minibombs, transform.position, Quaternion.identity);
        bomb3.GetComponent<Minibomb>().SetDirection(Minibomb.Direction.Right);
        bomb3.transform.localScale = new Vector3(bomb3.transform.localScale.x * ratio, bomb3.transform.localScale.y * ratio, 0);
        bomb3.GetComponent<CircleCollider2D>().enabled = true;
        Destroy(this.gameObject);
    }
}
