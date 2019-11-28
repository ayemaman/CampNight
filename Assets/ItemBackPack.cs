using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemBackPack : MonoBehaviour
{
    public Transform itemsParent;
    private Transform itemBoard;
    private Button[] itemButtons;
    public Sprite[] itemImages;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI descriptionText;


   
    private void Start()
    {
        itemBoard = itemsParent.GetChild(0);
        nameText = itemsParent.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        descriptionText = itemsParent.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        itemButtons = new Button[itemBoard.childCount];
        for(int i=0; i < itemBoard.childCount; i++)
        {
            itemButtons[i] = itemBoard.GetChild(i).GetComponent<Button>();
        }
    }

    public void Prepare()
    {
        //check what item is equipped in this slot
        int selectedItem= (int) GameMaster.gm.playerStats.equipedItems[GameMaster.gm.currentSlot];
        
        //if !NOITEM
        if (selectedItem > 0)
        {
            
            EventSystem.current.SetSelectedGameObject(itemButtons[GameMaster.gm.playerStats.availableItems.IndexOf((PlayerItems)selectedItem)].gameObject);

            nameText.text = GameMaster.gm.namesAndDesc[selectedItem - 1].name;
            descriptionText.text = GameMaster.gm.namesAndDesc[selectedItem - 1].desc;

            GameMaster.gm.PrepareEquipItem(selectedItem);
            
        }
        //if NOITEM
        else
        {
            if (GameMaster.gm.playerStats.availableItems[0] != PlayerItems.NOITEM)
            {
                EventSystem.current.SetSelectedGameObject(itemButtons[0].gameObject);
                GameMaster.gm.PrepareEquipItem((int)GameMaster.gm.playerStats.availableItems[0]);

            }
        }

        List<int> buffer = new List<int>();

        int pos = 0;
        foreach(PlayerItems item in GameMaster.gm.playerStats.availableItems){
            if (item != PlayerItems.NOITEM)
            {
                itemButtons[pos].interactable = true;
                // _thisButton.transition = Selectable.Transition.SpriteSwap;
                var ss = itemButtons[pos].spriteState;
                itemButtons[pos].image.sprite = GameMaster.gm.namesAndDesc[(int)item-1].normal;
                ss.highlightedSprite = GameMaster.gm.namesAndDesc[(int)item-1].selected;
                ss.selectedSprite = GameMaster.gm.namesAndDesc[(int)item - 1].selected; 
                ss.pressedSprite = GameMaster.gm.namesAndDesc[(int)item - 1].selected;
                itemButtons[pos].spriteState = ss;

                pos++;
            }
        }
        while (pos < 16)
        {
            itemButtons[pos].interactable = false;
            pos++;
        }

    }

    public void SetText(int item)
    {
        nameText.text = GameMaster.gm.namesAndDesc[item-1].name;
        descriptionText.text = GameMaster.gm.namesAndDesc[item-1].desc; 
    }
}

[System.Serializable]
public class NamesDescs
{
    public string name;
    [TextArea(3,10)]
    public string desc;
    public int price;
    public Sprite selected;
    public Sprite normal;
    public float cd;
}
