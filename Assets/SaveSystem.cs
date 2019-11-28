using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

public static class SaveSystem
{

    public static void Save(PlayerStats playerStats)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.sfp";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, playerStats);
        stream.Close();

    }

    public static PlayerStats Load()
    {
        string path = Application.persistentDataPath + "/save.sfp";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerStats data = formatter.Deserialize(stream) as PlayerStats;
            stream.Close();
            //loading to PlayerPrefs
            /*
            PlayerPrefs.SetInt("hp", data.hp);
            PlayerPrefs.SetInt("lvl", data.lastFinishedLevel);
            PlayerPrefs.SetInt("gold", data.gold);
            try
            {
                PlayerPrefs.SetInt("item1", (int)data.equipedItems[0]);
                PlayerPrefs.SetInt("item2", (int)data.equipedItems[1]);
                PlayerPrefs.SetInt("item3", (int)data.equipedItems[2]);
            }catch(IndexOutOfRangeException e)
            {
               // Debug.Log("NOITEMSAVAILABLE"+ e);
            }
            string buffer = "";
            foreach (PlayerItems item in data.availableItems) {
                buffer += (int)item +"/";
             }
            PlayerPrefs.SetString("availableItems", buffer);
            */
            return data;

        }
        else
        {
            Debug.LogError("save file not found");
            return new PlayerStats();
        }
    }

}






