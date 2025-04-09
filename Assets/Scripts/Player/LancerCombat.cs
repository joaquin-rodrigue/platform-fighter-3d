using System.Collections;
using UnityEngine;

public class LancerCombat : PlayerCombat
{
    private int lightCombo = 0;

    public override IEnumerator HeavyAttack()
    {
        CanAttack = false;
        heavyHitboxes[0].SetActive(true);
        heavyHitboxes[0].transform.SetLocalPositionAndRotation(new Vector3(-0.2f, 0, -0.55f), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);

        heavyHitboxes[1].SetActive(true);
        for (float i = 0; i < Mathf.PI * 6; i += 0.5f)
        {
            heavyHitboxes[0].transform.SetLocalPositionAndRotation(0.9f * new Vector3(Mathf.Sin(i), 0, Mathf.Cos(i)), Quaternion.identity);
            heavyHitboxes[1].transform.SetLocalPositionAndRotation(1.8f * new Vector3(Mathf.Sin(i), 0, Mathf.Cos(i)), Quaternion.identity);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.05f);

        heavyHitboxes[0].SetActive(false);
        heavyHitboxes[1].SetActive(false);
        yield return new WaitForSeconds(0.35f);

        CanAttack = true;
    }

    public override IEnumerator LightAttack()
    {
        CanAttack = false;
        StopCoroutine(ComboTimeout());
        lightHitboxes[0].SetActive(true);
        lightHitboxes[0].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 1.1f), Quaternion.identity);
        lightCombo++;
        if (lightCombo >= 3)
        {
            lightHitboxes[1].SetActive(true);
            lightHitboxes[1].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 1.9f), Quaternion.identity);
            lightCombo = 0;
        }
        yield return new WaitForSeconds(0.16f);

        lightHitboxes[0].SetActive(false);
        lightHitboxes[1].SetActive(false);
        StartCoroutine(ComboTimeout());

        if (lightCombo == 0)
        {
            yield return new WaitForSeconds(0.16f);
        }
        yield return new WaitForSeconds(0.06f);
        CanAttack = true;
    }

    public override IEnumerator MediumAttack()
    {
        CanAttack = false;
        StopCoroutine(ComboTimeout());
        mediumHitboxes[0].SetActive(true);
        mediumHitboxes[0].transform.SetLocalPositionAndRotation(new Vector3(0, 0.3f, -0.5f), Quaternion.identity);
        yield return new WaitForFixedUpdate();

        mediumHitboxes[1].SetActive(true);
        for (float i = 0; i <= 1.2f; i += 0.1f)
        {
            mediumHitboxes[0].transform.SetLocalPositionAndRotation(new Vector3(0, -2.4f * Mathf.Pow(i, 2) + 2.4f * i + 0.6f, 2f * i - 0.5f), Quaternion.identity);
            mediumHitboxes[1].transform.SetLocalPositionAndRotation(new Vector3(0, -4.8f * Mathf.Pow(i, 2) + 4.8f * i + 0.9f, 4f * i - 1f), Quaternion.identity);
            yield return new WaitForFixedUpdate();
        }
        mediumHitboxes[2].SetActive(true);
        mediumHitboxes[2].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 4.8f), Quaternion.identity);
        yield return new WaitForSeconds(0.1f);

        mediumHitboxes[0].SetActive(false);
        mediumHitboxes[1].SetActive(false);
        mediumHitboxes[2].SetActive(false);
        yield return new WaitForSeconds(0.15f);

        CanAttack = true;
    }

    private IEnumerator ComboTimeout()
    {
        yield return new WaitForSeconds(0.7f);
        lightCombo = 0;
    }
}
