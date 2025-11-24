using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public GameObject puerta;
    public int to_camino_id;    
    private int camino_id;
    
    [Header("Sistema de Mensajes")]
    public TextMeshProUGUI mensajeText;
    public GameObject panelMensaje;
    
    void Start()
    {
        camino_id = PlayerPrefs.GetInt("camino_id", 1);
        
        // Asegurarse de que el panel esté oculto al inicio
        if (panelMensaje != null)
            panelMensaje.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {       
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (camino_id == to_camino_id)
            {
                SceneManager.LoadScene(to_camino_id);
            }
            else
            {
                MostrarMensaje("No intentes alterar el orden de la mazmorra o te saldrá muy caro...");
            }
        }
    }
    
    void MostrarMensaje(string mensaje)
    {
        if (mensajeText != null)
            mensajeText.text = mensaje;
            
        if (panelMensaje != null)
            panelMensaje.SetActive(true);
            
        Dialogs.dialogActive = true;
        // Ocultar el mensaje después de 3 segundos
        Invoke("OcultarMensaje", 3f);
    }
    
    void OcultarMensaje()
    {
        if (panelMensaje != null)
            panelMensaje.SetActive(false);
        Dialogs.dialogActive = false;
    }
}
