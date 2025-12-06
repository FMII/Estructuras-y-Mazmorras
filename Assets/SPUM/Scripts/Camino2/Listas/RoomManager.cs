using UnityEngine;
using System.Collections.Generic;

public enum DiamondColor
{
    Rojo,
    Verde,
    Azul
}

public class RoomManager : MonoBehaviour
{
    [Header("Configuración de Secuencia")]
    public List<DiamondColor> secuenciaCorrecta = new List<DiamondColor>();
    public int cantidadDiamantes = 6;
    
    [Header("Referencias de Diamantes en Pared")]
    public List<DiamondSlot> slotsEnPared = new List<DiamondSlot>();
    
    [Header("Referencias UI")]
    public GameObject botonConfirmar;
    
    [Header("Debug")]
    public List<DiamondColor> secuenciaActual = new List<DiamondColor>();
    public bool puzzleCompletado = false;
    
    private static RoomManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Inicializar la secuencia correcta predefinida
        secuenciaCorrecta = new List<DiamondColor>
        {
            DiamondColor.Rojo,
            DiamondColor.Azul,
            DiamondColor.Verde,
            DiamondColor.Azul,
            DiamondColor.Rojo,
            DiamondColor.Verde
        };
        
        // Inicializar la secuencia actual vacía
        secuenciaActual.Clear();
        
        // Ocultar botón de confirmar al inicio
        if (botonConfirmar != null)
        {
            botonConfirmar.SetActive(false);
        }
        
        Debug.Log($"Secuencia correcta establecida: {string.Join(", ", secuenciaCorrecta)}");
    }

    // Llamar cuando el jugador toca/ve un nodo de color
    public void RegistrarNodo(DiamondColor color)
    {
        // Solo registrar para que el jugador vea, no para validación
        Debug.Log($"Nodo {color} visto por el jugador");
    }

    // Llamar cuando el jugador coloca un diamante en la pared
    public void ColocarDiamanteEnSlot(int slotIndex, DiamondColor color)
    {
        if (slotIndex >= 0 && slotIndex < slotsEnPared.Count)
        {
            slotsEnPared[slotIndex].SetColor(color);
            ActualizarSecuenciaActual();
        }
    }
    
    // Método público para actualizar sin cambiar el slot
    public void ActualizarSecuenciaActualPublica()
    {
        ActualizarSecuenciaActual();
    }

    void ActualizarSecuenciaActual()
    {
        secuenciaActual.Clear();
        
        foreach (var slot in slotsEnPared)
        {
            if (slot.tieneColor)
            {
                secuenciaActual.Add(slot.colorActual);
            }
        }
        
        // Mostrar/ocultar botón según si hay 6 diamantes
        if (botonConfirmar != null)
        {
            if (secuenciaActual.Count == cantidadDiamantes)
            {
                botonConfirmar.SetActive(true);
                Debug.Log("Secuencia completa. Presiona el botón para confirmar.");
            }
            else
            {
                botonConfirmar.SetActive(false);
            }
        }
        
        Debug.Log($"Secuencia actual: {string.Join(", ", secuenciaActual)} ({secuenciaActual.Count}/{cantidadDiamantes})");
    }
    
    // Método público para llamar desde el botón UI
    public void ConfirmarSecuencia()
    {
        if (secuenciaActual.Count == cantidadDiamantes && !puzzleCompletado)
        {
            VerificarSecuencia();
        }
    }

    void VerificarSecuencia()
    {
        bool esCorrecta = true;
        
        for (int i = 0; i < cantidadDiamantes; i++)
        {
            if (i >= secuenciaActual.Count || i >= secuenciaCorrecta.Count || 
                secuenciaActual[i] != secuenciaCorrecta[i])
            {
                esCorrecta = false;
                break;
            }
        }
        
        if (esCorrecta)
        {
            Debug.Log("¡SECUENCIA CORRECTA! Puerta abierta.");
            AbrirPuerta();
        }
        else
        {
            Debug.LogWarning("Secuencia incorrecta. Reiniciando...");
            ReiniciarPuzzle();
        }
    }

    void AbrirPuerta()
    {
        puzzleCompletado = true;
        
        // Ocultar botón
        if (botonConfirmar != null)
        {
            botonConfirmar.SetActive(false);
        }
        
        // Aquí puedes abrir una puerta, activar un teleport, etc.
        Debug.Log("Puzzle completado correctamente");
    }

    void ReiniciarPuzzle()
    {
        secuenciaActual.Clear();
        
        // Ocultar botón
        if (botonConfirmar != null)
        {
            botonConfirmar.SetActive(false);
        }
        
        foreach (var slot in slotsEnPared)
        {
            slot.Reiniciar();
        }
        
        Invoke("MostrarMensajeReinicio", 1f);
    }

    void MostrarMensajeReinicio()
    {
        Debug.Log("Intenta de nuevo...");
    }

    public static RoomManager GetInstance()
    {
        return instance;
    }
}
