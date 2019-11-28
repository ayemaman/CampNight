using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

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
    public bool roundFinished;
    public int score;

    private int health;
    

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

    public int currentSlot;
    public NamesDescs[] namesAndDesc = null;




    public bool paused = false;
    
    // private int iteration = 0;

    private bool waiting = false;
    private bool soundPlayed=false;

    //levelChangeStuff
    public int NumberOfStartingLevels;
    public int currentLevel = 0;


    //Debugingstuff
    public int level1walk = 10;
    public int level2walk = 10;
    public int level2jump = 5;
    public int level3walk = 1;
    public int level3jump = 1;
    public int level3fly = 50;

    

    private int toEquip;
    private ItemBackPack itemBackPack;

    /*
    public InputField level1walkInput;
   
    public InputField level2walkInput;
    public InputField level2jumpInput;

    public InputField level3walkInput;
    public InputField level3jumpInput;
    public InputField level3flyInput;
    
    public void SetMobAmount()
    {
     
            try
            {
            gm.level1walk = int.Parse(gm.level1walkInput.text);
            gm.level2walk = int.Parse(gm.level2walkInput.text);
            gm.level2jump = int.Parse(gm.level2jumpInput.text);
            gm.level3walk = int.Parse(gm.level3walkInput.text);
            gm.level3jump = int.Parse(gm.level3jumpInput.text);
            gm.level3fly = int.Parse(gm.level3flyInput.text);
            
            }
            catch (FormatException e)
            {
                Debug.Log("wrong format: " + e);
            }

        
    }
    */


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
        gm.playerStats = new PlayerStats();
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
        Spawner.instance.ready = false;
        gm.roundFinished = false;
        gm.score = 0;
        TouchScript.instance.hitCombo = 0;
        TouchScript.instance.comboScore = 0;
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
            SaveSystem.Save(gm.playerStats);
       
        }
        if (gm != null)
        {
            gm.playerStats = SaveSystem.Load();
       
        }
        
       
        if (scene.name.ToLower().Contains("level"))
        {
            //preparing hud
            TouchScript.instance.hitCombo = 0;
            TouchScript.instance.itemButtons[0] = GameObject.Find("ITEM1").GetComponent<Button>();
            if (gm.playerStats.equipedItems[0] == PlayerItems.NOITEM)
            {
                TouchScript.instance.itemButtons[0].gameObject.SetActive(false);
            }
            else
            {
                TouchScript.instance.itemButtons[0].gameObject.SetActive(true);
            }
            TouchScript.instance.itemButtons[1] = GameObject.Find("ITEM2").GetComponent<Button>();
            if(gm.playerStats.equipedItems[1] == PlayerItems.NOITEM)
            {
                TouchScript.instance.itemButtons[1].gameObject.SetActive(false);
            }
            else
            {
                TouchScript.instance.itemButtons[1].gameObject.SetActive(true);
            }
            TouchScript.instance.itemButtons[2] = GameObject.Find("ITEM3").GetComponent<Button>();
            if (gm.playerStats.equipedItems[2] == PlayerItems.NOITEM)
            {
                TouchScript.instance.itemButtons[2].interactable = false;
               
            }
            else
            {
                TouchScript.instance.itemButtons[2].interactable = true;
            }
            TouchScript.instance.itemsOnCdText[0] = TouchScript.instance.itemButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TouchScript.instance.itemsOnCdText[1] = TouchScript.instance.itemButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TouchScript.instance.itemsOnCdText[2] = TouchScript.instance.itemButtons[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TouchScript.instance.itemsOnCdText[0].gameObject.SetActive(false);
            TouchScript.instance.itemsOnCdText[1].gameObject.SetActive(false);
            TouchScript.instance.itemsOnCdText[2].gameObject.SetActive(false);
            TouchScript.instance.SetUpItemButtons();


            gm.UnPauseGame();
            tent = GameObject.FindGameObjectWithTag("Player");
            if (tent == null)
            {
                Debug.Log("No tent found!");
            }
            AudioManager.instance.StopAll();
            
            AudioManager.instance.PlaySound("forest");
            starTime = Time.time;
            spawnPoint1 = GameObject.Find("SpawnPointLeftBottom").transform;
            spawnPoint2 = GameObject.Find("SpawnPointRightBottom").transform;
            KillText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
            KillText.fontSize = 50;
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
            for (int i = 0; i < gm.playerStats.hp; i++)
            {
                hearts.transform.GetChild(i).gameObject.SetActive(true);
            }
            health = gm.playerStats.hp; ;

            
            

            if(scene.name== "FirstLevel")
            {
                
                Spawner.instance.Prepare(new MobAmountsForLevel[] {new MobAmountsForLevel(0, 20) });
            }
            if (scene.name == "SecondLevel")
            {
                
                Spawner.instance.Prepare(new MobAmountsForLevel[] { new MobAmountsForLevel(0, 15),new MobAmountsForLevel(1, 10) });
            }
            if (scene.name == "ThirdLevel")
            {
          
                Spawner.instance.Prepare(new MobAmountsForLevel[] { new MobAmountsForLevel(0, 10), new MobAmountsForLevel(1, 5), new MobAmountsForLevel(2, 5), new MobAmountsForLevel(6, 5) });
            }
            if(scene.name == "DarkForestLevel")
            {
                Spawner.instance.Prepare(new MobAmountsForLevel[] { new MobAmountsForLevel(0, 10), new MobAmountsForLevel(1, 10), new MobAmountsForLevel(2, 10) });
            }
            if (scene.name == "SunnyVillLevel")
            {
                Spawner.instance.Prepare(new MobAmountsForLevel[] { new MobAmountsForLevel(0, 10), new MobAmountsForLevel(1, 10), new MobAmountsForLevel(2, 10) });

            }
            if (scene.name == "HellVillLevel")
            {
                Spawner.instance.Prepare(new MobAmountsForLevel[] { new MobAmountsForLevel(0, 50), new MobAmountsForLevel(1, 50), new MobAmountsForLevel(2, 50) });
            }
            
            gm.UnPauseGame();
        }
        else
        {
            timerOn = false;
        }
        
        if (scene.name == "StartingScreen")
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
            gm.PauseGame();
            Time.timeScale = 1f;
            AudioManager.instance.StopAll();
            AudioManager.instance.PlaySound("menu");
            timerOn = false;
            gm.playerStats=SaveSystem.Load();
        }

        if(scene.name == "LvlSelect")
        {
            gm.itemBackPack = GameObject.FindGameObjectWithTag("LVLSELECTOR").GetComponent<ItemBackPack>();
        }

    }

    //for testing

       

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
            if (!FindObjectOfType<Enemy>())
            {
                if (roundFinished)
                {
                    //put sun

                    gm.PauseGame();
                    Spawner.instance.Stop();
                    if (!gm.soundPlayed)
                    {
                        Debug.Log(gm.soundPlayed);
                        LevelWon();
                        gm.ComboTextAnimator.SetTrigger("endAnimation");
                        gm.ComboText.text = "You've collected " + gm.score + " points this round";
                        gm.ComboText.fontSize = 40f;
                        gm._UpdateScore();
                        SaveSystem.Save(gm.playerStats);
                        AudioManager.instance.PlaySound("rooster");
                        gm.soundPlayed = true;
                        StartCoroutine(WaitForSeconds());
                    }

                    if (!gm.waiting)
                    {
                        if (SceneManager.GetActiveScene().name == "FirstLevel")
                        {
                            Debug.Log("here1");
                            if (gm.playerStats.lastFinishedLevel > 0)
                            {
                                Debug.Log("here2");
                                LoadLevel(3);
                            }
                            else
                            {
                                Debug.Log("here3");
                                LoadLevel(2);
                            }
                        }
                        else if (SceneManager.GetActiveScene().name == "ThirdLevel")
                        {
                            LoadMainMenu();
                        }
                        else
                        {
                            LoadLevel(3);
                        }
                        gm.soundPlayed = false;
                    }
                }
            }
            

        }
        

    }

    IEnumerator WaitForSeconds()
    {

        gm.waiting = true;
        yield return new WaitForSecondsRealtime(2f);
        gm.waiting = false;
       
        
        
    }

    public static void EndRound()
    {
        gm._EndRound();
    }

    private void _EndRound()
    {
        gm.roundFinished = true;
    }

    public void LevelWon()
    {
        gm.score += TouchScript.instance.comboScore * TouchScript.instance.hitCombo;
       // Debug.Log(gm.currentLevel-NumberOfStartingLevels);
        int totalpoints = 0;
        int totalCombo = 0;
        for (int i = 0; i < Spawner.instance.currentEnemies.Length; i++)
        {
           // Debug.Log("ENEMYAMOUNT "+Spawner.instance.enemyAmountStart[i]);
            totalpoints += Spawner.instance.currentEnemies[i].stats.points * Spawner.instance.enemyAmountStart[i];
            totalCombo += Spawner.instance.currentEnemies[i].stats.hp * Spawner.instance.enemyAmountStart[i];
        }
        totalpoints *= totalCombo;
        //Debug.Log("totalCombo:" + totalCombo + " TOTAL RESULT POSSIBLE: " + totalpoints);
        int stars = 0;

        if (gm.score > totalpoints/20)
        {
            stars = 1;
        }
        if (gm.score > ((totalpoints / 10)*5))
        {
            stars = 2;
        }

        if(gm.score > ( (totalpoints / 10) * 9))
        {
            stars = 3;
        }
        

        if (gm.playerStats.starProgress[gm.currentLevel - gm.NumberOfStartingLevels].Value < stars)
        {
            gm.playerStats.gold+= stars - gm.playerStats.starProgress[gm.currentLevel - gm.NumberOfStartingLevels].Value;
            //Debug.Log("setting for level " + (gm.currentLevel - gm.NumberOfStartingLevels));
            gm.playerStats.starProgress[gm.currentLevel - gm.NumberOfStartingLevels] = new KeyValuePair<int, int>(gm.currentLevel - gm.NumberOfStartingLevels, stars);
            
            if (gm.currentLevel - gm.NumberOfStartingLevels >= gm.playerStats.lastFinishedLevel)
            {
                gm.playerStats.lastFinishedLevel += 1;
                
            }
            SaveSystem.Save(gm.playerStats);

        }
      
        gm.playerStats=SaveSystem.Load();
       // Debug.Log("STAR PROGRESS "+gm.playerStats.starProgress[0]);

        
       // TODO find out current finished level, check amount of stars, update if progress.  if(gm.playerStats.starProgress[currentLevel - NumberOfStartingLevels])

      

       
    }



    public static void HitEnemy(GameObject enemy, int dmg)
    {
        BombardierMob bombardierMob = enemy.GetComponent<BombardierMob>();
        if (bombardierMob != null)
        {
            bombardierMob.ReleaseBomb();
        }
        else
        {
            gm._HitEnemy(enemy, dmg);
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
        if (gm.score > 99999999)
        {
            KillText.fontSize = 34;
        }
        else if (gm.score > 9999999)
        {
            KillText.fontSize = 38;
        }else if (gm.score > 999999)
        {
            KillText.fontSize = 42;
        }else if (gm.score > 99999)
        {
            KillText.fontSize = 46;
        }
   
            KillText.text = "Score:" + gm.score; 
    }

    public void PauseGame()
    {
        
        gm.paused = true;
        Time.timeScale = 0f;
        AudioManager.instance.PauseSound("forest");
        AudioManager.instance.PlaySound("menu");
    }
   

    public void UnPauseGame()
    {
        gm.paused = false;
        Time.timeScale = 1f;
        AudioManager.instance.StopSound("menu");
        AudioManager.instance.PlaySound("forest");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void GameOver()
    { 
        AudioManager.instance.PlaySound("gameOver");
        timerOn = false;
        Spawner.instance.ready = false;
        StopAllMobs();
        GameObject.Find("Canvas 1").transform.GetChild(7).gameObject.SetActive(true);
        GameObject.Find("Canvas 1").transform.GetChild(8).gameObject.SetActive(true);
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
      
        if (gm.currentLevel == 0 && gm.playerStats.lastFinishedLevel > 0)
        {
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
           // Debug.Log("here");
        }
        else
        {
            //Debug.Log("here2");
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
    
    public void CurrentSlot(int slot)
    {
        gm._CurrentSlot(slot);
    }
    private void _CurrentSlot(int slot)
    {
        gm.currentSlot = slot;
    }

    public void PrepareEquipItem(int item)
    {
        gm._PrepareEquipItem(item);
    }

    private int getItemIDfromAvailable(PlayerItems item)
    {
        return gm.playerStats.availableItems.IndexOf(item);
 
    }

    private void _PrepareEquipItem(int item)
    {
        Debug.Log(gm.playerStats.availableItems.IndexOf((PlayerItems) item));



        gm.toEquip = (int)gm.playerStats.availableItems[item];

        //gm.itemBackPack.SetText((int)gm.playerStats.availableItems[item]);
        gm.itemBackPack.SetText(item);
    }

    public void PrepareFromButton(int button)
    {
        gm._PrepareFromButton(button);
    }
    private void _PrepareFromButton(int button)
    {
        gm.toEquip = (int) gm.playerStats.availableItems[button];
        gm.itemBackPack.SetText((int)gm.playerStats.availableItems[button]);
    }

    public void EquipItems()
    {
        gm._EquipItems();
    }

    private void _EquipItems()
    {
        for (int i = 0; i < gm.playerStats.equipedItems.Length; i++)
        {
            if ((int)gm.playerStats.equipedItems[i] == gm.toEquip)
            {
               
                gm.playerStats.equipedItems[i] = gm.playerStats.equipedItems[gm.currentSlot];
            }
        }
        Debug.Log(gm.toEquip);
        gm.playerStats.equipedItems[gm.currentSlot] = (PlayerItems)gm.toEquip;
    }
    
    public void Save()
    {
        gm._Save();
    }
    
    private void _Save()
    {
        SaveSystem.Save(gm.playerStats);
    }

    public int GetTotalStartProgress()
    {
        int buffer = 0;
        foreach(KeyValuePair<int,int> key in GameMaster.gm.playerStats.starProgress)
        {
            buffer += key.Value;
        }
        return buffer;
    }
  
}
