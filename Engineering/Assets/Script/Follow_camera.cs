using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_camera : MonoBehaviour
{
    // Start is called before the first frame update
    public  Transform target;
    public float heightRelative = 5.0f;
    public float planeRelative = 10.0f;
    void Start()
    {
        
    }
    private void LateUpdate()
    {
        if (!target)
            return;

        float currentRotationAngle = transform.eulerAngles.y;
        float wantedRotationAngle = target.eulerAngles.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, 0.5f);
        transform.position = new Vector3(target.position.x, heightRelative, target.position.z);
        Quaternion currentRotation = Quaternion.Euler(0,currentRotationAngle,0);
        Vector3 rotatedPosition = currentRotation * Vector3.forward;
        transform.position -= rotatedPosition * planeRelative;
        transform.LookAt(target);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
