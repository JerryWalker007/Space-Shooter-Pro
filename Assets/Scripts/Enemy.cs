﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.0f;
    [SerializeField]
    private GameObject _laserPrefab;

    private Player _player;
    private Animator _anim;

    private AudioSource _audioSource;
    private float _fireRate = 3.0f;
    private float _canFire = -1;

    [SerializeField]
    private float _frequency = 7.0f;
    [SerializeField]
    private float _magnitude = 0.1f;
    private Vector3 _axis;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        //null check player
        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource for explosion is NULL.");
        }

        _axis = transform.right;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        //transform.Translate(Vector3.down * _speed * Time.deltaTime);
        //transform.position = (_pos + _axis * Mathf.Sin(Time.time * _frequency) * _magnitude);
        transform.Translate((Vector3.down * _speed *Time.deltaTime) + _axis * Mathf.Sin(Time.time * _frequency) * _magnitude);

        if (transform.position.y <= -7f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 8, 0);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            Destroy(this.gameObject, 2.8f);
            
        }
        else if (other.tag == "Laser" || other.tag == "HeatSeekingLaser")
        {
            Destroy(other.gameObject);
            
            if (_player != null)
            {
                _player.AddScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
            
        }
    }
}
