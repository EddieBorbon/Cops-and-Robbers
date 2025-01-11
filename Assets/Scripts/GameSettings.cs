using UnityEngine;

public enum PlayerRole
{
    Cop,
    Robber
}

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    public PlayerRole PlayerRole { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayerRole(PlayerRole role)
    {
        PlayerRole = role;
    }
}