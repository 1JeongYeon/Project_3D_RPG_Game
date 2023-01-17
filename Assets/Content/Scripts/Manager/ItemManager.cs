using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    void Awake()
    {
        Object[] objects;
        objects = Resources.LoadAll("Prefabs/Item/Item_Scriptable/");

        for (int i = 0; i < objects.Length; i++)
        {
            items.Add(objects[i] as Item);
        }
    }
}
