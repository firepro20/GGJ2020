using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Public Variables
    public AudioClip buttonClick;
    public RectTransform mainMenu;
    public RectTransform controlsMenu;

    // Private Variables
    
    private AudioSource audioSource;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Calls the LoadScene Coroutine to allow delay for button click playback
    public void LoadGame()
    {
        StartCoroutine("LoadScene");
    }
    
    // Loads Controls, toggles Main Menu on/off
    public void ShowControls()
    {
        StartCoroutine("ToggleControls");        
    }

    public void ShowMenu()
    {
        StartCoroutine("ToggleMenu");
    }

    public void ExitGame()
    {
        audioSource.PlayOneShot(buttonClick);
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }
    private IEnumerator LoadScene()
    {
        audioSource.PlayOneShot(buttonClick);
        yield return new WaitForSeconds(0.5f);

        // Load Game Scene which should be index 1
        SceneManager.LoadScene(1);
    }

    private IEnumerator ToggleControls()
    {
        audioSource.PlayOneShot(buttonClick);
        yield return new WaitForSeconds(0.25f);

        // Load Controls and disables menu
        mainMenu.gameObject.SetActive(false);
        controlsMenu.gameObject.SetActive(true);
    }
    private IEnumerator ToggleMenu()
    {
        audioSource.PlayOneShot(buttonClick);
        yield return new WaitForSeconds(0.25f);

        // Load Controls and disables menu
        controlsMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);

    }



}
