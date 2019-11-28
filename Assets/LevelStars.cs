using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelStars : MonoBehaviour
{
    
    public GameObject level1;
    public GameObject level2;
    public GameObject level3;

    public TextMeshProUGUI[] zoneStarTexts = null;
    

    public Sprite acquired;
    public Sprite closed;

    // Start is called before the first frame update
    void Start()
    {
        
        
        SetUpStars();
        //GameMaster.gm.playerStats;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpStars()
    {
        int k = 0;
        int buff = 0;
        for(int i=0;i < GameMaster.gm.playerStats.starProgress.Length; i++)
        {

            
            if ( ((i+1) % 3) == 0 )
            {
                buff += GameMaster.gm.playerStats.starProgress[i].Value;
                zoneStarTexts[k].text = "x " + buff;
                buff = 0;
                k++;
            }
            else
            {
                //Debug.Log("Adding level " + i + "star value: " + GameMaster.gm.playerStats.starProgress[i].Value);
                //Debug.Log("buff before: " + buff);
                buff += GameMaster.gm.playerStats.starProgress[i].Value;
                //Debug.Log("buff now: " + buff);
            }
            

        }
    }

    public void SetUpStars(int zone)
    {
        
            int i;
            for (i = 0; i < GameMaster.gm.playerStats.starProgress[zone*3].Value; i++)
                
            {
                
                level1.transform.GetChild(i).GetComponent<Image>().sprite = acquired;
            }

            while (i < 3)
            {
                level1.transform.GetChild(i).GetComponent<Image>().sprite = closed;
                i++;
            }


            for (i = 0; i < GameMaster.gm.playerStats.starProgress[(zone * 3)+1].Value; i++)
            {
                level2.transform.GetChild(i).GetComponent<Image>().sprite = acquired;
            }

            while (i < 3)
            {
                level2.transform.GetChild(i).GetComponent<Image>().sprite = closed;
                i++;
            }

            for (i = 0; i < GameMaster.gm.playerStats.starProgress[(zone * 3) + 2].Value; i++)
            {
                level3.transform.GetChild(i).GetComponent<Image>().sprite = acquired;
            }

            while (i < 3)
            {
                level3.transform.GetChild(i).GetComponent<Image>().sprite = closed;
                i++;
            }
        }

    



}
