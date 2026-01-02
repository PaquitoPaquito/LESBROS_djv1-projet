using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    public bool isDead;
    
    // time spent by a crewmate on a task
    [SerializeField] private float cooldown = 10f;
    [SerializeField] private Vector3 startingPosition;
    [SerializeField] private float mapRadius = 55f;
    
    // list of all the possible destinations for the crewmate to do tasks (or make believe)
    private List<GameObject> _possibleTargets = new List<GameObject>();
    private Collider[] _colliders = new Collider[128];
    private List<GameObject> _nextTargets = new List<GameObject>();
    private GameObject _target;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private int _runHashCode;
    private int _diesHashCode;

    IEnumerator Start()
    {
        _navMeshAgent.isStopped = true;
        ResetPosition();
        InitialisePossibleTargets();
        ResetCurrentTargets();
        yield return new WaitForSeconds(2);
        _animator.SetBool(_runHashCode, true);
        _navMeshAgent.isStopped = false;
    }
    
    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _runHashCode = Animator.StringToHash("IsRunning");
        _diesHashCode = Animator.StringToHash("Dies");
    }

    // Called whenever the crewmate passes through a target's detection range
    public void TaskReached(GameObject task)
    {
        if (task.Equals(_target))
        {
            StartCoroutine(WaitForTaskCompletion());
            ChooseNextTarget();
        }
    }

    public void ResetPosition()
    {
        transform.position = startingPosition;
        transform.rotation = Quaternion.LookRotation(-startingPosition);
    }

    public void Kill() 
    {
        // I left the dead bodies with their colliders because it is funnier
        isDead = true;
        _animator.SetTrigger(_diesHashCode);
        _navMeshAgent.isStopped = true;
    }

    IEnumerator WaitForTaskCompletion()
    {
        _animator.SetBool(_runHashCode, false);
        _navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(cooldown);
        _animator.SetBool(_runHashCode, true);
        _navMeshAgent.isStopped = false;
    }

    void InitialisePossibleTargets()
    {
        int size = Physics.OverlapSphereNonAlloc(Vector3.zero, mapRadius, _colliders);
        for  (int i = 0; i < size; i++)
        {
            if (_colliders[i].TryGetComponent<TaskCrewmateDetection>(out var targetCollider))
            {
                _possibleTargets.Add(targetCollider.gameObject);
            }
        }
    }
    
    void ChooseNextTarget()
    {
        // if there is no next target, we reset the list of next targets
        if (_nextTargets.Count == 0)
        {
            ResetCurrentTargets();
        }
        
        // we set the AI's destination as the first element of _nextTargets and remove it from it
        _target = _nextTargets[0];
        _navMeshAgent.SetDestination(_target.transform.position);
        _nextTargets.RemoveAt(0);
    }

    void ResetCurrentTargets()
    {
        // copy the list of possible targets
        _nextTargets.Clear();
        _nextTargets = new List<GameObject>(_possibleTargets);
        
        // shuffle it
        for (int i = 0; i < _nextTargets.Count; i++)
        {
            int j = Random.Range(0, _nextTargets.Count);
            (_nextTargets[i],  _nextTargets[j]) = (_nextTargets[j], _nextTargets[i]);
        }
        
        // change current target
        ChooseNextTarget();
    }
}
