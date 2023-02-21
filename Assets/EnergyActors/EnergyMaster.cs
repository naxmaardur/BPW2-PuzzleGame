using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyMaster : MonoBehaviour
{
    [SerializeField]
    EnergySwitch[] energySwitches;
    public GameObject[] targets;
    int totalEnergy;

    // Start is called before the first frame update
    void Start()
    {
        foreach (EnergySwitch energySwitch in energySwitches)
        {
            energySwitch.OnEnergyChanged += OnEnergyChanged;
        }
    }

    void OnEnergyChanged(int value)
    {
        if(value > 0)
        {
            totalEnergy++;
        }
        else
        {
            totalEnergy--;
        }
        if (totalEnergy >= energySwitches.Length)
        {
            Activate(true);
            return;
        }
        Activate(false);
    }


    void Activate(bool b)
    {
        Debug.Log(b);
        foreach (GameObject target in targets)
        {
            target.SendMessage("SetActiveState", b);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
