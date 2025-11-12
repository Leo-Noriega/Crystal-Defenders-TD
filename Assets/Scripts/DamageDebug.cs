using UnityEngine;
using UnityEngine.InputSystem;
public class DamageDebug : MonoBehaviour
{
    public TowerHealth th;
    public float dmg = 10f;
    public Key triggerKey = Key.K; // New Input System key (default: K)
    void Update()
    {
        var kb = Keyboard.current;
        if (kb != null)
        {
            var keyControl = kb[triggerKey];
            if (keyControl != null && keyControl.wasPressedThisFrame)
            {
                th.TakeDamage(dmg);
            }
        }
    }
}