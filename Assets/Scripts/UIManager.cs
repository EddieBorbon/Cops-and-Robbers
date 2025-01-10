using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI robbersRemainingText;

    void Update()
    {
        levelText.text = "Nivel: " + LevelManager.Instance.currentLevel;
        robbersRemainingText.text = "Robbers Restantes: " + (LevelManager.Instance.robbersPerLevel - LevelManager.Instance.robbersCaught);
    }
}