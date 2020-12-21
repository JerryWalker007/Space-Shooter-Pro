using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSeekingLaser : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float _speed = 8f;
    private float _rotateSpeed = 200f;
    private float _angle;
    private Vector2 _lastPosition;

    private Rigidbody2D _rigidbody2D;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LockOnTarget();
    }

    void LockOnTarget()
    {
        Vector2 direction = (Vector2)_target.position - _rigidbody2D.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        _rigidbody2D.angularVelocity = -rotateAmount * _rotateSpeed;

        _rigidbody2D.velocity = transform.up * _speed;
    }
}
