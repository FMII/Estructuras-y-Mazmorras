using UnityEngine;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Valida que el jugador entregue los bloques en el orden correcto
/// Práctica de búsqueda en BST: primero el azul, luego el rojo
/// </summary>
public class BSTSearchValidator : MonoBehaviour
{
    [Header("Configuración de Misiones")]
    public BSTBlock.ColorBloque primerColor = BSTBlock.ColorBloque.Azul;  // Primera búsqueda
    public BSTBlock.ColorBloque segundoColor = BSTBlock.ColorBloque.Rojo; // Segunda búsqueda
    
    [Header("Estado")]
    public int misionActual = 1; // 1 o 2
    private List<BSTBlock.ColorBloque> coloresEntregados = new List<BSTBlock.ColorBloque>();
    
    [Header("UI")]
    public TextMeshProUGUI textoMision;
    public TextMeshProUGUI textoProgreso;
    public TextMeshProUGUI textoFeedback;
    
    [Header("Referencias a Bloques (para marcar)")]
    public BSTBlock[] todosLosBloques;
    
    [Header("Teleport al Completar")]
    public Vector2 posicionFinal = new Vector2(104f, -1f);
    private Transform jugador;

    void Start()
    {
        // Buscar al jugador
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }
        
        ActualizarUI();
        MarcarBloqueObjetivo();
    }

    /// <summary>
    /// Valida si el color entregado es correcto según la misión actual
    /// </summary>
    public bool ValidarEntrega(BSTBlock.ColorBloque color)
    {
        bool esValido = false;
        
        Debug.Log($"🔍 Validando entrega: Color={color}, MisiónActual={misionActual}, Esperado={( misionActual == 1 ? primerColor : segundoColor)}");
        
        if (misionActual == 1)
        {
            // Primera misión: debe entregar el azul
            if (color == primerColor)
            {
                esValido = true;
                coloresEntregados.Add(color);
                
                MostrarFeedback($"✅ ¡Correcto! Encontraste el bloque {primerColor} (búsqueda en preorden - izquierda)", Color.green);
                
                // Avanzar a la siguiente misión
                Invoke("AvanzarMision2", 2f);
            }
            else
            {
                MostrarFeedback($"❌ Incorrecto. Debes buscar el bloque {primerColor} primero (recibiste {color})", Color.red);
            }
        }
        else if (misionActual == 2)
        {
            // Segunda misión: debe entregar el rojo
            if (color == segundoColor)
            {
                esValido = true;
                coloresEntregados.Add(color);
                
                MostrarFeedback($"✅ ¡Excelente! Encontraste el bloque {segundoColor} (búsqueda en postorden - derecha)", Color.green);
                
                // Completar el juego
                Invoke("CompletarJuego", 2f);
            }
            else
            {
                MostrarFeedback($"❌ Incorrecto. Ahora debes buscar el bloque {segundoColor} (recibiste {color})", Color.red);
            }
        }
        
        Debug.Log($"Resultado validación: {(esValido ? "✅ VÁLIDO" : "❌ NO VÁLIDO")}");
        return esValido;
    }

    void AvanzarMision2()
    {
        misionActual = 2;
        ActualizarUI();
        MarcarBloqueObjetivo();
    }

    void CompletarJuego()
    {
        MostrarFeedback("🎉 ¡JUEGO COMPLETADO! Has practicado búsqueda en BST correctamente", Color.cyan);
        ActualizarUI();
        
        // Mover al jugador a la posición final
        if (jugador != null)
        {
            jugador.position = new Vector3(posicionFinal.x, posicionFinal.y, jugador.position.z);
            Debug.Log($"Jugador movido a posición final: {posicionFinal}");
        }
        else
        {
            Debug.LogWarning("No se encontró el jugador para moverlo");
        }
    }

    void ActualizarUI()
    {
        if (textoMision != null)
        {
            if (misionActual == 1)
            {
                textoMision.text = $"MISIÓN 1: Busca el bloque {primerColor}\n(Recorre el árbol navegando por las salas)";
            }
            else if (misionActual == 2)
            {
                textoMision.text = $"MISIÓN 2: Busca el bloque {segundoColor}\n(Usa postorden - más a la derecha)";
            }
            else
            {
                textoMision.text = "✅ ¡Todas las misiones completadas!";
            }
        }
        
        if (textoProgreso != null)
        {
            if (coloresEntregados.Count > 0)
                textoProgreso.text = $"Bloques encontrados: {string.Join(", ", coloresEntregados)}";
            else
                textoProgreso.text = "Bloques encontrados: ninguno";
        }
    }

    void MostrarFeedback(string mensaje, Color color)
    {
        if (textoFeedback != null)
        {
            textoFeedback.text = mensaje;
            textoFeedback.color = color;
        }
        
        Debug.Log(mensaje);
    }

    /// <summary>
    /// Marca visualmente el bloque objetivo actual (opcional)
    /// </summary>
    void MarcarBloqueObjetivo()
    {
        if (todosLosBloques == null || todosLosBloques.Length == 0)
        {
            // Buscar todos los bloques en la escena
            todosLosBloques = FindObjectsOfType<BSTBlock>();
        }
        
        BSTBlock.ColorBloque colorBuscado = misionActual == 1 ? primerColor : segundoColor;
        
        foreach (BSTBlock bloque in todosLosBloques)
        {
            if (bloque.colorBloque == colorBuscado)
            {
                Debug.Log($"Bloque {colorBuscado} es el objetivo de esta misión");
                // Los bloques ya tienen su color aplicado automáticamente
            }
        }
    }

    /// <summary>
    /// Reinicia el juego (para testing)
    /// </summary>
    public void ReiniciarJuego()
    {
        misionActual = 1;
        coloresEntregados.Clear();
        ActualizarUI();
        MarcarBloqueObjetivo();
    }
}
