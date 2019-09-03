using UnityEngine;

/// <summary>
/// Класс, управляющий пулей, которая летит в заданную точку.
/// </summary>
public class BulletEnemy : Bullet
{
    /// <summary>
    /// Точка, в которую летит пуля.
    /// </summary>
    Vector2 tr;

    /// <summary>
    /// Установка начальных значений.
    /// </summary>
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
        tr = new Vector2(target.position.x, target.position.y);
    }

    /// <summary>
    /// Движение пули.
    /// </summary>
    void Update()
    {
        Move();
    }

    /// <summary>
    /// Реализация движения пули.
    /// </summary>
    protected override void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, tr, speed * Time.deltaTime);
        if (transform.position.x == tr.x && transform.position.y == tr.y)
            Destroy(gameObject);
    }
}
