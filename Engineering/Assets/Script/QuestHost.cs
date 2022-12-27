using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
[DisallowMultipleComponent]
public class QuestHost : MonoBehaviour, IQuestHolder
{
    public GameObject Player;
    public string[] titles;
    public List<AQuest> Quests { get; private set; } = new List<AQuest>();
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
    private void OnMouseDown()
    {
        Debug.Log(Quests[0].title + " accepted;");
        Character player = Player.GetComponent<Character>(); 
        this.Assign(Quests[0] as AAsignableQuest, player.QuestHolder);
        
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
#endif
}
