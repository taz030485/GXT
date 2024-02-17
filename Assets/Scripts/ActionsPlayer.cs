using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsPlayer : MonoBehaviour
{
    static ActionsPlayer manager = null;
    public delegate void ResetActions();
    public static event ResetActions OnResetActions;

    public delegate void PlayAction(int index, float speed);
    public static event PlayAction OnPlayAction;

    static bool playing = false;
    public static bool Playing
    {
        get
        {
            return playing;
        }
    }

    public float playspeed = 1;

    private void Awake()
    {
        manager = this;
    }

    public static void PlayActions()
    {
        playing = true;
        if (OnResetActions != null)
        {
            OnResetActions();
        }
        manager.StartCoroutine(manager.PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        for (int i = 1; i <= ActionsManager.MaxActions; i++)
        {
            if (OnPlayAction != null)
            {
                OnPlayAction(i, 1/playspeed);
            }
            yield return new WaitForSeconds(1/playspeed);
        }
        playing = false;
        ActionsManager.ClearActions();
    }
}
