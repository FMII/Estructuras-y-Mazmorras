using UnityEngine;

public class DoorTeleport : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Puerta de destino")]
    public DoorTeleport puertaDestino;
    
    public KeyCode teclaTeletransporte = KeyCode.E;
    public float offsetSpawn = 1f; // Distancia del jugador respecto a la puerta
    
    [Header("Visual (Opcional)")]
    public GameObject indicadorPresionaE;

    private bool jugadorCerca = false;
    private GameObject jugador;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            jugador = other.gameObject;
            
            if (indicadorPresionaE != null)
                indicadorPresionaE.SetActive(true);
                
            Debug.Log("Presiona [E] para entrar");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            jugador = null;
            
            if (indicadorPresionaE != null)
                indicadorPresionaE.SetActive(false);
        }
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(teclaTeletransporte))
        {
            Teletransportar();
        }
    }

    void Teletransportar()
    {
        if (puertaDestino != null && jugador != null)
        {
            Vector3 posicionDestino = puertaDestino.transform.position;
            
            // Añadir offset para que aparezca un poco alejado de la puerta
            Vector3 direccion = (posicionDestino - transform.position).normalized;
            posicionDestino += direccion * offsetSpawn;
            
            jugador.transform.position = posicionDestino;
            Debug.Log("Teletransportado");
        }
    }

    void OnDrawGizmos()
    {
        // Visualizar área de la puerta en el editor
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        
        if (puertaDestino != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, puertaDestino.transform.position);
        }
    }
}
