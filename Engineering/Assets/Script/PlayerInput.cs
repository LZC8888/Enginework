using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Vector3 m_Movement;
    private bool m_bIsAttack;
    private Character playerCharacter;
    private void Awake()
    {
        playerCharacter = GetComponent<Character>();
    }
    public Vector3 MoveInput
    {
        get
        {
            return m_Movement;
        }
    }
    public bool IsMoveInput
    {
        get
        {
            return !Mathf.Approximately(MoveInput.magnitude, 0);
        }
    }
    public bool bIsAttack
    {
        get
        {
            return m_bIsAttack;
        }
    }


    // Update is called once per frame
    void Update()
    {
        m_Movement.Set(
            Input.GetAxis("Horizontal"),
            0,
                Input.GetAxis("Vertical")
        );
        if (Input.GetButtonDown("Fire1") && !m_bIsAttack)
        {
            Debug.Log("Fire1 is readly");
            StartCoroutine(AttackAndWait());
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            playerCharacter.Interact();
        }
    }
    private IEnumerator AttackAndWait()
    {
        m_bIsAttack = true;
        yield return new WaitForSeconds(0.03f);
        m_bIsAttack = false;
    }
}
