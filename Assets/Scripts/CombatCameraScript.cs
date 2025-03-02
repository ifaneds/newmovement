using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCameraScript : MonoBehaviour
{
    public Transform Mount = null;
    
    Transform t;
    public float cameraSpeed;

    
    void Start()
    {
         t=transform;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Mount.position, Time.deltaTime * cameraSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Mount.rotation, Time.deltaTime * cameraSpeed);
    }
}
