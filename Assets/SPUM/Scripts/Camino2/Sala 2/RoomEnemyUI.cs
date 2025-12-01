using UnityEngine;
using TMPro;

public class RoomEnemyUI : MonoBehaviour
{
    [Header("Referencias")]
    public RoomEnemyManager roomManager;
    public TextMeshProUGUI textoOrden;
    public GameObject panelIndicador;
    
    [Header("Configuración")]
    public bool mostrarIndicadorSobreEnemigo = true;
    public Color colorEnemigoCorrecto = Color.green;
    public Color colorEnemigoIncorrecto = Color.red;

    void Start()
    {
        if (roomManager == null)
        {
            roomManager = GetComponent<RoomEnemyManager>();
        }
    }

    void Update()
    {
        if (roomManager != null && textoOrden != null)
        {
            textoOrden.text = roomManager.ObtenerOrdenVisualizacion();
        }
    }

    // Método para mostrar indicador sobre el enemigo correcto
    public void MostrarIndicadorEnemigoCorrecto()
    {
        if (!mostrarIndicadorSobreEnemigo || roomManager == null)
            return;

        // Encontrar el enemigo con el orden esperado
        int ordenEsperado = roomManager.siguienteOrdenEsperado;
        
        EnemyAI[] enemigos = FindObjectsOfType<EnemyAI>();
        foreach (var enemigo in enemigos)
        {
            if (enemigo.order == ordenEsperado)
            {
                // Aquí podrías agregar un sprite de flecha o efecto sobre el enemigo correcto
                // Por ejemplo, cambiar el color del sprite renderer
                SpriteRenderer[] sprites = enemigo.GetComponentsInChildren<SpriteRenderer>();
                foreach (var sprite in sprites)
                {
                    sprite.color = Color.Lerp(Color.white, colorEnemigoCorrecto, 0.3f);
                }
            }
        }
    }
}
