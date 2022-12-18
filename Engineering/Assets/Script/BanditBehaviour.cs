using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BanditBehaviour : MonoBehaviour
{
    public PlayerScanner playerScanner;
    public float timeToStopPursuit = 2.0f;//ֹͣ׷��ʱ��
    public float timeToWaitOnPursuit = 2.0f;//׷�ٵȴ�ʱ��
    public float attackDistance = 1.1f;//��������

    private playerControl m_Target;
    private EnemyController m_EnemyController;
    private Animator m_Animator;
    private float m_TimeSinceLostTarget = 0;//��ʧĿ��ʱ��
    private Vector3 m_OriginPosition;//��ʼ��λ������
    private Quaternion m_OriginRotation;//��ʼ����ת����

    private readonly int m_HashInPursuit = Animator.StringToHash("InPursuit");
    private readonly int m_HashNearBase = Animator.StringToHash("NearBase");
    private readonly int m_HashAttack = Animator.StringToHash("Attack");
   

    private void Awake()
    {
        m_EnemyController = GetComponent<EnemyController>();
        m_Animator = GetComponent<Animator>();
        m_OriginPosition = transform.position;//��ȡ��ʼ״̬��λ������
      //  Debug.Log(m_OriginPosition);
        m_OriginRotation = transform.rotation;//��ȡ��ʼ״̬����ת����
      //  Debug.Log(m_OriginRotation);
        
    }
    // Update is called once per frame
    void Update()
    {
        var target = playerScanner.Detect(transform);
        if (m_Target == null)//��һ�γ�ʼ��
        {
            m_Target = target != null ? target : null;
        }
        else
        {
            Vector3 toTarget = m_Target.transform.position - transform.position;
            if(toTarget.magnitude<=attackDistance)//����С�ڹ�������Ϳ�ʼ����
            {
                m_EnemyController.StopFollowTarget(); 
               Debug.Log("I will attack you");
                m_Animator.SetTrigger(m_HashAttack);
            }
            else//����׷��
            {
                m_Animator.SetBool(m_HashInPursuit, true);
                m_EnemyController.FollowTarget(m_Target.transform.position);
            }
            if (target==null)//�Ҳ���Ŀ��
            {
                m_TimeSinceLostTarget += Time.deltaTime;//��¼��ʧĿ��ʱ��
                if(m_TimeSinceLostTarget>=timeToStopPursuit)//ʧȥĿ��ʱ�����ָ��ʱ��
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
        bool bNearBase = toBase.magnitude < 1f;//���ظ���
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
