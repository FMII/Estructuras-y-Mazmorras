using UnityEngine;

[RequireComponent(typeof(SPUM_Prefabs))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMoveTopDown : MonoBehaviour
{
    public float speed = 3f;
    public KeyCode attackKey = KeyCode.Space;

    private Rigidbody2D rb;
    private Vector2 input;
    private SPUM_Prefabs spum;
    private PlayerAttack playerAttack;
    private HealthSystem healthSystem;
    private bool isAttacking = false;
    private bool isDamaged = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spum = GetComponent<SPUM_Prefabs>();
        playerAttack = GetComponent<PlayerAttack>();
        healthSystem = GetComponent<HealthSystem>();
        
        spum.PopulateAnimationLists();
        spum.OverrideControllerInit();
    }

    void Update()
    {
        // Ignorar inputs si hay diálogos activos
        if (Dialogs.dialogActive) return;
        
        // Si está recibiendo daño, no hacer nada
        if (isDamaged)
            return;

        // Detectar ataque
        if (Input.GetKeyDown(attackKey) && !isAttacking)
        {
            isAttacking = true;
            spum.PlayAnimation(PlayerState.ATTACK, 0);
            
            // Realizar el ataque después de un pequeño delay (cuando la animación golpea)
            Invoke("EjecutarAtaque", 0.3f);
            Invoke("FinalizarAtaque", 0.5f);
            return;
        }

        // Si está atacando, no hacer nada más
        if (isAttacking)
            return;

        // Leer input
        input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        // Cambiar animaciones según movimiento
        if (input.magnitude > 0)
        {
            spum.PlayAnimation(PlayerState.MOVE, 0);
        }
        else
        {
            spum.PlayAnimation(PlayerState.IDLE, 0);
        }

        // Voltear el personaje
        if (input.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (input.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void FinalizarAtaque()
    {
        isAttacking = false;
    }

    void EjecutarAtaque()
    {
        if (playerAttack != null)
        {
            playerAttack.RealizarAtaque();
        }
    }

    public void SetDamaged(bool damaged)
    {
        isDamaged = damaged;
        if (damaged)
        {
            input = Vector2.zero; // Detener movimiento
        }
    }

    void FixedUpdate()
    {
        if (!isDamaged)
        {
            rb.MovePosition(rb.position + input * speed * Time.fixedDeltaTime);
        }
    }
}
