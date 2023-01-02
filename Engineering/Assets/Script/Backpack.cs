using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Backpack : MonoBehaviour
{
    public List<IPackedItem> Items = new List<IPackedItem>();
    public Action onItemsChanged;
}
public interface IPackedItem
{
    string name { get; }
    Transform transform { get; }
    GameObject gameObject { get; }

    void OnUseFromBackPack(Backpack backpack, Character target);
}
public static class BackpackExtension
{

    public static void PutIn(this Backpack backpack,IPackedItem item)
    {
        item.gameObject.SetActive(false);
        backpack.Items.Add(item);
        item.transform.SetParent(backpack.transform);
        backpack.onItemsChanged?.Invoke();
    }
    public static bool TakeOut(this Backpack backpack,IPackedItem item)
    {
        bool removed = backpack.Items.Remove(item);
        item.gameObject.SetActive(true);
        backpack.onItemsChanged?.Invoke();
        return removed;
    }
}
