using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterQuestHolder : MonoBehaviour,IQuestAccepter
{
    private CharacterStatus _status;
    public List<AQuest> Quests { get; private set; } = new List<AQuest>();
    // Start is called before the first frame update
    private void Start()
    {
        if (TryGetComponent(out Character character)) _status = character.status;
    }
    public void OnAcceptQuest(AAsignableQuest quest)
    {
       // Debug.Log("1");
        quest.onFinished += _status.TWeakExp;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
