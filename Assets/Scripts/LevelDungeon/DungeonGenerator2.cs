using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator2 : MonoBehaviour
{
    class Room
    {
        public bool[] doors = new bool[4]; // arriba, abajo, derecha, izquierda
        public bool isStartRoom;
        public bool isEndRoom;
    }

    [SerializeField] int minRooms = 10;
    [SerializeField] int maxRooms = 20;
    [SerializeField] GameObject roomPrefab;
    [SerializeField] Vector2 roomSize = new Vector2(16, 16);

    Dictionary<Vector2Int, Room> dungeonMap = new Dictionary<Vector2Int, Room>();
    Vector2Int startRoomPos = Vector2Int.zero;
    Vector2Int endRoomPos;

    public int chestAmount;

    void Start()
    {
        GenerateDungeon();
        InstantiateDungeon();
        InstantiateObjectsInRooms();
    }

    void GenerateDungeon()
    {
        // 1. Generación inicial del mapa
        int roomCount = Random.Range(minRooms, maxRooms + 1);
        dungeonMap.Clear();

        // Crear la habitación de inicio
        dungeonMap[startRoomPos] = new Room() { isStartRoom = true };

        // Lista de habitaciones frontera (donde podemos añadir nuevas habitaciones)
        List<Vector2Int> frontier = new List<Vector2Int> { startRoomPos };

        // 2. Crecimiento del dungeon
        while (dungeonMap.Count < roomCount && frontier.Count > 0)
        {
            // Seleccionar una habitación aleatoria de la frontera
            int randomIndex = Random.Range(0, frontier.Count);
            Vector2Int currentPos = frontier[randomIndex];

            // Obtener direcciones disponibles para crecimiento
            List<int> availableDirections = GetAvailableDirections(currentPos);

            float randomChance = Random.value;
            if (availableDirections.Count > 0)
            {
                // Elegir una dirección aleatoria
                int direction = availableDirections[Random.Range(0, availableDirections.Count)];
                Vector2Int newPos = GetAdjacentPosition(currentPos, direction);

                // Crear la nueva habitación
                dungeonMap[newPos] = new Room();

                // Conectar las habitaciones
                dungeonMap[currentPos].doors[direction] = true;
                dungeonMap[newPos].doors[OppositeDirection(direction)] = true;

                // Añadir a la frontera
                frontier.Add(newPos);
            }
            else
            {
                // Eliminar de la frontera si no tiene direcciones disponibles
                frontier.RemoveAt(randomIndex);
            }
        }

        // 3. Establecer la habitación final (la más lejana)
        SetEndRoom();
    }

    List<int> GetAvailableDirections(Vector2Int position)
    {
        List<int> available = new List<int>();
        Vector2Int[] directions = {
            Vector2Int.up,    // 0 = arriba
            Vector2Int.down,  // 1 = abajo
            Vector2Int.right, // 2 = derecha
            Vector2Int.left   // 3 = izquierda
        };

        for (int i = 0; i < 4; i++)
        {
            Vector2Int neighborPos = position + directions[i];

            // Solo considerar direcciones donde no hay habitación
            if (!dungeonMap.ContainsKey(neighborPos))
            {
                available.Add(i);
            }
        }

        return available;
    }

    Vector2Int GetAdjacentPosition(Vector2Int position, int direction)
    {
        switch (direction)
        {
            case 0: return position + Vector2Int.up;
            case 1: return position + Vector2Int.down;
            case 2: return position + Vector2Int.right;
            case 3: return position + Vector2Int.left;
            default: return position;
        }
    }

    int OppositeDirection(int direction)
    {
        return direction switch
        {
            0 => 1, // arriba -> abajo
            1 => 0, // abajo -> arriba
            2 => 3, // derecha -> izquierda
            3 => 2, // izquierda -> derecha
            _ => direction
        };
    }

    void SetEndRoom()
    {
        // Usamos BFS para encontrar la habitación más lejana
        Dictionary<Vector2Int, int> distances = new Dictionary<Vector2Int, int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        distances[startRoomPos] = 0;
        queue.Enqueue(startRoomPos);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            // Explorar todas las direcciones conectadas
            for (int i = 0; i < 4; i++)
            {
                if (dungeonMap[current].doors[i])
                {
                    Vector2Int neighbor = GetAdjacentPosition(current, i);

                    if (!distances.ContainsKey(neighbor))
                    {
                        distances[neighbor] = distances[current] + 1;
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        // Encontrar la habitación con mayor distancia
        int maxDistance = -1;
        foreach (var kvp in distances)
        {
            if (kvp.Value > maxDistance)
            {
                maxDistance = kvp.Value;
                endRoomPos = kvp.Key;
            }
        }

        // Marcar como habitación final
        dungeonMap[endRoomPos].isEndRoom = true;
    }

    void InstantiateDungeon()
    {
        foreach (var kvp in dungeonMap)
        {
            Vector2Int pos = kvp.Key;
            Room room = kvp.Value;

            Vector3 worldPos = new Vector3(pos.x * roomSize.x, 0, pos.y * roomSize.y);
            GameObject roomObj = Instantiate(roomPrefab, worldPos, Quaternion.identity, transform);

            RoomBehaviour roomBehaviour = roomObj.GetComponent<RoomBehaviour>();
            roomBehaviour.UpdateRoom(room.doors, room.isStartRoom, room.isEndRoom);

            // Nombre descriptivo para depuración
            roomObj.name = room.isStartRoom ? "Start Room" :
                          room.isEndRoom ? "End Room" :
                          $"Room ({pos.x}, {pos.y})";
        }
    }

    void InstantiateObjectsInRooms()
    {
        RoomBehaviour[] roomBehaviours = GetComponentsInChildren<RoomBehaviour>();

        int chestPlaced = 0;
        while (chestAmount >= chestPlaced)
        {
            RoomBehaviour room = roomBehaviours[Random.Range(0, roomBehaviours.Length)];
            if (room.IsNormalRoom() && !room.IsNormalChestActivated())
            {
                room.PlaceChest();
                chestPlaced++;
            }
        }
    }
}