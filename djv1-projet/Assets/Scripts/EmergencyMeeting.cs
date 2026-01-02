using UnityEngine;

public class EmergencyMeeting : MonoBehaviour
{
    public bool canBeTriggered = false;

    [SerializeField] private float mapRadius = 55f;

    private Collider[] _colliders = new Collider[128];
    private bool _isTrigerred = false;
    
    void Update()
    {
        if (canBeTriggered)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartEmergencyMeeting();
            }
        }

        if (_isTrigerred)
        {
            _isTrigerred = false;
        }
    }

    private void StartEmergencyMeeting()
    {
        int size = Physics.OverlapSphereNonAlloc(Vector3.zero, mapRadius, _colliders);
        for  (int i = 0; i < size; i++)
        {
            if (_colliders[i].TryGetComponent<AiMovement>(out var ai))
            {
                ai.ResetPosition();
            }
            else if (_colliders[i].gameObject.TryGetComponent<PlayerMovement>(out var player))
            {
                player.ResetPosition();
            }
        }
        _isTrigerred = true;
    }
}
