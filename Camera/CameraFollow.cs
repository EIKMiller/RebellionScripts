using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _PlayerTransform;
    private Vector3 _Offset;

    [Header("Flipped Settings")]
    [SerializeField] private bool _Flipped = false;
    [SerializeField] private float _FlipAmount;

    [Header("Rotate Settings")]
    [SerializeField] private float _RotationSpeed;


    private void Awake()
    {
        if(!_PlayerTransform)
            _PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void Start()
    {
        _Offset = this.transform.position - _PlayerTransform.position;
    }

    public void FlipCamera()
    {
        _Flipped = !_Flipped;
        if(_Flipped)
        {
            Vector3 newPos = new Vector3(this.transform.position.x, this.transform.position.y, (this.transform.position.z + _FlipAmount));
            _Offset = newPos - _PlayerTransform.position;
            Quaternion quat = Quaternion.identity;
            quat.eulerAngles = new Vector3(0, 180, 0);
            this.transform.rotation = quat;
        } else 
        {
            Vector3 newPos = new Vector3(this.transform.position.x, this.transform.position.y, (this.transform.position.z - _FlipAmount));
            _Offset = newPos - _PlayerTransform.position;
            Quaternion quat = Quaternion.identity;
            quat.eulerAngles = new Vector3(0, 0, 0);
            this.transform.rotation = quat;
        }
    }


    private void LateUpdate()
    {
        //this.transform.position = _PlayerTransform.position + _Offset;
        if(Input.GetKey(KeyCode.Q))
        {
            this.transform.RotateAround(_PlayerTransform.position, Vector3.up, _RotationSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.E))
        {
            this.transform.RotateAround(_PlayerTransform.position, Vector3.up, -_RotationSpeed * Time.deltaTime);
        }
    }
}
