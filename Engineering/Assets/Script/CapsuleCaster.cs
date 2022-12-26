using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleCaster : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float checkRadius;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public int Cast(RaycastHit[] results)
    {
        var startPosition = startPoint.position;
        var endPosition = endPoint.position;
        Vector3 direction = endPosition - startPosition;
        Ray ray = new Ray(startPosition, direction);
        float distance = Vector3.Magnitude(direction);
        return Physics.SphereCastNonAlloc(ray, checkRadius, results, distance);
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(startPoint.position, checkRadius);
        Gizmos.DrawWireSphere(endPoint.position, checkRadius);
        Gizmos.DrawLine(startPoint.position, endPoint.position);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
