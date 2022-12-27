using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public const string HurtState = "Hurt";
    public const string AttackState = "Attack";
    public const string DeadState = "Dead";
    public Transform backpack;
    [SerializeField] 
    private string _uid = Guid.NewGuid().ToString();
    
    [Header("Attacking enourence")]
    public bool endure;

    [Header("Ragdoll")]
    public bool ragdollOnDead;

    [Header("Status")]
    public CharacterStatus status;

    [Header("References")]
  //  [SerializeField] private int health;
    [SerializeField] private Transform grabPoint;
    [SerializeField] private MeleeWeapon startWeapon;

    [SerializeField] private CollidersHandler ragdollcolliders;

    private Animator _animator;
    private Collider _collider;
    private CharacterQuestHolder _questHolder;
    
    public string Uid => _uid;
     public int Health => status.Health;
    public bool Dead => Health <= 0;
    public Animator Animator => _animator;
    public Collider Collider => _collider;
    public CharacterQuestHolder QuestHolder => _questHolder;

    public Action OnHurt;
    public Action OnDead;
    public MeleeWeapon Weapon { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out _animator);
        TryGetComponent(out _collider);
        TryGetComponent(out _questHolder);
        Grab(startWeapon);

    }
  /*  public void TweakHealth(int delta)
    {
        health += delta;
        Debug.Log(name + "'s health got changed: " + delta + ", and remain :" + health);
     //   OnMeleeAttackEnd();
    }*/
    public void OnHit(int damage)
    {
        if (Dead) return;
      //  Debug.Log(Dead);
        bool isAttacking = Animator.GetCurrentAnimatorStateInfo(0).IsName(AttackState);
       // Debug.Log(isAttacking);
        bool enduring = endure && isAttacking;

        if (!enduring) { Debug.Log("111"); Animator.SetTrigger(HurtState); }
        status.TWeakHealth(-damage);
        OnHurt?.Invoke();

        if (Dead)
        {
         //   Debug.Log("1");
            Animator.StopPlayback();
            Animator.SetBool(DeadState, true);
            
            OnDead?.Invoke();
            MessageCenter.OnCharacterDead?.Invoke(this);
         //   Animator.SetBool(DeadState, false);
            if (ragdollOnDead)
            {
            //    Debug.Log("Dead");
                Collider.enabled = false;
                Animator.enabled = false;
                ragdollcolliders.Enable(true);
            }
        }
        
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
#if UNITY_EDITOR
    [SerializeField] private string uidForEditor;
    private void OnValidate()
    {
        uidForEditor = Uid;
    }
#endif
}
[Serializable]
public class CharacterStatus
{
    [SerializeField] private int health;
    [SerializeField] private int exp;

    public int Health => health;
    public int Exp => exp;
    public void TWeakHealth(int delta) => health = Mathf.Max(health + delta, 0);
    public void TWeakExp(int delta) => exp = Mathf.Max(exp + delta, 0);
}
public static class CharacterExtension
{

}
