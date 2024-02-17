using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionsManager : MonoBehaviour
{
    public struct Action
    {
        public Vector3Int position;
        public ActionType type;
    }

    public enum ActionType
    {
        Start,
        Move,
        Attack
    }

    static ActionsManager manager = null;
    public TextMeshProUGUI text;
    static string baseText;

    static int maxActions = 5;
    public static int MaxActions
    {
        get
        {
            return maxActions;
        }
    }

    static int actionsLeft = 5;
    public static int ActionsLeft
    {
        get
        {
            return actionsLeft;
        }
    }

    static List<Action> actions = new();

    private void Awake()
    {
        manager = this;
        baseText = text.text;
    }

    private void Start()
    {
        manager.text.text = string.Format(baseText, actionsLeft);
    }

    public static void AddAction(ActionType type, Vector3Int position)
    {
        Action action = new()
        {
            position = position,
            type = type
        };
        actions.Add(action);
        UpdateActions();
    }

    public static void ClearActions()
    {
        actions.Clear();
        UpdateActions();
    }

    public static void DoActions()
    {   
        GridManager.AddEcho(actions);
        actions.Clear();
        UpdateActions();
        ActionsPlayer.PlayActions();
        manager.text.text = "Playing Actions";
    }

    static void UpdateActions()
    {
        if (ActionsPlayer.Playing) return;
        
        if (actions.Count == 0)
        {
            actionsLeft = maxActions;
        }else{
            actionsLeft = maxActions - actions.Count + 1;
        }
        manager.text.text = string.Format(baseText, actionsLeft);
    }
}
