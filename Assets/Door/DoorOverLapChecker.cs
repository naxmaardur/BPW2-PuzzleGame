using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOverLapChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerActor actor;
        if(other.TryGetComponent<PlayerActor>(out actor))
        {
            actor.SetCanShoot(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerActor actor;
        if (other.TryGetComponent<PlayerActor>(out actor))
        {
            actor.SetCanShoot(true);
        }
    }
}
