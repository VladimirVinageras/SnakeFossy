using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    [SerializeField] private float speedRotation;
   
    // Update is called once per frame
    void Update()
    {
     this.gameObject.transform.Rotate(Vector3.up*speedRotation*Time.deltaTime);   
    }
}
