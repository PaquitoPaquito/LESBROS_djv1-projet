using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float cameraSmoothTime = 0f;
    
    private Vector3 _velocity = new Vector3(0, 0, 0);

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref _velocity, cameraSmoothTime);
    }
}
