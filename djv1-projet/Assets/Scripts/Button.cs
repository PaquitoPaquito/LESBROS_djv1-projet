using UnityEngine;

public class Button : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<EmergencyMeeting>(out var emergencyMeeting))
        {
            emergencyMeeting.canBeTriggered = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<EmergencyMeeting>(out var emergencyMeeting))
        {
            emergencyMeeting.canBeTriggered = false;
        }
    }
}
