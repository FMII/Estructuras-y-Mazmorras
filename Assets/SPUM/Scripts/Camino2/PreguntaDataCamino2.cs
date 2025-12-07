using UnityEngine;

/// <summary>
/// Datos de una pregunta sobre Pilas, Colas y Listas
/// </summary>
[System.Serializable]
public class PreguntaDataCamino2
{
    [Header("Pregunta")]
    public string tema; // "Pilas", "Colas", "Listas"
    [TextArea(3, 5)]
    public string pregunta;
    
    [Header("Opciones")]
    public string[] opciones = new string[4]; // A, B, C, D
    public int respuestaCorrecta; // 0=A, 1=B, 2=C, 3=D
    
    [Header("Feedback")]
    [TextArea(2, 3)]
    public string explicacion; // Se muestra al responder
    
    // PREGUNTAS PREDEFINIDAS - PILAS
    public static PreguntaDataCamino2[] ObtenerPreguntasPilas()
    {
        return new PreguntaDataCamino2[]
        {
            new PreguntaDataCamino2
            {
                tema = "Pilas (Stack)",
                pregunta = "¿Cuál es el orden de una pila?",
                opciones = new string[] { "El último en entrar, primero en salir", "El primero en entrar, primero en salir", "Orden aleatorio", "Orden alfabético" },
                respuestaCorrecta = 0,
                explicacion = "Las pilas funcionan como una pila de platos: sacas primero el de arriba (el último que pusiste)."
            },
            new PreguntaDataCamino2
            {
                tema = "Pilas (Stack)",
                pregunta = "Si pongo 3 bloques uno sobre otro (Verde, Azul, Rojo), ¿cuál quito primero?",
                opciones = new string[] { "Rojo", "Verde", "Azul", "Todos juntos" },
                respuestaCorrecta = 0,
                explicacion = "El último bloque que pusiste (Rojo) está arriba, así que lo quitas primero."
            },
            new PreguntaDataCamino2
            {
                tema = "Pilas (Stack)",
                pregunta = "¿Qué pasa si intento quitar un bloque de una pila vacía?",
                opciones = new string[] { "Da error", "No pasa nada", "Aparece un bloque nuevo", "Se llenan todos" },
                respuestaCorrecta = 0,
                explicacion = "No puedes quitar bloques si no hay ninguno. Eso causa un error."
            }
        };
    }
    
    // PREGUNTAS PREDEFINIDAS - COLAS
    public static PreguntaDataCamino2[] ObtenerPreguntasColas()
    {
        return new PreguntaDataCamino2[]
        {
            new PreguntaDataCamino2
            {
                tema = "Colas (Queue)",
                pregunta = "¿Cuál es el orden de una cola?",
                opciones = new string[] { "El primero en llegar, primero en salir", "El último en llegar, primero en salir", "Orden aleatorio", "Todos a la vez" },
                respuestaCorrecta = 0,
                explicacion = "Las colas funcionan como una fila de personas: quien llega primero, es atendido primero."
            },
            new PreguntaDataCamino2
            {
                tema = "Colas (Queue)",
                pregunta = "Si llegan 3 enemigos en orden (Rojo, Azul, Verde), ¿cuál debo matar primero?",
                opciones = new string[] { "Rojo", "Verde", "Azul", "El que yo quiera" },
                respuestaCorrecta = 0,
                explicacion = "En una cola, el primero que llegó (Rojo) debe ser el primero en salir."
            },
            new PreguntaDataCamino2
            {
                tema = "Colas (Queue)",
                pregunta = "¿En qué se diferencia una cola de una pila?",
                opciones = new string[] { "El orden en que salen", "El color", "El tamaño", "No hay diferencia" },
                respuestaCorrecta = 0,
                explicacion = "Cola: primero en entrar, primero en salir. Pila: último en entrar, primero en salir."
            }
        };
    }
    
    // PREGUNTAS PREDEFINIDAS - LISTAS
    public static PreguntaDataCamino2[] ObtenerPreguntasListas()
    {
        return new PreguntaDataCamino2[]
        {
            new PreguntaDataCamino2
            {
                tema = "Listas (List)",
                pregunta = "¿Qué es una lista?",
                opciones = new string[] { "Una colección de elementos en orden", "Un solo número", "Una puerta", "Un enemigo" },
                respuestaCorrecta = 0,
                explicacion = "Una lista guarda varios elementos en orden, como una lista de compras."
            },
            new PreguntaDataCamino2
            {
                tema = "Listas (List)",
                pregunta = "Si tengo una lista [Rojo, Azul, Verde], ¿en qué posición está el Azul?",
                opciones = new string[] { "Posición 1", "Posición 0", "Posición 2", "No tiene posición" },
                respuestaCorrecta = 0,
                explicacion = "Las posiciones empiezan en 0. Rojo=0, Azul=1, Verde=2."
            },
            new PreguntaDataCamino2
            {
                tema = "Listas (List)",
                pregunta = "¿Puedo cambiar el tamaño de una lista después de crearla?",
                opciones = new string[] { "Sí", "No", "Solo si está vacía", "Solo una vez" },
                respuestaCorrecta = 0,
                explicacion = "Las listas pueden crecer o reducirse. Puedes agregar o quitar elementos cuando quieras."
            }
        };
    }
}
