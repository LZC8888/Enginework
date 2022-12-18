using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerScanner 
{
    public float detectionRadius = 5.0f;//��ǰ�����뾶
    public float detectionAngle = 90.0f;//��ǰ�����Ƕ�
    public float detectionNearbyRadius = 1.0f;//������뾶
    public float detectionNearbyAngle = 360.0f;//������Ƕ�

    public playerControl Detect(Transform detector)
    {
        if(playerControl.Instance==null)
        {
            return null;
        }
        Vector3 toPlayer = playerControl.Instance.transform.position - detector.position;
        toPlayer.y = 0;
        if ((toPlayer.magnitude <= detectionRadius) || 
            (toPlayer.magnitude <= detectionNearbyRadius))//��⵽����Ŷ
        {
            if (Vector3.Dot(toPlayer.normalized, detector.forward) >
                Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad) ||
                (Vector3.Dot(toPlayer.normalized, detector.forward) >
                Mathf.Cos(detectionNearbyAngle * 0.5f * Mathf.Deg2Rad)))
            {
                return playerControl.Instance;
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
