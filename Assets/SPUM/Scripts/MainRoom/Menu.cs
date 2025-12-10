using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Menú persistente que sobrevive cambios de escena
/// </summary>
public class Menu : MonoBehaviour
{
    public static Menu Instance { get; private set; }
    
    [Header("Referencias")]
    public Button botonRecargar;
    public Button botonSalir;
    
    [Header("Configuración")]
    public KeyCode teclaMenu = KeyCode.Escape;
    public bool iniciarOculto = true;
    
    private bool menuActivo = false;
    private Canvas canvas;

    void Awake()
    {
        // Singleton pattern - solo una instancia
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Obtener el Canvas
        canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No se encontró Canvas en el GameObject!");
        }
        
        // Configurar botones
        if (botonRecargar != null)
            botonRecargar.onClick.AddListener(RecargarEscena);
        
        if (botonSalir != null)
            botonSalir.onClick.AddListener(IrAEscenaPrincipal);
        
        // Ocultar menú al inicio
        if (iniciarOculto)
        {
            gameObject.SetActive(false);
            menuActivo = false;
        }
    }

    void Update()
    {
        // Alternar menú con tecla ESC
        if (Input.GetKeyDown(teclaMenu))
        {
            AlternarMenu();
        }
    }

    /// <summary>
    /// Mostrar u ocultar el menú
    /// </summary>
    public void AlternarMenu()
    {
        menuActivo = !menuActivo;
        gameObject.SetActive(menuActivo);
        
        // Pausar el juego cuando el menú está activo
        Time.timeScale = menuActivo ? 0f : 1f;
        
        Debug.Log($"Menú {(menuActivo ? "abierto" : "cerrado")}");
    }

    /// <summary>
    /// Mostrar el menú
    /// </summary>
    public void MostrarMenu()
    {
        menuActivo = true;
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Ocultar el menú
    /// </summary>
    public void OcultarMenu()
    {
        menuActivo = false;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Recargar la escena actual
    /// </summary>
    public void RecargarEscena()
    {
        Time.timeScale = 1f; // Reanudar antes de recargar
        Scene escenaActual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(escenaActual.buildIndex);
        Debug.Log($"Recargando escena: {escenaActual.name}");
    }

    /// <summary>
    /// Ir a la escena principal (índice 0)
    /// </summary>
    public void IrAEscenaPrincipal()
    {
        Time.timeScale = 1f; // Reanudar antes de cambiar
        SceneManager.LoadScene(0);
        Debug.Log("Cargando escena principal (índice 0)");
    }

    /// <summary>
    /// Cerrar el juego (solo funciona en build)
    /// </summary>
    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
