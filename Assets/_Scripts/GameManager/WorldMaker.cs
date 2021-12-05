using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class WorldMaker : MonoBehaviour
{
    [SerializeField] private GameObject startStep; 
    [SerializeField] private GameObject peopleRoadStep;
    [SerializeField] private GameObject diamondRoadStep;
    [SerializeField] private GameObject finishStep;
    [SerializeField] private GameObject plane;
   

    private Rigidbody _rigidbodyPlayer;
    
    [Range(0,5)] [SerializeField] private float distanceWaterFromRoad;
    [Range(0,10)] [SerializeField] private float aceptableDistanceToDestroySteps;

    private int
        planeCounter = 0,
        roadComplexCounter = 0;
    
    private float distanceBetweenSteps;

    private bool isFinishCreated;

    private Vector3 stepPosition, waterPlanePosition ;
     
    
    void Start()
    {
        waterPlanePosition = Vector3.down * (distanceWaterFromRoad + plane.transform.lossyScale.z/2);
        float distanceBetweenStartStepAndFirstStep =
            startStep.transform.lossyScale.z / 2 + peopleRoadStep.transform.lossyScale.z / 2;
        CreateStartStep(Vector3.zero);
        for (int i = 0; i < 2; i++)
        {
            CreateWaterPlane();
        }
        Vector3 complexStartPosition = Vector3.forward * distanceBetweenStartStepAndFirstStep;
        CreateRoadComplex(complexStartPosition);
      _rigidbodyPlayer = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!GameManager.Instance.IsTimeOut && GameManager.Instance.RushCounter < 4)
        {
            if (_rigidbodyPlayer.transform.position.z >= (peopleRoadStep.transform.lossyScale.z +
                                                          (roadComplexCounter * 3 *
                                                           peopleRoadStep.transform.lossyScale.z)))
            {
                CreateWaterPlane();
                Vector3 newComplexPosition = stepPosition + Vector3.forward * distanceBetweenSteps;
                CreateRoadComplex(newComplexPosition);
                roadComplexCounter++;
                DestroySteps("PeopleRoadStep");
                DestroySteps("DiamondRoadStep");
                DestroyPlanes();
            }
        }
        else
        {
            if (!isFinishCreated)
            {
                CreateWaterPlane();
                Vector3 newComplexPosition = stepPosition + Vector3.forward * distanceBetweenSteps;
                CreateFinishComplex(newComplexPosition);
                roadComplexCounter++;
                DestroySteps("PeopleRoadStep");
                DestroySteps("DiamondRoadStep");
                DestroyPlanes();
                isFinishCreated = true;
               
            }
        }
    }
    
    
    void CreateStartStep(Vector3 pPosition)
    {
        Instantiate(startStep, pPosition, startStep.transform.rotation);
    }

    void CreateWaterPlane()
    {
        Instantiate(plane, waterPlanePosition, plane.transform.rotation);
        planeCounter++;
        waterPlanePosition += Vector3.forward * plane.transform.localScale.z*10;
       
        
    }

    void CreateRoadComplex(Vector3 pStartPosition)
    {
        distanceBetweenSteps = peopleRoadStep.transform.lossyScale.z / 2 + diamondRoadStep.transform.lossyScale.z / 2;
        stepPosition = pStartPosition + Vector3.forward * distanceBetweenSteps;
        Instantiate(peopleRoadStep, pStartPosition, peopleRoadStep.transform.rotation);   // First Road
        Instantiate(diamondRoadStep, stepPosition, diamondRoadStep.transform.rotation);   //Second Road 
        stepPosition = pStartPosition + Vector3.forward * distanceBetweenSteps * 2;
        Instantiate(peopleRoadStep, stepPosition, peopleRoadStep.transform.rotation);
        
    }


    void CreateFinishComplex(Vector3 pStartPosition)
    {
        Instantiate(finishStep, pStartPosition, finishStep.transform.rotation); 
    }



    void DestroySteps(string tagForSearch)
    {
        GameObject[] roadStepsToDestroy = GameObject.FindGameObjectsWithTag(tagForSearch);
        foreach (var roadStep in roadStepsToDestroy)
        {
            if(_rigidbodyPlayer.transform.position.z > roadStep.transform.position.z + roadStep.transform.lossyScale.z/2 + aceptableDistanceToDestroySteps)
                Destroy(roadStep);
        }

        if (tagForSearch == "PeopleRoadStep")
        {
            DestroyGameElements("toEatPeople");
            DestroyGameElements("toAvoidPeople");
            
            
        }
        else if (tagForSearch == "DiamondRoadStep")
        {
            DestroyGameElements("Diamond");
            DestroyGameElements("Death");
        }
        
    }

    void DestroyGameElements(string tagForSearch)
    {
        GameObject[] elementsToDestroy = GameObject.FindGameObjectsWithTag(tagForSearch);
        foreach (var element in elementsToDestroy) 
        {
            if(_rigidbodyPlayer.transform.position.z > element.transform.position.z + element.transform.lossyScale.z/2 + aceptableDistanceToDestroySteps) 
                Destroy(element);
        }

    }

    void DestroyPlanes()   //A special function for planes. The scale of planes is x10. 
    {
        GameObject[] elementsToDestroy = GameObject.FindGameObjectsWithTag("Plane");
        foreach (var element in elementsToDestroy) 
        {
            if(_rigidbodyPlayer.transform.position.z > element.transform.position.z + element.transform.lossyScale.z/2 * 10 + aceptableDistanceToDestroySteps) 
                Destroy(element);
        }

        
    }



}
