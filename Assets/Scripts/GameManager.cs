using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Victory")]
    [SerializeField] private GameObject victoryPanel;

    [Header("Player")]
    [SerializeField] private PlayerControllerFPS playerController;

    private bool gameFinished;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Time.timeScale = 1f;

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    private void Start()
    {
        if (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerControllerFPS>();
        }
    }

    public void EnemyKilled()
    {
        if (gameFinished) return;

        int enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemiesAlive <= 1)
        {
            WinGame();
        }
    }

    public void WinGame()
    {
        if (gameFinished) return;

        gameFinished = true;

        if (playerController != null)
        {
            playerController.SetControlEnabled(false);
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}