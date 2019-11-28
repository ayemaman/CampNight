using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TouchScript : MonoBehaviour
{
    public static TouchScript instance;
    private Touch touch;
    private Vector2 touchPosition;
    private Collider2D touchCollider;

    

    /*
    private float _item1Timer;
    private float item1Timer;
    */
    public float[] itemsTimers=new float[3];
    private float[] _itemsTimers=new float[3];
    private bool[] itemsOnCD= { false, false, false };

    public TextMeshProUGUI[] itemsOnCdText=new TextMeshProUGUI[3];

    //public TextMeshProUGUI item1CDtext;
    public Button[] itemButtons=new Button[3];
    /*
    public Button item1;
    public Button item2;
    public Button item3;
    */

    public int comboScore = 0;

    // private Collider2D col;
    public GameObject touchParticle;
    public GameObject smokeParticle;
    public float radius;
    public float swatterRadius;

    public GameObject line;
    public GameObject SwatterPrefab;

    public int hitCombo = 0;
    private int maxCombo;
    private int lastCombo;
    private bool hitComboDown;

    public bool buttonIsPressed = false;
    //ITEMS
    private Vector3 buttonPos;
    public PlayerItems currentItem = PlayerItems.NOITEM;

    public float buttonOverfLow;



    public float RifleTimer;
    private float _RifleTimer;
    public float RifleBetweenShotsTimer;
    private float _RifleBetweenShotsTimer;
    public float RifleStartYCorrection;
    public int RifleShots=3;
    private int _RifleShots=3; //change after test;
    public bool firstPass=false;
    private GameObject Rifle;
    private bool canInstantiate;


    public int FLYswatterSHOT=2;
    private int _FLYswatterSHOT = 2;

    private GameObject Swatter;
    public GameObject SwatterTest;



    //



    //
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {

                Destroy(this.gameObject);
            }
            Debug.Log("More then one TouchScript in scene");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    
        void Start()
    {
        
    }

    private void putItemOnCd()
    {
        // Debug.Log(instance.currentItem);
        
        for(int i=0; i< GameMaster.gm.playerStats.equipedItems.Length; i++)
        {
            
            if (GameMaster.gm.playerStats.equipedItems[i] != PlayerItems.NOITEM)
            {
                //find what is current item
                if (GameMaster.gm.playerStats.equipedItems[i] == instance.currentItem)
                {
                    //set item on CD

                    instance.itemButtons[i].image.sprite = GameMaster.gm.namesAndDesc[(int)GameMaster.gm.playerStats.equipedItems[i] - 1].normal;
                    instance.itemsOnCD[i] = true;
                    instance._itemsTimers[i] = instance.itemsTimers[i];

                    instance.itemsOnCdText[i].gameObject.SetActive(true);
                    ResetItems();
                    instance.itemButtons[i].interactable = false;
                    break;
                }
            }
        }
    }

    private void putItemOutOfCd(int i)
    {
        instance.itemsOnCD[i] = false;
        instance.itemsOnCdText[i].gameObject.SetActive(false);
        instance.itemButtons[i].interactable = true;


    }

   

    // Update is called once per frame
    void Update()
    {
        if (GameMaster.gm.paused == true) {
        }
        else {
            for (int i = 0; i < instance.itemsOnCD.Length; i++)
            {
                //if item on Cooldown
                if (instance.itemsOnCD[i])
                {
                    //Debug.Log("here aaa");

                    if (instance._itemsTimers[i] > 0)
                    {
                        //Debug.Log(i+" TIMER :"+instance._itemsTimers[i]);
                        instance._itemsTimers[i] -= Time.deltaTime;
                        instance.itemsOnCdText[i].text = "" + (int)instance._itemsTimers[i];
                        // Debug.Log("ITEM " +i+" " +instance.itemsOnCD[i]);

                    }

                    //if cd timer < 0 => make item available to equip again / switch off timer text
                    else
                    {
                        putItemOutOfCd(i);
                    }
                }
            }
        }
        
     
        //IF GAME IS PAUSED
        if (GameMaster.gm.paused == true)
        {
            if (Input.touchCount > 0)
            {
                instance.touch = Input.GetTouch(0);
                if (instance.touch.phase == TouchPhase.Began)
                {
                    AudioManager.instance.PlaySound("tap");
                }
            }
        }
        //DURING GAME
        else
        {
            if (Input.touchCount > 0)
            {
                instance.touch = Input.GetTouch(0);
                instance.touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                instance.touchCollider = Physics2D.OverlapCircle(touchPosition, radius);

                

                switch (currentItem)
                {

                    
                    case PlayerItems.NOITEM:
                        instance.hitComboDown = false;
                        if (instance.touch.phase == TouchPhase.Began)
                        {
                            AudioManager.instance.PlaySound("tap");
                            Instantiate(touchParticle, new Vector3(instance.touchPosition.x, instance.touchPosition.y, -5), Quaternion.identity);
                            instance.touchCollider = Physics2D.OverlapCircle(instance.touchPosition, instance.radius);
                            if (instance.touchCollider != null)
                            {
                                if (instance.touchCollider.GetComponent<Enemy>() != null)
                                {

                                    GameMaster.HitEnemy(touchCollider.gameObject, 1);
                                    if (instance.touchCollider.GetComponent<BombardierMob>() == null && instance.touchCollider.GetComponent<Minibomb>() == null)
                                    {
                                        instance.hitCombo++;

                                        if (instance.hitCombo > 0)
                                        {

                                            GameMaster.gm.ComboText.text = instance.comboScore + System.Environment.NewLine + "x" + instance.hitCombo;
                                            GameMaster.gm.ComboTextAnimator.SetTrigger("startAnimation");
                                        }
                                    }


                                }
                                else
                                {
                                    instance.hitComboDown = true;
                                    for (int i = 0; i < GameMaster.gm.buttonsPos.Length; i++)
                                    {
                                        if (Vector2.Distance(new Vector2(Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).x, Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).y), instance.touchPosition) < instance.buttonOverfLow)
                                        {
                                            /*
                                            Debug.Log("BUTPOS: "+new Vector2(Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).x, Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).y));
                                            Debug.Log("touchPos: " + instance.touchPosition);
                                            */
                                            instance.hitComboDown = false;

                                        }
                                    }

                                    if (instance.hitComboDown)
                                    {


                                        GameMaster.gm.score += instance.comboScore * instance.hitCombo;
                                        GameMaster.UpdateScore();
                                        instance.hitCombo = 0;
                                        instance.comboScore = 0;
                                        GameMaster.gm.ComboTextAnimator.SetTrigger("clearAnimation");
                                    }

                                }
                            }
                            else
                            {
                               
                                for (int i = 0; i < GameMaster.gm.buttonsPos.Length; i++)
                                {
                                    instance.hitComboDown = true;
                                    if (Vector2.Distance(new Vector2(Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).x, Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).y), instance.touchPosition) < instance.buttonOverfLow)
                                    {
                                        /*
                                        Debug.Log("BUTPOS: " + new Vector2(Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).x, Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).y));
                                        Debug.Log("touchPos: " + instance.touchPosition);
                                        */
                                        instance.hitComboDown = false;
                                       
                                    }
                                }
                                
                                if (hitComboDown)
                                {
                                    
                                    
                                    GameMaster.gm.score += instance.comboScore * instance.hitCombo;
                                    GameMaster.UpdateScore();
                                    instance.hitCombo = 0;
                                    instance.comboScore = 0;
                                    GameMaster.gm.ComboTextAnimator.SetTrigger("clearAnimation");
                                }
                            }
                        }
                      
                        break;


                    case PlayerItems.RIFLE:

                        
                        if (instance.touch.phase == TouchPhase.Began)
                        {
                            Destroy(Rifle);
                            
                           
                            instance.canInstantiate = true;
                            //Destroy(Rifle);
                            
                            for (int i = 0; i < GameMaster.gm.buttonsPos.Length; i++)
                            {
                                /*
                                Debug.Log("{{{");
                                Debug.Log("BUTPOS: " + new Vector2(Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).x, Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).y));
                                Debug.Log("touchPos: " + instance.touchPosition);
                                Debug.Log("}}}");
                                */
                                if (Vector2.Distance(new Vector2(Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).x, Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).y), instance.touchPosition) < instance.buttonOverfLow)
                                {
                                    /*
                                    Debug.Log("BUTPOS: " + new Vector2(Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).x, Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).y));
                                    Debug.Log("touchPos: " + instance.touchPosition);
                                    */
                                    instance.canInstantiate = false;
                                    
                                }
                            }
                         
                            if (instance.canInstantiate)
                            {
                                instance.Rifle = Instantiate(line, new Vector2(GameMaster.gm.tent.transform.position.x, GameMaster.gm.tent.transform.position.y - RifleStartYCorrection), Quaternion.Euler(0, 0, Mathf.Atan2(touchPosition.y - (GameMaster.gm.tent.transform.position.y - RifleStartYCorrection), instance.touchPosition.x - GameMaster.gm.tent.transform.position.x) * Mathf.Rad2Deg));
                            }
                            



                        }
                        else if (instance.touch.phase == TouchPhase.Moved)
                        {
                       
                            if (instance.canInstantiate)
                            {
                                instance.touchPosition = Camera.main.ScreenToWorldPoint(instance.touch.position);
                                Rifle.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(touchPosition.y - (GameMaster.gm.tent.transform.position.y - instance.RifleStartYCorrection), touchPosition.x - GameMaster.gm.tent.transform.position.x) * Mathf.Rad2Deg);
                            }
                        }
                        else if (instance.touch.phase == TouchPhase.Ended)
                        {

                          
                            if (instance.firstPass)
                            {

                                instance.firstPass = false;
                            }
                            else
                            {
                                if (instance.canInstantiate)
                                {
                                    AudioManager.instance.PlaySound("rifleShot");
                                    instance._RifleShots -= 1;
                                    instance.Rifle.transform.GetChild(0).gameObject.SetActive(true);
                                    Destroy(Rifle, 0.1f);
                                }
                            }


                        }

                        if (instance._RifleShots == 0)
                        {
                            putItemOnCd();
                            instance._RifleShots = instance.RifleShots;
                        }
                        break;

                    case PlayerItems.FLY_SWATTER:


                        if (instance.touch.phase == TouchPhase.Began)
                        {

                            instance.canInstantiate = true;

                            for (int i = 0; i < GameMaster.gm.buttonsPos.Length; i++)
                            {

                                if (Vector2.Distance(new Vector2(Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).x, Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).y), instance.touchPosition) < instance.buttonOverfLow)
                                {

                                    instance.canInstantiate = false;

                                }
                            }
                            if (instance.canInstantiate)
                            {

                                // Debug.Log("instantiate swatter");
                                // instance.Swatter = Instantiate(line, new Vector2(GameMaster.gm.tent.transform.position.x, GameMaster.gm.tent.transform.position.y - RifleStartYCorrection), Quaternion.Euler(0, 0, Mathf.Atan2(touchPosition.y - (GameMaster.gm.tent.transform.position.y - RifleStartYCorrection), instance.touchPosition.x - GameMaster.gm.tent.transform.position.x) * Mathf.Rad2Deg));
                                if (instance._FLYswatterSHOT != 0)
                                {
                                    AudioManager.instance.PlaySound("slap");
                                    Instantiate(instance.smokeParticle, new Vector3(instance.touchPosition.x, instance.touchPosition.y, -5), Quaternion.identity);
                                    Instantiate(instance.SwatterTest, new Vector3(instance.touchPosition.x + 10, instance.touchPosition.y, -5), Quaternion.identity);
                                    instance._FLYswatterSHOT -= 1;
                                    Collider2D[] swatterCollider = Physics2D.OverlapCircleAll(instance.touchPosition, instance.swatterRadius);
                                    bool hit = false;
                                    foreach (Collider2D col in swatterCollider)
                                    {
                                        if (col.GetComponent<Enemy>() != null)
                                        {
                                            hit = true;
                                            instance.hitCombo++;
                                            GameMaster.HitEnemy(col.gameObject, 1);
                                            
                                        }
                                    }
                                    if (instance.hitCombo > 0 && hit)
                                    {
                                        GameMaster.gm.ComboText.text = instance.comboScore + System.Environment.NewLine + "x" + instance.hitCombo;
                                        GameMaster.gm.ComboTextAnimator.SetTrigger("startAnimation");
                                    }
                                }
                                
                            }
                        }
                        //Debug.Log(instance._FLYswatterSHOT);
                        if (instance._FLYswatterSHOT == 0)
                        {
                            putItemOnCd();
                            instance._FLYswatterSHOT = instance.FLYswatterSHOT;
                        }


                            break;

                }
            }
        }
    }

    /*
    public void ButtonPress()
    {
        instance.buttonIsPressed = true;
        Debug.Log(GameMaster.gm.playerStats.equipedItems[0]);
        Debug.Log(instance.currentItem);
    }
*/
    public void ResetItems()
    {
        //TODO more items to reset;
        if (instance.currentItem == PlayerItems.RIFLE)
        {
            instance._RifleShots = instance.RifleShots;
        }else if(instance.currentItem == PlayerItems.FLY_SWATTER)
        {
            instance._FLYswatterSHOT = instance.FLYswatterSHOT;
        }

        instance.currentItem = PlayerItems.NOITEM;
        
        //rifle reset
        // _RifleShots = RifleShots;
    }

    public void ItemOneOn()
    {

        //if FirstSlot is not on CD => allow to equip
 
        if (!instance.itemsOnCD[0])
        {
            if (instance.currentItem != GameMaster.gm.playerStats.equipedItems[0])
            {
                putItemOnCd();
                instance.currentItem = GameMaster.gm.playerStats.equipedItems[0];
                instance.firstPass = true;
                instance._itemsTimers[0] = instance.itemsTimers[0];
                instance.itemButtons[0].image.sprite = GameMaster.gm.namesAndDesc[(int)GameMaster.gm.playerStats.equipedItems[0] - 1].selected;
                
            }
            else
            {
                Debug.Log("here");  
                putItemOnCd();
                
            }
        }
        

    }

    public void ItemTwoOn()
    {
        //if 1slot is not on CD => allow to equip
        if (!instance.itemsOnCD[1])
        {

            if (instance.currentItem != GameMaster.gm.playerStats.equipedItems[1])
            {
                putItemOnCd();
                ResetItems();
                instance.currentItem = GameMaster.gm.playerStats.equipedItems[1];
                instance.firstPass = true;
                instance._itemsTimers[1] = instance.itemsTimers[1];
                Debug.Log((int)GameMaster.gm.playerStats.equipedItems[1] - 1);
                instance.itemButtons[1].image.sprite = GameMaster.gm.namesAndDesc[(int)GameMaster.gm.playerStats.equipedItems[1] - 1].selected;

            }
            else
            {
                
                putItemOnCd();
     
            }
        }
        
    }

    public void ItemThreeOn()
    {
        //if 1slot is not on CD => allow to equip
        if (!instance.itemsOnCD[2])
        {
            if (instance.currentItem != GameMaster.gm.playerStats.equipedItems[2])
            {
                putItemOnCd();
                ResetItems();
                instance.currentItem = GameMaster.gm.playerStats.equipedItems[2];
                instance.firstPass = true;
                instance._itemsTimers[2] = instance.itemsTimers[2];
                instance.itemButtons[2].image.sprite = GameMaster.gm.namesAndDesc[(int)GameMaster.gm.playerStats.equipedItems[2] - 1].selected;

            }
            else
            {

                putItemOnCd();

            }
        }
        
    }


    public void SetUpItemButtons()
    {
        if ((int)GameMaster.gm.playerStats.equipedItems[0] != 0)
        {
            itemButtons[0].image.sprite = GameMaster.gm.namesAndDesc[(int)GameMaster.gm.playerStats.equipedItems[0] - 1].normal;
            instance.itemsTimers[0] = GameMaster.gm.namesAndDesc[(int)GameMaster.gm.playerStats.equipedItems[0] - 1].cd;
            itemButtons[0].interactable = true;
        }
        else
        {
            itemButtons[0].interactable = false;
        }
        if ((int)GameMaster.gm.playerStats.equipedItems[1] != 0) {
            itemButtons[1].image.sprite = GameMaster.gm.namesAndDesc[(int)GameMaster.gm.playerStats.equipedItems[1] - 1].normal;
            instance.itemsTimers[1] = GameMaster.gm.namesAndDesc[(int)GameMaster.gm.playerStats.equipedItems[1] - 1].cd;
            itemButtons[1].interactable = true;
        }
        else
        {
            itemButtons[1].interactable = false;
        }
        if ((int)GameMaster.gm.playerStats.equipedItems[2] != 0)
        {
            itemButtons[2].image.sprite = GameMaster.gm.namesAndDesc[(int)GameMaster.gm.playerStats.equipedItems[2] - 1].normal;
            instance.itemsTimers[2] = GameMaster.gm.namesAndDesc[(int)GameMaster.gm.playerStats.equipedItems[2] - 1].cd;
            itemButtons[2].interactable = true;
        }
        else
        {
            itemButtons[2].interactable = false;
        }
    }
}

[System.Serializable]
public enum PlayerItems
{
    NOITEM,
    RIFLE,
    FLY_SWATTER

}



