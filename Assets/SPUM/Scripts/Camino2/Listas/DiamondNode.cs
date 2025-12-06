using UnityEngine;

public class DiamondNode : MonoBehaviour
{
    [Header("Configuración")]
    public DiamondColor color;
    public bool registrarAlTocar = true;
    public bool registrarAlVer = false;
    public float distanciaParaVer = 3f;
    
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Color colorRojo = Color.red;
    public Color colorVerde = Color.green;
    public Color colorAzul = Color.blue;
    
    private bool yaRegistrado = false;
    private Transform jugador;

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        // Aplicar color visual
        AplicarColor();
        
        // Buscar jugador
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }
    }

    void Update()
    {
        if (registrarAlVer && !yaRegistrado && jugador != null)
        {
            float distancia = Vector2.Distance(transform.position, jugador.position);
            if (distancia <= distanciaParaVer)
            {
                RegistrarNodo();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (registrarAlTocar && !yaRegistrado && other.CompareTag("Player"))
        {
            RegistrarNodo();
        }
    }

    void RegistrarNodo()
    {
        yaRegistrado = true;
        
        RoomManager roomManager = RoomManager.GetInstance();
        if (roomManager != null)
        {
            roomManager.RegistrarNodo(color);
            Debug.Log($"Nodo {color} registrado por el jugador");
            
            // Efecto visual de que fue registrado
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
            }
        }
    }

    void AplicarColor()
    {
        if (spriteRenderer == null) return;
        
        switch (color)
        {
            case DiamondColor.Rojo:
                spriteRenderer.color = colorRojo;
                break;
            case DiamondColor.Verde:
                spriteRenderer.color = colorVerde;
                break;
            case DiamondColor.Azul:
                spriteRenderer.color = colorAzul;
                break;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (registrarAlVer)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, distanciaParaVer);
        }
    }
}
