using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [Header("Referencias")]
    public RoomEnemyManager roomManager;
    
    [Header("Configuración")]
    public bool soloUnaVez = true;
    
    private bool yaActivada = false;

    void Start()
    {
        if (roomManager == null)
        {
            roomManager = GetComponentInParent<RoomEnemyManager>();
        }
        
        // Asegurarse de que la sala NO genere al inicio
        if (roomManager != null)
        {
            roomManager.generarAlInicio = false;
        }
        
        // Verificar que este objeto tenga un trigger
        Collider2D col = GetComponent<Collider2D>();
        if (col != null && !col.isTrigger)
        {
            col.isTrigger = true;
        }
        
        // Verificar si el jugador ya está dentro del trigger al iniciar
        Invoke("VerificarJugadorEnSala", 0.5f);
    }
    
    void VerificarJugadorEnSala()
    {
        // Buscar al jugador
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider2D triggerCol = GetComponent<Collider2D>();
            Collider2D playerCol = player.GetComponent<Collider2D>();
            
            if (triggerCol != null && playerCol != null)
            {
                // Verificar si el jugador está dentro del trigger
                if (triggerCol.bounds.Intersects(playerCol.bounds))
                {
                    if (!yaActivada)
                    {
                        ActivarSala();
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Objeto entró al trigger: {other.gameObject.name} con tag: {other.tag}");
        
        // Verificar si es el jugador
        if (other.CompareTag("Player"))
        {
            if (!yaActivada)
            {
                ActivarSala();
            }
            else
            {
            }
        }
        else
        {
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
        }
    }

    void ActivarSala()
    {
        if (soloUnaVez)
        {
            yaActivada = true;
        }
        
        
        // Iniciar generación de enemigos
        if (roomManager != null)
        {
            roomManager.IniciarGeneracion();
        }
    }

    void OnDrawGizmos()
    {
        // Visualizar el área del trigger
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null)
        {
            Gizmos.color = new Color(0, 1, 0, 0.3f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(col.offset, col.size);
        }
    }
}
