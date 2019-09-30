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
            Debug.Log("Adding RIFLE: " + hitCombo);
            if (hitCombo > 1)
            {
                GameMaster.gm.ComboText.text = "x" + hitCombo;
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
            GameMaster.addKill();
        }
    }
}
