using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] private Animator _transition;
    [SerializeField] private float _transitionTime = 1;

    private static SceneLoader _instance;

    public static SceneLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SceneLoader>();
                if (_instance == null)
                {
                    _instance = new SceneLoader();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null) { Destroy(this); }
        DontDestroyOnLoad(this);
    }

    private void LoadGame()
    {
        StartCoroutine(LoadScene(1));
    }

    public IEnumerator LoadScene(int sceneIndex)
    {
        FadeToBlack();
        yield return new WaitForSeconds(_transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }

    // TRANSITIONS
    public void FadeFromBlack()
    {
        _transition.SetBool("Curtain", true);
    }

    public void FadeToBlack()
    {
        _transition.SetBool("Curtain", false);
    }



    // UTILITIES
    private enum SceneIndexes
    {
        MAIN_MENU = 0,
        GAME = 1,
    }


}
