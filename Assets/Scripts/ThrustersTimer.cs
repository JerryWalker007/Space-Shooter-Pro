using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrustersTimer : MonoBehaviour
{
    private Image _barImage;
    [SerializeField]
    private float _energy;
    [SerializeField]
    private float _maxEnergy = 5.0f;

    private bool _thrustersOn = false;
    private Player _player;

    public GameObject onCooldownText;
    public float useEnergy = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        _barImage = transform.Find("Bar").GetComponent<Image>();
        _energy = _maxEnergy;
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        _barImage.fillAmount = GetEnergyNormalized();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(_energy > 0)
            {
                onCooldownText.SetActive(false);
                _player.DisableThrusters = false;
                _energy -= Time.deltaTime;
                _energy = Mathf.Clamp(_energy, 0f, _maxEnergy);
                GetEnergyNormalized();
            }
            else
            {
                onCooldownText.SetActive(true);
                _player.DisableThrusters = true;
                
            }
        }
        else
        {
            _energy += Time.deltaTime;
            _energy = Mathf.Clamp(_energy, 0f, _maxEnergy);
            GetEnergyNormalized();
        }
    }

    public float GetEnergyNormalized()
    {
        return _energy / _maxEnergy;
    }
}
