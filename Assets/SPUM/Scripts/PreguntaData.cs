using UnityEngine;

/// <summary>
/// Datos de una pregunta sobre los temas
/// </summary>
[System.Serializable]
public class PreguntaData
{
    [Header("Pregunta")]
    public string tema; // "Recursión", "Arreglos", "Genéricos"
    [TextArea(3, 5)]
    public string pregunta;
    
    [Header("Opciones")]
    public string[] opciones = new string[4]; // A, B, C, D
    public int respuestaCorrecta; // 0=A, 1=B, 2=C, 3=D
    
    [Header("Feedback")]
    [TextArea(2, 3)]
    public string explicacion; // Se muestra al responder
    
    // PREGUNTAS PREDEFINIDAS
    public static PreguntaData[] ObtenerPreguntasArreglos()
    {
        return new PreguntaData[]
        {
            new PreguntaData
            {
                tema = "Arreglos e Índices",
                pregunta = "¿Cómo se le llaman a las posiciones de los objetos en un arreglo?",
                opciones = new string[] { "Índices", "Valores", "Nombres", "Etiquetas" },
                respuestaCorrecta = 0,
                explicacion = "Las posiciones en un arreglo se llaman ÍNDICES, y empiezan en 0."
            },
            new PreguntaData
            {
                tema = "Arreglos",
                pregunta = "¿En qué posición está el primer elemento de un arreglo?",
                opciones = new string[] { "Posición 0", "Posición 1", "Posición -1", "No tiene posición" },
                respuestaCorrecta = 0,
                explicacion = "El primer elemento siempre está en el índice [0]."
            },
            new PreguntaData
            {
                tema = "Arreglos",
                pregunta = "Si un arreglo tiene 5 elementos, ¿cuál es el índice del último?",
                opciones = new string[] { "4", "5", "3", "0" },
                respuestaCorrecta = 0,
                explicacion = "Si hay 5 elementos, los índices van de 0 a 4. El último es [4]."
            }
        };
    }
    
    public static PreguntaData[] ObtenerPreguntasRecursion()
    {
        return new PreguntaData[]
        {
            new PreguntaData
            {
                tema = "Recursión",
                pregunta = "¿Qué pasa si una función recursiva no sigue un orden o condición de parada?",
                opciones = new string[] { "Se ejecuta infinitamente", "Funciona perfectamente", "Se detiene sola", "No pasa nada" },
                respuestaCorrecta = 0,
                explicacion = "Sin condición de parada, la recursión nunca termina y causa un error."
            },
            new PreguntaData
            {
                tema = "Recursión",
                pregunta = "¿Qué es una función recursiva?",
                opciones = new string[] { "Una función que se llama a sí misma", "Una función muy larga", "Una función sin parámetros", "Una función que no retorna nada" },
                respuestaCorrecta = 0,
                explicacion = "La recursión ocurre cuando una función se llama a sí misma para resolver un problema."
            },
            new PreguntaData
            {
                tema = "Recursión y Laberintos",
                pregunta = "¿Para qué sirve la recursión en un laberinto?",
                opciones = new string[] { "Explorar todos los caminos posibles", "Hacer el laberinto más grande", "Eliminar paredes", "Contar las celdas" },
                respuestaCorrecta = 0,
                explicacion = "La recursión permite probar cada camino hasta encontrar la salida."
            }
        };
    }
    
    public static PreguntaData[] ObtenerPreguntasGenericos()
    {
        return new PreguntaData[]
        {
            new PreguntaData
            {
                tema = "Tipos Genéricos",
                pregunta = "¿Cómo funcionan los parámetros de tipo en las clases genéricas?",
                opciones = new string[] { "Definen qué tipo de datos acepta la clase", "Hacen la clase más lenta", "Son opcionales y no cambian nada", "Solo sirven para texto" },
                respuestaCorrecta = 0,
                explicacion = "Los parámetros de tipo (como <T>) especifican qué tipo de datos puede manejar la clase."
            },
            new PreguntaData
            {
                tema = "Genéricos",
                pregunta = "Si tengo una Caja<Arma>, ¿qué puedo guardar?",
                opciones = new string[] { "Solo armas", "Cualquier cosa", "Solo números", "Solo texto" },
                respuestaCorrecta = 0,
                explicacion = "Una Caja<Arma> está parametrizada para aceptar SOLO objetos de tipo Arma."
            },
            new PreguntaData
            {
                tema = "Tipos Genéricos",
                pregunta = "¿Qué ventaja tienen las clases genéricas?",
                opciones = new string[] { "Reutilización de código con diferentes tipos", "Son más rápidas", "Ocupan menos memoria", "No tienen ventajas" },
                respuestaCorrecta = 0,
                explicacion = "Los genéricos permiten crear una clase que funciona con múltiples tipos sin repetir código."
            }
        };
    }
}
