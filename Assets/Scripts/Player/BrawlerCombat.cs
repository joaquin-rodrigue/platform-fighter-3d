using System.Collections;
using UnityEngine;

public class BrawlerCombat : PlayerCombat
{
    public override IEnumerator HeavyAttack()
    {
        CanAttack = false;
        heavyHitboxes[0].SetActive(true);
        heavyHitboxes[0].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0.5f), Quaternion.identity);
        yield return new WaitForSeconds(0.4f);

        heavyHitboxes[0].transform.localScale = new Vector3(2, 2, 2);
        Vector3 direction = rb.linearVelocity.normalized;
        rb.AddRelativeForce(new Vector3(direction.x * 2, 15, direction.z * 2), ForceMode.VelocityChange);
        heavyHitboxes[0].transform.SetLocalPositionAndRotation(new Vector3(0, 1, 0.75f), Quaternion.identity);
        yield return new WaitForSeconds(0.08f);
        yield return new WaitUntil(() => 
        { 
            rb.AddRelativeForce(Vector3.down * 3, ForceMode.Acceleration); 
            return rb.linearVelocity.y <= 0.5f; 
        });

        heavyHitboxes[0].SetActive(false);
        heavyHitboxes[0].transform.localScale = new Vector3(1, 1, 1);
        yield return new WaitForSeconds(0.24f);

        CanAttack = true;
    }

    public override IEnumerator LightAttack()
    {
        CanAttack = false;
        lightHitboxes[0].SetActive(true);
        lightHitboxes[0].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 1.5f), Quaternion.identity);
        yield return new WaitForSeconds(0.13f);

        lightHitboxes[0].SetActive(false);
        yield return new WaitForSeconds(0.07f);
        CanAttack = true;
    }

    public override IEnumerator MediumAttack()
    {
        CanAttack = false;
        mediumHitboxes[0].SetActive(true);
        mediumHitboxes[0].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0.8f), Quaternion.identity);
        yield return new WaitForSeconds(0.1f);

        mediumHitboxes[1].SetActive(true);
        mediumHitboxes[1].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 1.5f), Quaternion.identity);
        Vector3 direction = rb.linearVelocity.normalized;
        rb.AddRelativeForce(new Vector3(direction.x * 8, 4.5f, direction.z * 8), ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.25f);

        mediumHitboxes[0].SetActive(false);
        mediumHitboxes[1].SetActive(false);
        yield return new WaitForSeconds(0.15f);

        CanAttack = true;
    }
}
