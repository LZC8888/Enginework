using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStatus))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
public class Character : MonoBehaviour,IHittable,IWeaponBonusing
{
    public const string HurtState = "Hurt";
    public const string AttackState = "Attack";
    public const string DeadState = "Dead";
    public const int InteractBufferLength = 10;

    public string ID;
    
    [SerializeField] 
    private string _uid = Guid.NewGuid().ToString();
    
    [Header("Attacking enourence")]
    public bool endure;

    [Header("Ragdoll")]
    public bool ragdollOnDead;

    [Header("Status")]
    public CharacterStatus Status;

    [Header("References")]
  //  [SerializeField] private int health;
    public Transform grabPoint;
    public CapsuleCaster interactCaster;
    public Backpack backpack; 
    
    [SerializeField] private MeleeWeapon startWeapon;
    [SerializeField] private CollidersHandler ragdollcolliders;

  //public CharacterStatus Status;
    [NonSerialized]  private Animator _animator;
    [NonSerialized]  private Collider _collider;
    [NonSerialized]  private CharacterQuestHolder _questHolder;

    private RaycastHit[] _interactBuffer = new RaycastHit[InteractBufferLength];
    public bool DeadLastCheck { get; private set; } = false;
    public bool RagdollOnDead => ragdollcolliders != null;
    public int DamageBonus => Status.Damage;
    public string Uid => _uid;
    public int Health => Status.Health;
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
     //   CharacterManager.Instance.Register(this);
        TryGetComponent(out Status);
        TryGetComponent(out _animator);
        TryGetComponent(out _collider);
        TryGetComponent(out _questHolder);
      //  CheckDead();
        Grab(startWeapon);

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && gameObject.CompareTag("Player"))
        {
            var backpackPanel = UIManager.Instance.backpackPanel;
            backpackPanel.Backpack = backpack;
         //   Debug.Log("111");
            if (backpackPanel.CanvasGroup.alpha == 0)
                backpackPanel.Activate(true);
            else
                backpackPanel.Activate(false);
        }
    }
    private void OnDestroy()
    {
       // CharacterManager.Instance.Unregister(this);
    }
   /* private void Update()
    {
        if (RpgAdventure.PlayerInput.Instance.IsInteract && this == CharacterManager.Instance.player)
        {
            Interact();
        }
        if(RpgAdventure.PlayerInput.Instance.ToggleBackpack && this == CharacterManager.Instance.player)
        {
            var panel = UIManager.Instance.backpackPanel;
            panel.Backpack = backpack;
            panel.Activate(!panel.CanvasGroup.interactable);
        }
    }*/
    public bool CheckDead()
    {
        bool dead = Status.Dead;
        if (DeadLastCheck != dead && dead)
        {
            Animator.StopPlayback();
            Animator.SetBool(DeadState, true);

            OnDead?.Invoke();
            MessageCenter.OnCharacterDead?.Invoke(this);

            if (RagdollOnDead)
            {
                Collider.enabled = false;
                Animator.enabled = false;
                ragdollcolliders.Enable(true);
            }
        }
        if (Dead)
        {
            if(TryGetComponent(out BanditBehaviour enemy))
            {
                enemy.Death();
            }
        }
        DeadLastCheck = dead;
        return dead;
    }
    /*  public void TweakHealth(int delta)
      {
          health += delta;
          Debug.Log(name + "'s health got changed: " + delta + ", and remain :" + health);
       //   OnMeleeAttackEnd();
      }*/
    public void OnHit(int damage)
    {
        if (DeadLastCheck) return;
        
        bool isAttacking = Animator.GetCurrentAnimatorStateInfo(0).IsName(AttackState);
        bool enduring = endure && isAttacking;
        
        if (!enduring) Animator.SetTrigger(HurtState);
        Status.Health -= damage;
        OnHurt?.Invoke();
        
        CheckDead();
        
    }
    public void Grab(MeleeWeapon weapon)
    {
        if (Weapon != null)
        {
            //           Weapon.gameObject.SetActive(false);
            //           Weapon.transform.parent = backpack;
            Weapon.ResetDamageBonus();
            backpack.PutIn(Weapon);
        }
      //  if (backpack.Items.Contains(weapon)) backpack.TakeOut(weapon);
        Weapon = weapon;
        if (weapon != null)
        {
            weapon.transform.parent = grabPoint;
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
            weapon.SetDamageBonus(this);
        } 
    }
    public void Interact()
    {
        int count = interactCaster.Cast(_interactBuffer);
        if (count <= 0) return;
        IInteractable interactable;
        for(int i = 0; i < count; i++)
        {
            if(_interactBuffer[i].transform.TryGetComponent(out interactable))
            {
                interactable.OnInteract(this);
                return;
            }
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
//[Serializable]
/*public class CharacterStatus
{
    
    [SerializeField] private int health;
    [SerializeField] private int exp;

    public int Health => health;
    public int Exp => exp;
    public void TWeakHealth(int delta) => health = Mathf.Max(health + delta, 0);
    public void TWeakExp(int delta) => exp = Mathf.Max(exp + delta, 0);
}*/
public static class CharacterExtension
{
    public static bool TryGetCharacterInScene(this string characterID ,out Character target)
    {
        target = null;
        foreach(var iter in GameObject.FindObjectsOfType<Character>())
        {
            if (iter.ID == characterID)
            {
                target = iter;
                break;
            }
        }
        return target != null;
    }
}
