using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Puerta que carga una nueva escena automáticamente al tocarla
/// </summary>
public class DoorSceneLoader : MonoBehaviour
{
    [Header("Configuración")]
    public string nombreEscena; // Nombre de la escena a cargar
    
    [Header("Visual (Opcional)")]
    public TextMeshProUGUI mensajeText;
    public string mensajePersonalizado = "Cargando...";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CargarEscena();
        }
    }

    void CargarEscena()
    {
        if (string.IsNullOrEmpty(nombreEscena))
        {
            Debug.LogError("No se especificó el nombre de la escena a cargar");
            return;
        }
        
        if (mensajeText != null)
            mensajeText.text = mensajePersonalizado;
        
        Debug.Log($"Cargando escena: {nombreEscena}");
        SceneManager.LoadScene(nombreEscena);
    }

    void OnDrawGizmos()
    {
        // Visualizar puerta de salida
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        
        // Dibujar flecha indicando salida
        Gizmos.color = Color.yellow;
        Vector3 up = transform.position + Vector3.up * 0.8f;
        Gizmos.DrawLine(transform.position, up);
    }
}
