using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour
{
    [Header("Configuración de Ataque")]
    public int danioAtaque = 20;
    public float rangoAtaque;
    public LayerMask capaEnemigos;
    public Transform puntoAtaque; // Punto desde donde se detecta el ataque (opcional)

    void Start()
    {
    }

    void Update()
    {
        // Cuando se reproduce la animación de ataque, detectar enemigos
        // Esto se puede llamar desde PlayerMoveTopDown cuando ataca
    }

    public void RealizarAtaque()
    {
        // Usar la posición del jugador o un punto de ataque específico
        Vector2 posicionAtaque = puntoAtaque != null ? puntoAtaque.position : (Vector2)transform.position + Vector2.up * 0.3f;

        // Detectar enemigos en rango
        Collider2D[] enemigosGolpeados = Physics2D.OverlapCircleAll(posicionAtaque, rangoAtaque, capaEnemigos);

        foreach (Collider2D enemigo in enemigosGolpeados)
        {
            // Aplicar daño al enemigo
            HealthSystem healthSystem = enemigo.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.RecibirDanio(danioAtaque);
                Debug.Log($"Golpeaste a {enemigo.name} causando {danioAtaque} de daño");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualizar rango de ataque en el editor
        Gizmos.color = Color.red;
        Vector3 pos = puntoAtaque != null ? puntoAtaque.position : transform.position + Vector3.up * 0.3f;
        Gizmos.DrawWireSphere(pos, rangoAtaque);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
}
