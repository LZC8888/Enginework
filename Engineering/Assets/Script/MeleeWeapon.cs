using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CapsuleCaster))]
public class MeleeWeapon : MonoBehaviour
{
    public const int MaxHitcount = 10;

     public HitTag.Type hitTags;
    [Header("Damage")]
    public int damage;

    private RaycastHit[] _hitResults = new RaycastHit[MaxHitcount];
    public List<Transform> _hitCache = new List<Transform>();
    private CapsuleCaster _capsuleCaster;
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
            if(target.TryGetComponent(out Character character))
         //     if(target.TryGetComponent(out IHittable hittable))
            {
              //  hittable.OnHit(damage);
                character.OnHit(damage);
            }
            _hitCache.Add(target);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
