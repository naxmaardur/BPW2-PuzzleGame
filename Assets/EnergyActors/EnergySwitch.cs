using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySwitch : MonoBehaviour, IEnergyHolder
{
    [SerializeField]
    private int _energyCharges;
    private int _maxEnergyCharges = 1;
    [SerializeField]
    private GameObject _activeParticle;

    public int enegry { get { return _energyCharges; } set { _energyCharges = Mathf.Clamp(value, 0, _maxEnergyCharges); OnEnergyChanged(); } }
    public int maxEnegry { get { return _maxEnergyCharges; } set { } }

    public void AddEnergy()
    {
        enegry++;
    }

    public void TakeEnergy()
    {
        enegry--;
    }

    private void OnEnergyChanged()
    {
        if (_energyCharges > 0)
        {
            _activeParticle.SetActive(true);
        }
        else
        {
            _activeParticle.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
