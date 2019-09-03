using UnityEngine;

/// <summary>
/// Класс, управляющий движением пули, которая преследует игрока.
/// </summary>
public class Bullet_Boss2 : Bullet
{
    /// <summary>
    /// Точка, в которую летит пуля.
    /// </summary>
    Vector2 tr;

    /// <summary>
    /// Время, которое пуля существует.
    /// </summary>
    [SerializeField] float TimeOfLiving = 3f;

    /// <summary>
    /// Установка начальных значений.
    /// </summary>
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
    }

    /// <summary>
    /// Передвижение и счетчик времени существования.
    /// </summary>
    void Update()
    {
        if (TimeOfLiving <= 0)
            Destroy(gameObject);
        TimeOfLiving -= Time.deltaTime;
        Move();
    }

    /// <summary>
    /// Реализация движения за персонажем.
    /// </summary>
    protected override void Move()
    {
        tr = new Vector2(target.position.x, target.position.y);
        transform.position = Vector2.MoveTowards(transform.position, tr, speed * Time.deltaTime);
        if (transform.position.x == tr.x && transform.position.y == tr.y)
            Destroy(gameObject);
    }
}
