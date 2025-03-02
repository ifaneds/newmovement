using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboarding : MonoBehaviour
{
    
    private Camera cam;
    public GameObject player;

    private void Awake() {
        cam = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        if(player == null){
            player = GameObject.Find("Character");
        }
        if (!player.GetComponent<ExploreMovement>().combat)
        {
            cam = player.GetComponent<ExploreMovement>().exploreCam;
        }
        else
        {
            cam = player.GetComponent<ExploreMovement>().combatCam;
        }
        this.transform.forward = cam.transform.forward;
    }
}
