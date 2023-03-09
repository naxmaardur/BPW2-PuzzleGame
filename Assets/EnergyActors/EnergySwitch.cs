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
    [SerializeField]
    private GameObject _leftImage;
    [SerializeField]
    private GameObject _rightImage;


    public bool startActive;


    public int enegry { get { return _energyCharges; } set { _energyCharges = Mathf.Clamp(value, 0, _maxEnergyCharges); OnEnergyChanged(value); } }
    public int maxEnegry { get { return _maxEnergyCharges; } set { } }

    public delegate void EnergyChangedEvent(int value);
    public EnergyChangedEvent OnEnergyChanged;

    public void AddEnergy()
    {
        enegry++;
    }

    public void TakeEnergy()
    {
        enegry--;
    }

    private void EnergyChanged(int value)
    {
        if (value > 0)
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
        OnEnergyChanged += EnergyChanged;
        EnergyChanged(_energyCharges);
        if (startActive)
        {
            AddEnergy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnHover(int charges)
    {
        if(charges > 0 && _energyCharges < maxEnegry)
        {
            _leftImage.SetActive(true);
        }
        else
        {
            _leftImage.SetActive(false);
        }

        if(charges < 3 && _energyCharges != 0){
            _rightImage.SetActive(true);
        }
        else
        {
            _rightImage.SetActive(false);
        }
    }

    void OnHoverEnd()
    {
        _leftImage.SetActive(false);
        _rightImage.SetActive(false);
    }
}
