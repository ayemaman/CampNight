using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchScript : MonoBehaviour
{
    public static TouchScript instance;
    private Touch touch;
    private Vector2 touchPosition;
    private Collider2D touchCollider;

    public int comboScore = 0;

    // private Collider2D col;
    public GameObject touchParticle;
    public float radius;

    public GameObject line;

    public int hitCombo = 0;
    private int maxCombo;
    private int lastCombo;

    public bool buttonIsPressed = false;
    //ITEMS
    private Vector3 buttonPos;
    public ItemsState currentItem= ItemsState.NOITEM;

  
    public float RifleTimer;
    private float _RifleTimer;
    public float RifleBetweenShotsTimer;
    private float _RifleBetweenShotsTimer;
    public float RifleStartYCorrection;
    public int RifleShots=5;
    private int _RifleShots=5; //change after test;
    public bool firstPass=false;
    private GameObject Rifle;
    private bool canInstantiate;

    

    //
    public void ButtonPress()
    {
        instance.buttonIsPressed = true;
    }
    
    public void ResetItems()
    {
        RifleOff();
    }
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

    // Update is called once per frame
    void Update()
    {
        //IF GAME IS PAUSED
        if (GameMaster.gm.paused == true)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
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
                touch = Input.GetTouch(0);
                touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchCollider = Physics2D.OverlapCircle(touchPosition, radius);

                

                switch (currentItem)
                {

                    
                    case ItemsState.NOITEM:
                        bool hitComboDown = false;
                        if (touch.phase == TouchPhase.Began)
                        {
                            AudioManager.instance.PlaySound("tap");
                            Instantiate(touchParticle, new Vector3(touchPosition.x, touchPosition.y, -5), Quaternion.identity);
                            touchCollider = Physics2D.OverlapCircle(touchPosition, radius);
                            if (touchCollider != null)
                            {
                                if (touchCollider.GetComponent<Enemy>() != null)
                                {
                                    //Debug.Log(hitCombo);
                                    instance.hitCombo++;
                                    //Debug.Log("Adding NOITEM: " + hitCombo);
                                   // Debug.Log(hitCombo);
                                    GameMaster.HitEnemy(touchCollider.gameObject, 1);
                                    if (instance.hitCombo > 0)
                                    {
                                        
                                        GameMaster.gm.ComboText.text = comboScore+ "/n"+"x" + hitCombo;
                                        GameMaster.gm.ComboTextAnimator.SetTrigger("startAnimation");
                                    }

                                }
                                else
                                {

                                    for (int i=0; i<GameMaster.gm.buttonsPos.Length;i++)
                                    {
                                        if (Vector2.Distance(new Vector2(Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).x, Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).y), touchPosition) > 1.5f)
                                        {
                                            hitComboDown = true;
                                            
                                        }
                                    }
                                    if (hitComboDown)
                                    {
                                        instance.hitCombo = 0;
                                        instance.comboScore = 0;
                                        GameMaster.UpdateScore();
                                        GameMaster.gm.score += comboScore * hitCombo;
                                        GameMaster.gm.ComboTextAnimator.SetTrigger("clearAnimation");
                                    }

                                }
                            }
                            else
                            {
                                for (int i = 0; i < GameMaster.gm.buttonsPos.Length; i++)
                                {
                                    if (Vector2.Distance(new Vector2(Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).x, Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).y), touchPosition) > 1.5f)
                                    {
                                        hitComboDown = true;
                                       
                                    }
                                }
                                if (hitComboDown)
                                {
                                    instance.hitCombo = 0;
                                    instance.comboScore = 0;
                                    GameMaster.UpdateScore();
                                    GameMaster.gm.score += comboScore * hitCombo;
                                    GameMaster.gm.ComboTextAnimator.SetTrigger("clearAnimation");
                                }
                            }
                        }
                        break;


                    case ItemsState.RIFFLE:

                        
                        if (touch.phase == TouchPhase.Began)
                        {
                            Destroy(Rifle);
                           
                            instance.canInstantiate = true;
                            //Destroy(Rifle);
                            
                            for (int i = 0; i < GameMaster.gm.buttonsPos.Length; i++)
                            {
                             
                                
                                if (Vector2.Distance(new Vector2(Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).x, Camera.main.ScreenToWorldPoint(GameMaster.gm.buttonsPos[i]).y), touchPosition) < 1.5f)
                                {
                                    
                                    
                                    
                                    instance.canInstantiate = false;
                                    
                                }
                            }
                         
                            if (instance.canInstantiate)
                            {
                                Rifle = Instantiate(line, new Vector2(GameMaster.gm.tent.transform.position.x, GameMaster.gm.tent.transform.position.y - RifleStartYCorrection), Quaternion.Euler(0, 0, Mathf.Atan2(touchPosition.y - (GameMaster.gm.tent.transform.position.y - RifleStartYCorrection), touchPosition.x - GameMaster.gm.tent.transform.position.x) * Mathf.Rad2Deg));
                            }
                            



                        }
                        else if (touch.phase == TouchPhase.Moved)
                        {
                       
                            if (instance.canInstantiate)
                            {
                                touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                                Rifle.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(touchPosition.y - (GameMaster.gm.tent.transform.position.y - RifleStartYCorrection), touchPosition.x - GameMaster.gm.tent.transform.position.x) * Mathf.Rad2Deg);
                            }
                        }
                        else if (touch.phase == TouchPhase.Ended)
                        {

                          
                            if (firstPass)
                            {
                              
                                firstPass = false;
                            }
                            else
                            {
                                if (canInstantiate)
                                {
                                    AudioManager.instance.PlaySound("rifleShot");
                                    _RifleShots -= 1;
                                    Rifle.transform.GetChild(0).gameObject.SetActive(true);
                                    Destroy(Rifle, 0.1f);
                                }
                            }


                        }

                        if (_RifleShots == 0)
                        {
                            _RifleShots = RifleShots;
                        
                            instance.RifleOff();
                        }
                        break;

                }
            }
        }
    }
    

    public void RifleON()
    {
        if (instance.currentItem != ItemsState.RIFFLE)
        {
            instance.currentItem = ItemsState.RIFFLE;
            instance.firstPass = true;
        }
    }
    public void RifleOff()
    {   
        instance.currentItem = ItemsState.NOITEM;
    }


}

public enum ItemsState
{
    NOITEM,
    RIFFLE,
    
}



