using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
[DisallowMultipleComponent]
public class QuestHost : MonoBehaviour, IQuestHolder, IQuestHost, IInteractable
{
    public GameObject Player;
    public string[] titles;
    public List<AQuest> Quests { get; private set; } = new List<AQuest>();

    public Action OnQuestAssigned { get; set; }
    // Start is called before the first frame update
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").gameObject;
        foreach (string title in titles)
        {
            QuestInfo info = QuestManager.Instance.allQuests[title];
            AQuest quest;
            switch (info.QuestType)
            {
                case QuestInfo.Type.CharacterSlayingQuest:
                    quest = new CharacterSlayingQuest(info);
                    break;
                default:
                    return;
            }
            Assert.IsNotNull(quest);
            Quests.Add(quest);
        }
    }
    private void OnMouseDown()//拓展，距离近触发任务
    {
        /* Debug.Log(Quests[0].title + " accepted;");
         Character player = Player.GetComponent<Character>(); 
         this.Assign(Quests[0] as AAsignableQuest, player.QuestHolder);*/
         var questPanel = UIManager.Instance.questPanel;
         questPanel.QuestHost = this;
        // questPanel.Accepter = initiative.QuestHolder;
         questPanel.Activate(true);
    }
#if UNITY_EDITOR
    [Header("Editor Only")]
    private QuestManager _manager;
    public List<QuestInfo> questInfoes = new List<QuestInfo>();

    private void OnValidate()
    {
        if (_manager == null) _manager = FindObjectOfType<QuestManager>();
        _manager.InitializeAllQuests();
        questInfoes.Clear();
        foreach(string title in titles)
        {
            questInfoes.Add(_manager.allQuests[title]);
        }
    }

    public void OnInteract(Character initiative)
    {
        var questPanel = UIManager.Instance.questPanel;
        questPanel.QuestHost = this;
        questPanel.Accepter = initiative.QuestHolder;
        questPanel.Activate(true);
    }
#endif
}
