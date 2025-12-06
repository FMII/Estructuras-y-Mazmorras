using UnityEngine;

public class DiamondSlot : MonoBehaviour
{
    [Header("Estado")]
    public bool tieneColor = false;
    public DiamondColor colorActual;
    public int slotIndex;
    
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Sprite spriteVacio;
    public Sprite spriteRojo;
    public Sprite spriteVerde;
    public Sprite spriteAzul;
    
    [Header("Colores")]
    public Color colorRojo = Color.red;
    public Color colorVerde = Color.green;
    public Color colorAzul = Color.blue;
    public Color colorVacio = Color.gray;
    
    private Collider2D col;

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        // Asegurar que tenga collider para detectar clicks
        col = GetComponent<Collider2D>();
        if (col == null)
        {
            col = gameObject.AddComponent<BoxCollider2D>();
            Debug.Log($"Collider agregado automáticamente a {gameObject.name}");
        }
        
        Reiniciar();
    }
    
    void Update()
    {
        // Detectar clic con raycast
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            
            if (hit.collider != null && hit.collider == col)
            {
                CambiarColor();
            }
        }
    }

    public void SetColor(DiamondColor color)
    {
        tieneColor = true;
        colorActual = color;
        ActualizarVisual();
        Debug.Log($"Slot {slotIndex} cambió a color {color}");
    }

    public void Reiniciar()
    {
        tieneColor = false;
        
        if (spriteRenderer != null)
        {
            if (spriteVacio != null)
            {
                spriteRenderer.sprite = spriteVacio;
            }
            spriteRenderer.color = colorVacio;
        }
    }

    void ActualizarVisual()
    {
        if (spriteRenderer == null) return;
        
        switch (colorActual)
        {
            case DiamondColor.Rojo:
                if (spriteRojo != null) spriteRenderer.sprite = spriteRojo;
                spriteRenderer.color = colorRojo;
                break;
            case DiamondColor.Verde:
                if (spriteVerde != null) spriteRenderer.sprite = spriteVerde;
                spriteRenderer.color = colorVerde;
                break;
            case DiamondColor.Azul:
                if (spriteAzul != null) spriteRenderer.sprite = spriteAzul;
                spriteRenderer.color = colorAzul;
                break;
        }
    }

    void CambiarColor()
    {
        Debug.Log($"Click detectado en Slot {slotIndex}");
        
        // Ciclar entre colores al hacer clic
        if (!tieneColor)
        {
            tieneColor = true;
            colorActual = DiamondColor.Rojo;
            ActualizarVisual();
        }
        else
        {
            switch (colorActual)
            {
                case DiamondColor.Rojo:
                    colorActual = DiamondColor.Verde;
                    ActualizarVisual();
                    break;
                case DiamondColor.Verde:
                    colorActual = DiamondColor.Azul;
                    ActualizarVisual();
                    break;
                case DiamondColor.Azul:
                    Reiniciar();
                    // Notificar reinicio al RoomManager
                    RoomManager roomManager2 = RoomManager.GetInstance();
                    if (roomManager2 != null)
                    {
                        roomManager2.ActualizarSecuenciaActualPublica();
                    }
                    return;
            }
        }
        
        // Notificar al RoomManager
        RoomManager roomManager = RoomManager.GetInstance();
        if (roomManager != null)
        {
            roomManager.ActualizarSecuenciaActualPublica();
        }
    }
}
