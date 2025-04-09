using UnityEngine;

public class IceBomb : Projectile
{
    private void Start()
    {
        lifespan = 1f;
        speed = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        lifespan -= Time.deltaTime;
        if (lifespan <= 0)
        {
            Destroy(gameObject);
        }
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit something");
        if (other.gameObject.GetComponent<EnemyController>())
        {
            other.gameObject.GetComponent<EnemyController>().Die();
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("hit person");
            if (other.gameObject.GetComponentInParent<PlayerController>().GetPlayerID() == owner) return;
            //other.gameObject.GetComponentInParent<PlayerController>().GetHit(combat.MediumDamage());
        }
        //Destroy(gameObject);
    }
}
