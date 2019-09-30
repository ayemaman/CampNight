using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameMaster gm;
    public PlayerStats playerStats=new PlayerStats();
    private GameObject spawnPoint1go;
    private GameObject spawnPoint2go;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public bool timerOn;
    public float roundTime;
    public int score;

    private int health;
    public int maxHealth;

    private GameObject hearts;

    private int RandomForEnemy;

    public GameObject tent;


    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI KillText;

    private GameObject Combo;
    public TextMeshProUGUI ComboText;
    public Animator ComboTextAnimator;

    private float starTime;

    private GameObject[] buttons = null;
    public Vector3[] buttonsPos = null;
    private Button FirstButton;
    public Vector3 FirstButtonPos;



    
    public bool paused = false;
    
    // private int iteration = 0;

    private bool waiting = false;
    private bool soundPlayed=false;

    //levelChangeStuff
    public int NumberOfStartingLevels;
    public int currentLevel = 0;

    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
        else
        {
            if (gm != this)
            {
                
                Destroy(this.gameObject);
            }
            Debug.Log("More then one GM in scene");
            DontDestroyOnLoad(gameObject);
        }


       
        
        
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteAll();
        gm.playerStats.lastFinishedLevel = 0;
        SaveSystem.Save(gm.playerStats);
        gm.playerStats= SaveSystem.Load();

    }
    void Start()
    {

        


    }
    void OnEnable()
    {
        
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        TouchScript.instance.ResetItems();
        //PlayerPrefs.DeleteAll();
        if (gm == null)
        {
            gm = FindObjectOfType<GameMaster>();
        }
        PlayerStats initialSave = SaveSystem.Load();
        if (initialSave.lastFinishedLevel == 0)
        {
            Debug.Log("New game! Need to save and load initial PlayerStats");
            SaveSystem.Save(playerStats);
       
        }
        if (gm != null)
        {
            gm.playerStats = SaveSystem.Load();
            // Debug.Log("gm:" + gm.playerStats.lastFinishedLevel);
            // Debug.Log("prefs: " + PlayerPrefs.GetInt("lvl"));
        }
        
       
        if (scene.name.ToLower().Contains("level"))
        {
            TouchScript.instance.hitCombo = 0;
            gm.UnPauseGame();
            tent = GameObject.FindGameObjectWithTag("Player");
            if (tent == null)
            {
                Debug.Log("No tent found!");
            }
            AudioManager.instance.StopAll();
            Debug.Log("here");
            AudioManager.instance.PlaySound("forest");
            starTime = Time.time;
            spawnPoint1 = GameObject.Find("SpawnPoint").transform;
            spawnPoint2 = GameObject.Find("SpawnPoint2").transform;
            KillText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
            TimerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
            Combo = GameObject.Find("Combo");
            ComboText = Combo.GetComponent<TextMeshProUGUI>();
            ComboTextAnimator = Combo.GetComponent<Animator>();


            
            buttons = GameObject.FindGameObjectsWithTag("BUTTON");
            buttonsPos = new Vector3[gm.buttons.Length];
            for (int i=0;i<gm.buttons.Length;i++)
            {
                gm.buttonsPos[i] = gm.buttons[i].GetComponent<RectTransform>().transform.position;
            }


            timerOn = true;
            hearts = GameObject.Find("HP");
            gm.score = 0;
            for (int i = 0; i < maxHealth; i++)
            {
                hearts.transform.GetChild(i).gameObject.SetActive(true);
            }
            health = maxHealth;

            if(scene.name== "FirstLevel")
            {
                
                Spawner.instance.Prepare(new int[] {0});
            }
            if (scene.name == "SecondLevel")
            {
                
                Spawner.instance.Prepare(new int[] { 0,1 });
            }
            if (scene.name == "ThirdLevel")
            {
                Spawner.instance.Prepare(new int[] { 0, 1, 2 });
            }
            gm.UnPauseGame();
        }
        else
        {
            timerOn = false;
        }
        
        if (scene.name == "MainMenu")
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
            gm.PauseGame();
            AudioManager.instance.StopAll();
            AudioManager.instance.PlaySound("menu");
            timerOn = false;
        }

    }

    // Update is called once per frame
    void Update()
    {


        
        //Timer
        if (timerOn)
        {
           
            float t = Time.time - starTime;

            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f2");
            gm.TimerText.text = minutes + ":" + seconds;

            //if (SceneManager.GetActiveScene().name == "FirstLevel")
            if (t > roundTime)
            {
                //put sun

                gm.PauseGame();
                
                
                Spawner.instance.Stop();
                if (!soundPlayed)
                {

                    LevelWon();
                    SaveSystem.Save(gm.playerStats);
                    AudioManager.instance.PlaySound("rooster");
                    soundPlayed = true;
                    StartCoroutine(WaitForSeconds());
                }

                if (!waiting)
                {
                    if (SceneManager.GetActiveScene().name == "FirstLevel"){
                        
                        LoadLevel(2);
                    }
                    else if (SceneManager.GetActiveScene().name == "ThirdLevel")
                    {
                        LoadMainMenu();
                    }
                    else
                    {
                        LoadLevel(3);
                    }
                    soundPlayed = false;
                }


            }
            

        }
        

    }

    IEnumerator WaitForSeconds()
    {
        
        waiting = true;
        yield return new WaitForSecondsRealtime(2f);
        waiting = false;
       
        
        
    }

    public void LevelWon()
    {
        
        if (currentLevel - NumberOfStartingLevels >= PlayerPrefs.GetInt("lvl"))
        {
            Debug.Log("SCORE "+gm.score);
            
            gm.playerStats.gold += gm.score;
            Debug.Log("LASTFINISHEDLEVEL"+gm.playerStats.lastFinishedLevel);
            gm.playerStats.lastFinishedLevel += 1;
            SaveSystem.Save(gm.playerStats);
           
        }
        Debug.Log("GOLD"+gm.playerStats.gold);

       
    }
    public static void HitEnemy(GameObject enemy, int dmg)
    {
        gm._HitEnemy(enemy,dmg);
        if (dmg < 9000)
        {
            if (enemy.GetComponent<JumpingMob>() != null)
            {
                AudioManager.instance.PlaySound("oink2");
            }
            else
            {
                AudioManager.instance.PlaySound("oink1");
            }
        }
        
    }

    private void _HitEnemy(GameObject enemy, int dmg)
    {
        Enemy buffer = enemy.GetComponent<Enemy>();
        buffer.GetHit(dmg);
    }

    public static void DamagePlayer(int dmg)
    {
        gm._DamagePlayer(dmg);
    }

    private void _DamagePlayer(int dmg)
    {
        
        tent.GetComponent<Animator>().SetTrigger("dmg");
        
        for (int i=1;i<=dmg;i++)
        {
            if (health - i > -1)
            {
                hearts.transform.GetChild(health - i).gameObject.SetActive(false);
            }
        }
        health -= dmg;

        if (health <= 0)
        {
            GameOver();
        }
        AudioManager.instance.PlaySound("ouch");
    }

    public static void UpdateScore()
    {
        gm._UpdateScore();
    }

    private void _UpdateScore()
    {
        KillText.text = "Score:" + ++score; 
    }

    public void PauseGame()
    {
        
        gm.paused = true;
        Time.timeScale = 0f;
    }
   

    public void UnPauseGame()
    {
        gm.paused = false;
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void GameOver()
    {
        AudioManager.instance.PlaySound("gameOver");
        timerOn = false;
        Spawner.instance.gameObject.SetActive(false);
        StopAllMobs();
        GameObject.Find("Canvas").transform.GetChild(4).gameObject.SetActive(true);
        Invoke("LoadMainMenu", 2f);
        
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        gm.currentLevel = 0;
    }

    public void LoadNextLevel()
    {
        
        gm.currentLevel = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("CurrentLVL"+gm.currentLevel);
        Debug.Log("from stats"+gm.playerStats.lastFinishedLevel);
        if (gm.currentLevel == 0 && gm.playerStats.lastFinishedLevel > 0)
        {
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }
    public void LoadLevel(int i)
    {
        SceneManager.LoadScene(i);
        gm.currentLevel = i;
    }

    public void StopAllMobs()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        for(int i = 0; i < enemies.Length; i++)
        {
            enemies[i].stats.canMove = false;
        }
        spawnPoint1.gameObject.SetActive(false);
        spawnPoint2.gameObject.SetActive(false);

    }

    //Items
    
    
  
}
