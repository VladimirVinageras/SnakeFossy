using UnityEngine;

public class FinishStepManager : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.WellDone();
        }

    }
}
