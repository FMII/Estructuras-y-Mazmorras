using UnityEngine;

[RequireComponent(typeof(SPUM_Prefabs))]
[RequireComponent(typeof(Rigidbody2D))]
public class ArcherEnemyAI : MonoBehaviour
{
    [Header("Configuración")]
    public float rangoDeteccion = 8f;
    public float rangoAtaqueMinimo = 3f;
    public float rangoAtaqueMaximo = 7f;
    public float tiempoEntreDisparos = 2f;
    public int danioFlecha = 15;
    
    [Header("Movimiento")]
    public float velocidadMovimiento = 1.5f;
    public float distanciaRetroceso = 2f; // Distancia mínima que quiere mantener
    
    [Header("Referencias")]
    public Transform jugador;
    public GameObject prefabFlecha; // Prefab de la flecha
    public Transform puntoDisparo; // Punto desde donde sale la flecha
    
    private Rigidbody2D rb;
    private SPUM_Prefabs spum;
    private float tiempoProximoDisparo;
    private bool estaAtacando = false;
    private Vector2 direccionMovimiento;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spum = GetComponent<SPUM_Prefabs>();
        
        // Inicializar animaciones
        spum.PopulateAnimationLists();
        spum.OverrideControllerInit();
        
        // Buscar al jugador si no está asignado
        if (jugador == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                jugador = playerObj.transform;
            }
        }
        
        // Si no hay punto de disparo, usar la posición del enemigo
        if (puntoDisparo == null)
        {
            puntoDisparo = transform;
        }
    }

    void Update()
    {
        if (jugador == null || estaAtacando)
            return;

        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        // Si el jugador está dentro del rango de detección
        if (distanciaAlJugador <= rangoDeteccion)
        {
            // Si está muy cerca, retroceder
            if (distanciaAlJugador < distanciaRetroceso)
            {
                Retroceder();
            }
            // Si está en rango de ataque, disparar
            else if (distanciaAlJugador >= rangoAtaqueMinimo && distanciaAlJugador <= rangoAtaqueMaximo)
            {
                if (Time.time >= tiempoProximoDisparo)
                {
                    Atacar();
                }
                else
                {
                    // Quedarse quieto esperando
                    spum.PlayAnimation(PlayerState.IDLE, 0);
                    direccionMovimiento = Vector2.zero;
                }
            }
            // Si está fuera de rango, acercarse
            else if (distanciaAlJugador > rangoAtaqueMaximo)
            {
                Acercarse();
            }
            else
            {
                // En rango óptimo, quedarse quieto
                spum.PlayAnimation(PlayerState.IDLE, 0);
                direccionMovimiento = Vector2.zero;
            }
        }
        else
        {
            // Idle si el jugador está lejos
            spum.PlayAnimation(PlayerState.IDLE, 0);
            direccionMovimiento = Vector2.zero;
        }

        // Voltear hacia el jugador
        if (jugador.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (jugador.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void FixedUpdate()
    {
        if (!estaAtacando)
        {
            rb.MovePosition(rb.position + direccionMovimiento * velocidadMovimiento * Time.fixedDeltaTime);
        }
    }

    void Acercarse()
    {
        // Calcular dirección hacia el jugador
        Vector2 direccion = (jugador.position - transform.position).normalized;
        direccionMovimiento = direccion;
        
        // Animación de movimiento
        spum.PlayAnimation(PlayerState.MOVE, 0);
    }

    void Retroceder()
    {
        // Calcular dirección alejándose del jugador
        Vector2 direccion = (transform.position - jugador.position).normalized;
        direccionMovimiento = direccion;
        
        // Animación de movimiento
        spum.PlayAnimation(PlayerState.MOVE, 0);
    }

    void Atacar()
    {
        estaAtacando = true;
        direccionMovimiento = Vector2.zero;
        
        // Reproducir animación de ataque
        spum.PlayAnimation(PlayerState.ATTACK, 0);
        
        // Disparar la flecha después de un pequeño delay (cuando la animación muestra el disparo)
        Invoke("DispararFlecha", 0.3f);
        
        // Actualizar el tiempo del próximo disparo
        tiempoProximoDisparo = Time.time + tiempoEntreDisparos;
        
        // Finalizar ataque después de la animación
        Invoke("FinalizarAtaque", 0.6f);
    }

    void DispararFlecha()
    {
        if (prefabFlecha == null || jugador == null)
        {
            Debug.LogWarning("ArcherEnemyAI: Falta asignar el prefab de flecha o el jugador.");
            return;
        }

        // Calcular dirección hacia el jugador
        Vector2 direccion = (jugador.position - puntoDisparo.position).normalized;
        
        // Instanciar la flecha
        GameObject flecha = Instantiate(prefabFlecha, puntoDisparo.position, Quaternion.identity);
        
        // Inicializar la flecha
        Arrow arrowScript = flecha.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.Inicializar(direccion, danioFlecha);
        }
        
        Debug.Log("Arquero dispara flecha");
    }

    void FinalizarAtaque()
    {
        estaAtacando = false;
    }

    // Dibujar gizmos para visualizar rangos en el editor
    void OnDrawGizmosSelected()
    {
        // Rango de detección
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        
        // Rango de ataque máximo
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaqueMaximo);
        
        // Rango de ataque mínimo
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangoAtaqueMinimo);
        
        // Distancia de retroceso
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distanciaRetroceso);
    }
}
