using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelTransition : MonoBehaviour
{
    [SerializeField] 
    private int _targetSceneIndex;
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
            LoadLevel();
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(_targetSceneIndex);
    }

    public void Quit()
    {
        Application.Quit(); 
    }
}
