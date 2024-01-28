using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _cameraParent;

    private void Awake()
    {
        transform.parent = _cameraParent;
        transform.localPosition = Vector3.zero;
    }

    void LateUpdate()
    {
       transform.LookAt(_player.position);
    }
}
