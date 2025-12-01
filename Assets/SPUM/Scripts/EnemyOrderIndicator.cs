using UnityEngine;
using TMPro;

public class EnemyOrderIndicator : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject indicadorPrefab;
    public Vector3 offset = new Vector3(0, 1.5f, 0);
    public Color colorObjetivo = Color.green;
    public Color colorEspera = Color.red;
    public bool mostrarSoloObjetivo = false;
    
    private GameObject indicadorActual;
    private TextMeshProUGUI textoOrden;
    private SpriteRenderer spriteIndicador;
    private int ordenEnemigo;
    private RoomEnemyManager roomManager;

    public void InicializarIndicador(int orden, RoomEnemyManager manager)
    {
        ordenEnemigo = orden;
        roomManager = manager;
        
        CrearIndicadorTexto();
        InvokeRepeating("ActualizarIndicador", 0f, 0.1f);
    }

    void CrearIndicadorTexto()
    {
        // Crear un Canvas para el texto
        GameObject canvasObj = new GameObject("OrderCanvas");
        canvasObj.transform.SetParent(transform);
        canvasObj.transform.localPosition = offset;
        
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.sortingOrder = 100;
        
        RectTransform rectTransform = canvasObj.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(0.5f, 0.5f);
        rectTransform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
        
        // Crear el texto
        GameObject textoObj = new GameObject("OrderText");
        textoObj.transform.SetParent(canvasObj.transform);
        textoObj.transform.localPosition = Vector3.zero;
        
        textoOrden = textoObj.AddComponent<TextMeshProUGUI>();
        textoOrden.text = (ordenEnemigo + 1).ToString();
        textoOrden.fontSize = 80;
        textoOrden.alignment = TextAlignmentOptions.Center;
        textoOrden.color = colorEspera;
        textoOrden.fontStyle = FontStyles.Bold;
        textoOrden.outlineWidth = 0.2f;
        textoOrden.outlineColor = Color.black;
        
        RectTransform textoRect = textoObj.GetComponent<RectTransform>();
        textoRect.sizeDelta = new Vector2(100, 100);
        
        indicadorActual = canvasObj;
    }

    void ActualizarIndicador()
    {
        if (roomManager == null || textoOrden == null)
            return;

        bool esObjetivo = ordenEnemigo == roomManager.siguienteOrdenEsperado;
        
        // Cambiar color según si es el objetivo
        if (esObjetivo)
        {
            textoOrden.color = colorObjetivo;
            textoOrden.fontSize = 1; // Más grande si es el objetivo
            
            // Efecto pulsante
            float pulso = Mathf.PingPong(Time.time * 2f, 0.15f);
            indicadorActual.transform.localScale = new Vector3(0.005f + pulso * 0.005f, 0.005f + pulso * 0.005f, 0.005f);
        }
        else
        {
            textoOrden.color = colorEspera;
            textoOrden.fontSize = 0.5f;
            indicadorActual.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
            
            // Ocultar si solo se muestra el objetivo
            if (mostrarSoloObjetivo)
            {
                indicadorActual.SetActive(false);
                return;
            }
        }
        
        indicadorActual.SetActive(true);
        
        // Hacer que el indicador siempre mire a la cámara
        if (Camera.main != null)
        {
            indicadorActual.transform.LookAt(Camera.main.transform);
            indicadorActual.transform.Rotate(0, 180, 0);
        }
    }

    void OnDestroy()
    {
        CancelInvoke("ActualizarIndicador");
        if (indicadorActual != null)
        {
            Destroy(indicadorActual);
        }
    }
}
