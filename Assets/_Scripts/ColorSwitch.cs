using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitch : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Color newSnakeColor = this.gameObject.GetComponent<Renderer>().material.color;
        if (other.CompareTag("Jaws")) 
        {
            GameObject.FindWithTag("Jaws").GetComponent<Renderer>().material.color = newSnakeColor;
        }
        if (other.CompareTag("SnakeBody")) 
        {
            GameObject.FindWithTag("SnakeBody").GetComponent<Renderer>().material.color = newSnakeColor;
        }
        if (other.CompareTag("SnakeTail")) 
        {
            GameObject.FindWithTag("SnakeTail").GetComponent<Renderer>().material.color = newSnakeColor;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SnakeTail"))
        {
            Destroy(gameObject);
        }

    }
    
    
}
