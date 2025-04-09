using UnityEngine;

/// <summary>
/// Commmon parent class for all projectiles.
/// </summary>
public class Projectile : MonoBehaviour
{
    public int owner;
    protected float lifespan;
    protected float speed;
    [SerializeField] protected PlayerCombat combat;
}
