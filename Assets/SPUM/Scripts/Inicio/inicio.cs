using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class inicio : MonoBehaviour
{
    public TextMeshProUGUI wText, aText, sText, dText, eText, spaceText;
    public Button wbutton, abutton, sbutton, dbutton, ebutton, spacebutton;
    public Button botonNewGame, botonContinue, botonSalir, botonControles, botonRegresar;
    void Start()
    {
        botonRegresar.gameObject.SetActive(false);
        wbutton.gameObject.SetActive(false);
        abutton.gameObject.SetActive(false);
        sbutton.gameObject.SetActive(false);
        dbutton.gameObject.SetActive(false);
        ebutton.gameObject.SetActive(false);
        spacebutton.gameObject.SetActive(false);
        wText.gameObject.SetActive(false);
        aText.gameObject.SetActive(false);
        sText.gameObject.SetActive(false);
        dText.gameObject.SetActive(false);
        eText.gameObject.SetActive(false);
        spaceText.gameObject.SetActive(false);
    }

    public void BotonNewGame()
    {
        PlayerPrefs.SetInt("num_vidas", 3);
        PlayerPrefs.SetInt("camino_id", 1);
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

    public void BotonControles()
    {
        botonContinue.gameObject.SetActive(false);
        botonNewGame.gameObject.SetActive(false);
        botonSalir.gameObject.SetActive(false);
        botonControles.gameObject.SetActive(false);
        botonRegresar.gameObject.SetActive(true);
        wbutton.gameObject.SetActive(true);
        abutton.gameObject.SetActive(true);
        sbutton.gameObject.SetActive(true);
        dbutton.gameObject.SetActive(true);
        ebutton.gameObject.SetActive(true);
        spacebutton.gameObject.SetActive(true);
        wText.gameObject.SetActive(true);
        aText.gameObject.SetActive(true);
        sText.gameObject.SetActive(true);
        dText.gameObject.SetActive(true);
        eText.gameObject.SetActive(true);
        spaceText.gameObject.SetActive(true);
        Dialogs.dialogActive = true;
    }

    public void BotonRegresar()
    {
        botonContinue.gameObject.SetActive(true);
        botonNewGame.gameObject.SetActive(true);
        botonSalir.gameObject.SetActive(true);
        botonControles.gameObject.SetActive(true);
        botonRegresar.gameObject.SetActive(false);
        wbutton.gameObject.SetActive(false);
        abutton.gameObject.SetActive(false);
        sbutton.gameObject.SetActive(false);
        dbutton.gameObject.SetActive(false);
        ebutton.gameObject.SetActive(false);
        spacebutton.gameObject.SetActive(false);
        wText.gameObject.SetActive(false);
        aText.gameObject.SetActive(false);
        sText.gameObject.SetActive(false);
        dText.gameObject.SetActive(false);
        eText.gameObject.SetActive(false);
        spaceText.gameObject.SetActive(false);
        Dialogs.dialogActive = false;
    }

    void Update()
    {
        
    }
}
