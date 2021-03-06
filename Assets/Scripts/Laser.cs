﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    [SerializeField]
    private bool _isEnemyLaser = false;

    private ShakeBehavior _shakeBehavior;

    private void Awake()
    {
        _shakeBehavior = GameObject.Find("MainCamera").GetComponent<ShakeBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
       if (_isEnemyLaser == false)
        {
            MoveUp();
        }
       else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y >= 7f)
        {
              if (transform.parent != null)
             {
                 Destroy(transform.parent.gameObject);
             }

            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7f)
        {
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
        if (transform.position.y < -3.5f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                _shakeBehavior.TriggerShake();
            }
        }
    }
}
