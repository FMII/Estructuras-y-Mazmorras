using UnityEngine;
using TMPro;

/// <summary>
/// Manager del puzzle de cajas - verifica si todas están en posición correcta
/// </summary>
public class BoxPuzzleManager : MonoBehaviour
{
    [Header("Referencias")]
    public BoxTarget[] zonasObjetivo; // Las 5 posiciones [0][1][2][3][4]
    public PushableBox[] cajas; // Las 5 cajas
    
    [Header("UI")]
    public TextMeshProUGUI textoInstrucciones;
    public TextMeshProUGUI textoProgreso;
    public GameObject panelVictoria;
    
    [Header("Puerta")]
    public DoorTeleport puertaSalida; // Puerta que se desbloquea
    public GameObject objetoPuerta; // GameObject de la puerta para activar/desactivar
    
    private bool puzzleCompletado = false;
    private int cajasCorrectas = 0;

    void Start()
    {
        if (textoInstrucciones != null)
            textoInstrucciones.text = "Empuja cada caja a su posición correcta en el arreglo [0] [1] [2] [3] [4]";
        
        if (panelVictoria != null)
            panelVictoria.SetActive(false);
            
        // Desactivar puerta al inicio
        if (objetoPuerta != null)
            objetoPuerta.SetActive(false);
            
        ActualizarProgreso();
    }

    public void VerificarSolucion()
    {
        if (puzzleCompletado)
            return;

        cajasCorrectas = 0;

        // Verificar cada zona objetivo
        foreach (BoxTarget zona in zonasObjetivo)
        {
            if (zona.TieneCajaCorrecta())
            {
                cajasCorrectas++;
            }
        }

        ActualizarProgreso();

        // Si todas las cajas están correctas
        if (cajasCorrectas == zonasObjetivo.Length)
        {
            CompletarPuzzle();
        }
    }

    void ActualizarProgreso()
    {
        if (textoProgreso != null)
        {
            textoProgreso.text = $"Cajas correctas: {cajasCorrectas}/{zonasObjetivo.Length}";
        }
    }

    void CompletarPuzzle()
    {
        puzzleCompletado = true;

        if (panelVictoria != null)
            panelVictoria.SetActive(true);

        if (textoProgreso != null)
            textoProgreso.text = "¡Puzzle completado! La puerta está abierta";

        // Activar puerta de salida
        if (objetoPuerta != null)
            objetoPuerta.SetActive(true);

        if (puertaSalida != null)
            puertaSalida.enabled = true;

        // Guardar progreso
        PlayerPrefs.SetInt("PuzzleCajas_Completado", 1);
        PlayerPrefs.Save();

        Debug.Log("¡Puzzle de cajas completado!");
    }

    // Método para resetear el puzzle (opcional)
    public void ResetearPuzzle()
    {
        puzzleCompletado = false;
        cajasCorrectas = 0;

        // Resetear colores de cajas
        foreach (PushableBox caja in cajas)
        {
            caja.MarcarPosicionCorrecta(false);
        }

        if (panelVictoria != null)
            panelVictoria.SetActive(false);

        if (objetoPuerta != null)
            objetoPuerta.SetActive(false);

        ActualizarProgreso();
    }
}
