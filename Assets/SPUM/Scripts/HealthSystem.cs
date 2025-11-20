using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public int vidaMaxima = 100;
    public int vidaActual;
    
    [Header("Eventos")]
    public UnityEvent onDeath;
    public UnityEvent<int> onDamage;
    public UnityEvent<int> onHeal;

    private SPUM_Prefabs spum;
    private PlayerMoveTopDown playerMove;
    private bool estaMuerto = false;

    void Start()
    {
        vidaActual = vidaMaxima;
        spum = GetComponent<SPUM_Prefabs>();
        playerMove = GetComponent<PlayerMoveTopDown>();
    }

    public void RecibirDanio(int cantidad)
    {
        if (estaMuerto)
            return;

        vidaActual -= cantidad;
        vidaActual = Mathf.Max(vidaActual, 0);

        Debug.Log($"{gameObject.name} recibió {cantidad} de daño. Vida: {vidaActual}/{vidaMaxima}");

        // Reproducir animación de daño si existe
        if (spum != null && vidaActual > 0)
        {
            // Detener al jugador si tiene el componente PlayerMoveTopDown
            if (playerMove != null)
            {
                playerMove.SetDamaged(true);
            }
            
            spum.PlayAnimation(PlayerState.DAMAGED, 0);
            
            // Volver a IDLE después de la animación de daño (ajusta el tiempo según tu animación)
            CancelInvoke("VolverAIdle");
            Invoke("VolverAIdle", 0.5f); // Aumentado a 0.5 segundos
        }

        onDamage?.Invoke(cantidad);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void VolverAIdle()
    {
        if (spum != null && !estaMuerto)
        {
            spum.PlayAnimation(PlayerState.IDLE, 0);
            
            // Permitir que el jugador se mueva de nuevo
            if (playerMove != null)
            {
                playerMove.SetDamaged(false);
            }
        }
    }

    public void Curar(int cantidad)
    {
        if (estaMuerto)
            return;

        vidaActual += cantidad;
        vidaActual = Mathf.Min(vidaActual, vidaMaxima);

        Debug.Log($"{gameObject.name} se curó {cantidad}. Vida: {vidaActual}/{vidaMaxima}");
        onHeal?.Invoke(cantidad);
    }

    void Morir()
    {
        if (estaMuerto)
            return;

        estaMuerto = true;
        Debug.Log($"{gameObject.name} ha muerto");

        // Reproducir animación de muerte
        if (spum != null)
        {
            spum.PlayAnimation(PlayerState.DEATH, 0);
        }

        onDeath?.Invoke();

        // Destruir después de la animación de muerte
        Destroy(gameObject, 2f);
    }

    public bool EstaMuerto()
    {
        return estaMuerto;
    }

    public float GetPorcentajeVida()
    {
        return (float)vidaActual / vidaMaxima;
    }
}
