using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopMaster : MonoBehaviour
{
    public Transform itemButtonParent;
    private Button[] itemButtons=new Button[16];

    public Image itemImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI price;
    public TextMeshProUGUI cashMoney;
    public Button buy;
    private int currentlyWatcheditem;

    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i] = itemButtonParent.GetChild(i).GetComponent<Button>();
        }
        PrepareShop();
    }

    public void PrepareShop()
    {
        int CHANGETHISLATER = 2;
        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (GameMaster.gm.GetTotalStartProgress() > CHANGETHISLATER)
            {
                itemButtons[i].gameObject.SetActive(true);
            }
            else
            {
                itemButtons[i].gameObject.SetActive(false);
            }
            CHANGETHISLATER += 2;
        }
        cashMoney.text = "x"+GameMaster.gm.playerStats.gold;

    }

    public void GetItemDesc(int item)
    {
        nameText.text = GameMaster.gm.namesAndDesc[item - 1].name;
        descText.text = GameMaster.gm.namesAndDesc[item - 1].desc;
        price.text = "x"+GameMaster.gm.namesAndDesc[item - 1].price;
        itemImage.sprite = GameMaster.gm.namesAndDesc[item - 1].normal;
        currentlyWatcheditem = item;
        if (GameMaster.gm.playerStats.availableItems.Contains((PlayerItems)item))
        {
            buy.gameObject.SetActive(false);
        }
        else
        {
            buy.gameObject.SetActive(true);
        }
    }

    public void BuyItem()
    {
        
        string buffer = price.text.Substring(1, price.text.Length - 1);
        if (GameMaster.gm.playerStats.gold >= int.Parse(buffer))
        {
            GameMaster.gm.playerStats.gold -= int.Parse(buffer);
            for (int i = 0; i < GameMaster.gm.playerStats.availableItems.Count;i++)
            {
                if (GameMaster.gm.playerStats.availableItems[i] == PlayerItems.NOITEM)
                {
                    if (currentlyWatcheditem<3){
                        GameMaster.gm.playerStats.availableItems[i] = ((PlayerItems)currentlyWatcheditem);
                    }
                    break;
                }
            }
            cashMoney.text ="x"+GameMaster.gm.playerStats.gold;
            AudioManager.instance.PlaySound("coins");
            GameMaster.gm.Save();
        }

        
    }
}
