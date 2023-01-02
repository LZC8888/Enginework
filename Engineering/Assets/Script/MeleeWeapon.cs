using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponBonusing
{
    int DamageBonus { get; }
}
[RequireComponent(typeof(CapsuleCaster))]
public class MeleeWeapon : MonoBehaviour,IPackedItem
{
    public const int MaxHitcount = 10;

     public HitTag.Type hitTags;
    [Header("Damage")]
    public int damage;

    private RaycastHit[] _hitResults = new RaycastHit[MaxHitcount];
    public List<Transform> _hitCache = new List<Transform>();
    private CapsuleCaster _capsuleCaster;
    private IWeaponBonusing _bonus;
    private bool _attacking;
    

    public bool Attacking
    {
        get => _attacking;
        set
        {
          if (_attacking == value) return;
          if (!value)
            {
                _hitCache.Clear();
            }
            _attacking = value;
        }
    }
    public void SetDamageBonus(IWeaponBonusing bonus) => _bonus = bonus;
    public void ResetDamageBonus() => _bonus = null;
    public void Awake()
    {
          TryGetComponent(out _capsuleCaster);
     //   _capsuleCaster = GetComponent<CapsuleCaster>();
    }
    public void FixedUpdate()
    {
      
        if (!Attacking) {  return; }
    
        int hitCount = _capsuleCaster.Cast(_hitResults);
        
        if (hitCount <= 0) {  return; }
   
        for(int i = 0; i < hitCount; i++)
        {
            Transform target = _hitResults[i].transform;
            if(target.TryGetComponent(out HitTag tag))
            {
                if ((hitTags & tag.type) == 0) continue;
            }
            if (_hitCache.Contains(target)) continue;
       //     if(target.TryGetComponent(out Character character))
              if(target.TryGetComponent(out IHittable hittable))
            {
                  hittable.OnHit(damage);
                int bonus = 0;
                if (_bonus != null) bonus = _bonus.DamageBonus;
                hittable.OnHit(damage+bonus);
            }
            _hitCache.Add(target);
        }
    }
   void IPackedItem.OnUseFromBackPack(Backpack backpack,Character target)
    {
        backpack.TakeOut(this);
        target.Grab(this);
    }
}
