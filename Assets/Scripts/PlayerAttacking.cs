using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    public GameObject player;
    public Animator _animator;
    public Collider _collider;
    public GameObject hit;
    private int layerIndex;
    private bool canDamage;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = player.GetComponent<Animator>();
        _collider = GetComponent<Collider>();

      

    }

    // Update is called once per frame
    void Update()
    {
       layerIndex=player.GetComponent<ExploreMovement>().layerIndex;
       canDamage=player.GetComponent<ExploreMovement>().canDamage;
    }

    void OnTriggerEnter(Collider collider)
    {   
        if(canDamage){
        if (_animator.GetCurrentAnimatorStateInfo(layerIndex).IsTag("attack")){

        if (collider.gameObject.layer == LayerMask.NameToLayer("enemy")){
             hit = collider.gameObject;
         int damageValue = 0;
                if (player.GetComponent<ExploreMovement>().lastAttackType == "light")
                {
                    damageValue = 2;
                } else if (player.GetComponent<ExploreMovement>().lastAttackType == "heavy")
                {
                    damageValue = 5;
                } else if (player.GetComponent<ExploreMovement>().lastAttackType == "running")
                {
                    damageValue = 7;
                } 
          hit.GetComponent<EnemyStatManager>().CauseDamage(damageValue);

          hit = null;
     }
     }
    }
    }
    public void RadialAttack(float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        
        foreach (var hitCollider in hitColliders)
        {
            if (hitColliders.Length > 0){
            if (hitCollider.gameObject.layer == LayerMask.NameToLayer("enemy"))
            {
                hit = hitCollider.gameObject;
                

                hit.GetComponent<EnemyStatManager>().CauseDamage(4);

                 hit = null;
            }
            }
        }
    }
}
