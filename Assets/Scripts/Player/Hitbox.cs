using UnityEngine;

/// <summary>
/// Hitboxes.
/// </summary>
public class Hitbox : MonoBehaviour
{
    public PlayerCombat combat;
    [SerializeField] private bool isLight;
    [SerializeField] private bool isMedium;
    [SerializeField] private bool isHeavy;

    /// <summary>
    /// Checks whether a player is colliding witht he hitbox. If so, runs hitbox code.
    /// </summary>
    /// <param name="other">Object collided with.</param>
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("hit something");
        if (other.CompareTag("Player"))
        {
            // Tried rearranging this to use less GetComponent calls; it still has a rigidbody call though which might be made faster
            // Not that this is a huge concern right now; maybe if you get hit constantly it'd slow down the game but that's not really important
            PlayerController player = other.GetComponentInParent<PlayerController>();
            if (GetComponent<Projectile>())
            {
                int owner = GetComponent<Projectile>().owner;
                if (player.GetPlayerID() == owner)
                {
                    return;
                }
            }
            int damage = isLight ? combat.LightDamage() : isMedium ? combat.MediumDamage() : combat.HeavyDamage();
            //Debug.Log("hit a person");
            float knockback = isLight ? combat.LightKnockback() : isMedium ? combat.MediumKnockback() : combat.HeavyKnockback();
            Vector3 direction = transform.TransformVector(transform.localPosition.normalized);
            player.GetHit(damage, knockback, direction);
        }
    }
}
