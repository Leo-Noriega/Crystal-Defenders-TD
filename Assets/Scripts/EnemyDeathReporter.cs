using UnityEngine;

public class EnemyDeathReporter : MonoBehaviour
{
    [HideInInspector] public WaveManager manager;

    void OnDestroy()
    {
        if (manager != null)
        {
            manager.ReportEnemyDeath();
        }
    }
}