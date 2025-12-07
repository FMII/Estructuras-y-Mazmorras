using UnityEngine;
using TMPro;

/// <summary>
/// CAJA GENÉRICA: Solo acepta objetos de su tipo
/// Representa Caja<T> en programación
/// </summary>
public class GenericContainer : MonoBehaviour
{
    [Header("Tipo Genérico <T>")]
    public string tipoAceptado = "Arma"; // "Arma", "Comida", o "Tesoro"
    
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer iconoTipo; // Sprite del tipo (espada, manzana, gema)
    public TextMeshPro etiqueta; // Muestra "Caja<Arma>"
    public Color colorVacio = Color.gray;
    public Color colorLleno = Color.green;
    public Color colorError = Color.red;
    
    [Header("Estado")]
    public int capacidadMaxima = 2; // Cuántos objetos puede guardar
    private int objetosGuardados = 0;
    private System.Collections.Generic.List<GenericObject> objetos = new System.Collections.Generic.List<GenericObject>();

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
            
        spriteRenderer.color = colorVacio;
        
        // Mostrar etiqueta
        if (etiqueta != null)
            etiqueta.text = "Caja<" + tipoAceptado + ">\n0/" + capacidadMaxima;
    }

    public bool IntentarColocar(GenericObject objeto)
    {
        // Si ya está llena, no aceptar más
        if (objetosGuardados >= capacidadMaxima)
        {
            MostrarError();
            return false;
        }

        // VERIFICAR TIPO GENÉRICO
        if (objeto.tipoGenerico == tipoAceptado)
        {
            // ¡CORRECTO! El tipo coincide
            objetosGuardados++;
            objetos.Add(objeto);
            
            // Colocar objeto dentro de la caja (apilar)
            objeto.transform.SetParent(transform);
            float offsetY = (objetosGuardados - 1) * 0.3f; // Apilar verticalmente
            objeto.transform.localPosition = new Vector3(0, offsetY, 0);
            objeto.transform.localScale = Vector3.one * 0.5f; // Hacerlo más pequeño
            objeto.GetComponent<Collider2D>().enabled = false;
            
            // Actualizar color y etiqueta
            if (objetosGuardados >= capacidadMaxima)
                spriteRenderer.color = colorLleno;
            else
                spriteRenderer.color = Color.Lerp(colorVacio, colorLleno, (float)objetosGuardados / capacidadMaxima);
                
            if (etiqueta != null)
                etiqueta.text = "Caja<" + tipoAceptado + ">\n" + objetosGuardados + "/" + capacidadMaxima;
            
            // Notificar al manager
            SimpleGenericManager manager = FindObjectOfType<SimpleGenericManager>();
            if (manager != null)
                manager.VerificarCompletado();
                
            return true;
        }
        else
        {
            // ERROR: Tipo incorrecto
            MostrarError();
            return false;
        }
    }
    
    public bool EstaLlena()
    {
        return objetosGuardados >= capacidadMaxima;
    }

    void MostrarError()
    {
        // Parpadeo rojo
        spriteRenderer.color = colorError;
        Invoke("RestaurarColor", 0.3f);
    }

    void RestaurarColor()
    {
        if (objetosGuardados >= capacidadMaxima)
            spriteRenderer.color = colorLleno;
        else
            spriteRenderer.color = Color.Lerp(colorVacio, colorLleno, (float)objetosGuardados / capacidadMaxima);
    }
}
