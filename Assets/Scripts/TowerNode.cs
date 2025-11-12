using UnityEngine;

public class TowerNode : MonoBehaviour
{
    [Header("Setup")]
    public Transform buildPoint; 
    
    private GameObject currentTower;

    // --- ¡HEMOS BORRADO OnMouseDown() DE AQUÍ! ---

    // El ShopManager llamará a esta función
    public void BuildTower(GameObject towerPrefab)
    {
        if (currentTower != null) return; // Doble chequeo por si acaso

        GameObject newTower = Instantiate(towerPrefab, buildPoint.position, buildPoint.rotation);
        currentTower = newTower;
        Debug.Log("¡Torre construida!");
    }

    // Una función simple para que el Manager sepa si estamos ocupados
    public bool IsOccupied()
    {
        return currentTower != null;
    }
}