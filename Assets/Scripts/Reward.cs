using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward", menuName = "GXT/Reward", order = 2)]
public class Reward : ScriptableObject
{
    public enum RewardType
    {
        ActionIncrease,
        DamageIncrease,
        CheckPoint,
    }

    public RewardType type;
    public int value;

    public void GiveReward(Vector3Int position)
    {
        switch (type)
        {
            case RewardType.ActionIncrease:
                ActionsManager.AddMaxActions(value);
                break;
            case RewardType.DamageIncrease:
                Player.DamageIncrease(value);
                break;
            case RewardType.CheckPoint:
                Player.checkpoint = position;
                break;
            default:
                break;
        }
    }
}
