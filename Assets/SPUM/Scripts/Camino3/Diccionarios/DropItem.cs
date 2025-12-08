using UnityEngine;

/// <summary>
/// Item que puede ser arrastrado y soltado sobre una estatua
/// </summary>
public class DropItem : MonoBehaviour
{
    [Header("Configuración")]
    public int itemID; // ID único del item (1-8)
    public string nombreItem;
    
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    
    private bool estaSiendoArrastrado = false;
    private Vector3 offset;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Detectar click sobre el objeto
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                estaSiendoArrastrado = true;
                offset = transform.position - cam.ScreenToWorldPoint(Input.mousePosition);
                offset.z = 0;
            }
        }
        
        // Soltar el objeto
        if (Input.GetMouseButtonUp(0) && estaSiendoArrastrado)
        {
            estaSiendoArrastrado = false;
            VerificarDropZone();
        }
        
        // Mover el objeto mientras se arrastra
        if (estaSiendoArrastrado)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos + offset;
        }
    }

    void VerificarDropZone()
    {
        // Buscar todas las DropZones en la escena
        DropZone[] zonas = FindObjectsByType<DropZone>(FindObjectsSortMode.None);
        
        foreach (DropZone zona in zonas)
        {
            if (zona.EstaEnZona(transform.position))
            {
                Debug.Log($"Item {itemID} soltado sobre DropZone");
                zona.RecibirItem(this);
                return;
            }
        }
        
        Debug.Log($"Item {itemID} soltado fuera de cualquier zona");
    }

    public void Destruir()
    {
        Destroy(gameObject);
    }
}
