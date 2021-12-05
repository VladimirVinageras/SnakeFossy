using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondRoadManager : MonoBehaviour
{
     //Game Variables
    [SerializeField] private GameObject 
        diamondPickUp, 
        badPickup;
    
    private int isAGoodPickUp;
    [Range(1,10)][SerializeField] private int
        numberOfGroups = 4;

    private Vector3 pickUpCreationPosition;
    
    //Physics Variables
    [SerializeField] private float
        distanceFromEdges; 
        
    
    private float
        positionY,
        deltaXPosition,
        deltaYPosition,
        deltaZPosition,
        XStepSize,
        YStepSize,
        ZStepSize,
        ZMinEdgeForInstantiation,
        ZMaxEdgeForInstantiation;
    
        
    // Start is called before the first frame update
    void Start()
    {
        XStepSize = transform.lossyScale.x;
        ZStepSize = transform.lossyScale.z;
        deltaXPosition = XStepSize / 4;                                      //Calculating the middle of each way
        deltaZPosition = (ZStepSize - (distanceFromEdges * 2)) / numberOfGroups;
        ZMinEdgeForInstantiation = transform.position.z - ZStepSize/2 + distanceFromEdges;
        ZMaxEdgeForInstantiation = transform.position.z + ZStepSize/ 2 - distanceFromEdges * 2;
        positionY = transform.position.y + transform.lossyScale.y / 2 + diamondPickUp.transform.lossyScale.y / 1.95F;   
        SpawnGroups();
    }


    private void SpawnGroups()
    {
        pickUpCreationPosition  = new Vector3(transform.position.x, positionY, ZMinEdgeForInstantiation);
        
        while (pickUpCreationPosition.z >= ZMinEdgeForInstantiation && pickUpCreationPosition.z <= ZMaxEdgeForInstantiation)
        {
            isAGoodPickUp = Random.Range(0, 2);
            SpawnPickUpsInPlace(pickUpCreationPosition);
            pickUpCreationPosition.z = pickUpCreationPosition.z + deltaZPosition;
            pickUpCreationPosition.x = transform.position.x;
        }

    }
    
    
    
    private void  SpawnPickUpsInPlace(Vector3 pPickUpPosition)
    {
        if (isAGoodPickUp == 1)
        {
            Instantiate(diamondPickUp, pPickUpPosition + Vector3.forward * deltaXPosition * 0.5F, diamondPickUp.transform.rotation);
            Instantiate(diamondPickUp, pPickUpPosition - Vector3.forward * deltaXPosition * 0.5F, diamondPickUp.transform.rotation);
            Instantiate(badPickup, pPickUpPosition - Vector3.right*deltaXPosition, badPickup.transform.rotation);
            Instantiate(badPickup, pPickUpPosition + Vector3.right*deltaXPosition, badPickup.transform.rotation);
        }
        else
        {
            Instantiate(badPickup, pPickUpPosition, badPickup.transform.rotation);
            Instantiate(diamondPickUp, pPickUpPosition - Vector3.right * deltaXPosition,
                diamondPickUp.transform.rotation);
            Instantiate(diamondPickUp, pPickUpPosition + Vector3.right * deltaXPosition,
                diamondPickUp.transform.rotation);

        }
    }
}
