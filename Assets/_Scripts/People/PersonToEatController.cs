using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonToEatController : MonoBehaviour
{
    //Animation variables
    private static string WAS_EATEN = "wasEaten";
    private Animator _animator;
    
    
    //Physics variables
    private float personRotationAngle = 360f;

    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
    }
    
    void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.CompareTag("Player"))
        {
            _animator.SetTrigger(WAS_EATEN);
            gameObject.transform.Rotate(Vector3.up, personRotationAngle*Time.deltaTime);
            gameObject.transform.Rotate(Vector3.right, personRotationAngle*Time.deltaTime);
        }
        else if(other.gameObject.CompareTag("Jaws"))
        {
            Destroy(gameObject);   
            GameManager.Instance.UpdatePeopleScore(1);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.transform.Rotate(Vector3.up, personRotationAngle*Time.deltaTime);
            gameObject.transform.Rotate(Vector3.right, personRotationAngle*Time.deltaTime);
         
        }
    }
 
}
