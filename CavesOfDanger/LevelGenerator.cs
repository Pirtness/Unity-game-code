using UnityEngine;

/// <summary>
/// Класс, отвечающий за генерацию уровня.
/// </summary>
public class LevelGenerator : MonoBehaviour
{
    /// <summary>
    /// Вероятность, с которой будет построена комната.
    /// </summary>
    [SerializeField] float probability = 1;

    /// <summary>
    /// Максимальная глубина из центра уровня к крайней комнате.
    /// </summary>
    public int maxDepth = 1;

    //Массивы с шаблонами разных комнат.
    [SerializeField] GameObject[] fightRooms = new GameObject[1];
    [SerializeField] GameObject[] bossRooms = new GameObject[1];
    [SerializeField] GameObject[] trapRooms = new GameObject[1];
    [SerializeField] GameObject[] safeRooms = new GameObject[1];
    [SerializeField] GameObject startRoom;

    //Ширина и высота комнаты.
    float width = 1920 / 90f;
    float height = 1080 / 90f;

    /// <summary>
    /// Карта уровня.
    /// </summary>
    GameObject[,] Map;

    /// <summary>
    /// Объект игрока.
    /// </summary>
    GameObject player;

    /// <summary>
    /// Устанавливаются начальные значения. Генерируется карта уровня.
    /// </summary>
    void Start()
    {
        Map = new GameObject[2 * maxDepth + 1, 2 * maxDepth + 1];
        Draw();
        AddBoss();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Список комнат, в которых был игрок.
    /// </summary>
    string roomNames = "";
    /// <summary>
    /// Комната, в которой в данный момент находится игрок.
    /// </summary>
    string _roomName = "";

    /// <summary>
    /// Позиция текущей комнаты на карте.
    /// </summary>
    int x = 0, y = 0;

    /// <summary>
    /// Обновление карты. Установка новых комнат, блокировка дверей.
    /// </summary>
    void Update()
    {
        try
        {
            PlaceNewRoom();
            try { if (GameObject.FindGameObjectsWithTag("Enemy")[0] == null) { }; }
            catch
            {
                Transform r = GameObject.Find(_roomName).transform;
                Transform cols = r.Find("Colliders");
                Transform doors = r.Find("Doors");
                if (doors.Find("doorT").gameObject.activeInHierarchy)
                    cols.Find("Top").Find("TopDoor").gameObject.GetComponent<Collider2D>().isTrigger = true;
                if (doors.Find("doorB").gameObject.activeInHierarchy)
                    cols.Find("Bot").Find("BotDoor").gameObject.GetComponent<Collider2D>().isTrigger = true;
                if (doors.Find("doorL").gameObject.activeInHierarchy)
                    cols.Find("Left").Find("LeftDoor").gameObject.GetComponent<Collider2D>().isTrigger = true;
                if (doors.Find("doorR").gameObject.activeInHierarchy)
                    cols.Find("Right").Find("RightDoor").gameObject.GetComponent<Collider2D>().isTrigger = true;
            }
        }
        catch { }
    }

    /// <summary>
    /// Установка комнаты в место, где стоит игрок, если комнаты еще нет.
    /// </summary>
    private void PlaceNewRoom()
    {
        if (player != null)
        {
            if (_roomName != player.GetComponent<PlayerController>().RoomName)
            {
                _roomName = player.GetComponent<PlayerController>().RoomName;
                if (!roomNames.Contains(_roomName))
                {
                    roomNames += _roomName;
                    x = int.Parse(_roomName.Substring(4, _roomName.IndexOf('_') - 4));
                    y = int.Parse(_roomName.Substring(_roomName.IndexOf('_') + 1, _roomName.Length - _roomName.IndexOf('_') - 1));
                    var newRoom = Instantiate(Map[x, y], new Vector3((x - maxDepth) * width, (maxDepth - y) * height, 0), transform.rotation);
                    newRoom.name = "Room" + x + "_" + y;
                    AddWallsAndDoorsToRoom(x, y);
                }
            }
        }
    }

    /// <summary>
    /// Рисует первую комнату и вызывает отрисовку последующих
    /// </summary>
    public void Draw()
    {
        Map[maxDepth, maxDepth] = startRoom;
        Draw(0, 1, 0, 1);
        Draw(1, 1, 1, 0);
        Draw(2, 1, 0, -1);
        Draw(3, 1, -1, 0);

    }

    /// <summary>
    /// Постройка очередной комнаты.
    /// </summary>
    /// <param name="direction"> В каком из четырех направлений комната будет построена. </param>
    /// <param name="curDepth"> Текущая глубина. </param>
    /// <param name="x"> Место на карте (горизонталь). </param>
    /// <param name="y"> Место на карте (вертикаль). </param>
    public void Draw(int direction, int curDepth, int x, int y)
    {
        if (curDepth <= maxDepth)
        {
            if (Random.value <= probability)
            {
                float a = Random.value;
                if (Map[maxDepth + x, maxDepth - y] == null)
                    if (a <= 0.8)
                    {
                        PlaceFightRoom(x, y);
                    }
                    else if (a <= 0.9)
                    {
                        if (safeRooms.Length > 0)
                            PlaceSafeRoom(x, y);
                        else
                            PlaceFightRoom(x, y);
                    }
                    else //if (a <= 1)
                    {
                        if (trapRooms.Length > 0)
                            PlaceTrapRoom(x, y);
                        else
                            PlaceFightRoom(x, y);
                    }
                switch (direction)
                {
                    case 0:
                        Draw(0, curDepth + 1, x, y + 1);
                        Draw(1, curDepth + 1, x + 1, y);
                        Draw(3, curDepth + 1, x - 1, y);
                        break;
                    case 1:
                        Draw(0, curDepth + 1, x, y + 1);
                        Draw(1, curDepth + 1, x + 1, y);
                        Draw(2, curDepth + 1, x, y - 1);
                        break;
                    case 2:
                        Draw(1, curDepth + 1, x + 1, y);
                        Draw(2, curDepth + 1, x, y - 1);
                        Draw(3, curDepth + 1, x - 1, y);
                        break;
                    case 3:
                        Draw(0, curDepth + 1, x, y + 1);
                        Draw(2, curDepth + 1, x, y - 1);
                        Draw(3, curDepth + 1, x - 1, y);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Поместить случайную комнату-ловушку.
    /// </summary>
    /// <param name="x"> Место на карте (горизонталь). </param>
    /// <param name="y"> Место на карте (вертикаль). </param>
    private void PlaceTrapRoom(int x, int y)
    {
        Map[maxDepth + x, maxDepth - y] = trapRooms[0];
    }

    /// <summary>
    /// Поместить случайную безопасную комнату.
    /// </summary>
    /// <param name="x"> Место на карте (горизонталь). </param>
    /// <param name="y"> Место на карте (вертикаль). </param>
    private void PlaceSafeRoom(int x, int y)
    {
        Map[maxDepth + x, maxDepth - y] = safeRooms[0];
    }

    /// <summary>
    /// Поместить случайную комнату с противниками.
    /// </summary>
    /// <param name="x"> Место на карте (горизонталь). </param>
    /// <param name="y"> Место на карте (вертикаль). </param>
    private void PlaceFightRoom(int x, int y)
    {
        Map[maxDepth + x, maxDepth - y] = fightRooms[Random.Range(0, fightRooms.Length)];
    }

    /// <summary>
    /// Установка стен и дверей на карту.
    /// </summary>
    private void AddWallsAndDoors()
    {
        for (int i = 0; i < Map.GetLength(0); i++)
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                AddWallsAndDoorsToRoom(i, j);
            }
    }

    /// <summary>
    /// Установка стен и дверей в комнату.
    /// </summary>
    /// <param name="i"> Место на карте (горизонталь). </param>
    /// <param name="j"> Место на карте (вертикаль). </param>
    private void AddWallsAndDoorsToRoom(int i, int j)
    {
        if (Map[i, j] != null)
        {
            if (i == 0 || Map[i - 1, j] == null) //Стена слева
            {
                PlaceWall(i, j, 'L');
            }
            else //Дверь слева
            {
                PlaceDoor(i, j, 'L');
            }
            if (j == 0 || Map[i, j - 1] == null) //Стена сверху
            {
                PlaceWall(i, j, 'T');
            }
            else //Дверь сверху
            {
                PlaceDoor(i, j, 'T');
            }
            if (i == Map.GetUpperBound(0) || Map[i + 1, j] == null) //Стена справа
            {
                PlaceWall(i, j, 'R');
            }
            else //Дверь справа
            {
                PlaceDoor(i, j, 'R');
            }
            if (j == Map.GetUpperBound(1) || Map[i, j + 1] == null) //Стена снизу
            {
                PlaceWall(i, j, 'B');
            }
            else //Дверь снизу
            {
                PlaceDoor(i, j, 'B');
            }
        }
    }

    /// <summary>
    /// Установка двери в комнату.
    /// </summary>
    /// <param name="i"> Место на карте (горизонталь). </param>
    /// <param name="j"> Место на карте (вертикаль). </param>
    /// <param name="direction"> На какой стене будет дверь. </param>
    private void PlaceDoor(int i, int j, char direction)  
    {
        GameObject room = GameObject.Find("Room" + i + "_" + j);
        Transform Doors = room.transform.Find("Doors");
        GameObject door = Doors.Find("door" + direction).gameObject;
        door.SetActive(true);
    }

    /// <summary>
    /// Установка стены в комнату.
    /// </summary>
    /// <param name="i"> Место на карте (горизонталь). </param>
    /// <param name="j"> Место на карте (вертикаль). </param>
    /// <param name="direction"> С какой стороны будет стена. </param>
    private void PlaceWall(int i, int j, char direction)
    {
        GameObject room = GameObject.Find("Room" + i + "_" + j);
        Transform colliders = room.transform.Find("Colliders");
        Transform colliders2;

        Transform doors = room.transform.Find("Doors");
        doors.Find("door" + direction).gameObject.SetActive(false);

        GameObject wall;
        switch (direction)
        {
            case 'T':
                colliders2 = colliders.transform.Find("Top");
                wall = colliders2.Find("TopDoor").gameObject;
                wall.GetComponent<Collider2D>().isTrigger = false;
                break;
            case 'R':
                colliders2 = colliders.transform.Find("Right");
                wall = colliders2.Find("RightDoor").gameObject;
                wall.GetComponent<Collider2D>().isTrigger = false;
                break;
            case 'B':
                colliders2 = colliders.transform.Find("Bot");
                wall = colliders2.Find("BotDoor").gameObject;
                wall.GetComponent<Collider2D>().isTrigger = false;
                break;
            case 'L':
                colliders2 = colliders.transform.Find("Left");
                wall = colliders2.Find("LeftDoor").gameObject;
                wall.GetComponent<Collider2D>().isTrigger = false;
                break;
        }
    }

    /// <summary>
    /// Установка комнаты босса на карту.
    /// </summary>
    private void AddBoss()
    {
        bool made = false;
        int k;
        int g;
        do
        {
            k = Random.Range(0, Map.GetLength(0));
            g = Random.Range(0, Map.GetLength(0));
            if (Map[k, g] == null)
            {
                try
                {
                    if (Map[k + 1, g] != null)
                    { made = true; break; }
                }
                catch { }
                try
                {
                    if (Map[k - 1, g] != null)
                    { made = true; break; }
                }
                catch { }
                try
                {
                    if (Map[k, g + 1] != null)
                    { made = true; break; }
                }
                catch { }
                try
                {
                    if (Map[k, g - 1] != null)
                    { made = true; break; }
                }
                catch { }
            }
        } while (!made);
        Map[k, g] = bossRooms[0];
    }
}
