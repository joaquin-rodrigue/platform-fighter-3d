using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controller for player movement, health, and any other player input
/// </summary>
public class PlayerController : MonoBehaviour
{
    // ID and speed variables
    [SerializeField] private int playerID = 1;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 8f;
    [SerializeField] private float fastFallSpeed = 3f;

    // Related objects
    [SerializeField] private PlayerCombat combat;
    [SerializeField] private GameObject attackOffset;

    // Health and I-Frames values
    private int health = 150;
    private bool immunityFramesActive = false;
    private readonly float immunityTimer = 0.25f;

    // Input variables
    private Vector2 moveInput;
    private bool jumpInput;
    private bool jumpHeld;
    private bool crouching;
    private bool lightAttack;
    private bool mediumAttack;
    private bool heavyAttack;
    private bool canJump;

    // Components that need to be referenced
    private PlayerInput playerInput;
    private Rigidbody rb;
    private Collider collision;

    /// <summary>
    /// Gets a few component references. Mostly just because its faster.
    /// </summary>
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        collision = GetComponentInChildren<Collider>();
    }

    /// <summary>
    /// Sets variables for input handling.
    /// </summary>
    void Update()
    {
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        // This is a little goofy but WasPressedThisFrame() doesn't stay true for a full FixedUpdate cycle, only
        // for an Update cycle, so this should keep it true for the entire FixedUpdate cycle
        jumpInput = playerInput.actions["Jump"].WasPressedThisFrame() || jumpInput;
        jumpHeld = playerInput.actions["Jump"].IsInProgress();
        crouching = playerInput.actions["Crouch"].IsInProgress();

        lightAttack = playerInput.actions["Light"].WasPressedThisFrame() || lightAttack;
        mediumAttack = playerInput.actions["Medium"].WasPressedThisFrame() || mediumAttack;
        heavyAttack = playerInput.actions["Heavy"].WasPressedThisFrame() || heavyAttack;
    }

    /// <summary>
    /// Handles all inputs. Notice that because buttons pressed on one frame don't usually line up with the FixedUpdate cycle,
    /// some variables are set to stay true until this function, where they are then reset. It's kinda goofy.
    /// </summary>
    private void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        rb.AddRelativeForce((moveDirection - new Vector3(rb.linearVelocity.x / 15, 0, rb.linearVelocity.z / 15)) * moveSpeed, ForceMode.Force);

        // Jump
        if (jumpInput && canJump)
        {
            Vector3 jump = new Vector3(0, jumpHeight + rb.linearVelocity.magnitude * 2, 0);
            rb.AddRelativeForce(jump, ForceMode.Impulse);
            canJump = false;
            collision.gameObject.layer = LayerMask.NameToLayer("IgnorePlatform");
        }
        // Airborne and not pressing jump; this should make the player fall faster if they aren't currently holding the button
        if (!jumpHeld && !canJump)
        {
            rb.AddRelativeForce(new(0, -fastFallSpeed, 0), ForceMode.Acceleration);
        }
        
        // Drop through platforms while crouching, simple
        if (crouching)
        {
            collision.gameObject.layer = LayerMask.NameToLayer("IgnorePlatform");
        }
        // Just to see if we are falling; therefore we can collide with platforms again instead of going through
        else if (!canJump && rb.linearVelocity.y < 0)
        {
            collision.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        // Only rotate the attacking direction if we aren't currently attacking
        if (combat.CanAttack && rb.linearVelocity.magnitude > 0.01f)
        {
            attackOffset.transform.rotation = Quaternion.LookRotation(new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z), Vector3.up);
        }

        jumpInput = false;

        // Attack button handling
        if (lightAttack && combat.CanAttack)
        {
            StartCoroutine(combat.LightAttack());
        }
        if (mediumAttack && combat.CanAttack)
        {
            StartCoroutine(combat.MediumAttack());
        }
        if (heavyAttack && combat.CanAttack)
        {
            StartCoroutine(combat.HeavyAttack());
        }

        lightAttack = false;
        mediumAttack = false;
        heavyAttack = false;
    }

    /// <summary>
    /// Deals damage and knockback to this player.
    /// </summary>
    /// <param name="damage">How much health damage to do to the player.</param>
    /// <param name="knockback">The amount of knockback to deal.</param>
    /// <param name="direction">The direction of the knockback to deal.</param>
    public void GetHit(int damage, float knockback, Vector3 direction)
    {
        if (immunityFramesActive)
        {
            return;
        }

        health -= damage;
        //Debug.Log(health);
        rb.AddForce(knockback * direction, ForceMode.VelocityChange);
        StartCoroutine(ImmunityFrames());
    }

    /// <summary>
    /// Gives the player immunity frames. Right now, that's a quarter of a second every time. Probably going to change that.
    /// </summary>
    /// <returns>After 0.25 seconds, turns off immunity frames.</returns>
    public IEnumerator ImmunityFrames()
    {
        immunityFramesActive = true;
        yield return new WaitForSeconds(immunityTimer);
        immunityFramesActive = false;
    }

    /// <summary>
    /// Returns the current health.
    /// </summary>
    /// <returns>The player's current health.</returns>
    public int GetHealth() 
    {
        return health; 
    }

    /// <summary>
    /// Resets the player's health to 150.
    /// </summary>
    public void ResetHealth()
    {
        health = 150;
    }

    /// <summary>
    /// Sets this player's ID. Used for determining projectile ownership.
    /// </summary>
    /// <param name="id">This player's ID number.</param>
    public void SetPlayerID(int id)
    {
        playerID = id;
    }

    /// <summary>
    /// Gets this player's ID.
    /// </summary>
    /// <returns>This player's ID.</returns>
    public int GetPlayerID()
    {
        return playerID;
    }

    /// <summary>
    /// I believe this is unused in the current version; mostly because players don't collide with each other anymore.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        canJump = true;
        Debug.Log(Mathf.Sqrt(collision.impulse.magnitude));

        //Debug.Log("them: " + collision.rigidbody.linearVelocity);
        //Debug.Log("me: " + rb.linearVelocity);
        // i lose
        if (collision.collider.CompareTag("Player") && Mathf.Abs(collision.rigidbody.linearVelocity.magnitude) < Mathf.Abs(rb.linearVelocity.magnitude))
        {
            rb.AddRelativeForce(collision.impulse * -2, ForceMode.Impulse); // bouncy bumper cars type hits
        }
        // they lose
        else if (collision.collider.CompareTag("Player"))
        {
            collision.rigidbody.AddRelativeForce(collision.impulse * 2, ForceMode.Impulse);
        }
        else if (collision.impulse.magnitude > 10f)
        {
            //GetHit();
        }
    }
}
