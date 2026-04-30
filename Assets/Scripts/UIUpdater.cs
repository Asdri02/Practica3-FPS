using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private Health playerHealth;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthBar;

    [Header("Enemies")]
    [SerializeField] private TextMeshProUGUI enemiesText;

    private void Start()
    {
        if (playerHealth == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                playerHealth = playerObject.GetComponent<Health>();
            }
        }

        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthUI;
            UpdateHealthUI(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        }

        UpdateEnemiesUI();
    }

    private void Update()
    {
        UpdateEnemiesUI();
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthUI;
        }
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = "Vida: " + currentHealth;
        }

        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    private void UpdateEnemiesUI()
    {
        if (enemiesText != null)
        {
            int enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
            enemiesText.text = "Enemigos: " + enemiesAlive;
        }
    }
}