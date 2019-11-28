using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoard : MonoBehaviour
{
    public Button item1;
    public Button item2;
    public Button item3;

    public Sprite[] itemSprites = null;
    // Start is called before the first frame update
   

    public void Prepare()
    {
        item1.GetComponent<Image>().sprite = itemSprites[(int)GameMaster.gm.playerStats.equipedItems[0]];
        item2.GetComponent<Image>().sprite = itemSprites[(int)GameMaster.gm.playerStats.equipedItems[1]];
        item3.GetComponent<Image>().sprite = itemSprites[(int)GameMaster.gm.playerStats.equipedItems[2]];
    }
}
