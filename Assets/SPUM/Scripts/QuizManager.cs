using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Sistema de preguntas aleatorias al pasar puertas finales
/// </summary>
public class QuizManager : MonoBehaviour
{
    [Header("Banco de Preguntas")]
    public PreguntaData[] preguntasRecursion;
    public PreguntaData[] preguntasArreglos;
    public PreguntaData[] preguntasGenericos;
    
    [Header("UI del Quiz")]
    public GameObject panelQuiz;
    public TextMeshProUGUI textoPregunta;
    public Button[] botonesOpciones; // 4 botones (A, B, C, D)
    public TextMeshProUGUI[] textosOpciones; // Textos de cada botón
    public TextMeshProUGUI textoFeedback;
    
    [Header("Colores")]
    public Color colorNormal = Color.white;
    public Color colorCorrecto = Color.green;
    public Color colorIncorrecto = Color.red;
    
    private PreguntaData preguntaActual;
    private QuizDoor puertaActual;
    private bool esperandoRespuesta = false;

    void Start()
    {
        // Cargar preguntas predefinidas si los arrays están vacíos
        if (preguntasRecursion == null || preguntasRecursion.Length == 0)
            preguntasRecursion = PreguntaData.ObtenerPreguntasRecursion();
            
        if (preguntasArreglos == null || preguntasArreglos.Length == 0)
            preguntasArreglos = PreguntaData.ObtenerPreguntasArreglos();
            
        if (preguntasGenericos == null || preguntasGenericos.Length == 0)
            preguntasGenericos = PreguntaData.ObtenerPreguntasGenericos();
        
        if (panelQuiz != null)
            panelQuiz.SetActive(false);
            
        // Configurar botones
        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            int index = i; // Capturar el índice
            botonesOpciones[i].onClick.AddListener(() => SeleccionarRespuesta(index));
        }
    }

    /// <summary>
    /// Mostrar pregunta aleatoria de los 3 temas
    /// </summary>
    public void MostrarPreguntaAleatoria(QuizDoor puerta)
    {
        puertaActual = puerta;
        
        // Asegurarse de que las preguntas estén cargadas
        if (preguntasRecursion == null || preguntasRecursion.Length == 0)
            preguntasRecursion = PreguntaData.ObtenerPreguntasRecursion();
            
        if (preguntasArreglos == null || preguntasArreglos.Length == 0)
            preguntasArreglos = PreguntaData.ObtenerPreguntasArreglos();
            
        if (preguntasGenericos == null || preguntasGenericos.Length == 0)
            preguntasGenericos = PreguntaData.ObtenerPreguntasGenericos();
        
        // Elegir tema aleatorio
        int temaAleatorio = Random.Range(0, 3);
        PreguntaData[] bancoPreguntas;
        
        switch (temaAleatorio)
        {
            case 0:
                bancoPreguntas = preguntasRecursion;
                Debug.Log("Tema seleccionado: Recursión");
                break;
            case 1:
                bancoPreguntas = preguntasArreglos;
                Debug.Log("Tema seleccionado: Arreglos");
                break;
            default:
                bancoPreguntas = preguntasGenericos;
                Debug.Log("Tema seleccionado: Genéricos");
                break;
        }
        
        // Elegir pregunta aleatoria del tema
        if (bancoPreguntas != null && bancoPreguntas.Length > 0)
        {
            int indiceAleatorio = Random.Range(0, bancoPreguntas.Length);
            preguntaActual = bancoPreguntas[indiceAleatorio];
            
            if (preguntaActual != null && !string.IsNullOrEmpty(preguntaActual.pregunta))
            {
                Debug.Log("Pregunta cargada: " + preguntaActual.pregunta);
                MostrarPregunta();
            }
            else
            {
                Debug.LogError("Pregunta vacía detectada! Recargando...");
                MostrarPreguntaAleatoria(puerta);
            }
        }
        else
        {
            Debug.LogError("No hay preguntas en el banco seleccionado!");
        }
    }

    void MostrarPregunta()
    {
        if (preguntaActual == null) return;
        
        // Mostrar panel
        panelQuiz.SetActive(true);
        esperandoRespuesta = true;
        
        // Bloquear movimiento del jugador usando Dialogs
        Dialogs.dialogActive = true;
        
        // Mostrar pregunta
        textoPregunta.text = "[" + preguntaActual.tema + "]\n\n" + preguntaActual.pregunta;
        
        // Mostrar opciones
        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            if (i < preguntaActual.opciones.Length)
            {
                textosOpciones[i].text = preguntaActual.opciones[i];
                botonesOpciones[i].gameObject.SetActive(true);
                botonesOpciones[i].interactable = true;
                botonesOpciones[i].GetComponent<Image>().color = colorNormal;
            }
            else
            {
                botonesOpciones[i].gameObject.SetActive(false);
            }
        }
        
        // Limpiar feedback
        if (textoFeedback != null)
            textoFeedback.text = "";
    }

    void SeleccionarRespuesta(int opcion)
    {
        if (!esperandoRespuesta) return;
        
        esperandoRespuesta = false;
        
        // Verificar si es correcta
        if (opcion == preguntaActual.respuestaCorrecta)
        {
            // ¡CORRECTO!
            botonesOpciones[opcion].GetComponent<Image>().color = colorCorrecto;
            
            if (textoFeedback != null)
                textoFeedback.text = "✅ ¡CORRECTO!\n" + preguntaActual.explicacion;
            
            // Desactivar botones
            foreach (Button btn in botonesOpciones)
                btn.interactable = false;
            
            // Permitir continuar después de 3 segundos
            Invoke("CerrarYTeleportar", 3f);
        }
        else
        {
            // ❌ INCORRECTO
            botonesOpciones[opcion].GetComponent<Image>().color = colorIncorrecto;
            
            if (textoFeedback != null)
                textoFeedback.text = "❌ Incorrecto. Intenta de nuevo.";
            
            // Deshabilitar la opción incorrecta
            botonesOpciones[opcion].interactable = false;
            
            // Permitir reintentar (cambiar pregunta después de 2 segundos)
            Invoke("CambiarPregunta", 2f);
        }
    }

    void CambiarPregunta()
    {
        // Mostrar otra pregunta aleatoria
        MostrarPreguntaAleatoria(puertaActual);
    }

    void CerrarYTeleportar()
    {
        // Cerrar panel
        panelQuiz.SetActive(false);
        Dialogs.dialogActive = false; // Desbloquear inputs
        
        // Teleportar al jugador
        if (puertaActual != null)
        {
            puertaActual.TeleportarJugador();
        }
        
        preguntaActual = null;
        puertaActual = null;
    }

    /// <summary>
    /// Cerrar quiz sin teleportar (por si cancela)
    /// </summary>
    public void CerrarQuiz()
    {
        panelQuiz.SetActive(false);
        Dialogs.dialogActive = false;
        esperandoRespuesta = false;
        preguntaActual = null;
        puertaActual = null;
    }
}
