using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    
   public CinemachineFreeLook freeLookcamera;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            freeLookcamera.m_XAxis.m_MaxSpeed = 400;
            freeLookcamera.m_YAxis.m_MaxSpeed = 10;
        }
        if(Input.GetMouseButtonUp(1))
        {
            freeLookcamera.m_XAxis.m_MaxSpeed = 0;
            freeLookcamera.m_YAxis.m_MaxSpeed = 0;
        }
    }
}
