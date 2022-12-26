using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Transform backpack;
    [SerializeField] private int health;
    [SerializeField] private Transform grabPoint;
    [SerializeField] private MeleeWeapon startWeapon;
    private Animator _animator;
    public int Health => health;
    public Animator Animator => _animator;
    public MeleeWeapon Weapon { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out _animator);
        Grab(startWeapon);
    }
    public void TweakHealth(int delta)
    {
        health += delta;
        Debug.Log(name + "'s health got changed: " + delta + ", and remain :" + health);
     //   OnMeleeAttackEnd();
    }
    public void Grab(MeleeWeapon weapon)
    {
        if (Weapon != null)
        {
            Weapon.gameObject.SetActive(false);
            Weapon.transform.parent = backpack;
        }
        Weapon = weapon;
        if (weapon != null)
        {
            weapon.transform.parent = grabPoint;
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
        } 
    }
    
    public void OnMeleeAttackStart()
    {
        Weapon.Attacking = true;
    }
    public void OnMeleeAttackEnd()
    {
        Weapon.Attacking = false;
        Weapon._hitCache.Clear();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
