using UnityEngine;

public class TaskCrewmateDetection : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<AiMovement>(out var aiMovement))
        {
            aiMovement.TaskReached(this.gameObject);
        }
    }
}
