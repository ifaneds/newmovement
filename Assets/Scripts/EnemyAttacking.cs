using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    public GameObject player;
    public bool canDamage;
    public int damageValue;
    public GameObject mainBody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        canDamage=mainBody.GetComponent<SkeletonScript>().canDamage;
    }
    void OnTriggerEnter(Collider collider)
    {
        if (canDamage)
        {
            if (collider.gameObject == player)
            {
                if (player.GetComponent<ExploreMovement>().damageable)
                {
                    player.GetComponent<StatManager>().CauseDamage(damageValue);
                }
            }
        }
    }
    
    
    
}
