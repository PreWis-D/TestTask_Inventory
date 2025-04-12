using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerSaver
{
    private const string _saveInventory = "SaveInventory";

    public void SaveInventory(List<InventoryItem> inventoryItems)
    {
        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/" + _saveInventory);
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            string json = JsonUtility.ToJson(inventoryItems[i]);
            sw.WriteLine(json);
        }
        sw.Close();
    }

    public List<InventoryItem> LoadInventory()
    {
        List<InventoryItem> inventoryItems = new List<InventoryItem>();

        if (File.Exists(Application.persistentDataPath + "/" + _saveInventory))
        {
            string[] readed = File.ReadAllLines(Application.persistentDataPath + "/" + _saveInventory);
            for (int i = 0; i < readed.Length; i++)
            {
                InventoryItem item = JsonUtility.FromJson<InventoryItem>(readed[i]);
                inventoryItems.Add(item);
            }
        }
        
        return inventoryItems;
    }
}