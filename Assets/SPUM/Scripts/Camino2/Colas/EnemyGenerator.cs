using UnityEngine;
using System.Collections.Generic;

public class EnemyGenerator : MonoBehaviour
{
    [Header("Configuración de Generación")]
    public GameObject enemigoPrefab;
    public Transform puntoSpawn;
    public float tiempoEntreGeneraciones = 2f;
    
    [Header("Estado")]
    public int ordenGeneracion; // Orden en que este generador spawneó su enemigo
    public bool yaGenero = false;
    
    private RoomEnemyManager roomManager;
    private GameObject enemigoActual;

    void Start()
    {
        // Buscar el manager de la sala
        roomManager = GetComponentInParent<RoomEnemyManager>();
        if (roomManager == null)
        {
            Debug.LogError($"EnemyGenerator en {gameObject.name} no encontró RoomEnemyManager en el padre!");
        }

        if (puntoSpawn == null)
        {
            puntoSpawn = transform;
        }
    }

    public void GenerarEnemigo(int orden)
    {
        if (yaGenero || enemigoPrefab == null)
            return;

        ordenGeneracion = orden;
        yaGenero = true;

        // Generar el enemigo
        enemigoActual = Instantiate(enemigoPrefab, puntoSpawn.position, Quaternion.identity);
        
        // Configurar el enemigo con su orden
        EnemyAI enemyAI = enemigoActual.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.order = orden;
        }

        // Agregar indicador de orden
        EnemyOrderIndicator indicador = enemigoActual.AddComponent<EnemyOrderIndicator>();
        if (indicador != null)
        {
            indicador.InicializarIndicador(orden, roomManager);
        }

        // Configurar el HealthSystem para notificar al manager
        HealthSystem healthSystem = enemigoActual.GetComponent<HealthSystem>();
        if (healthSystem != null)
        {
            // Subscribirse al evento de muerte
            healthSystem.onDeath.AddListener(() => NotificarMuerteEnemigo(orden));
        }

        Debug.Log($"Generador {gameObject.name} spawneó enemigo con orden {orden}");
    }

    void NotificarMuerteEnemigo(int orden)
    {
        if (roomManager != null)
        {
            roomManager.EnemigoMuerto(orden);
        }
    }

    public bool TieneEnemigoVivo()
    {
        return enemigoActual != null;
    }

    public void ReiniciarGenerador()
    {
        yaGenero = false;
        if (enemigoActual != null)
        {
            Destroy(enemigoActual);
            enemigoActual = null;
        }
    }
}
