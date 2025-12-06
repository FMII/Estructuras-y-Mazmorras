using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Estado")]
    public bool estaActiva = false;
    
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Color colorInactivo = new Color(0.5f, 0.5f, 0.5f, 0.3f);
    public Color colorActivo = Color.white;
    
    [Header("Física")]
    public Collider2D platformCollider;
    
    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        if (platformCollider == null)
        {
            platformCollider = GetComponent<Collider2D>();
        }
        
        // Iniciar desactivada
        DesactivarPlataforma();
    }

    public void ActivarPlataforma()
    {
        estaActiva = true;
        
        // Cambiar color a activo
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorActivo;
        }
        
        // Activar collider como trigger (para que el jugador pase a través)
        if (platformCollider != null)
        {
            platformCollider.enabled = true;
            platformCollider.isTrigger = true;
        }
        
        Debug.Log($"Plataforma {gameObject.name} ACTIVADA");
    }

    public void DesactivarPlataforma()
    {
        estaActiva = false;
        
        // Cambiar color a inactivo (semi-transparente)
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorInactivo;
        }
        
        // Desactivar collider como sólido (bloquea el paso)
        if (platformCollider != null)
        {
            platformCollider.enabled = true;
            platformCollider.isTrigger = false;
        }
        
        Debug.Log($"Plataforma {gameObject.name} DESACTIVADA");
    }
}
