using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PersistentHealthUI : MonoBehaviour
{
    public static PersistentHealthUI Instance { get; private set; }
    
    [Header("Referencias UI")]
    public Image barraVidaFill; // La imagen que se llenará
    public TextMeshProUGUI textoVida;
    
    private HealthSystem playerHealth;
    
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
        BuscarJugador();
    }
    
    void Update()
    {
        // Rebuscar al jugador si no existe (cambio de escena)
        if (playerHealth == null || playerHealth.gameObject == null)
        {
            BuscarJugador();
        }
        
        // Actualizar UI
        if (playerHealth != null)
        {
            ActualizarBarraVida();
        }
    }
    
    void BuscarJugador()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                ActualizarBarraVida();
            }
        }
    }
    
    void ActualizarBarraVida()
    {
        float porcentaje = playerHealth.GetPorcentajeVida();
        
        if (barraVidaFill != null)
        {
            barraVidaFill.fillAmount = porcentaje;
            barraVidaFill.transform.SetAsLastSibling(); // Asegurar que esté al frente
            Debug.Log($"Barra actualizada: {playerHealth.vidaActual}/{playerHealth.vidaMaxima} = {porcentaje}, fillAmount={barraVidaFill.fillAmount}");
        }
        
        if (textoVida != null)
        {
            textoVida.text = $"{playerHealth.vidaActual}/{playerHealth.vidaMaxima}";
        }
    }
    
    public void MostrarUI(bool mostrar)
    {
        gameObject.SetActive(mostrar);
    }
}
