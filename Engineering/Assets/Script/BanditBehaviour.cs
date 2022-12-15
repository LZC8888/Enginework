using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BanditBehaviour : MonoBehaviour
{
    public float detectionRadius = 10.0f;
    public float detectionAngle = 90.0f;
    private playerControl m_Target;
    private NavMeshAgent m_NavMestAgent;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        m_NavMestAgent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
       m_Target = LookForPlayer();
        if (m_Target == null)
        {
            return;
        }
        Vector3 targetPosition = m_Target.transform.position;
        m_NavMestAgent.SetDestination(targetPosition);
    }
    private playerControl LookForPlayer()
    {
        if(playerControl.Instance==null)
        {
            return null;
        }
        Vector3 enemyPosition = transform.position;
        Vector3 toPlayer = playerControl.Instance.transform.position - enemyPosition;
        toPlayer.y = 0;

        if(toPlayer.magnitude<=detectionRadius)
        {
            if (Vector3.Dot(toPlayer.normalized, transform.forward) > Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
            {
                Debug.Log("Detecting the player");
                return playerControl.Instance;
            }
        }
        return null;
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Color c = new Color(0.8f, 0, 0, 0.4f);
        UnityEditor.Handles.color = c;
        Vector3 rotatedForward = Quaternion.Euler(0, -detectionAngle * 0.5f, 0) * transform.forward;
        UnityEditor.Handles.DrawSolidArc
            (transform.position, Vector3.up, rotatedForward, detectionAngle,detectionRadius);
    }
#endif
}
