using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogs : MonoBehaviour
{
    public TextMeshProUGUI dialogText, continueText;
    public Image dialogBox;
    public string[] dialogs;
    private int currentDialogIndex = 0;
    void Start()
    {
        dialogs = new string[]
        {
            "Bienvenido a la Masmorra", 
            "Has caido en lo mas temido sin saber del estudiante promedio", 
            "Estructuras y mas estructuras te abordaran en esta rebuscada aventura",
            "En el camino te encontraras con una serie de desafios logicos y uno que otro enemigo",
            "Pero no temas, manten la calma y sigue adelante, esto se lleva bastante facil analizando",
            "Sin mas te deseo una buena suerte"
        };
        
        if (dialogText != null && dialogs.Length > 0)
        {
            dialogText.text = dialogs[currentDialogIndex];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentDialogIndex < dialogs.Length)
        {
            currentDialogIndex++;
            if (currentDialogIndex < dialogs.Length)
            {
                if (dialogText != null)
                {
                    dialogText.text = dialogs[currentDialogIndex];
                }
            }
            else
            {
                if (dialogBox != null) dialogBox.gameObject.SetActive(false);
                if (continueText != null) continueText.gameObject.SetActive(false);
                if (dialogText != null) dialogText.gameObject.SetActive(false);
            }
        }
        
    }
}
