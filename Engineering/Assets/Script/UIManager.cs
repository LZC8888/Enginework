using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[AddComponentMenu("")]
[DisallowMultipleComponent]
[ExecuteAlways]
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public QuestPanel questPanel;
    public BackpackPanel backpackPanel;

    private void Awake()
    {
     //   Assert.IsNull(Instance);
        Instance = this;
    }
}
[RequireComponent(typeof(CanvasGroup))]
public abstract class AWindow : MonoBehaviour
{
    public bool activeOnAwake;
    [NonSerialized] public CanvasGroup CanvasGroup;

    protected virtual void Awake()
    {
        gameObject.SetActive(true);
        TryGetComponent(out CanvasGroup);
        Assert.IsNotNull(CanvasGroup);

        Activate(activeOnAwake);
    }
    public virtual void Activate(bool active)
    {
        CanvasGroup.alpha=active? 1f : 0f;
    }
}
