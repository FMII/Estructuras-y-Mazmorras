using UnityEngine;
using TMPro;

/// <summary>
/// Puerta que requiere responder una pregunta antes de teleportar
/// </summary>
public class QuizDoor : MonoBehaviour
{
    [Header("Configuración")]
    public Transform puntoDestino; // A dónde teleporta
    public QuizManager quizManager;
    
    [Header("Visual (Opcional)")]
    public GameObject indicadorPresionaE;
    
    [Header("UI (Opcional)")]
    public TextMeshProUGUI mensajeText;
    
    private bool jugadorCerca = false;
    private GameObject jugador;
    private bool preguntaMostrada = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            jugador = other.gameObject;
            
            if (indicadorPresionaE != null)
                indicadorPresionaE.SetActive(true);
                
            if (mensajeText != null)
                mensajeText.text = "Presiona [E] para el desafío final";
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
                
            if (mensajeText != null)
                mensajeText.text = "";
        }
    }

    void Update()
    {
        // Ignorar inputs si hay diálogos activos
        if (Dialogs.dialogActive) return;
        
        if (jugadorCerca && !preguntaMostrada && Input.GetKeyDown(KeyCode.E))
        {
            MostrarPregunta();
        }
    }

    void MostrarPregunta()
    {
        if (quizManager != null)
        {
            preguntaMostrada = true;
            quizManager.MostrarPreguntaAleatoria(this);
        }
    }

    public void TeleportarJugador()
    {
        if (puntoDestino != null && jugador != null)
        {
            jugador.transform.position = puntoDestino.position;
            
            // Resetear para que no vuelva a mostrar pregunta
            jugadorCerca = false;
            preguntaMostrada = false;
            
            if (indicadorPresionaE != null)
                indicadorPresionaE.SetActive(false);
                
            if (mensajeText != null)
                mensajeText.text = "";
            
            Debug.Log("¡Quiz completado! Teletransportado");
        }
    }

    void OnDrawGizmos()
    {
        // Visualizar puerta y destino
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        
        if (puntoDestino != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, puntoDestino.position);
            Gizmos.DrawWireSphere(puntoDestino.position, 0.3f);
        }
    }
}
