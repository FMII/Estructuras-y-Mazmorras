using UnityEngine;
using System.Collections.Generic;

public enum BlockColor
{
    Amarillo,
    Cafe,
    Verde,
    Azul
}

public class RoomStackManager : MonoBehaviour
{
    [Header("Configuración de Pila")]
    public List<BlockColor> ordenCorrecto = new List<BlockColor>();
    public Stack<BlockColor> pilaActual = new Stack<BlockColor>();
    
    [Header("Referencias")]
    public Transform pozo;
    public List<Platform> plataformas = new List<Platform>();
    
    [Header("Posiciones de Bloques en Pila")]
    public Transform puntoBase; // Posición del primer bloque
    public float alturaBloque = 1f; // Altura entre bloques
    
    [Header("Prefabs de Bloques")]
    public GameObject prefabBloqueVerde;
    public GameObject prefabBloqueAzul;
    public GameObject prefabBloqueCafe;
    public GameObject prefabBloqueAmarillo;
    
    [Header("Posiciones Iniciales")]
    public Transform puntoSpawnVerde;
    public Transform puntoSpawnAzul;
    public Transform puntoSpawnCafe;
    public Transform puntoSpawnAmarillo;
    
    [Header("Debug")]
    public List<BlockColor> pilaVisual = new List<BlockColor>();
    
    private static RoomStackManager instance;
    private int bloquesCorrectos = 0;
    private List<GameObject> bloquesActuales = new List<GameObject>();

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
        // Orden correcto: Verde, Azul, Cafe, Amarillo (desde abajo hacia arriba)
        ordenCorrecto = new List<BlockColor>
        {
            BlockColor.Verde,
            BlockColor.Azul,
            BlockColor.Cafe,
            BlockColor.Amarillo
        };
        
        pilaActual.Clear();
        bloquesCorrectos = 0;
        
        Debug.Log($"Orden correcto de pila (abajo→arriba): {string.Join(", ", ordenCorrecto)}");
        
        // Generar bloques iniciales
        GenerarBloques();
    }

    // Llamar cuando un bloque es lanzado al pozo
    public void ApilarBloque(BlockColor color, GameObject bloqueObj)
    {
        // Agregar a la pila
        pilaActual.Push(color);
        pilaVisual.Add(color);
        
        int posicionEnPila = pilaActual.Count - 1;
        
        Debug.Log($"Bloque {color} apilado en posición {posicionEnPila}. Pila: {string.Join(", ", pilaVisual)}");
        
        // Verificar si está en el orden correcto
        if (posicionEnPila < ordenCorrecto.Count && ordenCorrecto[posicionEnPila] == color)
        {
            // ¡Correcto!
            bloquesCorrectos++;
            Debug.Log($"✓ Bloque {color} en posición correcta ({bloquesCorrectos}/4)");
            
            // Activar la plataforma correspondiente
            if (posicionEnPila < plataformas.Count)
            {
                plataformas[posicionEnPila].ActivarPlataforma();
            }
            
            // Posicionar el bloque en la pila
            PosicionarBloqueEnPila(bloqueObj, posicionEnPila);
            
            // Verificar si completó la pila
            if (bloquesCorrectos == 4)
            {
                PuzzleCompletado();
            }
        }
        else
        {
            // ¡Incorrecto!
            Debug.LogWarning($"✗ Bloque {color} en posición INCORRECTA. Se esperaba {ordenCorrecto[posicionEnPila]}");
            ReiniciarPuzzle();
        }
    }

    void PosicionarBloqueEnPila(GameObject bloque, int posicion)
    {
        if (puntoBase == null) return;
        
        // Calcular posición en la pila
        Vector3 posicionFinal = puntoBase.position + Vector3.up * (posicion * alturaBloque);
        
        // Mover el bloque
        bloque.transform.position = posicionFinal;
        
        // Desactivar física del bloque
        Rigidbody2D rb = bloque.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    void PuzzleCompletado()
    {
        Debug.Log("========== ¡PUZZLE DE PILA COMPLETADO! ==========");
        Debug.Log("Todas las plataformas están activas. ¡Puedes pasar!");
    }

    void ReiniciarPuzzle()
    {
        Debug.Log("❌ ORDEN INCORRECTO - Reiniciando puzzle...");
        
        // Desactivar todas las plataformas
        foreach (var plataforma in plataformas)
        {
            plataforma.DesactivarPlataforma();
        }
        
        // Limpiar pila
        pilaActual.Clear();
        pilaVisual.Clear();
        bloquesCorrectos = 0;
        
        // Destruir todos los bloques
        foreach (var bloque in bloquesActuales)
        {
            if (bloque != null)
            {
                Destroy(bloque);
            }
        }
        bloquesActuales.Clear();
        
        // Regenerar bloques después de un momento
        Invoke("GenerarBloques", 1f);
        Invoke("MostrarMensajeReinicio", 1f);
    }

    void GenerarBloques()
    {
        // Generar bloque verde
        if (prefabBloqueVerde != null && puntoSpawnVerde != null)
        {
            GameObject bloqueVerde = Instantiate(prefabBloqueVerde, puntoSpawnVerde.position, Quaternion.identity);
            bloquesActuales.Add(bloqueVerde);
        }
        
        // Generar bloque azul
        if (prefabBloqueAzul != null && puntoSpawnAzul != null)
        {
            GameObject bloqueAzul = Instantiate(prefabBloqueAzul, puntoSpawnAzul.position, Quaternion.identity);
            bloquesActuales.Add(bloqueAzul);
        }
        
        // Generar bloque cafe
        if (prefabBloqueCafe != null && puntoSpawnCafe != null)
        {
            GameObject bloqueCafe = Instantiate(prefabBloqueCafe, puntoSpawnCafe.position, Quaternion.identity);
            bloquesActuales.Add(bloqueCafe);
        }
        
        // Generar bloque amarillo
        if (prefabBloqueAmarillo != null && puntoSpawnAmarillo != null)
        {
            GameObject bloqueAmarillo = Instantiate(prefabBloqueAmarillo, puntoSpawnAmarillo.position, Quaternion.identity);
            bloquesActuales.Add(bloqueAmarillo);
        }
        
        Debug.Log("Bloques regenerados");
    }

    void MostrarMensajeReinicio()
    {
        Debug.Log("Intenta apilar los bloques de nuevo en el orden correcto: Amarillo → Cafe → Verde → Azul");
    }

    public static RoomStackManager GetInstance()
    {
        return instance;
    }
}
