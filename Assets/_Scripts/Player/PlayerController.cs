#if UNITY_IOS || UNITY_ANDROID
        #define USING_MOBILE
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Physics variables
    [SerializeField] private float
        _playerMoveForwardSpeed = 3F,
        _playerMoveLateralSpeed = 8F,
        roadXPosition = 0F,
        roadXsize = 4F,
        distanceToBound = 0.1F;

    private float 
        leftXBound, 
        rightXBound; 
    
    
    //Game variables

    [SerializeField] float feverTime = 5F;
    private bool isFeverRush;
    private float _horizontalInput, _verticalInput;
    private Vector3 snakePosition, TouchPosition;
    private int diamondsCounter = 0; 
    
    
    
    // Start is called before the first frame update
    
   void Start()
   {
       leftXBound = roadXPosition - (roadXsize * 0.5F);
       rightXBound = roadXPosition + (roadXsize * 0.5F);
   }

    // Update is called once per frame
    void FixedUpdate()
    {
    #if USING_MOBILE
        snakePosition = transform.forward * _playerMoveForwardSpeed;

        if (!isFeverRush)
        {
            if (Input.touchCount > 0)
            {
                Vector3 touchPosition = Input.GetTouch(0).position;
                Debug.Log("TouchPosition is " + touchPosition);

                if (touchPosition.x > Screen.width * 0.5f)
                {
                    snakePosition += transform.right * _playerMoveLateralSpeed;
                }
                else
                {
                    snakePosition -= transform.right * _playerMoveLateralSpeed;

                }

            }
            gameObject.transform.position += snakePosition * Time.deltaTime;
            CheckBounds();
        }
        else
        {
            gameObject.transform.position += snakePosition * Time.deltaTime;
        }

#endif
        }
    
    void CheckBounds()
    
    {
        if (transform.position.x < leftXBound)
        {
            transform.position = new Vector3(leftXBound+ distanceToBound, transform.position.y, transform.position.z); 
        }
        
        if (transform.position.x > rightXBound)
        {
            transform.position = new Vector3(rightXBound-distanceToBound, transform.position.y, transform.position.z);
        }
        
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Diamond"))
        {
             diamondsCounter++;
             GameManager.Instance.UpdateDiamondScore(1);
             Destroy(other.gameObject);
             if (diamondsCounter >= 3 )
             {
                 GameManager.Instance.RestartDiamondScore();
                 if(!isFeverRush)
                 {
                     isFeverRush = true;
                     GameManager.Instance.IsFeverRush = isFeverRush;
                     GameManager.Instance.UpdateRushCounter(); 
                     FeverRush();
                     StartCoroutine(FeverRushCountdown());
                 }
             }
        }
        else if (other.gameObject.CompareTag("Death"))
        {
            if (isFeverRush)
            {
                Destroy(other.gameObject);
            }
            else
            {
                GameManager.Instance.GameOver();
            }
        }
        
        if (other.gameObject.CompareTag("MealCompletedPoint"))
        {
            GameManager.Instance.SnakeHasToGrow = true;
        }
        
    }

    
    IEnumerator FeverRushCountdown()
    {
        yield return new WaitForSecondsRealtime(feverTime);
           _playerMoveForwardSpeed = _playerMoveForwardSpeed * 0.3333F;
            isFeverRush = false;
            GameManager.Instance.IsFeverRush = isFeverRush;
            diamondsCounter = 0;
        
    }

    void FeverRush()
    {
        transform.position = new Vector3(roadXPosition, transform.position.y,transform.position.z);
        _playerMoveForwardSpeed *= 3;
    }

}
