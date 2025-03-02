using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreMovement: MonoBehaviour
{
    public Camera exploreCam;
    public Camera combatCam;

    private Animator _animator;
    private CharacterController _characterController;
    private Rigidbody _rigidbody;
    private bool isGrounded;

    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;

    private float velocityX;
    private float previousPositionX;
    private float velocityZ;
    private float previousPositionZ;
    private Vector3 moveDirection;
    private float Jump;
    private float Speed;
    private float lastAirTime;
    private float lastGroundTime;
    private float gravity = -9.8f;
    private Vector3 velocity;

    public bool combat;
    private float sprintSmoothing;
    public float rotationSpeed;
    public float jumpHeight;
    public float jumpDistance;
    private float jumpSmoothing;
    public float jumpButtonGracePeriod;

    public bool canDamage;


    //Combat only variables

    private float cVertical;
    private float cHorizontal;
    private float combatSmooth;
    private float mouseX;
    private float attackTime;
    private float combo;

    private bool lockedOn;
    public int enemyLocked;
    public Transform enemiestrans;
    public float lockRadius;
    public GameObject weapon;
    private List<GameObject> enemiesNear;
    public LayerMask locking;
    private GameObject target;

    private Transform focusPoint;

    private bool light;
    private bool heavy;
    public int layerIndex;
    private Collider[] inRadius;

    public bool damageable;

    private float health;

    public string lastAttackType;




    


   
    // Start is called before the first frame update
    void Start()
    {
        combatSmooth = 0.2f;
        sprintSmoothing = 0.2f;
        jumpSmoothing = 0.2f;

        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();

        enemiesNear = new List<GameObject>();

        attackTime = Time.time;
        
        exploreCam.enabled = true;
        combatCam.enabled = false;

        damageable = true;
    }

    // Update is called once per frame
    void Update()
    {
         if (enemiesNear.Count!=0)
         {
        enemiestrans = enemiesNear[enemyLocked].transform;
         }
        health = GetComponent<StatManager>().health;

        if (health <=0)
        {
            _animator.SetLayerWeight(0,0);
            _animator.SetLayerWeight(1,0);
            _animator.SetLayerWeight(2,1);
            _animator.SetBool("Dead",true);
        }

        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        Speed = 0;
        Speed = Input.GetAxis("Vertical");
        
        velocityX = ((transform.position.x - previousPositionX) / Time.deltaTime);
        previousPositionX = transform.position.x;
        velocityZ = ((transform.position.z - previousPositionZ) / Time.deltaTime);
        previousPositionZ = transform.position.z;
        
        if (Speed <0.1 && Speed >-0.1){
            _animator.SetBool("isMoving",false);
        } else {_animator.SetBool("isMoving",true);}

         if (Input.GetKeyDown(KeyCode.Tab))
        {
            combat = !combat;
            
            exploreCam.enabled = !exploreCam.enabled;
            combatCam.enabled = !combatCam.enabled;

           
        }
        enemiesNear.Clear();
        inRadius = Physics.OverlapSphere(transform.position, lockRadius, locking);
        foreach (var enemy in inRadius)
        {
            if (!enemiesNear.Contains(enemy.gameObject))
            {
                enemiesNear.Add(enemy.gameObject);
            }
        }


        if (!combat)
        {
        
        _animator.SetLayerWeight(1,0);
        _animator.SetLayerWeight(0,1);
        layerIndex=0;
        MoveFree();
        }
        if (combat)
        {
        _animator.SetLayerWeight(0,0);
        _animator.SetLayerWeight(1,1);
        layerIndex=1;
        MoveCombat();
        }
        
    }
    
        
        
    private void MoveFree()
    {
        
             if (!isGrounded){
                Jump = 2;
                lastAirTime = Time.time;
             }
             

            float inputDirection = Input.GetAxis("Horizontal");
            if (isGrounded)
            {   
                lastGroundTime = Time.time;
                
                
                
                Jump = 0;
                if (velocity.y<0)
                {
                    velocity.y = -2f;
                    velocity.x=0;
                    velocity.z=0;
                }
                if (Input.GetKey(KeyCode.LeftShift) && Speed==1 || Input.GetKeyUp(KeyCode.LeftShift) && Speed == 2)
                {
                    Speed = 2;
                    _animator.SetFloat("Speed", Speed,sprintSmoothing,Time.deltaTime);
                }
                else
                {
                    _animator.SetFloat("Speed", Speed,sprintSmoothing,Time.deltaTime);
                }
                if (!AnimatorIsPlaying(0,"attack") && !_animator.GetBool("Dead")){
                    if (Input.GetKey(KeyCode.D))
                    {
                        transform.Rotate(0,rotationSpeed*Time.deltaTime,0);
                    } 
                    if (Input.GetKey(KeyCode.A))
                    {
                        transform.Rotate(0,-rotationSpeed*Time.deltaTime,0);                  
                    }
                }
            
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                    velocity.x = (velocityX * jumpDistance);
                    velocity.z = (velocityZ * jumpDistance);
                    if (Speed>0.1){
                    velocity.x = (velocityX * 1.2f);
                    velocity.z = (velocityZ * 1.2f);
                    }
                    velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                    Jump = 1;
                    
                    }
            }

            if (Time.time - lastGroundTime <= jumpButtonGracePeriod){
                    
                    isGrounded = true;
                    Jump = 0;
            } 
            else {
                isGrounded=false;
                Jump = 2;
            }
            _animator.SetBool("isGrounded",isGrounded);

            if (Input.GetMouseButtonDown(0))
            {
               
                _animator.SetTrigger("Attack");      
                if (Speed<1.5){
                    lastAttackType = "light";
                }       else 
                {
                    lastAttackType="running";
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                StartCoroutine(waiter(0.2f));
                _animator.ResetTrigger("Attack");             
            }

            
            velocity.y += gravity * Time.deltaTime;
            _characterController.Move(velocity * Time.deltaTime);
                       
            
            _animator.SetFloat("Jump", Jump, jumpSmoothing, Time.deltaTime);
            

    }

    private bool AnimatorIsPlaying(string stateName){
        if(_animator.GetCurrentAnimatorStateInfo(1).IsName(stateName) &&
        _animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void MoveCombat()
    {
        if (isGrounded){
        if (velocity.y<0)
                {
                    velocity.z=0;
                }
        }

         if (Input.GetMouseButtonDown(0) && !AnimatorIsPlaying("Combat Movement"))
            {
                _animator.SetBool("light",true);
           
                _animator.SetFloat("attackTime", Time.time);

                lastAttackType = "light";
            }
         if (Input.GetMouseButtonDown(1) && !AnimatorIsPlaying("Combat Movement"))
         {
              _animator.SetBool("heavy",true);
            
            _animator.SetFloat("attackTime", Time.time);
            lastAttackType = "heavy";
         }



            if (lockedOn)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                   
                    if (enemyLocked<enemiesNear.Count-1)
                    {
                        enemyLocked += 1;          
                    }
                    else
                    {
                        enemyLocked=0;
                    }
                }
            }
            if (enemiesNear.Count!=0 && !_animator.GetBool("Dead"))
            {
                lockedOn = true;
                target = enemiesNear[enemyLocked];
                focusPoint = target.transform.Find("FocusPoint").transform;
                transform.LookAt(focusPoint,Vector3.up);
                
            } 
            else if (!_animator.GetBool("Dead"))
            {
                lockedOn = false;
                mouseX = Input.GetAxis("Mouse X") * Time.deltaTime;
                mouseX = mouseX * rotationSpeed;
                transform.Rotate(0,mouseX,0,Space.Self);              
            }
            cVertical = Input.GetAxis("Vertical");
            cHorizontal = Input.GetAxis("Horizontal");

            velocity.y += gravity * Time.deltaTime;
            _characterController.Move(velocity * Time.deltaTime);


            if (Input.GetMouseButtonDown(2))
            {
                _animator.SetBool("Blocking",true);
                damageable = false;
            }
            if (Input.GetMouseButtonUp(2))
            {
                _animator.SetBool("Blocking",false);
                damageable = true;
            }
            


        

        
       
        _animator.SetFloat("cVertical",cVertical,combatSmooth,Time.deltaTime);
        _animator.SetFloat("cHorizontal",cHorizontal,combatSmooth,Time.deltaTime);
        //_animator.SetInteger("combatStage",combatStage);
       // _animator.SetFloat("combo",combo);

    }
    
    

    bool AnimatorIsPlaying(int layer, string stateTag)
    {
    return _animator.GetCurrentAnimatorStateInfo(layer).IsTag(stateTag);
    }

    IEnumerator waiter(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }

    public void EnableAttacking(){
        canDamage=true;
    }
     public void DisableAttacking(){
        canDamage=false;
    }
    public void AreaDamage(float radius)
    {      
        weapon.GetComponent<PlayerAttacking>().RadialAttack(radius);  
    }
}


    








