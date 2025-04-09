using System.Collections;
using UnityEngine;

/// <summary>
/// Includes combat stats and attack functions that player classes can implement.
/// </summary>
public abstract class PlayerCombat : MonoBehaviour
{
    // Damage numbers
    [SerializeField] protected int lightDamage;
    [SerializeField] protected int mediumDamage;
    [SerializeField] protected int heavyDamage;

    // Knockback values
    [SerializeField] protected float lightKnockback;
    [SerializeField] protected float mediumKnockback;
    [SerializeField] protected float heavyKnockback;

    // Hitboxes/rigidbody references
    [SerializeField] protected GameObject[] lightHitboxes;
    [SerializeField] protected GameObject[] mediumHitboxes;
    [SerializeField] protected GameObject[] heavyHitboxes;
    [SerializeField] protected Rigidbody rb;

    /// <summary>
    /// Whether this player can attack or not. This is not necessarily whether an attack 
    /// is active or not, although usually an attack is active while CanAttack is false.
    /// </summary>
    public bool CanAttack { get; protected set; } = true;

    /// <summary>
    /// Light attack for this class.
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator LightAttack();
    /// <summary>
    /// Medium attack for this class.
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator MediumAttack();
    /// <summary>
    /// Heavy attack for this class.
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator HeavyAttack();

    /// <summary>
    /// Gets the light attack damage value.
    /// </summary>
    /// <returns>How much damage a light attack from this class deals.</returns>
    public int LightDamage()
    {
        return lightDamage;
    }

    /// <summary>
    /// Gets the medium attack damage value.
    /// </summary>
    /// <returns>How much damage a medium attack from this class deals.</returns>
    public int MediumDamage()
    {
        return mediumDamage;
    }

    /// <summary>
    /// Gets the heavy attack damage value.
    /// </summary>
    /// <returns>How much damage a heavy attack from this class deals.</returns>
    public int HeavyDamage()
    {
        return heavyDamage;
    }

    /// <summary>
    /// Gets the light attack knockback value.
    /// </summary>
    /// <returns>How much knockback a light attack from this class deals.</returns>
    public float LightKnockback()
    {
        return lightKnockback;
    }

    /// <summary>
    /// Gets the medium attack knockback value.
    /// </summary>
    /// <returns>How much knockback a medium attack from this class deals.</returns>
    public float MediumKnockback()
    {
        return mediumKnockback;
    }

    /// <summary>
    /// Gets the heavy attack knockback value.
    /// </summary>
    /// <returns>How much knockback a heavy attack from this class deals.</returns>
    public float HeavyKnockback()
    {
        return heavyKnockback;
    }
}
