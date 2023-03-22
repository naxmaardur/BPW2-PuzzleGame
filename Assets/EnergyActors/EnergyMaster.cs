using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyMaster : MonoBehaviour
{
    [SerializeField]
    private EnergySwitch[] _energySwitches;
    [SerializeField]
    private GameObject[] _targets;
    private int _totalEnergy;
    [SerializeField]
    private EnergyDisplay[] _energyDisplays;
    public bool overwriteEnergyNeeded;
    public float energyNeededOverwriteValue;
    private AudioSource _source;

    // Start is called before the first frame update
    void Awake()
    {
        _source = GetComponent<AudioSource>();
        foreach (EnergySwitch energySwitch in _energySwitches)
        {
            energySwitch.OnEnergyChanged += OnEnergyChanged;
        }
    }

    void OnEnergyChanged(int value)
    {
        if(value > 0)
        {
            _totalEnergy++;
        }
        else
        {
            _totalEnergy--;
        }
        foreach(EnergyDisplay display in _energyDisplays)
        {
            display.UpdateLights(_totalEnergy);
        }
        if (_totalEnergy >= _energySwitches.Length)
        {
            Activate(true);
            _source.Play();
            return;
        }
        if(overwriteEnergyNeeded && _totalEnergy >= energyNeededOverwriteValue)
        {
            Activate(true);
            _source.Play();
            return;
        }
        Activate(false);
    }

    void Activate(bool b)
    {
        Debug.Log(b);
        foreach (GameObject target in _targets)
        {
            target.SendMessage("SetActiveState", b);
        }
    }
}
