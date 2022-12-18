using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BanditBehaviour : MonoBehaviour
{
    public PlayerScanner playerScanner;
    public float timeToStopPursuit = 2.0f;//停止追踪时间
    public float timeToWaitOnPursuit = 2.0f;//追踪等待时间
    public float attackDistance = 1.1f;//攻击距离

    private playerControl m_Target;
    private EnemyController m_EnemyController;
    private Animator m_Animator;
    private float m_TimeSinceLostTarget = 0;//丢失目标时间
    private Vector3 m_OriginPosition;//初始点位置坐标
    private Quaternion m_OriginRotation;//初始点旋转坐标

    private readonly int m_HashInPursuit = Animator.StringToHash("InPursuit");
    private readonly int m_HashNearBase = Animator.StringToHash("NearBase");
    private readonly int m_HashAttack = Animator.StringToHash("Attack");
   

    private void Awake()
    {
        m_EnemyController = GetComponent<EnemyController>();
        m_Animator = GetComponent<Animator>();
        m_OriginPosition = transform.position;//获取初始状态的位置坐标
      //  Debug.Log(m_OriginPosition);
        m_OriginRotation = transform.rotation;//获取初始状态的旋转坐标
      //  Debug.Log(m_OriginRotation);
        
    }
    // Update is called once per frame
    void Update()
    {
        var target = playerScanner.Detect(transform);
        if (m_Target == null)//第一次初始化
        {
            m_Target = target != null ? target : null;
        }
        else
        {
            Vector3 toTarget = m_Target.transform.position - transform.position;
            if(toTarget.magnitude<=attackDistance)//距离小于攻击距离就开始攻击
            {
                m_EnemyController.StopFollowTarget(); 
               Debug.Log("I will attack you");
                m_Animator.SetTrigger(m_HashAttack);
            }
            else//继续追踪
            {
                m_Animator.SetBool(m_HashInPursuit, true);
                m_EnemyController.FollowTarget(m_Target.transform.position);
            }
            if (target==null)//找不到目标
            {
                m_TimeSinceLostTarget += Time.deltaTime;//记录丢失目标时长
                if(m_TimeSinceLostTarget>=timeToStopPursuit)//失去目标时间大于指定时长
                {
                    m_Target = null;
                    m_Animator.SetBool(m_HashInPursuit, false);
                    StartCoroutine(WaitOnPursuit());
                    Debug.Log("Stopping the enemy");
                }
            }
            else
            {
                Debug.Log("Continuing the enemy");
                m_TimeSinceLostTarget = 0;
            }
        }
        Vector3 toBase = m_OriginPosition- transform.position;
        toBase.y = 0;
        bool bNearBase = toBase.magnitude < 1f;//基地附近
        m_Animator.SetBool(m_HashNearBase, bNearBase);
        if(bNearBase)
        {
            Quaternion targetRotation = Quaternion.RotateTowards
                (transform.rotation, m_OriginRotation, 360 * Time.deltaTime);
            transform.rotation = targetRotation;

            Debug.Log("I will arrive");
        }
        
    }
    private IEnumerator WaitOnPursuit()
    {
        yield return new WaitForSeconds(timeToWaitOnPursuit);
        Debug.Log("I will return");
        m_EnemyController.FollowTarget(m_OriginPosition);

    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Color c = new Color(0.8f, 0, 0, 0.4f);
        UnityEditor.Handles.color = c;
        Vector3 rotatedForward = Quaternion.Euler(0, -playerScanner.detectionAngle * 0.5f, 0) * transform.forward;
        Vector3 rotatedNearbyForward = Quaternion.Euler(0, -playerScanner.detectionNearbyAngle * 0.5f, 0) * transform.forward;
        UnityEditor.Handles.DrawSolidArc
            (transform.position, Vector3.up, rotatedForward, playerScanner.detectionAngle, playerScanner.detectionRadius);
        UnityEditor.Handles.DrawSolidArc
            (transform.position, Vector3.up, rotatedNearbyForward, playerScanner.detectionNearbyAngle, playerScanner.detectionNearbyRadius);
    }
#endif
}
