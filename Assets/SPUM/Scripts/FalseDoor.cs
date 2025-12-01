using UnityEngine;
using TMPro;

public class FalseDoor : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Punto de respawn donde aparecerá el jugador")]
    public Transform puntoRespawn;
    
    [Header("Mensaje (Opcional)")]
    public bool mostrarMensaje = true;
    public string mensajeTrampa = "¡Era una trampa! Vuelves al inicio...";
    public TextMeshProUGUI mensajeText;
    public GameObject panelMensaje;
    public float tiempoMensaje = 2f;
    
    [Header("Visual (Opcional)")]
    public GameObject efectoTrampa; // Partículas o efecto visual al activarse

    private bool trampaActivada = false;

    void Start()
    {
        // Asegurar que el panel esté oculto al inicio
        if (panelMensaje != null)
            panelMensaje.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !trampaActivada)
        {
            ActivarTrampa(other.gameObject);
        }
    }

    void ActivarTrampa(GameObject jugador)
    {
        trampaActivada = true;

        // Mostrar efecto visual si existe
        if (efectoTrampa != null)
        {
            Instantiate(efectoTrampa, transform.position, Quaternion.identity);
        }

        // Mostrar mensaje
        if (mostrarMensaje && mensajeText != null && panelMensaje != null)
        {
            MostrarMensaje(mensajeTrampa);
        }

        // Teletransportar al jugador al respawn
        if (puntoRespawn != null)
        {
            jugador.transform.position = puntoRespawn.position;
            Debug.Log("Jugador teletransportado al respawn por puerta falsa");
        }
        else
        {
            Debug.LogWarning("FalseDoor: No hay punto de respawn asignado!");
        }

        // Resetear la trampa después de un tiempo para que pueda activarse de nuevo
        Invoke("ResetearTrampa", 1f);
    }

    void MostrarMensaje(string mensaje)
    {
        if (mensajeText != null)
            mensajeText.text = mensaje;

        if (panelMensaje != null)
            panelMensaje.SetActive(true);

        // Ocultar el mensaje después del tiempo especificado
        Invoke("OcultarMensaje", tiempoMensaje);
    }

    void OcultarMensaje()
    {
        if (panelMensaje != null)
            panelMensaje.SetActive(false);
    }

    void ResetearTrampa()
    {
        trampaActivada = false;
    }

    void OnDrawGizmos()
    {
        // Visualizar la puerta falsa en el editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        
        // Dibujar línea hacia el punto de respawn
        if (puntoRespawn != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, puntoRespawn.position);
            Gizmos.DrawWireSphere(puntoRespawn.position, 0.3f);
        }
    }
}
