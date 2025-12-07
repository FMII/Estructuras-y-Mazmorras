using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Sistema de diálogos por sala que carga desde JSON
/// </summary>
public class DialogosSala : MonoBehaviour
{
    [Header("UI Referencias")]
    public GameObject canvasDialogos;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI continueText;
    public Image dialogBox;
    
    [Header("Configuración")]
    public TextAsset archivoJSON; // Arrastra el dialogos_salas.json aquí
    
    private DialogosSalaData datosDialogos;
    private string[] dialogosActuales;
    private int currentDialogIndex = 0;
    private bool dialogosActivos = false;
    private HashSet<int> salasVisitadas = new HashSet<int>(); // Para no repetir

    void Start()
    {
        CargarJSON();
        
        // Ocultar el canvas al inicio
        if (canvasDialogos != null)
            canvasDialogos.SetActive(false);
    }

    void Update()
    {
        // Solo procesar inputs si hay diálogos activos
        if (dialogosActivos && Input.GetKeyDown(KeyCode.Mouse0))
        {
            AvanzarDialogo();
        }
    }

    void CargarJSON()
    {
        if (archivoJSON == null)
        {
            Debug.LogError("No se asignó el archivo JSON de diálogos");
            return;
        }
        
        try
        {
            datosDialogos = JsonUtility.FromJson<DialogosSalaData>(archivoJSON.text);
            Debug.Log($"JSON cargado: {datosDialogos.salas.Count} salas");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error al cargar JSON: {ex.Message}");
        }
    }

    /// <summary>
    /// Llamado por SalaTrigger cuando el jugador entra a una sala
    /// </summary>
    public void MostrarDialogosDeSala(int salaID)
    {
        // Si ya visitó esta sala, no mostrar de nuevo
        if (salasVisitadas.Contains(salaID))
        {
            Debug.Log($"Sala {salaID} ya fue visitada");
            return;
        }
        
        // Buscar los diálogos de esta sala en el JSON
        SalaDialogos salaData = datosDialogos?.salas.Find(s => s.salaID == salaID);
        
        if (salaData != null && salaData.dialogos.Count > 0)
        {
            dialogosActuales = salaData.dialogos.ToArray();
            salasVisitadas.Add(salaID);
            MostrarPrimerDialogo();
            Debug.Log($"Mostrando {dialogosActuales.Length} diálogos de sala {salaID}: {salaData.nombreSala}");
        }
        else
        {
            Debug.LogWarning($"No hay diálogos para sala {salaID}");
        }
    }

    void MostrarPrimerDialogo()
    {
        currentDialogIndex = 0;
        dialogosActivos = true;
        Dialogs.dialogActive = true; // Bloquear movimiento del jugador
        
        // Mostrar canvas
        if (canvasDialogos != null)
            canvasDialogos.SetActive(true);
        
        if (dialogBox != null)
            dialogBox.gameObject.SetActive(true);
        
        if (continueText != null)
            continueText.gameObject.SetActive(true);
        
        // Mostrar primer diálogo
        if (dialogText != null && dialogosActuales.Length > 0)
        {
            dialogText.gameObject.SetActive(true);
            dialogText.text = dialogosActuales[currentDialogIndex];
        }
    }

    void AvanzarDialogo()
    {
        currentDialogIndex++;
        
        if (currentDialogIndex < dialogosActuales.Length)
        {
            // Mostrar siguiente diálogo
            if (dialogText != null)
            {
                dialogText.text = dialogosActuales[currentDialogIndex];
            }
        }
        else
        {
            // Terminar diálogos
            CerrarDialogos();
        }
    }

    void CerrarDialogos()
    {
        dialogosActivos = false;
        Dialogs.dialogActive = false; // Desbloquear movimiento
        
        // Ocultar canvas
        if (canvasDialogos != null)
            canvasDialogos.SetActive(false);
        
        if (dialogBox != null)
            dialogBox.gameObject.SetActive(false);
        
        if (continueText != null)
            continueText.gameObject.SetActive(false);
        
        if (dialogText != null)
            dialogText.gameObject.SetActive(false);
        
        Debug.Log("Diálogos completados");
    }

    /// <summary>
    /// Resetear salas visitadas (para testing)
    /// </summary>
    public void ResetearSalasVisitadas()
    {
        salasVisitadas.Clear();
        Debug.Log("Salas visitadas reseteadas");
    }
}
