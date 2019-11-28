using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("here");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class PlayerStats
{
    public int hp;
    public int lastFinishedLevel = 0;
    public int gold;
    public PlayerItems[] equipedItems = new PlayerItems[3];
    public List<PlayerItems> availableItems = new List<PlayerItems>();
    public KeyValuePair<int, int>[] starProgress = new KeyValuePair<int, int>[12];

    public PlayerStats()
    {
        this.hp = 5;
        this.lastFinishedLevel = 0;
        this.gold = 0;
        PlayerItems[] equip = { PlayerItems.NOITEM, PlayerItems.NOITEM, PlayerItems.NOITEM };
        this.equipedItems = equip;
        this.availableItems = new List<PlayerItems>();
        for(int i = 0; i < 16; i++)
        {
            this.availableItems.Add(PlayerItems.NOITEM);
        }
        for(int i = 0; i < starProgress.Length; i++)
        {
            this.starProgress[i] = new KeyValuePair<int, int>(i, 0);
        }
        
    }

    public PlayerStats(int hp, int lastLvl, int gold, PlayerItems[] equipedItems, List<PlayerItems> availableItems, KeyValuePair<int, int>[] starProgress)
    {
        this.hp = hp;
        this.lastFinishedLevel = lastLvl;
        this.gold = gold;
        this.equipedItems = equipedItems;
        this.availableItems = availableItems;
        this.starProgress = starProgress;
    }
}
