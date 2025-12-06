using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SalaTrigger : MonoBehaviour
{
    [Header("Configuración de Sala")]
    public int salaID;
    
    [Header("Configuración")]
    public bool soloUnaVez = false;
    
    private bool yaActivada = false;

    void Start()
    {
        // Verificar que este objeto tenga un trigger
        Collider2D col = GetComponent<Collider2D>();
        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning($"El collider en {gameObject.name} NO es trigger. Activándolo automáticamente.");
            col.isTrigger = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si es el jugador
        if (other.CompareTag("Player"))
        {
            if (!soloUnaVez || !yaActivada)
            {
                GuardarSalaID();
                
                if (soloUnaVez)
                {
                    yaActivada = true;
                }
            }
        }
    }

    void GuardarSalaID()
    {
        PlayerPrefs.SetInt("sala_id", salaID);
        PlayerPrefs.Save();
        Debug.Log($"Sala ID guardado: {salaID}");
    }

    void OnDrawGizmos()
    {
        // Visualizar el área del trigger
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.color = new Color(0, 0, 1, 0.3f); // Azul semi-transparente
            
            if (col is BoxCollider2D)
            {
                BoxCollider2D boxCol = col as BoxCollider2D;
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawCube(boxCol.offset, boxCol.size);
            }
            else if (col is CircleCollider2D)
            {
                CircleCollider2D circleCol = col as CircleCollider2D;
                Gizmos.DrawSphere(transform.position + (Vector3)circleCol.offset, circleCol.radius);
            }
        }

    }
}
