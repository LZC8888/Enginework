using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[RequireComponent(typeof(Button))]
public class PackedItemButton : MonoBehaviour
{
    public TextMeshProUGUI title;

    [NonSerialized] public Backpack Backpack;
    [NonSerialized] public IPackedItem Item;
    private Button _button;
    Character player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Character>();
        TryGetComponent(out _button);
        transform.GetChild(0).TryGetComponent(out title);
    }
    public void Activate(bool active,Backpack backpack=null,IPackedItem item = null)
    {
        _button.interactable = active;
        if (active)
        {
            Backpack = backpack;
            Item = item;
            title.text = item.name;
            _button.onClick.AddListener(UseItem);
        }
        else
        {
            Backpack = null;
            Item = null;
            title.text = "";
            _button.onClick.RemoveListener(UseItem);
        }
    }
    private void UseItem()
    {
        Item.OnUseFromBackPack(Backpack, CharacterManager.Instance.player);
    }
}
