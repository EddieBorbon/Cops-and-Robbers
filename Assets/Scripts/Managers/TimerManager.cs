using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public float timerDuration = 60f;
    public float currentTime;
    private bool isGameStarted = false;

    public void StartTimer()
    {
        isGameStarted = true;
        currentTime = timerDuration;
    }

    public void UpdateTimer()
    {
        if (isGameStarted && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
    }

    public bool IsTimeUp()
    {
        return currentTime <= 0;
    }
}