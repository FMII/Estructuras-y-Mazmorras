using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class RoomEnemyManager : MonoBehaviour
{
    [Header("Generadores")]
    public List<EnemyGenerator> generadores = new List<EnemyGenerator>();
    
    [Header("Configuración")]
    public float tiempoEntreGeneraciones = 10f;
    public float tiempoEntreOlas = 6f;
    public int numeroDeOlas = 5;
    public bool generarAlInicio = true;
    public GameObject playerReferencia;
    
    [Header("Eventos")]
    public UnityEvent onOlaCompletada;
    public UnityEvent onTodasLasOlasCompletadas;
    public UnityEvent onOrdenIncorrecto;
    
    [Header("Estado (Debug)")]
    public List<int> ordenGeneracion = new List<int>();
    public int siguienteOrdenEsperado = 0;
    public int enemigosGenerados = 0;
    public int olaActual = 0;
    public bool salaCompletada = false;
    
    private bool generacionIniciada = false;

    void Start()
    {
        // Encontrar todos los generadores hijos si la lista está vacía
        if (generadores.Count == 0)
        {
            generadores = GetComponentsInChildren<EnemyGenerator>().ToList();
        }

        if (generarAlInicio)
        {
            IniciarGeneracion();
        }
    }

    public void IniciarGeneracion()
    {
        if (generacionIniciada)
            return;

        generacionIniciada = true;
        olaActual = 0;
        salaCompletada = false;
        
        IniciarNuevaOla();
    }

    void IniciarNuevaOla()
    {
        olaActual++;
        Debug.Log($"========== INICIANDO OLA {olaActual}/{numeroDeOlas} ==========");
        
        // Resetear generadores para la nueva ola
        foreach (var gen in generadores)
        {
            gen.yaGenero = false;
            Debug.Log($"Reseteando generador: {gen.gameObject.name}");
        }
        
        ordenGeneracion.Clear();
        
        // Crear una lista aleatoria del orden de generación (0 a 4 para 5 generadores)
        List<int> indices = Enumerable.Range(0, generadores.Count).ToList();
        
        // Mezclar aleatoriamente
        for (int i = 0; i < indices.Count; i++)
        {
            int temp = indices[i];
            int randomIndex = Random.Range(i, indices.Count);
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        ordenGeneracion = indices;
        siguienteOrdenEsperado = 0;
        enemigosGenerados = 0;

        Debug.Log($"Ola {olaActual} - Orden de generación: {string.Join(", ", ordenGeneracion)}");
        
        // Iniciar la generación en cadena
        GenerarSiguienteEnemigo();
    }

    void GenerarSiguienteEnemigo()
    {
        if (enemigosGenerados >= generadores.Count)
        {
            Debug.Log("Todos los enemigos han sido generados. Esperando que mueran en orden...");
            return;
        }

        int indiceGenerador = ordenGeneracion[enemigosGenerados];
        int ordenActual = enemigosGenerados;

        if (indiceGenerador < generadores.Count)
        {
            generadores[indiceGenerador].GenerarEnemigo(ordenActual);
            enemigosGenerados++;
            
            // Generar el siguiente enemigo después de un tiempo
            if (enemigosGenerados < generadores.Count)
            {
                Invoke("GenerarSiguienteEnemigo", tiempoEntreGeneraciones);
            }
        }
    }

    public void EnemigoMuerto(int orden)
    {
        Debug.Log($"Enemigo con orden {orden} murió. Se esperaba orden {siguienteOrdenEsperado}");

        if (orden == siguienteOrdenEsperado)
        {
            // ¡Orden correcto!
            Debug.Log($"¡Correcto! Enemigo {orden} murió en el orden esperado.");
            siguienteOrdenEsperado++;

            // Verificar si todos los enemigos de esta ola han muerto en orden
            if (siguienteOrdenEsperado >= generadores.Count)
            {
                OlaCompletada();
            }
        }
        else
        {
            // ¡Orden incorrecto!
            Debug.LogWarning($"¡ORDEN INCORRECTO! Se mató al enemigo {orden} pero se esperaba al {siguienteOrdenEsperado}");
            onOrdenIncorrecto?.Invoke();
            
            // Opcional: Reiniciar la sala o aplicar penalización
            ReiniciarSala();
        }
    }

    void OlaCompletada()
    {
        Debug.Log($"¡OLA {olaActual} COMPLETADA! Todos los enemigos murieron en el orden correcto.");
        onOlaCompletada?.Invoke();
        
        // Verificar si hay más olas
        if (olaActual < numeroDeOlas)
        {
            Debug.Log($"Preparando siguiente ola en {tiempoEntreOlas} segundos...");
            Invoke("IniciarNuevaOla", tiempoEntreOlas);
        }
        else
        {
            SalaCompletada();
        }
    }

    void SalaCompletada()
    {
        salaCompletada = true;
        Debug.Log($"========== ¡SALA COMPLETADA! {numeroDeOlas} OLAS SUPERADAS ==========");
        onTodasLasOlasCompletadas?.Invoke();
        playerReferencia.transform.position = new Vector2(-58f, -5f);
        
        PlayerMoveTopDown playerMove = playerReferencia.GetComponent<PlayerMoveTopDown>();
        if (playerMove != null)
        {
            playerMove.enabled = true;
        }
    }

    public void ReiniciarSala()
    {
        Debug.Log("❌ ORDEN INCORRECTO - Reiniciando sala completa...");
        
        // Cancelar generaciones y olas pendientes
        CancelInvoke("GenerarSiguienteEnemigo");
        CancelInvoke("IniciarNuevaOla");
        
        // Resetear todos los generadores
        foreach (var gen in generadores)
        {
            gen.ReiniciarGenerador();
        }

        // Resetear estado
        generacionIniciada = false;
        salaCompletada = false;
        siguienteOrdenEsperado = 0;
        enemigosGenerados = 0;
        olaActual = 0;
        ordenGeneracion.Clear();

        // Reiniciar después de un momento
        Invoke("IniciarGeneracion", 2f);
    }

    // Método para obtener información visual del orden correcto
    public string ObtenerOrdenVisualizacion()
    {
        if (ordenGeneracion.Count == 0)
            return "No iniciado";

        string resultado = $"OLA {olaActual}/{numeroDeOlas}\n";
        resultado += "Orden de muerte esperado:\n";
        for (int i = 0; i < ordenGeneracion.Count; i++)
        {
            string estado = "";
            if (i < siguienteOrdenEsperado)
                estado = " ✓";
            else if (i == siguienteOrdenEsperado)
                estado = " ← SIGUIENTE";
                
            resultado += $"{i + 1}. Generador {ordenGeneracion[i] + 1}{estado}\n";
        }
        return resultado;
    }

    void OnDrawGizmos()
    {
        if (generadores == null || generadores.Count == 0)
            return;

        // Dibujar conexiones entre generadores según el orden
        if (Application.isPlaying && ordenGeneracion.Count > 0)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < ordenGeneracion.Count - 1; i++)
            {
                if (ordenGeneracion[i] < generadores.Count && ordenGeneracion[i + 1] < generadores.Count)
                {
                    Vector3 from = generadores[ordenGeneracion[i]].transform.position;
                    Vector3 to = generadores[ordenGeneracion[i + 1]].transform.position;
                    Gizmos.DrawLine(from, to);
                }
            }
        }
    }
}
