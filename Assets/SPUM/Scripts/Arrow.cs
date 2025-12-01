using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 10f;
    public int danio = 15;
    public float tiempoDeVida = 3f;
    
    private Vector2 direccion;
    private Rigidbody2D rb;
    private bool inicializada = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            // Configurar Rigidbody para que no se vea afectado por física
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        
        // Destruir la flecha después de un tiempo
        Destroy(gameObject, tiempoDeVida);
    }

    public void Inicializar(Vector2 dir, int dano)
    {
        direccion = dir.normalized;
        danio = dano;
        inicializada = true;
        
        // Asegurar que la flecha esté en el plano correcto (Z = 0)
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;
        
        // Rotar la flecha en la dirección del movimiento
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angulo);
        
        // Aplicar velocidad inmediatamente
        if (rb != null)
        {
            rb.linearVelocity = direccion * velocidad;
        }
    }

    void FixedUpdate()
    {
        if (inicializada && rb != null)
        {
            rb.linearVelocity = direccion * velocidad;
            
            // Mantener Z en 0
            Vector3 pos = transform.position;
            pos.z = 0;
            transform.position = pos;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Si golpea al jugador
        if (other.CompareTag("Player"))
        {
            HealthSystem health = other.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.RecibirDanio(danio);
            }
            
            Destroy(gameObject);
            return;
        }
        
        // Si golpea una pared (opcional, solo si tienes paredes con tag "Wall")
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
