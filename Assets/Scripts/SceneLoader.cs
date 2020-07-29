using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] private Player _player;
    [SerializeField] private Animator _transition;
    [SerializeField] private float _transitionTime = 0.5f;

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

    // TRANSITIONS
    public IEnumerator LoadScene(int sceneIndex)
    {
        _transition.SetBool("Curtain", true);
        yield return new WaitForSeconds(_transitionTime);

        SceneManager.LoadScene(sceneIndex);

        _transition.SetBool("Curtain", false);
        yield return new WaitForSeconds(_transitionTime);
    }

    public IEnumerator FadeFromBlack()
    {
        _transition.SetBool("Curtain", true);
        yield return new WaitForSeconds(_transitionTime);
    }

    public IEnumerator FadeToBlack()
    {
        _transition.SetBool("Curtain", false);
        yield return new WaitForSeconds(_transitionTime);
    }

    public IEnumerator BlankCrossfade()
    {
        _transition.SetBool("Curtain", true);
        _player._playerInput.Disable();
        _player.StopAllMovement();
        yield return new WaitForSeconds(_transitionTime);

        _transition.SetBool("Curtain", false);
        yield return new WaitForSeconds(_transitionTime);
        _player._playerInput.Enable();
    }

    // UTILITIES
    private enum SceneIndexes
    {
        MAIN_MENU = 0,
        GAME = 1,
    }


}
