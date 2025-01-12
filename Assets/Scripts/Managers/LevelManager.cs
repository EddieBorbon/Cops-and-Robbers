using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance; // Singleton instance

    public PlayerManager playerManager;
    public BotManager botManager;
    public MoneyManager moneyManager;
    public CameraManager cameraManager;
    public TimerManager timerManager;
    public UIManager uiManager;
    public GameStateManager gameStateManager;

    public int robbersPerLevel = 3;
    public int moneyPerLevel = 5;
    public int copsPerLevel = 2;

    private int robbersLeft;
    private int moneyLeft;

    void Start()
    {
        InitializeLevel(GameManager.Instance.currentLevel);
    }

    public void InitializeLevel(int level)
    {
        robbersPerLevel = 3 + level;
        moneyPerLevel = 5 + level;
        copsPerLevel = 2 + level;

        robbersLeft = robbersPerLevel;
        moneyLeft = 0;

        playerManager.SpawnPlayer();
        playerManager.MovePlayerToSpawn();

        if (GameSettings.Instance.PlayerRole == PlayerRole.Cop)
        {
            botManager.SpawnBots(robbersPerLevel, 0);
        }
        else
        {
            botManager.SpawnBots(0, copsPerLevel);
            moneyManager.SpawnMoney(moneyPerLevel);
        }

        cameraManager.SetupCamera(playerManager.GetPlayerTransform());
        timerManager.StartTimer();
    }

    void Update()
    {
        timerManager.UpdateTimer();
        uiManager.UpdateTimerUI(timerManager.currentTime);

        if (timerManager.IsTimeUp())
        {
            gameStateManager.GameOver();
        }
    }
}