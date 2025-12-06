using UnityEngine;
using TMPro;

public class DoorTeleport : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Puerta de destino")]
    public DoorTeleport puertaDestino;
    
    public KeyCode teclaTeletransporte = KeyCode.E;
    public float offsetSpawn = 1f; // Distancia del jugador respecto a la puerta
    
    [Header("Visual (Opcional)")]
    public GameObject indicadorPresionaE;

    [Header("UI (Opcional)")]
    public GameObject canvasDoor; // Objeto del canvas que muestra el mensaje
    public TextMeshProUGUI mensajeText;
    public GameObject panelMensaje;
    [Header("Comportamiento")]
    [Tooltip("Si está activado, mostrar el mensaje bloqueará inputs globales usando Dialogs.dialogActive")] 
    public bool bloquearInputsAlMostrar = false;

    private bool jugadorCerca = false;
    private GameObject jugador;

    void Start()
    {
        // Asegurarse de que el canvasDoor esté desactivado al inicio
        if (canvasDoor != null)
        {
            canvasDoor.SetActive(false);
        }
    }

    void MostrarMensaje(string mensaje)
    {
        // Cancelar cualquier ocultado pendiente y mostrar el panel hasta que el jugador salga
        CancelInvoke("OcultarMensaje");

        if (mensajeText != null)
            mensajeText.text = mensaje;

        if (panelMensaje != null)
            panelMensaje.SetActive(true);

        // Opcional: bloquear inputs globales si el diseñador lo quiere
        if (bloquearInputsAlMostrar)
            Dialogs.dialogActive = true;
    }

    void OcultarMensaje()
    {
        if (panelMensaje != null)
            panelMensaje.SetActive(false);

        if (bloquearInputsAlMostrar)
            Dialogs.dialogActive = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            jugador = other.gameObject;
            
            if (indicadorPresionaE != null)
                indicadorPresionaE.SetActive(true);

            MostrarMensaje("Presiona [E] para entrar");

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

            OcultarMensaje();
        }
    }

    void Update()
    {
        // Ignorar inputs si hay diálogos activos
        if (Dialogs.dialogActive) return;
        
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
