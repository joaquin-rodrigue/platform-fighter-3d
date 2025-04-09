using UnityEngine;

public class BigEnemyController : EnemyController
{
    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;

        transform.Translate(0, 0, speed * Time.deltaTime);
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * obstacleRange, Color.green, 0.05f);
        RaycastHit hit;

        if (Physics.SphereCast(ray, 0.16f, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            if ((hitObject.GetComponent<PlayerController>() || hitObject.GetComponent<EnemyController>()) && fireball == null && canAttack)
            {
                fireball = Instantiate(fireballPrefab);
                fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                fireball.transform.rotation = transform.rotation;
                fireball.transform.Rotate(0, 0, 0);
                fireball = Instantiate(fireballPrefab);
                fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                fireball.transform.rotation = transform.rotation;
                fireball.transform.Rotate(0, 10, 0);
                fireball = Instantiate(fireballPrefab);
                fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                fireball.transform.rotation = transform.rotation;
                fireball.transform.Rotate(0, 20, 0);
                fireball = Instantiate(fireballPrefab);
                fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                fireball.transform.rotation = transform.rotation;
                fireball.transform.Rotate(0, -10, 0);
                fireball = Instantiate(fireballPrefab);
                fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                fireball.transform.rotation = transform.rotation;
                fireball.transform.Rotate(0, -20, 0);
                StartCoroutine(AttackCooldown());
            }
            else if (hit.distance < obstacleRange)
            {
                float angle = Random.Range(-110, 110);
                transform.Rotate(0, angle, 0);
            }
        }
    }
}
