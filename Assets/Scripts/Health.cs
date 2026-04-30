using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    [Header("Type")]
    [SerializeField] private bool isPlayer;
    [SerializeField] private bool isEnemy;

    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public Action<int, int> OnHealthChanged;
    public Action OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log(gameObject.name + " took damage. Current health: " + currentHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke();

        if (isEnemy)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.EnemyKilled();
            }

            Destroy(gameObject);
        }

        if (isPlayer)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}