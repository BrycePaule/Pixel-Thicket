using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader _instance;

    [SerializeField] private Player _player;
    [SerializeField] private Animator _transition;
    [SerializeField] private Animator _fade;
    [SerializeField] private float _transitionTime = 0.5f;

    public bool _readyOut;

    public bool _finA;
    public bool _finB;

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
        if (_instance != null) { Destroy(this.gameObject); }
    }

    private void LoadGame()
    {
        StartCoroutine(LoadScene(1));
    }

    private void Reset()
    {
        _readyOut = false;
        _finA = false;
        _finB = false;
    }

    // TRANSITIONS
    public IEnumerator LoadScene(int sceneIndex)
    {
        // _transition.SetBool("Curtain", true);
        // yield return new WaitForSeconds(_transitionTime);

        SceneManager.LoadScene(sceneIndex);

        _transition.SetBool("Curtain", false);
        yield return new WaitForSeconds(_transitionTime);
    }

    public IEnumerator UnloadScene(int sceneIndex)
    {
        SceneManager.UnloadSceneAsync(sceneIndex);

        _transition.SetBool("Curtain", false);
        yield return new WaitForSeconds(_transitionTime);
    }

    public IEnumerator Transition()
    {
        StartCoroutine(FadeToBlack());

        while (!_readyOut & !_finA)
        {
            yield return null;
        }

        print(_finA);

        StartCoroutine(FadeFromBlack());

    }

    // EXIT TRANSITIONS
    public IEnumerator FadeToBlack()
    {   
        _fade.SetTrigger("FadeToBlack");
        yield return new WaitForSeconds(2);
        _fade.ResetTrigger("FadeToBlack");
        // _fade.SetBool("ScreenCovered", true);

        _finA = true;
    }

    // ENTER TRANSITIONS
    public IEnumerator FadeFromBlack()
    {
        _fade.SetTrigger("FadeFromBlack");
        yield return new WaitForSeconds(2);
        _fade.ResetTrigger("FadeFromBlack");
        // _fade.SetBool("ScreenCovered", false);

        Reset();
    }

    // TOTAL TRANSITIONS
    public IEnumerator BlankCrossfade()
    {
        _transition.SetBool("Curtain", true);
        _player._playerInput.Disable();
        _player.StopAllMovement();
        yield return new WaitForSeconds(_transitionTime);

        _transition.SetBool("Curtain", false);
        yield return new WaitForSeconds(_transitionTime * 0.5f);
        _player._playerInput.Enable();
    }

}
