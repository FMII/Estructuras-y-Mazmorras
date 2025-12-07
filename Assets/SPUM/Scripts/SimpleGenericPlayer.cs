using UnityEngine;
using TMPro;

/// <summary>
/// JUGADOR SIMPLE: Recoge y coloca objetos en cajas
/// </summary>
public class SimpleGenericPlayer : MonoBehaviour
{
    [Header("Objeto en Mano")]
    private GenericObject objetoEnMano = null;
    
    [Header("UI Feedback")]
    public TextMeshProUGUI textoFeedback;
    
    private GenericContainer cajaActual = null;

    void Update()
    {
        // Si tiene objeto y está cerca de una caja, presiona E para colocar
        if (objetoEnMano != null && cajaActual != null && Input.GetKeyDown(KeyCode.E))
        {
            IntentarColocarEnCaja();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Detectar si está cerca de una caja
        GenericContainer caja = other.GetComponent<GenericContainer>();
        if (caja != null && objetoEnMano != null)
        {
            cajaActual = caja;
            MostrarMensaje("Presiona E para colocar");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<GenericContainer>() != null)
        {
            cajaActual = null;
            MostrarMensaje("");
        }
    }

    public void RecogerObjeto(GenericObject obj)
    {
        objetoEnMano = obj;
        MostrarMensaje("Recogiste: " + obj.tipoGenerico);
    }

    public bool TieneObjeto()
    {
        return objetoEnMano != null;
    }

    void IntentarColocarEnCaja()
    {
        if (cajaActual.IntentarColocar(objetoEnMano))
        {
            // ¡Éxito! El tipo coincide
            MostrarMensaje("¡Correcto! Caja<" + cajaActual.tipoAceptado + "> acepta " + objetoEnMano.tipoGenerico);
            objetoEnMano = null;
        }
        else
        {
            // Error: tipo incorrecto
            MostrarMensaje("ERROR: Caja<" + cajaActual.tipoAceptado + "> no acepta " + objetoEnMano.tipoGenerico);
        }
    }

    void MostrarMensaje(string mensaje)
    {
        if (textoFeedback != null)
        {
            textoFeedback.text = mensaje;
            if (mensaje != "")
                Invoke("LimpiarMensaje", 3f);
        }
    }

    void LimpiarMensaje()
    {
        if (textoFeedback != null)
            textoFeedback.text = "";
    }
}
