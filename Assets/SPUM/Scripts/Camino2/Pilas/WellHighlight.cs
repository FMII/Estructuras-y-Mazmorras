using UnityEngine;

public class WellHighlight : MonoBehaviour
{
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Color colorNormal = new Color(0.3f, 0.3f, 0.3f, 0.5f);
    public Color colorIluminado = new Color(0.5f, 1f, 0.5f, 0.8f);
    
    private bool bloqueEncima = false;

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorNormal;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Detectar si hay un bloque siendo arrastrado encima
        StackableBlock bloque = other.GetComponent<StackableBlock>();
        if (bloque != null && !bloque.estaApilado)
        {
            if (!bloqueEncima)
            {
                bloqueEncima = true;
                Iluminar();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Cuando el bloque sale del área
        StackableBlock bloque = other.GetComponent<StackableBlock>();
        if (bloque != null)
        {
            bloqueEncima = false;
            Apagar();
        }
    }

    void Iluminar()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorIluminado;
        }
    }

    void Apagar()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorNormal;
        }
    }
}
