using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("COLLISION:"+collision.name);
        if (collision.CompareTag("ENEMY"))
        {

            int hitCombo = ++TouchScript.instance.hitCombo;
            TouchScript.instance.comboScore += collision.GetComponent<Enemy>().stats.points;

            if (hitCombo > 0)
            {

                GameMaster.gm.ComboText.text = TouchScript.instance.comboScore + System.Environment.NewLine + "x" + hitCombo;
                GameMaster.gm.ComboTextAnimator.SetTrigger("startAnimation");
                if (Random.Range(0, 2) == 1)
                {
                    AudioManager.instance.PlaySound("oink1");
                }
                else
                {
                    AudioManager.instance.PlaySound("oink2");
                }
            }
            Destroy(collision.gameObject);

        }

        else if (collision.CompareTag("BombardierBomb"))
        {
            collision.gameObject.GetComponent<BombardierBomb>().Multiply();
            Debug.Log("Multiply");
        }

        else if (collision.CompareTag("Bombardier"))
        {
            Debug.Log("Time to drop");
            BombardierMob bombardier = collision.gameObject.GetComponent<BombardierMob>();
            if (!bombardier.dropped)
            {
                bombardier.ReleaseBomb();
            }
        }
            
        else if (collision.CompareTag("Minibomb"))
        {
            Debug.Log("Atack minibomb");
            GameMaster.HitEnemy(collision.gameObject, 1);
        }
    }
}
