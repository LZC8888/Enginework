using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformConstrain : MonoBehaviour
{
    public Transform target;
    public void FixedUpdate()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
   /* public void OnDrawGizmos()
    {
        FixedUpdate();
    }*/
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
