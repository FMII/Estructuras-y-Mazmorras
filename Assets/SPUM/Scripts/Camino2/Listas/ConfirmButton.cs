using UnityEngine;

public class ConfirmButton : MonoBehaviour
{
    [Header("Referencias")]
    public RoomManager roomManager;
    
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Color colorNormal = Color.white;
    public Color colorHover = Color.yellow;
    public Color colorClick = Color.green;
    
    private Collider2D col;
    private bool estaActivo = false;

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        if (roomManager == null)
        {
            roomManager = RoomManager.GetInstance();
        }
        
        // Asegurar que tenga collider
        col = GetComponent<Collider2D>();
        if (col == null)
        {
            col = gameObject.AddComponent<BoxCollider2D>();
        }
        
        // Ocultar al inicio
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!estaActivo) return;
        
        // Detectar hover
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        
        if (hit.collider == col)
        {
            // Mouse sobre el botón
            if (spriteRenderer != null)
            {
                spriteRenderer.color = colorHover;
            }
            
            // Detectar click
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
        }
        else
        {
            // Mouse fuera del botón
            if (spriteRenderer != null)
            {
                spriteRenderer.color = colorNormal;
            }
        }
    }

    void OnClick()
    {
        Debug.Log("Botón CONFIRMAR presionado");
        
        // Efecto visual de click
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorClick;
        }
        
        // Llamar al RoomManager
        if (roomManager != null)
        {
            roomManager.ConfirmarSecuencia();
        }
        
        // Volver al color normal
        Invoke("ResetColor", 0.2f);
    }

    void ResetColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorNormal;
        }
    }

    void OnEnable()
    {
        estaActivo = true;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorNormal;
        }
    }

    void OnDisable()
    {
        estaActivo = false;
    }
}
