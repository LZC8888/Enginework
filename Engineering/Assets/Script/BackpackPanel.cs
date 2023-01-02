using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BackpackPanel : AWindow
{
    [NonSerialized] public Backpack Backpack;

    private PackedItemButton[] _buttons;
    // Start is called before the first frame update
    private void Start()
    {
        _buttons = GetComponentsInChildren<PackedItemButton>(true);
    }
    public override void Activate(bool active)
    {
        base.Activate(active);
        if (active) Backpack.onItemsChanged += UpdateItemButtons;
        else
        {
            if (Backpack != null) Backpack.onItemsChanged -= UpdateItemButtons;
            Backpack = null;
        }
        if (active) UpdateItemButtons();
    }
    
    public void UpdateItemButtons()
    {
        foreach(var button in _buttons)
        {
            button.Activate(false);
        }
        if (Backpack == null) return;

        int count = Backpack.Items.Count;

        for(int i=0;i<count;i++)
        {
            var button = _buttons[i];
            button.Activate(true, Backpack, Backpack.Items[i]);
        }
    }
}
