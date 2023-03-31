//using statements
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //declares components needed for audio
    AudioSource music;
    public AudioClip button;
    

    //changes scene to the one given in the button onclick
    public void sceneChange(string name){

        //initializes and plays button sound effect before changing scene
        music = GetComponent<AudioSource>();
        music.PlayOneShot(button, 1);
        SceneManager.LoadScene(name);
    }

    //quits application
    public void close(){
        Application.Quit();
    }


}
