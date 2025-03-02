using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreCameraScript : MonoBehaviour
{
    public Transform Mount = null;
    public float Speed = 5.0f;
    Transform t;
    public float fixedRotation=0f;

    // Start is called before the first frame update
    void Start()
    {
        t=transform;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        transform.position = Vector3.Lerp(transform.position, Mount.position, Time.deltaTime * Speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Mount.rotation, Time.deltaTime * Speed);
     // t.eulerAngles=new Vector3 (t.eulerAngles.x, fixedRotation, t.eulerAngles.z);
    }
}
