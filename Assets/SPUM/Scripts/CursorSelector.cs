using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Selector de cursor persistente que sobrevive cambios de escena
/// Se puede usar en UI o llamar desde código
/// </summary>
public class CursorSelector : MonoBehaviour, IPointerClickHandler
{
    public static CursorSelector Instance { get; private set; }
    
    [Header("Cursor por Defecto")]
    public Sprite defaultCursorSprite;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;
    
    [Header("Configuración")]
    public bool aplicarAlIniciar = true;
    public bool persistirEntreEscenas = true;
    
    private Texture2D currentTexture;
    
    void Awake()
    {
        // Singleton pattern si se quiere persistencia
        if (persistirEntreEscenas)
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        if (aplicarAlIniciar)
        {
            AplicarCursorPorDefecto();
        }
    }
    
    void AplicarCursorPorDefecto()
    {
        if (defaultCursorSprite != null)
        {
            SetCursor(defaultCursorSprite, hotspot);
        }
    }
    
    /// <summary>
    /// Llamado cuando se hace click en el elemento UI
    /// Usa el sprite asignado en cursorSprite de este GameObject
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        Sprite spriteEnBoton = GetComponent<UnityEngine.UI.Image>()?.sprite;
        if (spriteEnBoton != null)
        {
            SetCursor(spriteEnBoton, hotspot);
        }
    }
    
    /// <summary>
    /// Establece el cursor desde un Sprite
    /// </summary>
    public void SetCursor(Sprite sprite, Vector2 hotspotOffset)
    {
        if (sprite == null)
        {
            Debug.LogWarning("Sprite del cursor es null");
            return;
        }
        
        Texture2D texture = SpriteToTexture2D(sprite);
        SetCursor(texture, hotspotOffset);
    }
    
    /// <summary>
    /// Establece el cursor desde una Texture2D
    /// </summary>
    public void SetCursor(Texture2D texture, Vector2 hotspotOffset)
    {
        if (texture == null)
        {
            Debug.LogWarning("Texture del cursor es null");
            return;
        }
        
        currentTexture = texture;
        Cursor.SetCursor(texture, hotspotOffset, cursorMode);
        Debug.Log($"Cursor cambiado: {texture.name}, hotspot: {hotspotOffset}");
    }
    
    /// <summary>
    /// Restaura el cursor por defecto del sistema
    /// </summary>
    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
        currentTexture = null;
        Debug.Log("Cursor reseteado al sistema");
    }
    
    /// <summary>
    /// Convierte un Sprite a Texture2D
    /// </summary>
    private Texture2D SpriteToTexture2D(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            // El sprite es parte de un atlas, necesitamos extraer la región
            Texture2D newTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] pixels = sprite.texture.GetPixels(
                (int)sprite.rect.x,
                (int)sprite.rect.y,
                (int)sprite.rect.width,
                (int)sprite.rect.height
            );
            newTexture.SetPixels(pixels);
            newTexture.Apply();
            return newTexture;
        }
        else
        {
            return sprite.texture;
        }
    }
    
    /// <summary>
    /// Obtiene la textura actual del cursor
    /// </summary>
    public Texture2D GetCurrentTexture()
    {
        return currentTexture;
    }
}
