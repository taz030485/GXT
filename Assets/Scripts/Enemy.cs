using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TextMeshProUGUI text;
    string textBase;

    public AudioSource hurtAudio;
    public AudioSource deathAudio;

    Animator animator;

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
        textBase = text.text;
        text.text = string.Format(textBase, currentHealth, maxHealth);

        animator = GetComponentInChildren<Animator>();
    }

    void ResetActions()
    {
        if (reward.type != Reward.RewardType.CheckPoint)
        {
            currentHealth = maxHealth;
            text.text = string.Format(textBase, currentHealth, maxHealth);
            enemyHealthBar.UpdateHealthBar(currentHealth / (float)maxHealth);
            enemyHealthBar.Show();
            if (animator != null)
            {
                animator.SetTrigger("Reset");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -=damage;
        if (animator != null)
        {
            animator.SetTrigger("GetHit");
            hurtAudio.Play();
        }
        text.text = string.Format(textBase, currentHealth, maxHealth);
        enemyHealthBar.UpdateHealthBar(currentHealth / (float)maxHealth);
        if (currentHealth <= 0)
        {
            enemyHealthBar.Hide();
            if (animator != null)
            {
                animator.SetTrigger("Die");
                deathAudio.Play();
            }
            if (!rewardGiven)
            {
                reward.GiveReward(position);
                rewardGiven = true;

                RewardUI.ShowReward(reward);

                if (reward.type == Reward.RewardType.CheckPoint)
                {
                    gameObject.SetActive(false);
                    GridManager.RemoveEnemy(position);
                }
            }
        }
    }
}
