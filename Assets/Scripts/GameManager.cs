using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject defeatCanvas;

    private int score = 0;
    private bool gameOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        UpdateScoreUI();
        if (defeatCanvas != null)
            defeatCanvas.SetActive(false);
    }

    public void AddScore(int amount)
    {
        if (gameOver) return;
        score += amount;
        UpdateScoreUI();
    }

    public void TriggerDefeat()
    {
        if (gameOver) return;
        gameOver = true;

        Time.timeScale = 0f;
        if (defeatCanvas != null)
            defeatCanvas.SetActive(true);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Puntos: " + score.ToString();
    }
}
