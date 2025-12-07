using UnityEngine;

/// <summary>
/// Puerta que requiere responder un quiz antes de teleportar (Camino 2)
/// </summary>
public class QuizDoorCamino2 : MonoBehaviour
{
    [Header("Configuración de Teleporte")]
    public Vector2 posicionDestino = new Vector2(0, 0);
    
    [Header("Referencias")]
    public QuizManagerCamino2 quizManager;
    
    [Header("Visual (Opcional)")]
    public SpriteRenderer spriteRenderer;
    public Color colorNormal = Color.cyan;
    public Color colorActivo = Color.yellow;
    
    private bool jugadorCerca = false;
    private GameObject jugador;

    void Start()
    {
        // Buscar el QuizManager si no está asignado
        if (quizManager == null)
        {
            quizManager = FindObjectOfType<QuizManagerCamino2>();
            
            if (quizManager == null)
            {
                Debug.LogError("No se encontró QuizManagerCamino2 en la escena!");
            }
        }
        
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorNormal;
        }
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            ActivarQuiz();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            jugador = other.gameObject;
            
            if (spriteRenderer != null)
            {
                spriteRenderer.color = colorActivo;
            }
            
            Debug.Log("Presiona E para responder el quiz y continuar");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            jugador = null;
            
            if (spriteRenderer != null)
            {
                spriteRenderer.color = colorNormal;
            }
        }
    }

    void ActivarQuiz()
    {
        if (quizManager != null)
        {
            Debug.Log("Iniciando quiz de Camino 2...");
            quizManager.MostrarPreguntaAleatoria(this);
        }
        else
        {
            Debug.LogError("No hay QuizManager asignado!");
        }
    }

    /// <summary>
    /// Llamado por el QuizManager cuando el jugador responde correctamente
    /// </summary>
    public void TeleportarJugador()
    {
        if (jugador != null)
        {
            jugador.transform.position = new Vector3(posicionDestino.x, posicionDestino.y, jugador.transform.position.z);
            Debug.Log($"Jugador teleportado a {posicionDestino}");
        }
        else
        {
            // Buscar al jugador si no está guardado
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = new Vector3(posicionDestino.x, posicionDestino.y, player.transform.position.z);
                Debug.Log($"Jugador teleportado a {posicionDestino}");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Dibujar línea hacia el destino
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, new Vector3(posicionDestino.x, posicionDestino.y, transform.position.z));
        
        // Dibujar esfera en el destino
        Gizmos.DrawWireSphere(new Vector3(posicionDestino.x, posicionDestino.y, transform.position.z), 0.5f);
    }
}
