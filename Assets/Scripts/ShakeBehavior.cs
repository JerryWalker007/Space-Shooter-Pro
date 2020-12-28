using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeBehavior : MonoBehaviour
{
    
    private Transform transform;
    [SerializeField]
    private float shakeDuration = 0f;
    [SerializeField]
    private float shakeMagnitude = 0.15f;
    [SerializeField]
    private float dampingSpeed = 1.0f;

    // The initial position of the GameObject
    Vector3 initialPosition;

    void Awake()
    {
        if (transform == null)
        {
            transform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }

    public void TriggerShake()
    {
        shakeDuration = 1.0f;
    }
}
