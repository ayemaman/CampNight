using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public float spawnTimer;
    private float _spawnTimer;
    public float spawnChance = 80;
    public float spawnChanceChange = 10;
    public float spawnChanceTimer;
    private float _spawnChanceTimer;
    private float GameTimer;
    private bool ready = false;

   
    private int totalChance = 0;
    Enemy[] currentEnemies;





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

        if (ready)
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
    }


    public void Prepare(int[] id)
    {
        instance.spawnChance=80;
        instance.totalChance = 0;
        
        instance._spawnTimer = instance.spawnTimer;
        instance._spawnChanceTimer = instance.spawnChanceTimer;
        instance.spawnPoint1 = GameObject.Find("SpawnPoint").transform;
        instance.spawnPoint2 = GameObject.Find("SpawnPoint2").transform;
        instance.currentEnemies = new Enemy[id.Length];
        for (int i = 0; i < id.Length; i++)
        {
            instance.currentEnemies[i] = enemies[i];
            instance.totalChance += enemies[i].stats.spawnChance;
        }
        ready = true;

    }

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
                    Instantiate(instance.currentEnemies[buff], spawnPoint1.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(instance.currentEnemies[buff], spawnPoint2.position, Quaternion.identity);
                }
                notFound = false;

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
                Instantiate(currentEnemies[--buff], spawnPoint1.position, Quaternion.identity);
            }
            else
            {
                Instantiate(currentEnemies[--buff], spawnPoint2.position, Quaternion.identity);
            }
        }


    }

    public void Stop()
    {
        ready = false;
    }
}
