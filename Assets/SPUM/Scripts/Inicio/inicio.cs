using UnityEngine;
using UnityEngine.SceneManagement;

public class inicio : MonoBehaviour
{


    void Start()
    {

    }

    public void BotonNewGame()
    {
        PlayerPrefs.SetInt("num_vidas", 3);
        PlayerPrefs.SetInt("camino_id", 0);
        SceneManager.LoadScene(0);
    }

    public void BotonContinue()
    {
        if (PlayerPrefs.HasKey("camino_id"))
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            PlayerPrefs.SetInt("num_vidas", 3);
            SceneManager.LoadScene(0);
        }
    }

    public void BotonSalir()
    {
        Application.Quit();
    }

    void Update()
    {
        
    }
}
