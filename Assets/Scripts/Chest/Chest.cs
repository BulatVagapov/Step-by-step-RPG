using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private GameObject _coins;
    public GameObject Coins { get { return _coins; } }
    [SerializeField] private List<ParticleSystem> _explosionParticalSystems;
    public List<ParticleSystem> ExplosionParticalSystems { get { return _explosionParticalSystems; } }

    [SerializeField] private GameObject _explosionTrace;
    public GameObject ExplosionTrace { get { return _explosionTrace; } }
    [SerializeField] private float _explosionDamage;
    public float ExplosionDamage { get { return _explosionDamage; } }
    private IChestAction _currentChestAction;
    private UnityEvent _chestIsOpenEvent = new UnityEvent();
    public UnityEvent ChestIsOpenEvent { get { return _chestIsOpenEvent; } }

    public IEnumerator OpenChest()
    {
        MapManager.player.Animator.SetBool("walk", true);

        while (!MapManager.player.Movement.RotateUnitToTarget(MapManager.map.Find(x => x.Equals(transform.position))))
        {
            yield return new WaitForEndOfFrame();
        }

        MapManager.player.Animator.SetBool("walk", false);

        _animator.SetTrigger("Open");

        //yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length - 0.4f);

        yield return new WaitForSeconds(0.5f);
        //Do some action, like explosion
        _chestIsOpenEvent?.Invoke();
        MapManager.player.TurnIsOverEvent?.Invoke();
    }

    private void SelectChestAction()
    {
        int i = Random.Range(0, 2);

        switch (i)
        {
            case 0:
                _currentChestAction =  new TreasureInsideAction(this);
                break;
            case 1:
                _currentChestAction =  new ExplosionChestAction(this);
                break;
        }
    }


    void Awake()
    {
        _animator = GetComponent<Animator>();
        _coins.SetActive(false);
        SelectChestAction();
        _chestIsOpenEvent.AddListener(_currentChestAction.DoChestAction);
        _explosionTrace.SetActive(false);
    }

    private void Start()
    {
        if (MapManager.map.Exists(x => x.Equals(transform.position)))
        {
            MapManager.map.Find(x => x.Equals(transform.position)).isclosed = true;
        }
    }
}
