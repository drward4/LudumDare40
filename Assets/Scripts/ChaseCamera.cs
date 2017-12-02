using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera : MonoBehaviour
{
    public GameObject Target;


    private void LateUpdate()
    {
        this.transform.position = new Vector3(this.Target.transform.position.x, this.Target.transform.position.y, this.transform.position.z);
    }
}
