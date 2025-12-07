using UnityEngine;
using TMPro;

/// <summary>
/// MANAGER SIMPLE: Verifica cuando todas las cajas están llenas
/// </summary>
public class SimpleGenericManager : MonoBehaviour
{
    [Header("Cajas del Puzzle")]
    public GenericContainer[] cajas;
    
    [Header("UI")]
    public TextMeshProUGUI textoProgreso;
    public GameObject puertaSalida; // Se activa al completar
    
    [Header("Explicación")]
    public TextMeshProUGUI textoExplicacion;

    void Start()
    {
        if (textoExplicacion != null)
        {
            textoExplicacion.text = "TIPOS GENÉRICOS:\n" +
                                   "Caja<Arma> solo acepta Armas\n" +
                                   "Caja<Comida> solo acepta Comida\n" +
                                   "Caja<Tesoro> solo acepta Tesoros\n\n" +
                                   "Presiona E para recoger y colocar";
        }
        
        if (puertaSalida != null)
            puertaSalida.SetActive(false);
            
        ActualizarProgreso();
    }

    public void VerificarCompletado()
    {
        int cajasLlenas = 0;
        
        foreach (GenericContainer caja in cajas)
        {
            if (caja.EstaLlena())
                cajasLlenas++;
        }
        
        ActualizarProgreso();
        
        // Si todas están llenas, ganar
        if (cajasLlenas == cajas.Length)
        {
            CompletarPuzzle();
        }
    }

    void ActualizarProgreso()
    {
        if (textoProgreso != null)
        {
            int llenas = 0;
            foreach (GenericContainer caja in cajas)
            {
                if (caja.EstaLlena())
                    llenas++;
            }
            textoProgreso.text = "Cajas completadas: " + llenas + "/" + cajas.Length;
        }
    }

    void CompletarPuzzle()
    {
        if (textoProgreso != null)
            textoProgreso.text = "¡PUZZLE COMPLETADO!\nTodos los tipos coinciden correctamente";
            
        if (puertaSalida != null)
            puertaSalida.SetActive(true);
    }
}
