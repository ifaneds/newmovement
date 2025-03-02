using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyStatManager : MonoBehaviour
{
    public float health;
    public GameObject player;

    private Animator animator;
    private int layerIndex;

    public GameObject prefab;
    

    

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Animator>() != null)
        {
        animator = GetComponent<Animator>();
        }
        player = GameObject.Find("Character");
        
    }

    // Update is called once per frame
    void Update()
    {
       




        layerIndex=player.GetComponent<ExploreMovement>().layerIndex;
    }
    public void CauseDamage(int damageAmount){
    
        health -= damageAmount;

        //transform.LookAt(player.transform, Vector3.up);
        if (animator != null)
        {
        animator.SetTrigger("Stagger");
        }
        DamageNumber(damageAmount);
    }
    public void DamageNumber(int damageAmount){
        var position = transform.position + new Vector3(0, (float)1.5, 0);
        var popup = Instantiate(prefab, position, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = damageAmount.ToString();

        Destroy(popup, 1f);
    }




}
