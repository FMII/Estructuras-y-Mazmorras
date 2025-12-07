using System;
using System.Collections.Generic;

/// <summary>
/// Estructura de datos para los diálogos de las salas desde JSON
/// </summary>
[Serializable]
public class DialogosSalaData
{
    public List<SalaDialogos> salas;
}

[Serializable]
public class SalaDialogos
{
    public int salaID;
    public string nombreSala;
    public List<string> dialogos;
}
