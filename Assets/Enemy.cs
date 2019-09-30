using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [System.Serializable]
    public class EnemyStats
    {
        public int hp;
        public int dmg;
        public bool canMove = true;
        public int points;
        public int spawnChance;
    }
    
    public EnemyStats stats = new EnemyStats();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetHit(int dmg)
    {
        stats.hp -= dmg;
        if (stats.hp <= 0)
        {
            Destroy(this.gameObject);
            if (dmg < 9001)
            {
                TouchScript.instance.comboScore += stats.points;
                //GameMaster.addKill();
            }
        }
    }

    public void CheckIfDestroy()
    {
        if (transform.position.x < GameMaster.gm.spawnPoint1.transform.position.x)
        {
           
            Destroy(this.gameObject);
        }
        else if (transform.position.x > GameMaster.gm.spawnPoint2.transform.position.x)
        {
            
            Destroy(this.gameObject);
        }
      
    }

  
   
}
