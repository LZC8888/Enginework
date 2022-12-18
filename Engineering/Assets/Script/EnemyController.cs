using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent m_NavMeshAgent;
    private Animator m_Animator;
    private float m_SpeedModifier = 2f;
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void OnAnimatorMove()
    {
        // if (m_NavMeshAgent == null)
        if (m_NavMeshAgent.enabled)
        {

            //return;
            //}
            m_NavMeshAgent.speed =
                (m_Animator.deltaPosition / Time.fixedDeltaTime).magnitude * m_SpeedModifier;
        }
    }
    public bool FollowTarget(Vector3 position)
    {
       if(!m_NavMeshAgent.enabled)
        {
            m_NavMeshAgent.enabled = true;
        }
        return m_NavMeshAgent.SetDestination(position);
    }
    public void StopFollowTarget()
    {
        m_NavMeshAgent.enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
