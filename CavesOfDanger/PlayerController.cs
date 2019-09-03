using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// Класс, управляющий игроком.
/// </summary>
public class PlayerController : MonoBehaviour
{
    //Элементы интерфейса, на которых отображается статистика
    [SerializeField] Slider sliderHP;
    [SerializeField] Text numOfMinesText;
    [SerializeField] Text moneyText;
    [SerializeField] Text AttackForceText;
    [SerializeField] Text DefenceText;
    [SerializeField] GameObject DeathMenu;
    [SerializeField] GameObject GameMenu;
    [SerializeField] Text eventText;


    /// <summary>
    /// Скорость игрока.
    /// </summary>
    [SerializeField] float speed = 8f;

    /// <summary>
    /// Количество жизни игрока.
    /// </summary>
    [SerializeField] float healthPoints = 10f;

    /// <summary>
    /// Максимальное количество мин.
    /// </summary>
    [SerializeField] int maxNumOfMines = 7;

    /// <summary>
    /// Объект мины.
    /// </summary>
    [SerializeField] GameObject mine;

    /// <summary>
    /// Время установки мины.
    /// </summary>
    [SerializeField] float TimeOfMineSetting = 1f;

    /// <summary>
    /// Текущее время установки мины.
    /// </summary>
    private float CurrentTimeOfMineSetting = 0f;
    
    /// <summary>
    /// Управление анимацией.
    /// </summary>
    Animator animatorController;

    /// <summary>
    /// В какую сторону направлен игрок.
    /// </summary>
    char playerDirection = 'F';

    /// <summary>
    /// Компонента Rigidbody2D.
    /// </summary>
    Rigidbody2D rb;

    //Ширина и высота комнат.
    float width = 1920 / 90f;
    float height = 1080 / 90f;
    /// <summary>
    /// Максимальная глубина комнат.
    /// </summary>
    int maxDepth;

    /// <summary>
    /// Определение комнаты, в которой находится игрок.
    /// </summary>
    public string RoomName
    {
        get
        {
            string s = "Room";
            float a, b;
            if ((a = (float)(transform.position.x * 1.0 / width)) > 0)
                a = (int)(a + 0.5) + maxDepth;
            else
                a = (int)(a - 0.5) + maxDepth;

            if ((b = (float)(transform.position.y * 1.0 / height)) > 0)
                b = -1*(int)(b + 0.5) + maxDepth;
            else
                b = -1*(int)(b - 0.5) + maxDepth;
            
            s += a + "_" + b;
            return s;
        }
    }

    /// <summary>
    /// Текущее количество здоровья.
    /// </summary>
    float _currHealthPoints;
    /// <summary>
    /// Текущее количество здоровья. При изменении меняет количество здоровья в интерфейсе.
    /// </summary>
    public float currHealthPoints
    {
        get { return _currHealthPoints; }
        set
        {
            _currHealthPoints = value;
            sliderHP.value = currHealthPoints / healthPoints;
            if (currHealthPoints <= 0)
            {
                GameMenu.SetActive(false);
                DeathMenu.SetActive(true);
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Текущее количество мин.
    /// </summary>
    int _numberOfMines = 0;
    /// <summary>
    /// Текущее количество мин. При изменении изменяет количество мин в интерфейсе.
    /// </summary>
    public int numberOfMines
    {
        get { return _numberOfMines; }
        set
        {
            _numberOfMines = value;
            numOfMinesText.text = value.ToString();
        }
    }

    /// <summary>
    /// Количество монет.
    /// </summary>
    int _money = 0;
    /// <summary>
    /// Количество монет с отображением в интерфейсе и сохранением.
    /// </summary>
    public int money
    {
        get { return _money; }
        set
        {
            _money = value;
            moneyText.text = value.ToString();
            PlayerPrefs.SetInt("Money", value);
        }
    }

    /// <summary>
    /// Количество защиты.
    /// </summary>
    int _defence = 0;
    /// <summary>
    /// Количество защиты с отображением в интерфейсе.
    /// </summary>
    public int defence
    {
        get { return _defence; }
        set
        {
            _defence = value;
            DefenceText.text = value.ToString() + "%";
        }
    }

    /// <summary>
    /// Сила атаки.
    /// </summary>
    int _attackForce = 100;
    /// <summary>
    /// Сила атаки с отображением в интерфейсе.
    /// </summary>
    public int attackForce
    {
        get { return _attackForce; }
        set
        {
            _attackForce = value;
            AttackForceText.text = value.ToString() + "%";
        }
    }
    
    /// <summary>
    /// Установка начальных значений. Загрузка значений из сохранений.
    /// </summary>
    void Start()
    {
        speed *= Time.fixedDeltaTime;
        transform.position = new Vector3(0, 0, 0);
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
        rb = GetComponent<Rigidbody2D>();
        animatorController = GetComponent<Animator>();
        maxDepth = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>().maxDepth;
        
        LoadValues();
        sliderHP.value = currHealthPoints/healthPoints;

        CurrentTimeOfMineSetting = 0f;

        StartCoroutine("MinesAndHealthRegen");
    }

    /// <summary>
    /// Загрузка значения из сохранений, если они есть. Иначе устанавливаются начальные значения. 
    /// </summary>
    private void LoadValues()
    {
        if (PlayerPrefs.HasKey("Mines"))
            numberOfMines = PlayerPrefs.GetInt("Mines");
        else
            numberOfMines = maxNumOfMines;
        if (PlayerPrefs.HasKey("Health"))
            currHealthPoints = PlayerPrefs.GetFloat("Health");
        else
            currHealthPoints = healthPoints;
        if (PlayerPrefs.HasKey("Defence"))
            defence = PlayerPrefs.GetInt("Defence");
        else
            defence = 0;
        if (PlayerPrefs.HasKey("Attack"))
            attackForce = PlayerPrefs.GetInt("Attack");
        else
            attackForce = 100;
        if (PlayerPrefs.HasKey("Money"))
            money = PlayerPrefs.GetInt("Money");
        else money = 0;
    }
    
    /// <summary>
    /// Обновление игрока, движение, атака, анимация. 
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            KillThemAll();
        if (Input.GetKeyDown(KeyCode.Escape))
            GameMenu.SetActive(!GameMenu.activeInHierarchy);
        if (CurrentTimeOfMineSetting <= 0)
        {
            MovePlayer();
            CheckAttack();
        }
        else
        {
            CurrentTimeOfMineSetting -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Получить урон.
    /// </summary>
    /// <param name="damage"> Количество урона (по умолчанию 1). </param>
    public void TakeDamage(int damage = 1)
    {
        currHealthPoints -= 1.0f*damage*(100 - defence)/100;        
    }

    /// <summary>
    /// Контролирует движение персонажа соответственно нажатым клавишам
    /// </summary>
    private void MovePlayer()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            MoveRight();
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            MoveLeft();
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            MoveBack();
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            MoveFront();
        else
            Stay();


        if (((Input.GetKey(KeyCode.RightArrow)) & (Input.GetKey(KeyCode.UpArrow))) ||
                ((Input.GetKey(KeyCode.D)) & (Input.GetKey(KeyCode.W))))
            rb.MovePosition(rb.position + new Vector2(1, 1) * speed / (float)Math.Sqrt(2));
        else if (((Input.GetKey(KeyCode.RightArrow)) & (Input.GetKey(KeyCode.DownArrow))) ||
                ((Input.GetKey(KeyCode.D)) & (Input.GetKey(KeyCode.S))))
            rb.MovePosition(rb.position + new Vector2(1, -1) * speed / (float)Math.Sqrt(2));
        else if (((Input.GetKey(KeyCode.LeftArrow)) & (Input.GetKey(KeyCode.UpArrow))) ||
            ((Input.GetKey(KeyCode.A)) & (Input.GetKey(KeyCode.W))))
            rb.MovePosition(rb.position + new Vector2(-1, 1) * speed / (float)Math.Sqrt(2));
        else if (((Input.GetKey(KeyCode.LeftArrow)) & (Input.GetKey(KeyCode.DownArrow))) ||
                ((Input.GetKey(KeyCode.A)) & (Input.GetKey(KeyCode.S))))
            rb.MovePosition(rb.position + new Vector2(-1, -1) * speed / (float)Math.Sqrt(2));
    }

    /// <summary>
    /// Движение вправо.
    /// </summary>
    private void MoveRight()
    {
        rb.MovePosition(rb.position + Vector2.right * speed);
        animatorController.Play("WalkRight");
        playerDirection = 'R';
    }

    /// <summary>
    /// Движение вниз.
    /// </summary>
    private void MoveFront()
    {
        rb.MovePosition(rb.position - Vector2.up * speed);
        animatorController.Play("WalkFront");
        playerDirection = 'F';
    }

    /// <summary>
    /// Движение влево.
    /// </summary>
    private void MoveLeft()
    {
        rb.MovePosition(rb.position - Vector2.right * speed);
        animatorController.Play("WalkLeft");
        playerDirection = 'L';
    }

    /// <summary>
    /// Движение вверх.
    /// </summary>
    private void MoveBack()
    {
        rb.MovePosition(rb.position + Vector2.up * speed);
        animatorController.Play("WalkBack");
        playerDirection = 'B';
    }

    /// <summary>
    /// Проигрывание анимации в состоянии покоя.
    /// </summary>
    private void Stay()
    {
        rb.MovePosition(rb.position);
        switch (playerDirection)
        {
            case 'F':
                animatorController.Play("Idle");
                break;
            case 'R':
                animatorController.Play("IdleR");
                break;
            case 'L':
                animatorController.Play("IdleL");
                break;
            case 'B':
                animatorController.Play("IdleB");
                break;
        }
    }

    /// <summary>
    /// Проверить, нажата ли клавиша для атаки.
    /// </summary>
    private void CheckAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetMine();
        }
    }

    /// <summary>
    /// Установить мину.
    /// </summary>
    private void SetMine()
    {
        if (numberOfMines > 0)
        {
            CurrentTimeOfMineSetting = TimeOfMineSetting;
            animatorController.Play("Mine" + playerDirection);
            Instantiate(mine, transform.position + new Vector3(0, -0.5f, 0), transform.rotation);
            numberOfMines--;
        }
    }

    /// <summary>
    /// Поднять мину.
    /// </summary>
    public void GetMine()
    {
        CurrentTimeOfMineSetting = TimeOfMineSetting;
        animatorController.Play("Mine" + playerDirection);
        if (numberOfMines < maxNumOfMines)
            numberOfMines++;
    }

    /// <summary>
    /// Поднять бонус.
    /// </summary>
    /// <param name="type"> Тип поднятого бонуса. </param>
    public void GetItem(int type)
    {
        switch (type)
        {
            case 0: //жизнь+
                if (currHealthPoints + 2 <= healthPoints)
                {
                    currHealthPoints+=2;
                    eventText.text = "+2 жизни";
                }
                else if (currHealthPoints < healthPoints)
                {
                    currHealthPoints = healthPoints;
                    eventText.text = "+2 жизни";
                }
                break;
            case 1: //защита+
                if (defence < 40)
                {
                    defence += 5;
                    eventText.text = "+5 защиты";
                }
                break;
            case 2: //атака+
                if (attackForce < 200)
                {
                    attackForce += 5;
                    eventText.text = "+5 атаки";
                }
                break;
            case 3: //жизнь-
                if (currHealthPoints > 1)
                {
                    currHealthPoints--;
                    eventText.text = "-1 жизнь";
                }
                break;
            case 4: //защита-
                if (defence > 0)
                {
                    defence-=5;
                    eventText.text = "-5 защиты";
                }
                break;
            case 5: //атака-
                if (attackForce > 50)
                {
                    attackForce-=5;
                    eventText.text = "-5 атаки";
                }
                break;
            case 6: //монеты
                money += 50;
                eventText.text = "+50 очков";
                break;
        }
    }

    /// <summary>
    /// Добавить очки к рекорду.
    /// </summary>
    /// <param name="points"> Количество добавляемых очков. </param>
    public void AddPointsToScore(int points)
    {
        money += points;
        eventText.text = "+" + points.ToString() + " очков";
    }

    /// <summary>
    /// Убить всех противников.
    /// </summary>
    public static void KillThemAll()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var go in gos)
        {
            Destroy(go);
        }
    }

    /// <summary>
    /// Восстановление количество мин и здоровья.
    /// </summary>
    /// <returns></returns>
    IEnumerator MinesAndHealthRegen()
    {
        while (currHealthPoints >= 0)
        {
            for (int i = 0; i < 5; i++)
            {
                if (numberOfMines < maxNumOfMines)
                    numberOfMines++;
                yield return new WaitForSeconds(2f);
            }

            if (currHealthPoints + 1 <= healthPoints)
                currHealthPoints++;
            else if (currHealthPoints < healthPoints)
                currHealthPoints = healthPoints;            
        }
    }
}