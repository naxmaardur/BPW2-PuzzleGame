using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LeveLTransition : MonoBehaviour
{

    [SerializeField] private int _targetSceneIndex;
    private int _targetsInside;


    private void OnTriggerEnter(Collider other)
    {
        PlayerActor p;
        if (other.TryGetComponent<PlayerActor>(out p))
        {
            _targetsInside++;
            InsideUpdated(_targetsInside);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerActor p;
        if (other.TryGetComponent<PlayerActor>(out p))
        {
            _targetsInside--;
        }
    }

    void InsideUpdated(int count)
    {
        if (count == 2)
        {
            SceneManager.LoadScene(_targetSceneIndex);
        }
    }

}