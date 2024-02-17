using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Image border;
    public Toggle toggle;

    private void Awake()
    {
        manager = this;
    }

    public void ToggleChanged(bool toggle)
    {
        if (toggle)
        {
            playspeed = 5;
        }else{
            playspeed = 1;
        }
    }

    public static void PlayActions()
    {
        playing = true;
        manager.border.color = Color.red;
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
        border.color = Color.green;
        ActionsManager.ClearActions();
    }
}
