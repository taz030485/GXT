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

    public static float playspeed = 1;
    public static float PlaySpeed
    {
        get
        {
            return playspeed;
        }
    }

    public Image border;
    public Toggle toggle;
    public GameObject resetButton;

    private void Awake()
    {
        manager = this;
        playing = false;
    }

    private void Start()
    {
        toggle.isOn = playspeed == 5;
    }

    public void ToggleChanged(bool toggleValue)
    {
        if (toggleValue)
        {
            playspeed = 5;
        }else{
            playspeed = 1;
        }
    }

    public static void PlayActions()
    {
        playing = true;
        manager.resetButton.SetActive(false);
        manager.border.color = Color.red;
        if (OnResetActions != null)
        {
            OnResetActions();
        }
        manager.StartCoroutine(manager.PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        int actions = ActionsManager.MaxActions;
        for (int i = 1; i <= actions; i++)
        {
            if (OnPlayAction != null)
            {
                OnPlayAction(i, 1/playspeed);
            }
            yield return new WaitForSeconds(1/playspeed);
        }
        playing = false;
        manager.resetButton.SetActive(true);
        border.color = Color.green;
        ActionsManager.ClearActions();
        Player.ResetTo(Player.checkpoint);
    }
}
