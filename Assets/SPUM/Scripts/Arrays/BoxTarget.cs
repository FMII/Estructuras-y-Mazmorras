using UnityEngine;

/// <summary>
/// Zona objetivo donde debe ir una caja específica
/// </summary>
public class BoxTarget : MonoBehaviour
{
    [Header("Configuración")]
    public int numeroEsperado; // Qué número de caja debe ir aquí
    
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Color colorVacio = new Color(1f, 1f, 1f, 0.3f);
    public Color colorOcupado = new Color(0f, 1f, 0f, 0.5f);
    
    [Header("Texto (Opcional)")]
    public TextMesh textoNumero; // Muestra [0], [1], etc.
    
    private PushableBox cajaActual;
    private bool posicionCorrecta = false;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
            
        if (textoNumero != null)
            textoNumero.text = $"[{numeroEsperado}]";
            
        ActualizarColor();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PushableBox caja = other.GetComponent<PushableBox>();
        if (caja != null)
        {
            cajaActual = caja;
            VerificarPosicion();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PushableBox caja = other.GetComponent<PushableBox>();
        if (caja != null && caja == cajaActual)
        {
            cajaActual.MarcarPosicionCorrecta(false);
            cajaActual = null;
            posicionCorrecta = false;
            ActualizarColor();
            
            // Notificar al manager
            BoxPuzzleManager manager = FindObjectOfType<BoxPuzzleManager>();
            if (manager != null)
                manager.VerificarSolucion();
        }
    }

    void VerificarPosicion()
    {
        if (cajaActual != null)
        {
            posicionCorrecta = (cajaActual.numeroCaja == numeroEsperado);
            cajaActual.MarcarPosicionCorrecta(posicionCorrecta);
            ActualizarColor();
            
            // Notificar al manager
            BoxPuzzleManager manager = FindObjectOfType<BoxPuzzleManager>();
            if (manager != null)
                manager.VerificarSolucion();
        }
    }

    void ActualizarColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = posicionCorrecta ? colorOcupado : colorVacio;
        }
    }

    public bool TieneCajaCorrecta()
    {
        return posicionCorrecta && cajaActual != null;
    }

    void OnDrawGizmos()
    {
        // Visualizar la zona objetivo en el editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}
