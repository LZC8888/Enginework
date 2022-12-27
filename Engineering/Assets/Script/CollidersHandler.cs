using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidersHandler : MonoBehaviour
{
    public bool enableOnstart;
    private Collider[] _colliders;
    // Start is called before the first frame update
    private  void Start()
    {
        _colliders = GetComponentsInChildren<Collider>();
        foreach(var collider in _colliders)
        {
            collider.attachedRigidbody.isKinematic = !enableOnstart;
            collider.enabled = enableOnstart;
        }
    }
    public void Enable(bool enabled)
    {
        foreach(var collider in _colliders)
        {
            collider.enabled = enabled;
            collider.attachedRigidbody.isKinematic = !enabled;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
