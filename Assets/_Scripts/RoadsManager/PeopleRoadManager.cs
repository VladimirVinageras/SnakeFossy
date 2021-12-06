

using UnityEngine;



public class PeopleRoadManager : MonoBehaviour
{
    //Game Variables
    [SerializeField] private GameObject
        personToEat,
        personToAvoid,
        MealCompletedPoint,
        switcherColorPlane;
        

    private int isAPersonToEat; // Used to randomize spawn
   
    [Range(1,10)][SerializeField] private int
        numberOfGroups = 4,            
        numberOfPeople = 5;

    private Vector3 groupCreationPosition;
    private GameObject objectToInstantiate;
    
    //Physics Variables
    [SerializeField] private float 
        distanceFromEdges, 
        instantiationOffsetRange;
    
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

    private Color newPersonToEatColor;
    private Color newPersonToAvoidColor;
    
        
    // Start is called before the first frame update
    void Start()
    {
        XStepSize = transform.lossyScale.x;
        ZStepSize = transform.lossyScale.z;
        deltaXPosition = XStepSize / 4;                                      //Calculating the middle of each way
        deltaZPosition = (ZStepSize - (distanceFromEdges * 2)) / numberOfGroups;
        ZMinEdgeForInstantiation = transform.position.z - ZStepSize/2 + distanceFromEdges * 2;
        ZMaxEdgeForInstantiation = transform.position.z + ZStepSize/ 2 - distanceFromEdges/2;
        positionY = transform.position.y + transform.localScale.y/ 2F + personToEat.transform.lossyScale.y/ 2F;
        newPersonToEatColor = GameManager.Instance.GetColorForPeopleToEat();
        newPersonToAvoidColor = GameManager.Instance.GetColorForPeopleToAvoid();
        SpawnGroups();
     }


    private void SpawnGroups()
    {
       groupCreationPosition  = new Vector3(transform.position.x - deltaXPosition, positionY, ZMinEdgeForInstantiation);
        while (groupCreationPosition.z >= ZMinEdgeForInstantiation && groupCreationPosition.z <= ZMaxEdgeForInstantiation)
        {
            isAPersonToEat = Random.Range(0, 2);
            SpawnPeopleInPlace(groupCreationPosition);
            groupCreationPosition.x = transform.position.x + deltaXPosition;
            isAPersonToEat = isAPersonToEat == 0 ? 1 : 0;
            SpawnPeopleInPlace(groupCreationPosition);
            groupCreationPosition.z = groupCreationPosition.z + deltaZPosition;
            groupCreationPosition.x = transform.position.x - deltaXPosition;
        }
        
        GameObject[] peopleToEat = GameObject.FindGameObjectsWithTag("toEatPeople");
        foreach (var person in peopleToEat)
        {
            if (person.transform.position.z >= ZMinEdgeForInstantiation-instantiationOffsetRange  &&
                person.transform.position.z <= ZMaxEdgeForInstantiation + instantiationOffsetRange)  
            {
                person.gameObject.GetComponent<Renderer>().material.color = newPersonToEatColor;
            }
            
        }
        
        Vector3 switcherColorPosition = transform.position + 
                                        Vector3.back * transform.lossyScale.z * 0.5F +
                                       Vector3.up* transform.lossyScale.y * 0.5F;
     
        Instantiate(switcherColorPlane, switcherColorPosition, switcherColorPlane.transform.rotation);
        GameObject[] switchersToCheck = GameObject.FindGameObjectsWithTag("PainterPlane");

        foreach (var switcher in switchersToCheck)
        {
            if (switcher.transform.position == switcherColorPosition)
            {
                switcher.gameObject.GetComponent<Renderer>().material.color = newPersonToEatColor;
                
            }
        } 
        GameObject[] peopleToAvoid = GameObject.FindGameObjectsWithTag("toAvoidPeople");
        foreach (var person in peopleToAvoid)
        {
            person.gameObject.GetComponent<Renderer>().material.color = newPersonToAvoidColor;
        }

    }
    
    private void  SpawnPeopleInPlace(Vector3 pGroupPosition)
    {
        if (isAPersonToEat == 1)
        {
        
            PeopleInstantiation(pGroupPosition, personToEat);
            Instantiate(MealCompletedPoint, 
                pGroupPosition + Vector3.forward * instantiationOffsetRange * 1.1F,
                MealCompletedPoint.transform.rotation);
        }
        else
        {
        
            PeopleInstantiation(pGroupPosition, personToAvoid);
        }
    }

    private void PeopleInstantiation(Vector3 pGroupPosition, GameObject pObjectToInstantiate)
    {
        Vector3 personCreationPosition = new Vector3();
        for (int i = 0; i < numberOfPeople; i++)
        {
            personCreationPosition = pGroupPosition +
                                     Vector3.forward * Random.Range(-instantiationOffsetRange,
                                         instantiationOffsetRange) +
                                     Vector3.right * Random.Range(-instantiationOffsetRange,
                                         instantiationOffsetRange);
            pObjectToInstantiate.transform.Rotate(Vector3.up * Random.Range(0, 360));

            Instantiate(pObjectToInstantiate, personCreationPosition, pObjectToInstantiate.transform.rotation);
                
        }
    }
}
