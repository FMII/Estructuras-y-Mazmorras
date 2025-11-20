using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Configuración de Ataque")]
    public int danioAtaque = 20;
    public float rangoAtaque = 1.5f;
    public LayerMask capaEnemigos;
    public Transform puntoAtaque; // Punto desde donde se detecta el ataque (opcional)

    void Update()
    {
        // Cuando se reproduce la animación de ataque, detectar enemigos
        // Esto se puede llamar desde PlayerMoveTopDown cuando ataca
    }

    public void RealizarAtaque()
    {
        // Usar la posición del jugador o un punto de ataque específico
        Vector2 posicionAtaque = puntoAtaque != null ? puntoAtaque.position : transform.position;

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
        Vector3 pos = puntoAtaque != null ? puntoAtaque.position : transform.position;
        Gizmos.DrawWireSphere(pos, rangoAtaque);
    }
}
