using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMovement : MonoBehaviour
{
    
    
    private float Speed = 0.0f;
    private Animator _animator = null;
    
   
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {

        Speed = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical")==1){
            Speed *= 2;
            
        } 
        if (Input.GetKey(KeyCode.D)){
            transform.Rotate(0,90,0,Space.World);
        }
        _animator.SetFloat("Speed", Speed,0.01f,Time.deltaTime);
        
       
        
    }
}
