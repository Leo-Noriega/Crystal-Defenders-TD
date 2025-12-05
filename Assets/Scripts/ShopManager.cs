using UnityEngine;
using UnityEngine.EventSystems; // ¡¡MUY IMPORTANTE AÑADIR ESTO!!

public class ShopManager : MonoBehaviour
{
    // --- Singleton (para que sea fácil de encontrar) ---
    public static ShopManager instance;
    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
    }
    // ---------------------------------------------------

    [Header("Setup")]
    public GameObject shopPanel; 
    public LayerMask nodeLayerMask; // <-- NUEVO: Para que el rayo solo golpee nodos

    [Header("Prefabs de Torres")]
    public GameObject mageTowerPrefab;
    public GameObject mortarTowerPrefab;
    public GameObject lightningTowerPrefab;
    
    private TowerNode selectedNode;
    private Camera mainCamera; // <-- NUEVO: Para guardar la cámara

    void Start()
    {
        // Guardamos la cámara al inicio para no llamarla 100 veces por frame
        mainCamera = Camera.main; 
        shopPanel.SetActive(false); // Asegurarnos que esté apagado
    }

    void Update()
    {
        // 1. Checamos si el usuario hizo clic
        if (Input.GetMouseButtonDown(0))
        {
            // 2. ¡IMPORTANTE! Checamos si el clic fue SOBRE LA UI (un botón)
            // Si es así, no hacemos nada en el mundo 3D.
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return; // El clic fue en un botón, no en el mundo
            }

            // 3. El clic fue en el mundo, lanzamos un rayo
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            
            // 4. Checamos si el rayo golpeó algo
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, nodeLayerMask)) // 100f = distancia, nodeLayerMask
            {
                // 5. Intentamos obtener el script TowerNode de lo que golpeamos
                if (hit.collider.TryGetComponent(out TowerNode node))
                {
                    // ¡Golpeamos un nodo!
                    if (node.IsOccupied())
                    {
                        // Está ocupado. (Aquí podrías abrir el menú de "Upgrade")
                        Debug.Log("Nodo ocupado.");
                        CloseShopMenu(); // Cerramos el menú por si estaba abierto en otro nodo
                    }
                    else
                    {
                        // ¡Está libre! Abrimos el menú.
                        OpenShopMenu(node);
                    }
                }
            }
            else
            {
                // 6. Si dimos clic en el pasto o en cualquier otra cosa, cerramos el menú
                CloseShopMenu();
            }
        }
    }


    // El TOWER NODE llama a esta función
    public void OpenShopMenu(TowerNode node)
    {
        selectedNode = node;
        shopPanel.SetActive(true);
    }

    // --- Lógica de Compra (Sin cambios) ---
    public void BuyMageTower()
    {
        if (selectedNode == null) return;
        selectedNode.BuildTower(mageTowerPrefab);
        CloseShopMenu();
    }
    
    public void BuyMortarTower()
    {
        if (selectedNode == null) return;
        selectedNode.BuildTower(mortarTowerPrefab);
        CloseShopMenu();
    }

    public void BuyLightningTower()
    {
        if (selectedNode == null) return;
        selectedNode.BuildTower(lightningTowerPrefab);
        CloseShopMenu();
    }
    
    // Función para cerrar el menú
    public void CloseShopMenu()
    {
        shopPanel.SetActive(false);
        selectedNode = null;
    }
}