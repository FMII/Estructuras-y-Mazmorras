using UnityEngine;

[RequireComponent(typeof(SPUM_Prefabs))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidadMovimiento = 2f;
    public float rangoDeteccion = 5f;
    public float rangoAtaque = 1.5f;
    public float tiempoEntreAtaques = 1.5f;
    public int danioAtaque = 10;
    
    [Header("Referencias")]
    public Transform jugador;
    public LayerMask capaJugador;

    private Rigidbody2D rb;
    private SPUM_Prefabs spum;
    private float tiempoProximoAtaque;
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
    }

    void Update()
    {
        if (jugador == null || estaAtacando)
            return;

        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        // Si el jugador está dentro del rango de ataque
        if (distanciaAlJugador <= rangoAtaque && Time.time >= tiempoProximoAtaque)
        {
            Atacar();
        }
        // Si el jugador está dentro del rango de detección pero fuera del rango de ataque
        else if (distanciaAlJugador <= rangoDeteccion)
        {
            Perseguir();
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

    void Perseguir()
    {
        // Calcular dirección hacia el jugador
        Vector2 direccion = (jugador.position - transform.position).normalized;
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
        
        // Aplicar daño al jugador
        if (jugador != null)
        {
            HealthSystem jugadorHealth = jugador.GetComponent<HealthSystem>();
            if (jugadorHealth != null)
            {
                jugadorHealth.RecibirDanio(danioAtaque);
            }
        }
        
        // Actualizar el tiempo del próximo ataque
        tiempoProximoAtaque = Time.time + tiempoEntreAtaques;
        
        // Finalizar ataque después de la animación
        Invoke("FinalizarAtaque", 0.6f); // Ajusta según la duración de tu animación
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
        
        // Rango de ataque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}
