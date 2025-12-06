using UnityEngine;

public class StackableBlock : MonoBehaviour
{
    [Header("Configuración")]
    public BlockColor color;
    
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Color colorAmarillo = Color.yellow;
    public Color colorCafe = new Color(0.6f, 0.3f, 0.1f);
    public Color colorVerde = Color.green;
    public Color colorAzul = Color.blue;
    
    [Header("Física")]
    public bool puedeSerAgarrado = true;
    public bool estaApilado = false;
    
    private Rigidbody2D rb;
    private bool estaArrastrado = false;
    private Vector3 offset;
    private bool estaSobrePozo = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        AplicarColor();
    }

    void Update()
    {
        if (!puedeSerAgarrado || estaApilado) return;
        
        // Detectar cuando el jugador hace clic en el bloque
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                estaArrastrado = true;
                offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                offset.z = 0;
                
                // Hacer que el bloque sea kinematic mientras se arrastra
                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }
            }
        }
        
        // Arrastrar el bloque
        if (estaArrastrado)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos + offset;
        }
        
        // Soltar el bloque
        if (Input.GetMouseButtonUp(0) && estaArrastrado)
        {
            estaArrastrado = false;
            
            // Si está sobre el pozo, apilar
            if (estaSobrePozo && !estaApilado)
            {
                estaApilado = true;
                puedeSerAgarrado = false;
                
                RoomStackManager manager = RoomStackManager.GetInstance();
                if (manager != null)
                {
                    manager.ApilarBloque(color, gameObject);
                }
                return;
            }
            
            // Mantener kinematic para que no caiga
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Detectar si está sobre el pozo
        if (other.CompareTag("Well"))
        {
            estaSobrePozo = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Detectar si salió del pozo
        if (other.CompareTag("Well"))
        {
            estaSobrePozo = false;
        }
    }

    void AplicarColor()
    {
        if (spriteRenderer == null) return;
        
        switch (color)
        {
            case BlockColor.Amarillo:
                spriteRenderer.color = colorAmarillo;
                break;
            case BlockColor.Cafe:
                spriteRenderer.color = colorCafe;
                break;
            case BlockColor.Verde:
                spriteRenderer.color = colorVerde;
                break;
            case BlockColor.Azul:
                spriteRenderer.color = colorAzul;
                break;
        }
    }
}
