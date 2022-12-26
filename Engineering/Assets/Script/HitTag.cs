using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTag : MonoBehaviour
{
    [Flags]
    public enum Type
    {
        None = 0,
        Alliance = 1,
        Enemy = 2,
        Character = Alliance | Enemy,
        Item=4,
        Everything=~0

    }
    public Type type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
