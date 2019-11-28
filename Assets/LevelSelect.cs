using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    
   
    public TextMeshProUGUI areaName;
   
    public Canvas canvas;
    
    public Text text;
    public GameObject ZoneSelectCanvas;
    public GameObject LevelSelectCanvas;

    public Button[] LevelButtons = new Button[3];
    public Badge[] LevelSprites = null;
    public Button[] ZoneButtons = new Button[4];
    public Badge[] ZoneSprites= new Badge[4];

    [SerializeField]
    public string[] ZoneName = null;

    private void Awake()
    {
        SetUpZoneSelect();

    }

    private void Update()
    {
       
    }

 
    public void SetUpZoneSelect()
    {
        
        LevelSelectCanvas.SetActive(false);
        ZoneSelectCanvas.SetActive(true);

        int lastLVL = GameMaster.gm.playerStats.lastFinishedLevel;
           // PlayerPrefs.GetInt("lvl");
        int zonesOpened = lastLVL / 3;
        int i;
        
        for (i = 0; i <= zonesOpened; i++)
        {
            ZoneButtons[i].interactable = true;
            ZoneButtons[i].GetComponent<Image>().sprite = ZoneSprites[i].open;
        }
        
        while (i < 4)
        {
            ZoneButtons[i].interactable = false;
            ZoneButtons[i].GetComponent<Image>().sprite = ZoneSprites[i].closed;
            i++;
        }
        
    }

    public void SetUpLevelSelect(int zone)
    {
        areaName.text = ZoneName[zone];
        ZoneSelectCanvas.SetActive(false);
        LevelSelectCanvas.SetActive(true);
       

        int lastLVL = GameMaster.gm.playerStats.lastFinishedLevel;
            //PlayerPrefs.GetInt("lvl");
        if (lastLVL / 3 > zone)
        {
            for (int c = 0; c < 3; c++)
            {
                LevelButtons[c].interactable = true;

                int levelToLoad = ((zone) * 3) + c + GameMaster.gm.NumberOfStartingLevels;
                LevelButtons[c].onClick.RemoveAllListeners();
                LevelButtons[c].onClick.AddListener(delegate { GameMaster.gm.LoadLevel(levelToLoad); });
                LevelButtons[c].GetComponent<Image>().sprite = LevelSprites[zone].open;
            }
        }
        else
        {
            int i;
            for (i = 0; i <= lastLVL % 3; i++)
            {
                LevelButtons[i].interactable = true;

                int levelToLoad = ((zone) * 3) + i + GameMaster.gm.NumberOfStartingLevels;
                LevelButtons[i].onClick.RemoveAllListeners();
                LevelButtons[i].onClick.AddListener(delegate { GameMaster.gm.LoadLevel(levelToLoad); });
                LevelButtons[i].GetComponent<Image>().sprite = LevelSprites[zone].open;
            }
            if (i - 1 > 0)
            {
                LevelButtons[i - 1].GetComponent<Image>().sprite = LevelSprites[zone].closed;
            }
            while (i < 3)
            {

                LevelButtons[i].interactable = false;
                LevelButtons[i].GetComponent<Image>().sprite = LevelSprites[zone].closed;
                i++;
            }
        }
    }

}
[System.Serializable]
public class Badge
{
    [SerializeField]
    public Sprite open;
    [SerializeField]
    public Sprite closed;
}


