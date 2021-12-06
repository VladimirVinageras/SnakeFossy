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
        rightXBound,
        deltaXPosition; 
    
    
    //Game variables

    [SerializeField] float feverTime = 5F;
    private bool isFeverRush;
    private float _horizontalInput;
    private Vector3 snakePosition, touchPosition ;
    private int diamondsCounter = 0; 
    
    //Inpput Variables
    private Touch touch;
    
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
            /*  if (Input.touchCount > 0)                                      //If is used this variant, is necessary to set active UI/ControlsScreen 
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
            gameObject.transform.position += snakePosition * Time.deltaTime;
            CheckBounds();
          }
          else
          {
              gameObject.transform.position += snakePosition * Time.deltaTime;
          }
          */


            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    transform.position +=
                        Vector3.right *
                        (touch.deltaPosition.x * _playerMoveLateralSpeed * Time.deltaTime);  

                }
            }

            transform.position += Vector3.forward * _playerMoveForwardSpeed * Time.deltaTime;
            CheckBounds();
        }
        else
        {
            gameObject.transform.position += snakePosition * Time.deltaTime;
        }
#else
         snakePosition = transform.forward * _playerMoveForwardSpeed;
         if (!isFeverRush)
         {
             _horizontalInput = Input.GetAxis("Horizontal");
             transform.Translate(Vector3.right * _playerMoveLateralSpeed * _horizontalInput* 20F * Time.deltaTime);
         }
         
         transform.position += Vector3.forward * _playerMoveForwardSpeed * Time.deltaTime;
         CheckBounds();
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

    void FeverRush()
    {
        transform.position = new Vector3(roadXPosition, transform.position.y,transform.position.z);
        _playerMoveForwardSpeed *= 3;
    }
    
    IEnumerator FeverRushCountdown()
    {
        yield return new WaitForSecondsRealtime(feverTime);
           _playerMoveForwardSpeed = _playerMoveForwardSpeed * 0.3333F;
            isFeverRush = false;
            GameManager.Instance.IsFeverRush = isFeverRush;
            diamondsCounter = 0;
        
    }



}
