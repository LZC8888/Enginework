using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[AddComponentMenu("")]
[DisallowMultipleComponent]
[ExecuteAlways]
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }
    public Character player;

    private void Awake()
    {
        Assert.IsNull(Instance);
        Instance = this;
    }
}
