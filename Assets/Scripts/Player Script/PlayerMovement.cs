using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float sprint = 8f;
    private float vertical;
    private float horizontal;

    public Rigidbody2D rb;
    public Animator anim;

    public float stamina = 100f;
    public float staminaDrain = 0.5f;
    public float staminaRegen = 0.5f;
    public float staminaInterval = 0.5f;

    public float sprintCooldown = 1f;

    private float sprintCooldownTimer = 0f;
    private bool sprintOnCooldown = false;

    private float staminaTimer = 0f;
    private bool wasSprinting = false;
    private bool sprintLocked = false;

    private int direction = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Move();
        Sprint();
    }

    void Move()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(horizontal, vertical);
        bool isMoving = movement.magnitude > 0;

        anim.SetFloat("horizontal", Mathf.Abs(horizontal));
        anim.SetFloat("vertical", Mathf.Abs(vertical));
        anim.SetBool("isMoving", isMoving);

        if (horizontal < 0 && direction == 1 || horizontal > 0 && direction == -1)
        {
            flip();
        }

        bool shiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (stamina <= 0)
            sprintLocked = true;

        if (!shiftPressed)
            sprintLocked = false;

        bool isSprinting =
            shiftPressed &&
            isMoving &&
            stamina > 0 &&
            !sprintOnCooldown &&
            !sprintLocked;

        rb.velocity = movement * (isSprinting ? sprint : speed);

        Stamina(isSprinting);
        SprintState(isSprinting);
    }

    void flip()
    {
        direction *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    void Stamina(bool isSprinting)
    {
        staminaTimer += Time.deltaTime;
        if (staminaTimer < staminaInterval) return;

        if (isSprinting)
        {
            stamina -= staminaDrain;
            if (stamina < 0) stamina = 0;
        }
        else if (stamina < 100f)
        {
            stamina += staminaRegen;
            if (stamina > 100f) stamina = 100f;
        }

        staminaTimer = 0f;
    }

    void SprintState(bool isSprinting)
    {
        if (wasSprinting && !isSprinting)
        {
            sprintOnCooldown = true;
            sprintCooldownTimer = 0f;
        }

        wasSprinting = isSprinting;
    }

    void Sprint()
    {
        if (!sprintOnCooldown) return;

        sprintCooldownTimer += Time.deltaTime;
        if (sprintCooldownTimer >= sprintCooldown)
            sprintOnCooldown = false;
    }
}
