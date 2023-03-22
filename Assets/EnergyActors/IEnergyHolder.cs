using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnergyHolder
{
    public int enegry { get; set; }
    public int maxEnegry { get; set; }
    public void AddEnergy();
    public void TakeEnergy();
}
