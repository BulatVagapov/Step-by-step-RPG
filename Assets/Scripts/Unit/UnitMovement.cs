using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitMovement : MonoBehaviour
{
    private Unit _unit;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    public Transform look;
    private MapLocation _previousPosition;
    public MapLocation MapPosition { get { return _previousPosition; } }
    
    public IEnumerator MoveAction(MapLocation destinationLocation)
    {
        _previousPosition.willBeClosed = false;
        destinationLocation.willBeClosed = true;

        if (!destinationLocation.Equals(transform.position))
        {
            _unit.Animator.SetBool("walk", true);

            while (destinationLocation.x != transform.position.x || destinationLocation.z != transform.position.z)
            {
                RotateUnitToTarget(destinationLocation);
                Move(destinationLocation);
                yield return new WaitForEndOfFrame();
            }

            if (destinationLocation.x >= transform.position.x && destinationLocation.z >= transform.position.z)
            {
                destinationLocation.isclosed = true;
                _previousPosition.isclosed = false;
                _unit.TurnIsOverEvent?.Invoke();
                _unit.Animator.SetBool("walk", false);
                _previousPosition = destinationLocation;
            }
        }
        else
        {
            _unit.TurnIsOverEvent?.Invoke();
        }
    }

    public bool RotateUnitToTarget(MapLocation destinationLocation)
    {
        Vector3 direction = new Vector3(destinationLocation.x, look.position.y, destinationLocation.z) - look.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        look.rotation = Quaternion.Lerp(look.rotation, rotation, _rotationSpeed * Time.deltaTime);

        if (Mathf.Abs(look.rotation.y) > Mathf.Abs(rotation.y) - 0.01 && Mathf.Abs(look.rotation.y) < Mathf.Abs(rotation.y) + 0.01)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private void Move(MapLocation destinationLocation)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(destinationLocation.x, transform.position.y, destinationLocation.z), _speed * Time.deltaTime);
    }

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    private void Start()
    {
        _previousPosition = MapManager.map.Find(x => x.Equals(new MapLocation(transform.position.x, transform.position.z)));
    }
}
