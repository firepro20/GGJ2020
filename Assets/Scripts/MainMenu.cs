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
        Cursor.visible = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        StopAllCoroutines();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Calls the LoadScene Coroutine to allow delay for button click playback
    public void LoadGame()
    {
        audioSource.PlayOneShot(buttonClick);
        SceneManager.LoadScene("Game");
    }
    
    // Loads Controls, toggles Main Menu on/off
    public void ShowControls()
    {
        audioSource.PlayOneShot(buttonClick);
        // Load Controls and disables menu
        mainMenu.gameObject.SetActive(false);
        controlsMenu.gameObject.SetActive(true);
    }

    public void ShowMenu()
    {
        audioSource.PlayOneShot(buttonClick);
        // Load Controls and disables menu
        controlsMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        audioSource.PlayOneShot(buttonClick);
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }
}
