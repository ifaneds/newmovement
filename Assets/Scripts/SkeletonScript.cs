using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonScript : MonoBehaviour
{
    private Animator animator;
    private float speed;
    public float lookRadius;
    private GameObject player;
    private float distance;
    public float attackRange;
    private bool attacking;
    public bool canDamage;
    private float health;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Character");
        
        
    }

    // Update is called once per frame
    void Update()
    {
        health = GetComponent<EnemyStatManager>().health;

        if (health <=0)
        {
            animator.SetLayerWeight(0,0);
            animator.SetLayerWeight(1,1);
            
            animator.SetBool("Dead",true);
        }

        if (!animator.GetBool("Dead")){
        distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < lookRadius)
        {
            if (distance <= attackRange)
            {
                speed = 0;
                animator.SetFloat("Speed",speed);

                attacking = true;
                animator.SetBool("Attacking",attacking);

                if (!animator.GetBool("attackFinished")){
                transform.LookAt(player.transform.position,Vector3.up);
            }
                
            } 
            else 
            {
                

                speed = 1;
                animator.SetFloat("Speed",speed);
            
                attacking = false;
                animator.SetBool("Attacking",attacking);
                
                transform.LookAt(player.transform.position,Vector3.up);
            }
            
        }
        
        else 
        {
            speed = 0;
            animator.SetFloat("Speed",speed);

            attacking = false;
            animator.SetBool("Attacking",attacking);
        }
        }
    }

    public void EnableAttacking(){
        canDamage=true;
    }
     public void DisableAttacking(){
        canDamage=false;
    }


}
