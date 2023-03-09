using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDisplay : MonoBehaviour
{
    [SerializeField]
    private Light[] _ligts;

    public void UpdateLights(int i)
    {
        i--;
        for(int x = 0; x < _ligts.Length; x++)
        {
            if(x <= i)
            {
                _ligts[x].color = Color.green;
            }
            else
            {
                _ligts[x].color = Color.red;
            }
        }


    }

}
