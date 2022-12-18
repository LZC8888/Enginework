using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class playerControl : MonoBehaviour
{
   public static playerControl Instance
    {
        get
        {
            return s_Instance;
        }
    }
    // Start is called before the first frame update
    public float speed=10.0f;//定义移动速度
    public float Rotatespeed=6.28f;//旋转速度
    public float gravity = 20;//重力
    const float k_Acceleration = 20.0f;//加速度
    const float k_Deceleration = 35.0f;//减速度
    public float jumpforce = 50f; //弹力
    public float y_falling = -2.0f;//是否跌落的值
    public float y_return = -100.0f;//是否重开的值
    public float m_MaxRotationSpeed = 1200;
    public float m_MinRotationSpeed = 800;

    private static playerControl s_Instance;
    private PlayerInput m_playerInput;
    private CharacterController m_chController;
    private CameraControl m_CameraController;
    private Animator m_Animator;
 
    private float m_DesiredForwardSpeed;
    private float m_ForwardSpeed=0.0f;
    private float m_VerticalSpeed = 0;
    private Quaternion m_TargetRotation;
    private Vector3 moveDirection;
    private bool jump = false;
    private bool falling = false;
    private bool landing = false;

    private readonly int m_HashForwardSpeed = Animator.StringToHash("speed");
    private readonly int m_HashMeleeAttack = Animator.StringToHash("MeleeAttack");

    private void FixedUpdate()
    {
        ComputeMovement();
     //   ComputeVerticalMovement();
        ComputeRotation();
        if (m_playerInput.IsMoveInput)
        {
            float rotationSpeed = 
                Mathf.Lerp(m_MaxRotationSpeed, m_MinRotationSpeed, m_ForwardSpeed / m_DesiredForwardSpeed);
            m_TargetRotation = 
                Quaternion.RotateTowards(transform.rotation, m_TargetRotation, rotationSpeed * Time.fixedDeltaTime);
            transform.rotation = m_TargetRotation;
        }
        m_Animator.ResetTrigger(m_HashMeleeAttack);
        if(m_playerInput.bIsAttack)
        {
            m_Animator.SetTrigger(m_HashMeleeAttack);
        }
    }
    
    private void Awake()
    {
       
        m_chController = GetComponent<CharacterController>();
       // m_MainCamera = Camera.main;
        m_CameraController = GetComponent<CameraControl>();
        m_playerInput = GetComponent<PlayerInput>();
        m_Animator = GetComponent<Animator>();
        s_Instance = this;
    }
    private void OnAnimatorMove()
    {
        //m_chController.Move(m_Animator.deltaPosition);
        Vector3 movement = m_Animator.deltaPosition;
        movement += m_VerticalSpeed * Vector3.up * Time.fixedDeltaTime;
        m_chController.Move(movement);
    }
  /*  void ComputeVerticalMovement()
    {
        m_VerticalSpeed = -gravity;
    }*/
    void ComputeMovement()
    {
      //  Vector3 moveInput = m_playerInput.MoveInput;
        Vector3 moveInput = m_playerInput.MoveInput.normalized;
        m_DesiredForwardSpeed = moveInput.magnitude * speed;
        float acceleration = m_playerInput.IsMoveInput ? k_Acceleration : k_Deceleration;
        m_ForwardSpeed = 
            Mathf.MoveTowards(m_ForwardSpeed, m_DesiredForwardSpeed, Time.fixedDeltaTime*acceleration);

        m_Animator.SetFloat(m_HashForwardSpeed, m_ForwardSpeed);
        
        //跳跃检测
        if (jump)
        {
            moveDirection = transform.TransformDirection(moveInput);
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
    void ComputeRotation()
    {
        Vector3 moveInput = m_playerInput.MoveInput.normalized;
        Vector3 cameraDirection = Quaternion.Euler(0,
            m_CameraController.freeLookcamera.m_XAxis.Value,0)*Vector3.forward;
        Quaternion targetRotation;
        if(Mathf.Approximately(Vector3.Dot(moveInput,Vector3.forward),-1.0f))
        {
            targetRotation = Quaternion.LookRotation(-cameraDirection);
        }
        else
        {
            Quaternion movementRotation = Quaternion.FromToRotation(Vector3.forward, moveInput);
            targetRotation = Quaternion.LookRotation(movementRotation * cameraDirection);
        }
        //Quaternion movementRotation = Quaternion.FromToRotation(Vector3.forward, moveInput);
        //Quaternion targetRotation = Quaternion.LookRotation(movementRotation * cameraDirection);
        m_TargetRotation = targetRotation;
    }
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
  
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump")&&IsGrounded())
        {
            jump = true;
        }
        if (IsFalling())
        {
           
            falling = true;
            Invoke("ComputeMovement", 3);
        }
    }
    
}
