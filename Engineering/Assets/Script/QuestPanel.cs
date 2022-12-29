using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
public class QuestPanel : AWindow
{
    public VerticalLayoutGroup questsGroup;
    public GameObject buttonPrefab;

    public IQuestHost QuestHost;
    public IQuestAccepter Accepter;

    private List<QuestButton> _questButtons = new List<QuestButton>();

    protected override  void Awake()
    {
        base.Awake();
        Assert.IsNotNull(questsGroup);
        Assert.IsNotNull(buttonPrefab);
    }
    public override void Activate(bool active)
    {
        base.Activate(active);
        if (active) QuestHost.OnQuestAssigned += UpdateQuestList;
      /*  else
        {
            QuestHost.OnQuestAssigned -= UpdateQuestList;
            QuestHost = null;
        }*/
        UpdateQuestList();
    }
    public void UpdateQuestList()
    {
        foreach(var button in _questButtons)
        {
            button.title.text = "";
            button.description.text = "";
            button.Activate(false);
        }
        if (QuestHost == null) return;
        int questCount = QuestHost.Quests.Count;
        int newCount = questCount - _questButtons.Count;
        while (newCount > 0)
        {
            var newButton = Instantiate(buttonPrefab, questsGroup.transform);
            if(newButton.TryGetComponent(out QuestButton button))
            {
                button.Activate(false);
                _questButtons.Add(button);
            }
            newCount--;
        }
        for(int i = 0; i < questCount; i++)
        {
            var button = _questButtons[i];
            var quest = QuestHost.Quests[i];
            button.Activate(true, QuestHost, quest as AAsignableQuest,Accepter);
        }
    }
}
