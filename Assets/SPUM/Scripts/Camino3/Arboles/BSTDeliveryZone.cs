using UnityEngine;
using TMPro;

/// <summary>
/// Zona donde el jugador debe llevar bloques específicos
/// Ahora recibe bloques arrastrados físicamente
/// </summary>
public class BSTDeliveryZone : MonoBehaviour
{
    [Header("Configuración")]
    public BSTBlock.ColorBloque colorEsperado; // El color del bloque que debe traerse aquí
    
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public TextMeshPro textoIndicacion;
    public Color colorNormal = new Color(1f, 1f, 1f, 0.3f);
    public Color colorActivo = new Color(1f, 1f, 0f, 0.5f);
    public Color colorCorrecto = Color.green;
    public Color colorIncorrecto = Color.red;
    
    private BSTSearchValidator validator;

    void Start()
    {
        validator = FindObjectOfType<BSTSearchValidator>();
        
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null)
            spriteRenderer.color = colorNormal;
        
        if (textoIndicacion != null)
        {
            textoIndicacion.text = $"Zona {colorEsperado}";
        }
    }

    /// <summary>
    /// Llamado por BSTBlock cuando se suelta en esta zona
    /// </summary>
    public void RecibirBloque(BSTBlock bloque)
    {
        if (validator == null) return;
        
        bool esValido = validator.ValidarEntrega(bloque.colorBloque);
        
        if (esValido)
        {
            // Correcto - bloquear el bloque en esta posición
            if (spriteRenderer != null)
                spriteRenderer.color = colorCorrecto;
            
            bloque.transform.position = transform.position;
            bloque.BloquearArrastre();
            
            Debug.Log($"✅ Bloque {bloque.colorBloque} entregado correctamente!");
            
            if (textoIndicacion != null)
                textoIndicacion.text = $"✅ {colorEsperado}";
        }
        else
        {
            // Incorrecto - rechazar el bloque
            if (spriteRenderer != null)
            {
                spriteRenderer.color = colorIncorrecto;
                Invoke("RestablecerColor", 1f);
            }
            
            bloque.RegresarAPosicionInicial();
            Debug.Log($"❌ Bloque incorrecto. Se esperaba {colorEsperado}, recibió {bloque.colorBloque}");
        }
    }

    void RestablecerColor()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = colorNormal;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        
        #if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.7f, $"Zona: {colorEsperado}");
        #endif
    }
}
