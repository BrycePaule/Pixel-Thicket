using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] private Animator _transition;
    [SerializeField] private float _transitionTime = 1;



    private void LoadGame()
    {
        StartCoroutine(LoadScene(1));
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        // player animation
        _transition.SetTrigger("Start");

        // wait
        yield return new WaitForSeconds(_transitionTime);

        // load scene
        SceneManager.LoadScene(sceneIndex);
    }
}
