using UnityEngine;
using TMPro;

/// <summary>
/// Puerta que requiere responder un quiz antes de teleportar (Camino 2)
/// </summary>
public class QuizDoorCamino2 : MonoBehaviour
{
    [Header("Configuración de Teleporte")]
    public Vector2 posicionDestino = new Vector2(0, 0);
    
    [Header("Referencias")]
    public QuizManagerCamino2 quizManager;
    
    [Header("Visual")]
    public GameObject indicadorPresionaE;
    public SpriteRenderer spriteRenderer;
    public Color colorNormal = Color.cyan;
    public Color colorActivo = Color.yellow;
    
    [Header("UI (Opcional)")]
    public GameObject canvasMensaje; // Canvas padre (opcional)
    public TextMeshProUGUI mensajeText;
    public string mensajeEntrada = "Presiona [E] para el desafío final";
    
    private bool jugadorCerca = false;
    private GameObject jugador;
    private bool preguntaMostrada = false;

    void Start()
    {
        // Desactivar canvas al inicio
        if (canvasMensaje != null)
        {
            canvasMensaje.SetActive(false);
        }
        
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
        // Ignorar inputs si hay diálogos activos
        if (Dialogs.dialogActive) return;
        
        if (jugadorCerca && !preguntaMostrada && Input.GetKeyDown(KeyCode.E))
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
            
            Debug.Log("Jugador cerca de QuizDoorCamino2");
            
            if (indicadorPresionaE != null)
            {
                indicadorPresionaE.SetActive(true);
                Debug.Log("Indicador activado");
            }
            else
            {
                Debug.LogWarning("indicadorPresionaE es null!");
            }
            
            if (spriteRenderer != null)
            {
                spriteRenderer.color = colorActivo;
            }
            
            if (mensajeText != null)
            {
                // Activar canvas si existe
                if (canvasMensaje != null)
                {
                    canvasMensaje.SetActive(true);
                    Debug.Log("Canvas activado");
                }
                
                mensajeText.text = mensajeEntrada;
                Debug.Log($"Mensaje establecido: {mensajeEntrada}");
            }
            else
            {
                Debug.LogWarning("mensajeText es null!");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            jugador = null;
            preguntaMostrada = false;
            
            if (indicadorPresionaE != null)
                indicadorPresionaE.SetActive(false);
            
            if (spriteRenderer != null)
            {
                spriteRenderer.color = colorNormal;
            }
            
            if (mensajeText != null)
            {
                mensajeText.text = "";
                
                if (canvasMensaje != null)
                    canvasMensaje.SetActive(false);
            }
        }
    }

    void ActivarQuiz()
    {
        if (quizManager != null)
        {
            preguntaMostrada = true;
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
            
            // Resetear para que no vuelva a mostrar pregunta
            jugadorCerca = false;
            preguntaMostrada = false;
            
            if (indicadorPresionaE != null)
                indicadorPresionaE.SetActive(false);
                
            if (mensajeText != null)
            {
                mensajeText.text = "";
                
                if (canvasMensaje != null)
                    canvasMensaje.SetActive(false);
            }
            
            Debug.Log($"¡Quiz completado! Teleportado a {posicionDestino}");
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
