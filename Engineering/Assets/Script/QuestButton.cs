using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Assertions;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class QuestButton : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    
    [NonSerialized] public IQuestHost Host;
    [NonSerialized] public AAsignableQuest Quest;
    [NonSerialized] public CanvasGroup CanvasGroup;
    [NonSerialized] public IQuestAccepter Accepter;

    private Button _button;
    public Button.ButtonClickedEvent onClick => _button.onClick;

    private void Awake()
    {
        TryGetComponent(out CanvasGroup);
        Assert.IsNotNull(CanvasGroup);

        TryGetComponent(out _button);
        Assert.IsNotNull(_button);
    }

    public void Activate(bool active,IQuestHost host = null, AAsignableQuest quest = null,IQuestAccepter accepter = null)
    {
        Assert.IsFalse(active && (host == null || quest == null));
        Host = host;
        Quest = quest;
        Accepter = accepter;

        CanvasGroup.alpha = active ? 1f : 0f;
        CanvasGroup.interactable = active;
        
        if (active) onClick.AddListener(OnAccept);
        else onClick.RemoveAllListeners();

        title.text = active ? quest.title : "";
        description.text = active ? quest.description : "";
    }
    private void OnAccept()
    {
         Host.Assign(Quest, CharacterManager.Instance.player.QuestHolder);
       // host.Assign(quest, Accepter);
    }
}
