using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestor de la sala de diccionarios
/// Controla la entrega de items a la estatua y generación de enemigos
/// </summary>
public class DictionaryRoomManager : MonoBehaviour
{
    [Header("Configuración de Spawn")]
    public Transform[] spawnZona1; // Primera zona de spawn
    public Transform[] spawnZona2; // Segunda zona de spawn
    
    [Header("Prefabs")]
    public GameObject enemigoPrefab;
    public GameObject[] itemPrefabs; // Array de 8 items diferentes
    
    [Header("Configuración de Juego")]
    public int totalLlaves = 8; // Total de llaves en el juego
    public int enemigosMinPorItem = 2;
    public int enemigosMaxPorItem = 5;
    
    private int llaveCorrectaID = -1; // Se asignará aleatoriamente después de 3 llaves
    
    [Header("Estado")]
    public int itemsEntregados = 0;
    private List<int> itemsUsados = new List<int>(); // IDs de items ya entregados
    private int siguienteItemRequerido = -1; // -1 significa cualquiera al inicio
    
    [Header("UI")]
    private TextMeshProUGUI textoProgreso;
    private TextMeshProUGUI textoFeedback;
    
    [Header("Configuración de Sala")]
    public int salaIDRequerida = 3;
    private int salaActual = -1;
    private bool sistemaActivo = false;

    void Start()
    {
        CrearUI();
        OcultarUI();
    }

    void Update()
    {
        int nuevaSala = PlayerPrefs.GetInt("sala_id", -1);
        if (nuevaSala != salaActual)
        {
            salaActual = nuevaSala;
            if (salaActual == salaIDRequerida && !sistemaActivo)
            {
                IniciarSistema();
            }
            else if (salaActual != salaIDRequerida)
            {
                OcultarUI();
            }
        }
    }

    void CrearUI()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }

        // Texto de progreso
        GameObject progresoObj = new GameObject("TextoProgresoDiccionarios");
        progresoObj.transform.SetParent(canvas.transform, false);
        textoProgreso = progresoObj.AddComponent<TextMeshProUGUI>();
        textoProgreso.fontSize = 24;
        textoProgreso.alignment = TextAlignmentOptions.TopLeft;
        RectTransform progresoRect = progresoObj.GetComponent<RectTransform>();
        progresoRect.anchorMin = new Vector2(0, 1);
        progresoRect.anchorMax = new Vector2(0, 1);
        progresoRect.pivot = new Vector2(0, 1);
        progresoRect.anchoredPosition = new Vector2(20, -20);
        progresoRect.sizeDelta = new Vector2(500, 80);

        // Texto de feedback
        GameObject feedbackObj = new GameObject("TextoFeedbackDiccionarios");
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

    void IniciarSistema()
    {
        sistemaActivo = true;
        itemsEntregados = 0;
        itemsUsados.Clear();
        
        MostrarUI();
        ActualizarUI();
    }

    public void OnItemEntregado(DropItem item)
    {
        Debug.Log($"OnItemEntregado llamado - Llave ID: {item.itemID}, Sistema Activo: {sistemaActivo}");
        
        if (!sistemaActivo)
        {
            Debug.LogWarning("Sistema no está activo! Verifica que estés en la sala correcta.");
            return;
        }
        
        // Verificar si ya fue usado
        if (itemsUsados.Contains(item.itemID))
        {
            MostrarFeedback("Esta llave ya fue entregada!", Color.yellow);
            return;
        }
        
        // Marcar como usado
        itemsUsados.Add(item.itemID);
        itemsEntregados++;
        
        // Destruir el item
        Debug.Log($"Destruyendo item {item.itemID}");
        item.Destruir();
        
        // Después de 3 llaves, elegir una llave correcta aleatoria
        if (itemsEntregados == 3 && llaveCorrectaID == -1)
        {
            // Elegir una llave que NO haya sido entregada todavía
            List<int> llavesDisponibles = new List<int>();
            for (int i = 1; i <= totalLlaves; i++)
            {
                if (!itemsUsados.Contains(i))
                {
                    llavesDisponibles.Add(i);
                }
            }
            
            if (llavesDisponibles.Count > 0)
            {
                llaveCorrectaID = llavesDisponibles[Random.Range(0, llavesDisponibles.Count)];
                Debug.Log($"Llave correcta asignada: {llaveCorrectaID}");
            }
        }
        
        // Verificar si es la llave correcta (solo después de haber definido cuál es)
        if (llaveCorrectaID != -1 && item.itemID == llaveCorrectaID)
        {
            // Llave correcta - completar misión sin generar enemigos
            MostrarFeedback("Llave correcta! Completaste la mision!", Color.green);
            Invoke("CargarEscenaPrincipal", 3f);
        }
        else
        {
            // Llave incorrecta - generar enemigos y mostrar feedback rojo
            MostrarFeedback($"Llave equivocada! Aparecen enemigos!", Color.red);
            GenerarEnemigos();
            ActualizarUI();
        }
    }

    void GenerarEnemigos()
    {
        int cantidad = Random.Range(enemigosMinPorItem, enemigosMaxPorItem + 1);
        
        Debug.Log($"Intentando generar {cantidad} enemigos");
        Debug.Log($"Zona1 spawns: {(spawnZona1 != null ? spawnZona1.Length : 0)}, Zona2 spawns: {(spawnZona2 != null ? spawnZona2.Length : 0)}");
        Debug.Log($"Enemigo prefab asignado: {(enemigoPrefab != null ? "Si" : "NO")}");
        
        if (enemigoPrefab == null)
        {
            Debug.LogError("No hay enemigoPrefab asignado en el Inspector!");
            return;
        }
        
        for (int i = 0; i < cantidad; i++)
        {
            // Elegir zona aleatoriamente
            bool usarZona1 = Random.value > 0.5f;
            Transform[] zonaElegida = usarZona1 ? spawnZona1 : spawnZona2;
            
            if (zonaElegida == null || zonaElegida.Length == 0)
            {
                Debug.LogWarning($"Zona {(usarZona1 ? 1 : 2)} no configurada o vacía");
                continue;
            }
            
            // Elegir spawn aleatorio
            Transform spawn = zonaElegida[Random.Range(0, zonaElegida.Length)];
            
            if (spawn == null)
            {
                Debug.LogWarning("Spawn es null");
                continue;
            }
            
            // Posición con offset
            Vector3 pos = spawn.position + new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0f
            );
            
            // Crear enemigo
            GameObject enemigo = Instantiate(enemigoPrefab, pos, Quaternion.identity);
            Debug.Log($"Enemigo {i+1}/{cantidad} creado en posición {pos}");
        }
        
        Debug.Log($"Generados {cantidad} enemigos");
    }

    void ActualizarUI()
    {
        if (textoProgreso != null)
        {
            textoProgreso.text = $"Llaves incorrectas entregadas: {itemsEntregados}";
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

    void CargarEscenaPrincipal()
    {
        PlayerPrefs.SetInt("camino_id", 3);
        SceneManager.LoadScene("Main");
    }

    void MostrarUI()
    {
        if (textoProgreso != null) textoProgreso.gameObject.SetActive(true);
        if (textoFeedback != null) textoFeedback.gameObject.SetActive(true);
    }

    void OcultarUI()
    {
        if (textoProgreso != null) textoProgreso.gameObject.SetActive(false);
        if (textoFeedback != null) textoFeedback.gameObject.SetActive(false);
    }
}
