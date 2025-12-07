using UnityEngine;

/// <summary>
/// OBJETO SIMPLE: Solo tiene un tipo (Arma, Comida, Tesoro)
/// </summary>
public class GenericObject : MonoBehaviour
{
    [Header("¿Qué tipo de objeto es?")]
    public string tipoGenerico = "Arma"; // Cambia a "Comida" o "Tesoro"
    
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    
    private bool jugadorCerca = false;
    private bool recogido = false;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !recogido)
        {
            jugadorCerca = true;
            // Resaltar amarillo cuando está cerca
            spriteRenderer.color = Color.yellow;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (!recogido)
                spriteRenderer.color = Color.white;
        }
    }

    void Update()
    {
        // Presiona E para recoger
        if (jugadorCerca && !recogido && Input.GetKeyDown(KeyCode.E))
        {
            Recoger();
        }
    }

    void Recoger()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            SimpleGenericPlayer playerScript = player.GetComponent<SimpleGenericPlayer>();
            if (playerScript != null && !playerScript.TieneObjeto())
            {
                recogido = true;
                playerScript.RecogerObjeto(this);
                
                // Pegarse al jugador
                transform.SetParent(player.transform);
                transform.localPosition = new Vector3(0.5f, 0.5f, 0);
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    public void Soltar()
    {
        recogido = false;
        transform.SetParent(null);
        GetComponent<Collider2D>().enabled = true;
        spriteRenderer.color = Color.white;
    }
}
