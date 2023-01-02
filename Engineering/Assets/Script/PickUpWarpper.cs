using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWarpper : MonoBehaviour,IInteractable
{
    [NonSerialized] public IPackedItem Item;
    private void Start()
    {
        Item = GetComponentInChildren<IPackedItem>();
    }
    public void OnInteract(Character initiative)
    {
        initiative.backpack.PutIn(Item);
        Destroy(gameObject);
    }
}
