using UnityEngine;

/// <summary>
/// Script del jugador para interactuar con las cajas
/// </summary>
public class PlayerBoxInteraction : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidadEmpuje = 1.5f; // Velocidad reducida al empujar
    
    private PushableBox cajaAgarrada;
    private PlayerMoveTopDown playerMove;
    private float velocidadOriginal;

    void Start()
    {
        playerMove = GetComponent<PlayerMoveTopDown>();
        if (playerMove != null)
        {
            velocidadOriginal = playerMove.speed;
        }
    }

    public bool PuedeAgarrarCaja()
    {
        return cajaAgarrada == null;
    }

    public void AgarrarCaja(PushableBox caja)
    {
        cajaAgarrada = caja;
        
        // Reducir velocidad del jugador al empujar
        if (playerMove != null)
        {
            playerMove.speed = velocidadEmpuje;
        }
        
        Debug.Log("Caja agarrada - Muévete para empujarla");
    }

    public void SoltarCaja()
    {
        cajaAgarrada = null;
        
        // Restaurar velocidad normal
        if (playerMove != null)
        {
            playerMove.speed = velocidadOriginal;
        }
        
        Debug.Log("Caja soltada");
    }

    public bool TieneCajaAgarrada()
    {
        return cajaAgarrada != null;
    }
}
