using UnityEngine;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Gestor de la sala de conjuntos
/// Controla la mecánica de aprendizaje de unión e intersección
/// </summary>
public class ConjuntosRoomManager : MonoBehaviour
{
    [Header("Configuración de Conjuntos")]
    public Transform[] spawnsConjuntoA; // Puntos de spawn para conjunto A
    public Transform[] spawnsConjuntoB; // Puntos de spawn para conjunto B
    public Transform[] spawnsInterseccion; // Puntos de spawn para A ∩ B (están en ambos)
    
    [Header("Prefabs de Enemigos")]
    public GameObject enemigoPrefabTipo1; // Enemigos que pertenecen al conjunto
    public GameObject enemigoPrefabTipo2; // Enemigos que NO pertenecen
    
    public enum TipoOperacion { Union, Interseccion }
    
    [Header("Misión")]
    public TipoOperacion operacionActual = TipoOperacion.Union;
    
    [Header("Configuración de Juego")]
    public int enemigosASpawnear = 15;
    public int enemigosCorrectosPorEliminar = 5;
    
    [Header("Estado")]
    public int enemigosCorrectosEliminados = 0;
    public int enemigosIncorrectosEliminados = 0;
    private List<GameObject> enemigosActivos = new List<GameObject>();
    
    [Header("UI")]
    private TextMeshProUGUI textoMision;
    private TextMeshProUGUI textoProgreso;
    private TextMeshProUGUI textoFeedback;
    
    [Header("Referencia")]
    public ConjuntosEnemyGenerator enemyGenerator;
    
    [Header("Configuración de Sala")]
    public int salaIDRequerida = 2; // ID de sala donde se activa este sistema
    private int salaActual = -1;
    
    [Header("Teleport al Completar")]
    public Vector2 posicionFinal = new Vector2(150f, -7f);
    private Transform jugador;

    void Start()
    {
        if (enemyGenerator == null)
            enemyGenerator = GetComponent<ConjuntosEnemyGenerator>();
        
        // Buscar al jugador
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }
        
        CrearUI();
        OcultarUI();
    }
    
    void Update()
    {
        int nuevaSala = PlayerPrefs.GetInt("sala_id", -1);
        if (nuevaSala != salaActual)
        {
            salaActual = nuevaSala;
            if (salaActual == salaIDRequerida)
            {
                IniciarMision();
                MostrarUI();
            }
            else
            {
                OcultarUI();
            }
        }
    }

    void CrearUI()
    {
        // Crear Canvas si no existe
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }

        // Crear texto de misión
        GameObject misionObj = new GameObject("TextoMision");
        misionObj.transform.SetParent(canvas.transform, false);
        textoMision = misionObj.AddComponent<TextMeshProUGUI>();
        textoMision.fontSize = 24;
        textoMision.alignment = TextAlignmentOptions.TopLeft;
        RectTransform misionRect = misionObj.GetComponent<RectTransform>();
        misionRect.anchorMin = new Vector2(0, 1);
        misionRect.anchorMax = new Vector2(0, 1);
        misionRect.pivot = new Vector2(0, 1);
        misionRect.anchoredPosition = new Vector2(20, -20);
        misionRect.sizeDelta = new Vector2(600, 100);

        // Crear texto de progreso
        GameObject progresoObj = new GameObject("TextoProgreso");
        progresoObj.transform.SetParent(canvas.transform, false);
        textoProgreso = progresoObj.AddComponent<TextMeshProUGUI>();
        textoProgreso.fontSize = 20;
        textoProgreso.alignment = TextAlignmentOptions.TopLeft;
        RectTransform progresoRect = progresoObj.GetComponent<RectTransform>();
        progresoRect.anchorMin = new Vector2(0, 1);
        progresoRect.anchorMax = new Vector2(0, 1);
        progresoRect.pivot = new Vector2(0, 1);
        progresoRect.anchoredPosition = new Vector2(20, -130);
        progresoRect.sizeDelta = new Vector2(400, 50);

        // Crear texto de feedback
        GameObject feedbackObj = new GameObject("TextoFeedback");
        feedbackObj.transform.SetParent(canvas.transform, false);
        textoFeedback = feedbackObj.AddComponent<TextMeshProUGUI>();
        textoFeedback.fontSize = 28;
        textoFeedback.alignment = TextAlignmentOptions.Center;
        textoFeedback.fontStyle = FontStyles.Bold;
        RectTransform feedbackRect = feedbackObj.GetComponent<RectTransform>();
        feedbackRect.anchorMin = new Vector2(0.5f, 0.5f);
        feedbackRect.anchorMax = new Vector2(0.5f, 0.5f);
        feedbackRect.pivot = new Vector2(0.5f, 0.5f);
        feedbackRect.anchoredPosition = new Vector2(0, 200);
        feedbackRect.sizeDelta = new Vector2(800, 60);
    }

    void IniciarMision()
    {
        enemigosCorrectosEliminados = 0;
        enemigosIncorrectosEliminados = 0;
        
        // Generar enemigos
        if (enemyGenerator != null)
        {
            enemyGenerator.GenerarEnemigos(operacionActual, enemigosASpawnear);
        }
        
        ActualizarUI();
    }

    /// <summary>
    /// Llamado cuando el jugador elimina un enemigo
    /// </summary>
    public void OnEnemigoEliminado(GameObject enemigo)
    {
        ConjuntosEnemy enemyScript = enemigo.GetComponent<ConjuntosEnemy>();
        if (enemyScript == null) return;
        
        bool esEnemigoCorrecto = ValidarEnemigo(enemyScript);
        
        if (esEnemigoCorrecto)
        {
            enemigosCorrectosEliminados++;
            MostrarFeedback($"✅ ¡Correcto! Este enemigo pertenece a {ObtenerNombreOperacion()}", Color.green);
            
            if (enemigosCorrectosEliminados >= enemigosCorrectosPorEliminar)
            {
                CompletarMision();
            }
        }
        else
        {
            enemigosIncorrectosEliminados++;
            MostrarFeedback($"❌ Incorrecto. Este enemigo NO pertenece a {ObtenerNombreOperacion()}", Color.red);
        }
        
        enemigosActivos.Remove(enemigo);
        ActualizarUI();
    }

    bool ValidarEnemigo(ConjuntosEnemy enemy)
    {
        switch (operacionActual)
        {
            case TipoOperacion.Union:
                // A ∪ B: debe estar en A, en B, o en ambos
                return enemy.estaEnConjuntoA || enemy.estaEnConjuntoB;
                
            case TipoOperacion.Interseccion:
                // A ∩ B: debe estar en AMBOS conjuntos
                return enemy.estaEnConjuntoA && enemy.estaEnConjuntoB;
                
            default:
                return false;
        }
    }

    string ObtenerNombreOperacion()
    {
        return operacionActual switch
        {
            TipoOperacion.Union => "A ∪ B (Unión)",
            TipoOperacion.Interseccion => "A ∩ B (Intersección)",
            _ => "Operación desconocida"
        };
    }

    void ActualizarUI()
    {                                                       
        if (textoMision != null)
        {
            string mision = operacionActual == TipoOperacion.Union
                ? "MISION: Elimina enemigos que pertenezcan a A U B\n(Estan en conjunto A, conjunto B, o en ambos)"
                : "MISION: Elimina enemigos que pertenezcan a A n B\n(Estan en el conunto  AB)";
            
            textoMision.text = mision;
        }
        
        if (textoProgreso != null)
        {
            textoProgreso.text = $"Correctos: {enemigosCorrectosEliminados}/{enemigosCorrectosPorEliminar} | Incorrectos: {enemigosIncorrectosEliminados}";
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
        CancelInvoke("LimpiarFeedback");
        Invoke("LimpiarFeedback", 2f);
    }

    void LimpiarFeedback()
    {
        if (textoFeedback != null)
            textoFeedback.text = "";
    }

    void CompletarMision()
    {
        MostrarFeedback($"🎉 ¡MISIÓN COMPLETADA! Entiendes {ObtenerNombreOperacion()}", Color.cyan);
        
        // Cambiar a la siguiente operación o completar
        if (operacionActual == TipoOperacion.Union)
        {
            Invoke("CambiarAInterseccion", 3f);
        }
        else
        {
            Invoke("JuegoCompletado", 3f);
        }
    }

    void CambiarAInterseccion()
    {
        operacionActual = TipoOperacion.Interseccion;
        LimpiarEnemigos();
        IniciarMision();
    }

    void LimpiarEnemigos()
    {
        foreach (GameObject enemigo in enemigosActivos)
        {
            if (enemigo != null)
                Destroy(enemigo);
        }
        enemigosActivos.Clear();
    }

    void JuegoCompletado()
    {
        MostrarFeedback("✅ ¡Has completado todas las operaciones de conjuntos!", Color.green);
        Invoke("OcultarUI", 3f);
        
        // Mover al jugador a la posición final
        if (jugador != null)
        {
            Invoke("MoverJugador", 3f);
        }
        else
        {
            Debug.LogWarning("No se encontró el jugador para moverlo");
        }
    }
    
    void MoverJugador()
    {
        if (jugador != null)
        {
            jugador.position = new Vector3(posicionFinal.x, posicionFinal.y, jugador.position.z);
            Debug.Log($"Jugador movido a posición final: {posicionFinal}");
        }
    }

    public void AgregarEnemigo(GameObject enemigo)
    {
        enemigosActivos.Add(enemigo);
    }
    
    void MostrarUI()
    {
        if (textoMision != null) textoMision.gameObject.SetActive(true);
        if (textoProgreso != null) textoProgreso.gameObject.SetActive(true);
        if (textoFeedback != null) textoFeedback.gameObject.SetActive(true);
    }
    
    void OcultarUI()
    {
        if (textoMision != null) textoMision.gameObject.SetActive(false);
        if (textoProgreso != null) textoProgreso.gameObject.SetActive(false);
        if (textoFeedback != null) textoFeedback.gameObject.SetActive(false);
    }
}
