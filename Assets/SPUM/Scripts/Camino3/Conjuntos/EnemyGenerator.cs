using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Generador de enemigos para el juego de conjuntos
/// Crea enemigos que pertenecen a diferentes conjuntos
/// </summary>
public class ConjuntosEnemyGenerator : MonoBehaviour
{
    private ConjuntosRoomManager roomManager;
    
    void Start()
    {
        roomManager = GetComponent<ConjuntosRoomManager>();
    }

    /// <summary>
    /// Genera enemigos según la operación actual
    /// </summary>
    public void GenerarEnemigos(ConjuntosRoomManager.TipoOperacion operacion, int cantidad)
    {
        if (roomManager == null) return;
        
        // Recopilar todos los spawns disponibles
        List<SpawnInfo> spawnsA = new List<SpawnInfo>();
        List<SpawnInfo> spawnsB = new List<SpawnInfo>();
        List<SpawnInfo> spawnsInterseccion = new List<SpawnInfo>();
        
        // Spawns solo en A
        if (roomManager.spawnsConjuntoA != null)
        {
            foreach (Transform spawn in roomManager.spawnsConjuntoA)
            {
                if (spawn != null && !EstaEnInterseccion(spawn))
                {
                    spawnsA.Add(new SpawnInfo(spawn, true, false));
                }
            }
        }
        
        // Spawns solo en B
        if (roomManager.spawnsConjuntoB != null)
        {
            foreach (Transform spawn in roomManager.spawnsConjuntoB)
            {
                if (spawn != null && !EstaEnInterseccion(spawn))
                {
                    spawnsB.Add(new SpawnInfo(spawn, false, true));
                }
            }
        }
        
        // Spawns en intersección (A ∩ B)
        if (roomManager.spawnsInterseccion != null)
        {
            foreach (Transform spawn in roomManager.spawnsInterseccion)
            {
                if (spawn != null)
                {
                    spawnsInterseccion.Add(new SpawnInfo(spawn, true, true));
                }
            }
        }
        
        int enemigosGenerados = 0;
        
        // Generar 5 enemigos en intersección
        int enemigosInterseccion = Mathf.Min(5, cantidad);
        if (spawnsInterseccion.Count > 0)
        {
            for (int i = 0; i < enemigosInterseccion; i++)
            {
                SpawnInfo spawnInfo = spawnsInterseccion[Random.Range(0, spawnsInterseccion.Count)];
                GenerarEnemigo(spawnInfo, operacion, enemigosGenerados + 1, cantidad);
                enemigosGenerados++;
            }
        }
        
        // Calcular cuántos enemigos faltan
        int enemigosFaltantes = cantidad - enemigosGenerados;
        
        // Distribuir el resto entre A y B (3 por conjunto)
        int enemigosEnA = Mathf.Min(3, enemigosFaltantes);
        int enemigosEnB = Mathf.Min(3, enemigosFaltantes - enemigosEnA);
        
        // Generar enemigos en A
        if (spawnsA.Count > 0)
        {
            for (int i = 0; i < enemigosEnA; i++)
            {
                SpawnInfo spawnInfo = spawnsA[Random.Range(0, spawnsA.Count)];
                GenerarEnemigo(spawnInfo, operacion, enemigosGenerados + 1, cantidad);
                enemigosGenerados++;
            }
        }
        
        // Generar enemigos en B
        if (spawnsB.Count > 0)
        {
            for (int i = 0; i < enemigosEnB; i++)
            {
                SpawnInfo spawnInfo = spawnsB[Random.Range(0, spawnsB.Count)];
                GenerarEnemigo(spawnInfo, operacion, enemigosGenerados + 1, cantidad);
                enemigosGenerados++;
            }
        }
        
        Debug.Log($"Generados {enemigosGenerados} enemigos para operación {operacion}: {enemigosInterseccion} intersección, {enemigosEnA} en A, {enemigosEnB} en B");
    }
    
    void GenerarEnemigo(SpawnInfo spawnInfo, ConjuntosRoomManager.TipoOperacion operacion, int numero, int total)
    {
        // Decidir qué prefab usar
        GameObject prefabAUsar = DecidirPrefab(spawnInfo, operacion);
        
        if (prefabAUsar != null)
        {
            // Añadir offset aleatorio para que no se superpongan
            Vector3 posicionConOffset = spawnInfo.spawn.position + new Vector3(
                Random.Range(-2f, 2f),
                Random.Range(-2f, 2f),
                0f
            );
            
            // Crear enemigo
            GameObject enemigo = Instantiate(prefabAUsar, posicionConOffset, Quaternion.identity);
            
            // Configurar el enemigo
            ConjuntosEnemy enemyScript = enemigo.GetComponent<ConjuntosEnemy>();
            if (enemyScript == null)
            {
                enemyScript = enemigo.AddComponent<ConjuntosEnemy>();
            }
            
            enemyScript.estaEnConjuntoA = spawnInfo.estaEnA;
            enemyScript.estaEnConjuntoB = spawnInfo.estaEnB;
            enemyScript.roomManager = roomManager;
            
            roomManager.AgregarEnemigo(enemigo);
            
            Debug.Log($"Enemigo {numero}/{total} spawneado: A={spawnInfo.estaEnA}, B={spawnInfo.estaEnB}");
        }
    }

    GameObject DecidirPrefab(SpawnInfo info, ConjuntosRoomManager.TipoOperacion operacion)
    {
        // Para hacer el juego más interesante, mezclar enemigos correctos e incorrectos
        
        // Enemigos en intersección (ambos conjuntos) - siempre tipo 1
        if (info.estaEnA && info.estaEnB)
        {
            return roomManager.enemigoPrefabTipo1;
        }
        
        // Enemigos solo en A o solo en B
        if (operacion == ConjuntosRoomManager.TipoOperacion.Union)
        {
            // Para unión, estos también son correctos
            return roomManager.enemigoPrefabTipo1;
        }
        else
        {
            // Para intersección, estos son incorrectos
            return roomManager.enemigoPrefabTipo2;
        }
    }

    bool EstaEnInterseccion(Transform spawn)
    {
        if (roomManager.spawnsInterseccion == null) return false;
        
        foreach (Transform interseccion in roomManager.spawnsInterseccion)
        {
            if (interseccion == spawn)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Clase auxiliar para almacenar info de spawn
    /// </summary>
    private class SpawnInfo
    {
        public Transform spawn;
        public bool estaEnA;
        public bool estaEnB;
        
        public SpawnInfo(Transform spawn, bool enA, bool enB)
        {
            this.spawn = spawn;
            this.estaEnA = enA;
            this.estaEnB = enB;
        }
    }
}
