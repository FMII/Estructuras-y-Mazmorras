using UnityEngine;

/// <summary>
/// Zona de drop (estatua) que recibe items y genera enemigos
/// </summary>
public class DropZone : MonoBehaviour
{
    [Header("Referencias")]
    public DictionaryRoomManager roomManager;
    
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Color colorNormal = Color.white;
    public Color colorHighlight = new Color(1f, 1f, 0.5f);
    
    [Header("Detección")]
    public float radioDeteccion = 2f; // Radio para detectar items cercanos

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            
        if (spriteRenderer != null)
            spriteRenderer.color = colorNormal;
    }

    void OnMouseEnter()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = colorHighlight;
    }

    void OnMouseExit()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = colorNormal;
    }

    public void RecibirItem(DropItem item)
    {
        Debug.Log($"Item recibido: {item.nombreItem} (ID: {item.itemID})");
        
        if (roomManager != null)
        {
            roomManager.OnItemEntregado(item);
        }
    }
    
    // Verificar si un item está sobre esta zona
    public bool EstaEnZona(Vector3 posicionItem)
    {
        float distancia = Vector2.Distance(transform.position, posicionItem);
        return distancia <= radioDeteccion;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
    }
}
