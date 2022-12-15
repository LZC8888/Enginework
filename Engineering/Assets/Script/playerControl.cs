using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class playerControl : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed=10.0f;//定义移动速度
    public float Rotatespeed=6.28f;//旋转速度
    public float gravity = 20;//重力
    const float k_Acceleration = 20.0f;//加速度
    const float k_Deceleration = 35.0f;//减速度
    public float jumpforce = 50f; //弹力
    public float y_falling = -2.0f;
    public float y_return = -100.0f;

    private PlayerInput m_playerInput;
    private CharacterController m_chController;
    private Camera m_MainCamera;
 //   private CameraControl m_CameraController;
    private Animator m_Animator;
 
    private float m_DesiredForwardSpeed;
    private float m_ForwardSpeed=0.0f;
 //   private Quaternion m_TargetRotation;
    private Vector3 moveDirection;
    private bool jump = false;
    private bool falling = false;
    private bool landing = false;

    private readonly int m_HashForwardSpeed = Animator.StringToHash("speed");


    private void FixedUpdate()
    {
        ComputeMovement();
     //   ComputeRotation();
      /*  if (m_playerInput.IsMoveInput)
        {
            transform.rotation = m_TargetRotation;
        }*/
    }
    
    private void Awake()
    {
       
        m_chController = GetComponent<CharacterController>();
        m_MainCamera = Camera.main;
      //  m_CameraController = GetComponent<CameraControl>();
        m_playerInput = GetComponent<PlayerInput>();
        m_Animator = GetComponent<Animator>();
    }
    void ComputeMovement()
    {
        Vector3 moveInput = m_playerInput.MoveInput;
       // Vector3 moveInput = m_playerInput.MoveInput.normalized;
        m_DesiredForwardSpeed = moveInput.magnitude * speed;
        float acceleration = m_playerInput.IsMoveInput ? k_Acceleration : k_Deceleration;
        m_ForwardSpeed = 
            Mathf.MoveTowards(m_ForwardSpeed, m_DesiredForwardSpeed, Time.fixedDeltaTime*acceleration);
       Quaternion camRotation = m_MainCamera.transform.rotation;
        Vector3 targetDirection = camRotation * moveInput;
        targetDirection.Normalize();
        targetDirection.y = 0;
        m_chController.Move(targetDirection * speed * Time.fixedDeltaTime);
       //   m_chController.transform.rotation = Quaternion.Euler(0, camRotation.eulerAngles.y, 0);
        Quaternion dir = Quaternion.LookRotation(moveInput);
        this.transform.rotation =
            Quaternion.Lerp(this.transform.rotation, dir, Time.deltaTime * Rotatespeed);
        m_Animator.SetFloat(m_HashForwardSpeed, moveInput.magnitude);
        
        //跳跃检测
        if (jump)
        {
            moveDirection = m_playerInput.MoveInput;
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            moveDirection.y = jumpforce;
            m_Animator.SetTrigger("Jump");
            Debug.Log("I can Jump");
            jump = false;
            Debug.Log("I finish Jump");
        }
        moveDirection.y -= gravity * Time.deltaTime;
        m_chController.Move(moveDirection * Time.deltaTime);
        //跌落检测
        if (falling)
        {
            m_Animator.SetTrigger("Falling");
            Debug.Log("I  Falling now");
            falling = false;
            landing = true;
        }
        //重生检测
        if(landing&&transform.position.y<y_return)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
          //  m_Animator.SetTrigger("Land"); 
            Debug.Log("I am resurgence");
            landing = false;
        }
}
 /*   void ComputeRotation()
    {
        Vector3 moveInput = m_playerInput.MoveInput.normalized;
        Vector3 cameraDirection = Quaternion.Euler(0,
            m_CameraController.freeLookCamera.m_XAxis.value,0)*Vector3.forward;
        Quaternion movementRotation = Quaternion.FromToRotation(Vector3.forward, moveInput);
        Quaternion targetRotation = Quaternion.LookRotation(movementRotation * cameraDirection);
        m_TargetRotation = targetRotation;
    }*/
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.5f);
        
    }
    bool IsFalling()
    {
        if (transform.position.y < y_falling)
            return true;
        return false;
    }
  /*  bool IsReturn()
    {
        if (transform.position.y < y_return)
            return true;
        return false;
    }*/
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump")&&IsGrounded())
        {
            jump = true;
        }
        if(IsFalling())
        {
           
            falling = true;
            Invoke("ComputeMovement", 3);
        }
       /* if(IsReturn())
        {
            landing = true;
        }*/
    }
    
}
