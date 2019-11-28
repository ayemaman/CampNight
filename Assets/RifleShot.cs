using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ENEMY"))
        {
            
            int hitCombo=++TouchScript.instance.hitCombo;
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
    }
}
