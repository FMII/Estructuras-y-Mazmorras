using UnityEngine;

/// <summary>
/// Caja que puede ser empujada por el jugador
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PushableBox : MonoBehaviour
{
    [Header("Identificación")]
    public int numeroCaja; // 0, 1, 2, 3, 4 (representa el índice del arreglo)
    
    [Header("Configuración")]
    public float velocidadMovimiento = 2f;
    public KeyCode teclaInteractuar = KeyCode.E;
    
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Color colorNormal = Color.white;
    public Color colorEnPosicion = Color.green;
    public Color colorAgarrada = Color.yellow;
    public GameObject indicadorE; // Sprite o texto que muestra "E"
    
    private Rigidbody2D rb;
    private bool enPosicionCorrecta = false;
    private bool jugadorCerca = false;
    private bool estaAgarrada = false;
    private GameObject jugador;
    private PlayerBoxInteraction playerInteraction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Configurar física
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.bodyType = RigidbodyType2D.Kinematic; // Kinematic para control manual
        
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (indicadorE != null)
            indicadorE.SetActive(false);
            
        ActualizarColor();
    }

    void Update()
    {
        if (jugadorCerca && !estaAgarrada && Input.GetKeyDown(teclaInteractuar))
        {
            AgarrarCaja();
        }
        else if (estaAgarrada && Input.GetKeyDown(teclaInteractuar))
        {
            SoltarCaja();
        }
        
        if (estaAgarrada && jugador != null)
        {
            MoverConJugador();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !estaAgarrada)
        {
            jugadorCerca = true;
            jugador = other.gameObject;
            playerInteraction = jugador.GetComponent<PlayerBoxInteraction>();
            
            if (indicadorE != null)
                indicadorE.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            
            if (indicadorE != null)
                indicadorE.SetActive(false);
        }
    }

    void AgarrarCaja()
    {
        if (playerInteraction != null && playerInteraction.PuedeAgarrarCaja())
        {
            estaAgarrada = true;
            playerInteraction.AgarrarCaja(this);
            
            if (indicadorE != null)
                indicadorE.SetActive(false);
                
            ActualizarColor();
        }
    }

    void SoltarCaja()
    {
        estaAgarrada = false;
        
        if (playerInteraction != null)
            playerInteraction.SoltarCaja();
            
        ActualizarColor();
    }

    void MoverConJugador()
    {
        // Calcular dirección desde el jugador
        Vector2 direccionDesdeJugador = (transform.position - jugador.transform.position).normalized;
        
        // Posición objetivo: a 1 unidad del jugador
        Vector2 posicionObjetivo = (Vector2)jugador.transform.position + direccionDesdeJugador * 1f;
        
        // Mover suavemente hacia la posición
        rb.MovePosition(Vector2.Lerp(transform.position, posicionObjetivo, velocidadMovimiento * Time.deltaTime));
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Mantener por compatibilidad pero ya no usado
    }

    public void MarcarPosicionCorrecta(bool correcta)
    {
        enPosicionCorrecta = correcta;
        ActualizarColor();
    }

    void ActualizarColor()
    {
        if (spriteRenderer != null)
        {
            if (estaAgarrada)
                spriteRenderer.color = colorAgarrada;
            else if (enPosicionCorrecta)
                spriteRenderer.color = colorEnPosicion;
            else
                spriteRenderer.color = colorNormal;
        }
    }

    public bool EstaAgarrada()
    {
        return estaAgarrada;
    }

    public bool EstaEnPosicionCorrecta()
    {
        return enPosicionCorrecta;
    }

    void OnDrawGizmos()
    {
        // Mostrar el número de la caja en el editor
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.5f);
    }
}
