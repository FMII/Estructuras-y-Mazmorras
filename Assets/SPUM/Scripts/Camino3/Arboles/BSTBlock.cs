using UnityEngine;

/// <summary>
/// Bloque simple con un color que el jugador puede identificar
/// Puede ser arrastrado haciendo click sostenido usando Raycast
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class BSTBlock : MonoBehaviour
{
    public enum ColorBloque { Azul, Rojo, Verde, Amarillo, Morado }
    
    [Header("Configuración")]
    public ColorBloque colorBloque;
    
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    
    [Header("Arrastre")]
    public bool puedeSerArrastrado = true;
    public float zPosicionArrastre = -1f;
    private bool estaArrastrando = false;
    private Vector3 offsetArrastre;
    private Camera camara;
    private Vector3 posicionInicial;
    private float zInicial;
    private Collider2D col;
    
    // Para raycast
    private bool mousePresionadoSobreEsteBloque = false;

    void Start()
    {
        AplicarColor();
        camara = Camera.main;
        posicionInicial = transform.position;
        zInicial = transform.position.z;
        
        col = GetComponent<Collider2D>();
        if (col == null)
        {
            col = gameObject.AddComponent<BoxCollider2D>();
            Debug.LogWarning($"Se agregó BoxCollider2D automáticamente a {gameObject.name}");
        }
        
        // Puede ser trigger o no, funciona con ambos
        Debug.Log($"Bloque {colorBloque} inicializado en {posicionInicial}");
    }

    void Update()
    {
        if (!puedeSerArrastrado) return;
        
        Vector3 posicionMouse = ObtenerPosicionMouse();
        
        // Detectar click inicial con raycast
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(posicionMouse, Vector2.zero);
            
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Click sobre este bloque
                mousePresionadoSobreEsteBloque = true;
                estaArrastrando = true;
                offsetArrastre = transform.position - posicionMouse;
                offsetArrastre.z = 0;
                
                Debug.Log($"🖱️ Raycast detectó click en bloque {colorBloque}");
            }
        }
        
        // Arrastrando
        if (Input.GetMouseButton(0) && mousePresionadoSobreEsteBloque && estaArrastrando)
        {
            Vector3 nuevaPos = posicionMouse + offsetArrastre;
            nuevaPos.z = zPosicionArrastre;
            transform.position = nuevaPos;
        }
        
        // Soltar
        if (Input.GetMouseButtonUp(0) && mousePresionadoSobreEsteBloque)
        {
            mousePresionadoSobreEsteBloque = false;
            
            if (estaArrastrando)
            {
                estaArrastrando = false;
                
                // Restaurar Z
                Vector3 pos = transform.position;
                pos.z = zInicial;
                transform.position = pos;
                
                Debug.Log($"🖱️ Soltando bloque {colorBloque} en {transform.position}");
                
                // Buscar zona cercana
                BSTDeliveryZone zona = EncontrarZonaCercana();
                
                if (zona != null)
                {
                    Debug.Log($"📦 Bloque {colorBloque} soltado en zona {zona.colorEsperado}");
                    zona.RecibirBloque(this);
                }
                else
                {
                    Debug.Log($"↩️ No hay zona cerca, regresando a posición inicial");
                    RegresarAPosicionInicial();
                }
            }
        }
    }

    Vector3 ObtenerPosicionMouse()
    {
        if (camara == null) camara = Camera.main;
        
        Vector3 posMouse = Input.mousePosition;
        posMouse.z = Mathf.Abs(camara.transform.position.z - zInicial);
        return camara.ScreenToWorldPoint(posMouse);
    }

    BSTDeliveryZone EncontrarZonaCercana()
    {
        BSTDeliveryZone[] zonas = FindObjectsOfType<BSTDeliveryZone>();
        float distanciaMinima = 2f;
        BSTDeliveryZone zonaMasCercana = null;
        float menorDistancia = distanciaMinima;
        
        foreach (BSTDeliveryZone zona in zonas)
        {
            float distancia = Vector2.Distance(
                new Vector2(transform.position.x, transform.position.y), 
                new Vector2(zona.transform.position.x, zona.transform.position.y)
            );
            
            Debug.Log($"Distancia a zona {zona.colorEsperado}: {distancia:F2}");
            
            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                zonaMasCercana = zona;
            }
        }
        
        if (zonaMasCercana != null)
        {
            Debug.Log($"✓ Zona más cercana: {zonaMasCercana.colorEsperado} a {menorDistancia:F2} unidades");
        }
        
        return zonaMasCercana;
    }

    public void RegresarAPosicionInicial()
    {
        transform.position = posicionInicial;
        Debug.Log($"↩️ Bloque {colorBloque} regresado a {posicionInicial}");
    }

    public void BloquearArrastre()
    {
        puedeSerArrastrado = false;
        estaArrastrando = false;
        mousePresionadoSobreEsteBloque = false;
        Debug.Log($"🔒 Bloque {colorBloque} bloqueado");
    }
    
    void AplicarColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = ObtenerColor();
        }
    }
    
    Color ObtenerColor()
    {
        return colorBloque switch
        {
            ColorBloque.Azul => Color.blue,
            ColorBloque.Rojo => Color.red,
            ColorBloque.Verde => Color.green,
            ColorBloque.Amarillo => Color.yellow,
            ColorBloque.Morado => new Color(0.5f, 0f, 0.5f),
            _ => Color.white
        };
    }

    void OnDrawGizmos()
    {
        Gizmos.color = ObtenerColor();
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.5f);
        
        if (puedeSerArrastrado)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.6f, 0.1f);
        }
    }
}
