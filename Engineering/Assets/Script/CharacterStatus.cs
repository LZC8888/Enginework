using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    [Serializable]
    public struct Attribute
    {
        public int maxHealth;
        public int damage;
        public int levelExp;
    }
    [SerializeField] private int health;
    [SerializeField] private int damage;

    [Header("Experience")]
    [SerializeField] private int exp;
    [SerializeField] private int level;

    [Header("Levels Presets")]
    [SerializeField] private Attribute[] attributePresets;

    public int Health { get => health; set { health = value;ValidateHealth(); } }
    public int Exp { get => exp; set { exp = value;ValidateExp(); } }
 //   public void TweakExp(int delta) => Exp += delta;
//    public void TWeakHealth(int delta) => health = Mathf.Max(health + delta, 0);
    public int Damage => damage;
    public Attribute Attr => attributePresets[level];
    public bool Dead => health <= 0;

    public void ValidateHealth() => health = Mathf.Clamp(health, 0, Attr.maxHealth);
    public void ValidateExp()
    {
        int maxLevel = Mathf.Max(attributePresets.Length -1, 0);
        int maxExp = attributePresets[maxLevel].levelExp;
        exp = Mathf.Min(exp, maxExp);

        while (level < maxLevel && exp >= attributePresets[level].levelExp)
        {
            OnLevelUp();
        }
    }
    private void Validate()
    {
        level = 0;
        ValidateExp();
        ValidateHealth();
        damage = Attr.damage;
    }
    private void OnLevelUp()
    {
        int maxHealth = Attr.maxHealth;
        level++;
        health += Attr.maxHealth - maxHealth;
        damage = Attr.damage;
    }
    private void Awake()
    {
        Validate();
    }
    private void OnValidate()
    {
        Validate();
    }
    public void TweakExp(int delta)
    {
        exp = Mathf.Max(exp + delta, 0);
        Validate();
    }
}
