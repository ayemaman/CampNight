using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    public Transform spawnPointLeftBottom, spawnPointRightBottom, spawnPointLeftMiddle, spawnPointRightMiddle, spawnPointLeftTop, spawnPointRightTop;


    public float spawnTimer;
    private float _spawnTimer;
    public float spawnChance = 80;
    public float spawnChanceChange = 10;
    public float spawnChanceTimer;
    private float _spawnChanceTimer;
    private float GameTimer;
    public bool ready = false;

   
    private int totalChance = 0;
    public Enemy[] currentEnemies;
    public int[] enemyAmount;
    public int[] enemyAmountStart;




    private int RandomForEnemy;
    public Enemy[] enemies;
    // Start is called before the first frame update


   

    void Awake()
    {
        
        
        if (instance != null)
        {
            if (instance != this)
            {

                Destroy(this.gameObject);
                instance.gameObject.SetActive(true);
            }
            Debug.Log("More then one Spawner in scene");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {

        if (instance.ready)
        {
            
            if (CheckIfSpawnsLeft())
            {
              
                instance._spawnTimer -= Time.deltaTime;
                instance._spawnChanceTimer -= Time.deltaTime;
                if (instance._spawnChanceTimer < 0)
                {
                    instance.spawnChance -= instance.spawnChanceChange;

                    instance._spawnChanceTimer = instance.spawnChanceTimer;
                }

                if (instance._spawnTimer < 0)
                {
                    if (Random.Range(0, 101) > instance.spawnChance)
                    {

                        instance.Spawn(Random.Range(0, 2));
                    }
                    instance._spawnTimer = instance.spawnTimer;
                }
            }
            else
            {
                //Debug.Log("round finished");
                GameMaster.EndRound();
            }
        }
    }


    public void Prepare(MobAmountsForLevel[] mobs)
    {
        instance.spawnChance = 80;
        instance.totalChance = 0;

        instance._spawnTimer = instance.spawnTimer;
        instance._spawnChanceTimer = instance.spawnChanceTimer;
        instance.spawnPointLeftBottom = GameObject.Find("SpawnPointLeftBottom").transform;
        instance.spawnPointRightBottom = GameObject.Find("SpawnPointRightBottom").transform;
   
        instance.spawnPointLeftMiddle = GameObject.Find("SpawnPointLeftMiddle").transform;
        instance.spawnPointRightMiddle = GameObject.Find("SpawnPointRightMiddle").transform;
        instance.spawnPointLeftTop = GameObject.Find("SpawnPointLeftTop").transform;
        instance.spawnPointRightTop = GameObject.Find("SpawnPointRightTop").transform;
        


        instance.currentEnemies = new Enemy[mobs.Length];
        instance.enemyAmount = new int[mobs.Length];
        
        for (int i = 0; i < mobs.Length; i++)
        {
            instance.currentEnemies[i] = instance.enemies[mobs[i].mobNum];
            Debug.Log(currentEnemies[i]);
            instance.enemyAmount[i] = mobs[i].amount;
            instance.totalChance += instance.enemies[i].stats.spawnChance;

        }
        instance.enemyAmountStart = new int[mobs.Length];
        instance.enemyAmount.CopyTo(instance.enemyAmountStart, 0);
        instance.ready = true;
    }

    //public void Prepare(int[] id)
    //{
    //    instance.spawnChance=80;
    //    instance.totalChance = 0;
        
    //    instance._spawnTimer = instance.spawnTimer;
    //    instance._spawnChanceTimer = instance.spawnChanceTimer;
    //    instance.spawnPointLeftBottom = GameObject.Find("SpawnPoint").transform;
    //    instance.spawnPointRightBottom = GameObject.Find("SpawnPoint2").transform;
    //    instance.spawnPointLeftMiddle = GameObject.Find("SpawnPointLeftMiddle").transform;
    //    instance.spawnPointRightMiddle = GameObject.Find("SpawnPointRightMiddle").transform;
    //    instance.spawnPointLeftTop = GameObject.Find("SpawnPointLeftTop").transform;
    //    instance.spawnPointRighTop = GameObject.Find("SpawnPointRighTop").transform;
    //    instance.currentEnemies = new Enemy[id.Length];
    //    for (int i = 0; i < id.Length; i++)
    //    {
    //        instance.currentEnemies[i] = enemies[i];
    //        instance.totalChance += enemies[i].stats.spawnChance;
    //    }
    //    instance.ready = true;

    //}

    void Spawn(int spawnPoint)
    {
        

        int whoToSpawn = Random.Range(0, instance.totalChance + 1);
        int buff = 0;
        int lastMaxChance = 0;
        bool notFound = true;
       // Debug.Log("START-------------------------------------------------------------");
        while (notFound && buff < currentEnemies.Length)
        {
            /*
            Debug.Log("lastMaxChance: "+ lastMaxChance);
            Debug.Log("CurentEnemiesSpawn: "+currentEnemies[buff].stats.spawnChance);
            Debug.Log("whoTO: " + whoToSpawn);
            Debug.Log("RESULT: " + (currentEnemies[buff].stats.spawnChance + lastMaxChance >= whoToSpawn));
            if (instance.currentEnemies[buff].stats.spawnChance + lastMaxChance >= whoToSpawn)
            {
                Debug.Log("END>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            }
            */
            if (instance.currentEnemies[buff].stats.spawnChance + lastMaxChance >= whoToSpawn)
            {
                if (spawnPoint == 0)
                {
                    if (CheckIfSpawnsForMobLeft(buff))
                    {
                        if (instance.currentEnemies[buff] is BombardierMob || instance.currentEnemies[buff] is UmberallaMob || instance.currentEnemies[buff] is FlyingDutchMob)
                        {
                            Instantiate(instance.currentEnemies[buff], spawnPointLeftTop.position, Quaternion.identity);
                        }
                        else if (instance.currentEnemies[buff] is SpiralMob || instance.currentEnemies[buff] is FlyingMob)
                        {
                            Instantiate(instance.currentEnemies[buff], spawnPointLeftMiddle.position, Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(instance.currentEnemies[buff], spawnPointLeftBottom.position, Quaternion.identity);
                        }

                      
                        //Debug.Log("mob spawned:" + buff);
                        //Debug.Log("SpawnsLeft: firstMob:" + enemyAmount[0] + " secondMob:" + enemyAmount[1] + "thirdMob: "+ enemyAmount[2]);
                        enemyAmount[buff]--;
                        notFound = false;
                       // Debug.Log("NOW: " + "SpawnsLeft: firstMob:" + enemyAmount[0] + " secondMob:" + enemyAmount[1] + "thirdMob: " + enemyAmount[2]);
                    }
                    else
                    {
                        buff++;
                    }
                }
                else
                {
                    if (CheckIfSpawnsForMobLeft(buff))
                    {

                        if (instance.currentEnemies[buff] is BombardierMob || instance.currentEnemies[buff] is UmberallaMob || instance.currentEnemies[buff] is FlyingDutchMob)
                        {
                            Instantiate(instance.currentEnemies[buff], spawnPointRightTop.position, Quaternion.identity);
                        }
                        else if (instance.currentEnemies[buff] is SpiralMob || instance.currentEnemies[buff] is FlyingMob)
                        {
                            Instantiate(instance.currentEnemies[buff], spawnPointRightMiddle.position, Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(instance.currentEnemies[buff], spawnPointRightBottom.position, Quaternion.identity);
                        }

                        // Debug.Log("mob spawned:" + buff);
                        //Debug.Log("SpawnsLeft: firstMob:" + enemyAmount[0] + " secondMob:" + enemyAmount[1] + "thirdMob: " + enemyAmount[2]);
                        enemyAmount[buff]--;
                        notFound = false;
                        //Debug.Log("NOW: " + "SpawnsLeft: firstMob:" + enemyAmount[0] + " secondMob:" + enemyAmount[1] + "thirdMob: " + enemyAmount[2]);
                    }
                    else
                    {
                        buff++;
                    }
                }
            }
            else
            {
                lastMaxChance += instance.currentEnemies[buff].stats.spawnChance;
                buff++;
            }
        }
        
        if (notFound)
        {
       
            if (spawnPoint == 0)
            {
                if (CheckIfSpawnsForMobLeft(buff - 1))
                {
                    if (instance.currentEnemies[buff-1] is BombardierMob || instance.currentEnemies[buff-1] is UmberallaMob || instance.currentEnemies[buff-1] is FlyingDutchMob)
                    {
                        Instantiate(instance.currentEnemies[buff--], spawnPointLeftTop.position, Quaternion.identity);
                    }
                    else if (instance.currentEnemies[buff - 1] is SpiralMob || instance.currentEnemies[buff - 1] is FlyingMob)
                    {
                        Instantiate(instance.currentEnemies[buff--], spawnPointLeftMiddle.position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(instance.currentEnemies[buff--], spawnPointLeftBottom.position, Quaternion.identity);
                    }

                    enemyAmount[buff]--;
                 
                }
            }
            else
            {
                if (CheckIfSpawnsForMobLeft(buff - 1))
                {
                    if (instance.currentEnemies[buff - 1] is BombardierMob || instance.currentEnemies[buff - 1] is UmberallaMob || instance.currentEnemies[buff - 1] is FlyingDutchMob)
                    {
                        Instantiate(instance.currentEnemies[buff--], spawnPointRightTop.position, Quaternion.identity);
                    }
                    else if (instance.currentEnemies[buff - 1] is SpiralMob || instance.currentEnemies[buff - 1] is FlyingMob)
                    {
                        Instantiate(instance.currentEnemies[buff--], spawnPointRightMiddle.position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(instance.currentEnemies[buff--], spawnPointRightBottom.position, Quaternion.identity);
                    }

                    enemyAmount[buff]--;
                    
                }
            }
            
        }


    }

    public bool CheckIfSpawnsForMobLeft(int enemy)
    {
        if (enemyAmount[enemy] > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckIfSpawnsLeft()
    {
        bool left = false;
        foreach(int i in enemyAmount)
        {
            
            if (i > 0)
            {
                left = true;
                break;
            }
        }
        return left;
    }

    public void Stop()
    {
        instance.ready = false;
    }
}

//mobNum for mobPosition in array, amount for amount of mobs to spawn on level;
public class MobAmountsForLevel
{

    public int mobNum { get; set; }
    public int amount { get; set; }

    public MobAmountsForLevel(int mobNum, int amount)
    {
        this.mobNum = mobNum;
        this.amount = amount;
    }

    public void OneLess()
    {
        this.amount--;
    }
}