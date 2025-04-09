using UnityEngine;

public class GroundPound : Projectile
{
    private void Start()
    {
        lifespan = 0.4f;
        speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        lifespan -= Time.deltaTime;
        if (lifespan <= 0)
        {
            Destroy(gameObject);
        }
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
            //sother.gameObject.GetComponentInParent<PlayerController>().GetHit(combat.HeavyDamage());
        }
        //Destroy(gameObject);
    }
}
