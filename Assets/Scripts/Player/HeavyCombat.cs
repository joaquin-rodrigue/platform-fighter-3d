using System.Collections;
using UnityEngine;

public class HeavyCombat : PlayerCombat
{
    public override IEnumerator HeavyAttack()
    {
        CanAttack = false;
        heavyHitboxes[0].SetActive(true);
        heavyHitboxes[0].transform.SetLocalPositionAndRotation(new Vector3(-0.3f, 0, -0.5f), Quaternion.identity);
        
        for (float i = 2; i < Mathf.PI * 2f; i += 0.2f)
        {
            heavyHitboxes[0].transform.SetLocalPositionAndRotation(0.8f * new Vector3(Mathf.Sin(i), 0, Mathf.Cos(i)), Quaternion.identity);
            yield return new WaitForFixedUpdate();
        }

        heavyHitboxes[1].SetActive(true);
        heavyHitboxes[1].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 1.8f), Quaternion.identity);
        yield return new WaitForSeconds(0.18f);

        heavyHitboxes[0].SetActive(false);
        heavyHitboxes[1].SetActive(false);
        yield return new WaitForSeconds(0.38f);

        CanAttack = true;
    }

    public override IEnumerator LightAttack()
    {
        CanAttack = false;
        yield return new WaitForSeconds(0.1f);

        lightHitboxes[0].SetActive(true);
        lightHitboxes[0].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 1.3f), Quaternion.identity);
        Vector3 direction = rb.linearVelocity.normalized;
        rb.AddRelativeForce(new Vector3(direction.x * 3.5f, 0, direction.z * 3.5f), ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.15f);

        lightHitboxes[0].SetActive(false);
        yield return new WaitForSeconds(0.17f);
        CanAttack = true;
    }

    public override IEnumerator MediumAttack()
    {
        CanAttack = false;
        yield return new WaitForSeconds(0.25f);

        mediumHitboxes[0].SetActive(true);
        mediumHitboxes[0].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 1), Quaternion.identity);
        yield return new WaitUntil(() =>
        {
            if (/* something something this hitbox is hit by another hitbox */ false)
            {
                CanAttack = true;
                return true;
            }
            return false;
        }, new System.TimeSpan(10000000), Nothing);

        mediumHitboxes[0].SetActive(false);
        if (CanAttack)
        {
            yield return new WaitForFixedUpdate();
        }
        else
        {
            yield return new WaitForSeconds(0.65f);
            CanAttack = true;
        }
    }

    // literally here so it can do nothing lol
    private void Nothing()
    {

    }
}
