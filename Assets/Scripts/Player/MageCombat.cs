using System.Collections;
using UnityEngine;

public class MageCombat : PlayerCombat
{
    [SerializeField] private GameObject lightProjectile;
    [SerializeField] private GameObject mediumProjectile;
    [SerializeField] private GameObject heavyProjectile;

    public override IEnumerator HeavyAttack()
    {
        CanAttack = false;
        heavyHitboxes[0].SetActive(true);
        heavyHitboxes[0].transform.SetLocalPositionAndRotation(new Vector3(0, 1, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.06f);

        rb.AddRelativeForce(new Vector3(0, 12, 0), ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.4f);

        rb.AddRelativeForce(new Vector3(0, -24, 0), ForceMode.VelocityChange);
        heavyHitboxes[0].transform.SetLocalPositionAndRotation(new Vector3(0, -0.75f, 0), Quaternion.identity);
        heavyHitboxes[0].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => rb.linearVelocity.y >= -0.01f);

        // this is awful 
        GameObject projectile = Instantiate(heavyProjectile, transform.position, Quaternion.identity);
        projectile.GetComponent<GroundPound>().owner = rb.gameObject.GetComponent<PlayerController>().GetPlayerID();
        yield return new WaitForSeconds(0.55f);

        heavyHitboxes[0].SetActive(false);
        CanAttack = true;
    }

    public override IEnumerator LightAttack()
    {
        CanAttack = false;
        yield return new WaitForSeconds(0.15f);

        lightHitboxes[0].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 1), Quaternion.identity);
        // horrid coding right here
        GameObject projectile = Instantiate(lightProjectile, lightHitboxes[0].transform.position, lightHitboxes[0].GetComponentInParent<Transform>().rotation);
        projectile.GetComponent<Fireball>().owner = rb.gameObject.GetComponent<PlayerController>().GetPlayerID();
        yield return new WaitForSeconds(0.1f);

        CanAttack = true;
    }

    public override IEnumerator MediumAttack()
    {
        CanAttack = false;
        mediumHitboxes[0].SetActive(true);
        mediumHitboxes[0].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0.5f), Quaternion.identity);
        yield return new WaitForSeconds(0.35f);

        mediumHitboxes[0].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 2.1f), Quaternion.identity);
        // yes officer this code right here
        GameObject projectile = Instantiate(mediumProjectile, mediumHitboxes[0].transform.position, mediumHitboxes[0].GetComponentInParent<Transform>().rotation);
        projectile.GetComponent<IceBomb>().owner = rb.gameObject.GetComponent<PlayerController>().GetPlayerID();
        Vector3 direction = (mediumHitboxes[0].transform.position - transform.position) * -1;
        mediumHitboxes[0].SetActive(false);
        
        rb.AddRelativeForce(new Vector3(direction.x * 4, 9, direction.z * 4), ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.33f);

        CanAttack = true;
    }
}
