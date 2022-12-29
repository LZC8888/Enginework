using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;


[AddComponentMenu("")]
[DisallowMultipleComponent]
[ExecuteAlways]
public class QuestManager : MonoBehaviour
{
   public static QuestManager Instance { get; private set; }
    public string questPath = "Assets\\Script\\Quests.json";
    public SortedList<string, QuestInfo> allQuests = new SortedList<string, QuestInfo>();

    private void Awake()
    {
        
        Assert.IsNull(Instance);
        Instance = this;
        InitializeAllQuests();
    }
    private void OnDestroy()
    {
        Instance = null;
    }
    public void InitializeAllQuests()
    {
        allQuests.Clear();
        var json = File.ReadAllText(questPath);
        var infoes = json.ToArray<QuestInfo>();
        foreach(var info in infoes)
        {
            allQuests.Add(info.title, info);
        }
    }
    private void OnValidate()
    {
        InitializeAllQuests();
        infoesEditor = new List<QuestInfo>(allQuests.Values);
    }
#if UNITY_EDITOR
    [SerializeField] private List<QuestInfo> infoesEditor;
#endif
}
[Serializable]
public struct QuestInfo
{
    public string questType;

    public string title;
    public string description;

    public int exp;
    public string targetGuid;

    public Type QuestType => Enum.Parse<Type>(questType);
    public enum Type
    {
        None,
        CharacterSlayingQuest,
    }
}
public interface IQuestHolder
{
    List<AQuest> Quests { get; }
}
public interface IQuestHost : IQuestHolder
{
    Action OnQuestAssigned { get; set; }
}
public interface IQuestAccepter : IQuestHolder
{
    void OnAcceptQuest(AAsignableQuest quest);    
}

[Serializable]
public abstract class AQuest
{
    public string title;
    public string description;
}
[Serializable]
public abstract class AAsignableQuest: AQuest
{
    public int exp;
    public bool Started { get; private set; }
    public Action<int> onFinished;
    
    public virtual bool StartQuest()
    {
        if (Started) return false;
        Started = true;
        return true;
    } 
}
[Serializable]
public class CharacterSlayingQuest : AAsignableQuest {
    public string targetGuid;
    public CharacterSlayingQuest(QuestInfo info)
    {
        title = info.title;
        description = info.description;
        exp = info.exp;
        targetGuid = info.targetGuid;
    }
    public override bool StartQuest()
    {
        bool valid = base.StartQuest();
        if (valid) MessageCenter.OnCharacterDead += CheckIsFinished;
        return valid;
    }
    ~CharacterSlayingQuest()
    {
        MessageCenter.OnCharacterDead -= CheckIsFinished;
    }
    protected void CheckIsFinished(Character character)
    {
        if (character.Uid == targetGuid)
        {
            MessageCenter.OnCharacterDead -= CheckIsFinished;
            onFinished?.Invoke(exp);
            onFinished = null;
        }
    }
}
public static class IQuestExtension
{
    public static void Send(this IQuestHolder from,AQuest quest,IQuestHolder to)
    {
        from.Quests.Remove(quest);
        to.Quests.Add(quest);
        
    }
    public static void Assign(this IQuestHost from, AAsignableQuest quest, IQuestAccepter to)
    {
        from.Send(quest, to);
        from.OnQuestAssigned?.Invoke();
        
        to.OnAcceptQuest(quest);
        quest.StartQuest();
    }
}
