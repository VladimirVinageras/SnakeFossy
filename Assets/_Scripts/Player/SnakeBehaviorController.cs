using System;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBehaviorController : MonoBehaviour
{
    private List<Vector3> _pPreviousSnakePosition;    
    private List<GameObject> _snake;
    private Transform 
        snakeRing, 
        previousSnakeRing ;
    
    private Vector3 lookDirection;

    [SerializeField] private GameObject
        _snakeHead,
        _snakeBody,
        _snakeTail;
   
   [SerializeField] private int visibleSnakeLenght;
   [SerializeField] private float 
       followSpeed = 8F,
       maxDistanceBetweenSteps = 0.8F, 
       multiplierToManageDistanceBetweenRings = 1.34F,
       rushMultiplierToManageDistanceBetweenRings = 4.02F;
  
   public float 
        distanceBetweenRings, 
        indexOffsetBetweenRings = 1.34F;     //indexOffsetBetweenRings  decrease distance between snake rings

   
   void Start()
    {
        _snake = new List<GameObject>();
        _snake.Add(_snakeHead);
        _snake.Add(_snakeBody);
        _snake.Add(_snakeTail);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 1; i < _snake.Count; i++)
        {
            snakeRing = _snake[i].transform;
            previousSnakeRing = _snake[i-1].transform;
            lookDirection = (previousSnakeRing.position - snakeRing.position);
            snakeRing.transform.Translate(lookDirection *
                                          Time.deltaTime * 
                                          followSpeed * 
                                          indexOffsetBetweenRings);
            
            distanceBetweenRings = Math.Abs(previousSnakeRing.lossyScale.z/2 - snakeRing.lossyScale.z/2);
            
            if (distanceBetweenRings > maxDistanceBetweenSteps)
            {
                snakeRing.position = new Vector3(snakeRing.position.x, snakeRing.position.y,
                    previousSnakeRing.position.z - maxDistanceBetweenSteps);
            }
        }

        if (GameManager.Instance.IsFeverRush)
        {
            indexOffsetBetweenRings = rushMultiplierToManageDistanceBetweenRings;
        }
        else
        {
            indexOffsetBetweenRings = multiplierToManageDistanceBetweenRings;
        }

        if (GameManager.Instance.SnakeHasToGrow)
        {
            
            int posBeforeTail = _snake.Count - 1;
            int tailPos = _snake.Count;
            if (_snake.Count <= visibleSnakeLenght)
            {
                GameObject newRing = Instantiate(_snakeBody, _snakeTail.transform.position,
                    _snakeTail.transform.rotation);
             
                newRing.gameObject.transform.SetParent(this.transform);
                _snake.Insert(posBeforeTail, newRing);
                _snake[tailPos].gameObject.transform.Translate(Vector3.back * distanceBetweenRings);
            }

            GameManager.Instance.SnakeHasToGrow = false;
        }


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MealCompletedPoint"))   
        {
            int posBeforeTail = _snake.Count - 1;
            int tailPos = _snake.Count;
            if (_snake.Count <= visibleSnakeLenght)
            {
                GameObject newRing = Instantiate(_snakeBody, _snakeTail.transform.position,
                    _snakeTail.transform.rotation);
                newRing.gameObject.transform.SetParent(this.transform);
                _snake.Insert(posBeforeTail, newRing);
                _snake[tailPos].gameObject.transform.Translate(Vector3.back * distanceBetweenRings);
            }
        }
    }
    
}
