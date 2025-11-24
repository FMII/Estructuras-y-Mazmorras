using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SQLite4Unity3d;
using System.Collections.Generic;

public class Dialogs : MonoBehaviour
{
    public TextMeshProUGUI dialogText, continueText;
    public Image dialogBox;
    public string[] dialogs;
    private int currentDialogIndex = 0;

    void Start()
    {
        LoadDialogsFromDB();
        
        if (dialogText != null && dialogs != null && dialogs.Length > 0)
        {
            dialogText.text = dialogs[currentDialogIndex];
        }
    }

    void LoadDialogsFromDB()
    {
        string dbPath = System.IO.Path.Combine(Application.streamingAssetsPath, "juegointegradora.db");
        
        try
        {
            var db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly);
            var query = "SELECT d.texto FROM dialogos d JOIN camino_sala_dialogos csd ON d.id = csd.dialogo_id JOIN camino_sala cs ON csd.camino_sala_id = cs.id WHERE cs.camino_id = 0";
            var results = db.Query<DialogoRow>(query);
            
            var dialogList = new List<string>();
            foreach (var row in results)
            {
                dialogList.Add(row.texto);
            }
            dialogs = dialogList.ToArray();
            
            db.Close();
            Debug.Log("Dialogos cargados desde DB: " + dialogs.Length);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error cargando dialogos desde DB: " + ex.Message);
            // Fallback a dialogos por defecto
            dialogs = new string[] 
            { "Bienvenido al juego.", "Este es un dialogo de prueba.", "Disfruta jugando!"
            };
        }
    }

    public class DialogoRow
    {
        public string texto { get; set; }
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
