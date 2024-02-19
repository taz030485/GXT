using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RewardUI : MonoBehaviour
{
    static RewardUI manager = null;
    public GameObject rewardUI;
    public TextMeshProUGUI textBox;
    static string textBase;

    private void Awake()
    {
        manager = this;
        textBase = textBox.text;
    }

    public static void ShowReward(Reward reward)
    {
        manager.textBox.text = string.Format(textBase, reward.rewardText);
        manager.rewardUI.SetActive(true);
    }
}
