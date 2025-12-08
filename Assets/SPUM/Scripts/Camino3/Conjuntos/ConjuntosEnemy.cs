using UnityEngine;

/// <summary>
/// Script para enemigos en el juego de teoría de conjuntos
/// Cada enemigo pertenece a uno o ambos conjuntos (A, B, o A ∩ B)
/// </summary>
public class ConjuntosEnemy : MonoBehaviour
{
    [Header("Pertenencia a Conjuntos")]
    public bool estaEnConjuntoA = false;
    public bool estaEnConjuntoB = false;
    
    [Header("Referencias")]
    public ConjuntosRoomManager roomManager;
    
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    
    // Colores para indicar pertenencia
    private static readonly Color colorSoloA = new Color(0.3f, 0.3f, 1f); // Azul
    private static readonly Color colorSoloB = new Color(1f, 0.3f, 0.3f); // Rojo
    private static readonly Color colorAmbos = new Color(0.7f, 0.3f, 1f); // Morado
    
    void Start()
    {
        // Obtener sprite renderer si no está asignado
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        
        // Aplicar color después de un frame para asegurar que el sprite esté listo
        Invoke("AplicarColorSegunConjunto", 0.1f);
    }
    
    /// <summary>
    /// Aplica un color al enemigo según su pertenencia a los conjuntos
    /// </summary>
    public void AplicarColorSegunConjunto()
    {
        if (spriteRenderer == null) return;
        
        if (estaEnConjuntoA && estaEnConjuntoB)
        {
            // Pertenece a ambos (intersección)
            spriteRenderer.color = colorAmbos;
        }
        else if (estaEnConjuntoA)
        {
            // Solo en A
            spriteRenderer.color = colorSoloA;
        }
        else if (estaEnConjuntoB)
        {
            // Solo en B
            spriteRenderer.color = colorSoloB;
        }
        else
        {
            // No pertenece a ninguno (esto no debería pasar)
            spriteRenderer.color = Color.white;
        }
    }
    
    /// <summary>
    /// Llamar este método cuando el enemigo muera
    /// </summary>
    public void OnDeath()
    {
        if (roomManager != null)
        {
            roomManager.OnEnemigoEliminado(gameObject);
        }
        
        Destroy(gameObject);
    }
    
    void OnDestroy()
    {
        // La notificación se hace en OnDeath() antes de destruir
        // Este método se deja vacío para evitar doble validación
    }
    
    // Para debug
    void OnDrawGizmos()
    {
        if (estaEnConjuntoA && estaEnConjuntoB)
            Gizmos.color = colorAmbos;
        else if (estaEnConjuntoA)
            Gizmos.color = colorSoloA;
        else if (estaEnConjuntoB)
            Gizmos.color = colorSoloB;
        else
            Gizmos.color = Color.white;
        
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
