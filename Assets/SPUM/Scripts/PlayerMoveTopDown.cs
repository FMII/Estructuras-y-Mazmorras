using UnityEngine;

[RequireComponent(typeof(SPUM_Prefabs))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMoveTopDown : MonoBehaviour
{
    public float speed = 3f;

    private Rigidbody2D rb;
    private Vector2 input;
    private SPUM_Prefabs spum;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spum = GetComponent<SPUM_Prefabs>();
        
        spum.PopulateAnimationLists();
        spum.OverrideControllerInit();
    }

    void Update()
    {
        // Leer input
        input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        if (input.magnitude > 0)
        {
            spum.PlayAnimation(PlayerState.MOVE, 0);
        }
        else
        {
            spum.PlayAnimation(PlayerState.IDLE, 0);
        }

        if (input.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (input.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * speed * Time.fixedDeltaTime);
    }
}
