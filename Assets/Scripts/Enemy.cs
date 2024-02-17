using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 3;
    int currentHealth = 0;

    public bool IsAlive
    {
        get
        {
            return currentHealth > 0;
        }
    }

    public Reward reward;
    bool rewardGiven = false;
    Vector3Int position;

    public EnemyHealthBar enemyHealthBar;

    private void OnEnable()
    {
        ActionsPlayer.OnResetActions += ResetActions;
    }

    private void OnDisable()
    {
        ActionsPlayer.OnResetActions -= ResetActions;
    }

    private void Start()
    {
        position = GridManager.AddEnemy(this);
        currentHealth = maxHealth;
    }

    void ResetActions()
    {
        if (reward.type != Reward.RewardType.CheckPoint)
        {
            currentHealth = maxHealth;
            enemyHealthBar.UpdateHealthBar(currentHealth / (float)maxHealth);
            enemyHealthBar.Show();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -=damage;
        enemyHealthBar.UpdateHealthBar(currentHealth / (float)maxHealth);
        if (currentHealth <= 0)
        {
            enemyHealthBar.Hide();
            //Die
            if (!rewardGiven)
            {
                reward.GiveReward(position);
                rewardGiven = true;

                if (reward.type == Reward.RewardType.CheckPoint)
                {
                    GridManager.RemoveEnemy(position);
                }
            }
        }
    }
}
