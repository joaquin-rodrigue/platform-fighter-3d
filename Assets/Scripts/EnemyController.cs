using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] protected GameObject fireballPrefab;
    protected GameObject fireball;

    public float speed = 2.0f;
    public float obstacleRange = 0.5f;
    public float attackSpeed = 0.8f;
    protected bool canAttack = true;
    protected bool isAlive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isAlive = true;
    }

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
                StartCoroutine(AttackCooldown());
            }
            else if (hit.distance < obstacleRange)
            {
                float angle = Random.Range(-110, 110);
                transform.Rotate(0, angle, 0);
            }
        }
    }

    public void Die()
    {
        StartCoroutine(DeathAnimation());
    }

    protected IEnumerator DeathAnimation()
    {
        transform.rotation = Quaternion.Euler(new Vector3(100, transform.rotation.y, 0));
        isAlive = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    protected IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }
}
