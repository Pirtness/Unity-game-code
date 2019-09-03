using UnityEngine;

/// <summary>
/// Класс, управляющий пулей, которая летит в определенном направлении.
/// </summary>
public class BulletBoss : Bullet
{
    /// <summary>
    /// Направление движения пули.
    /// </summary>
    public Vector3 direction = new Vector3(0, 1);

    /// <summary>
    /// Компонента Rigidbody2D пули.
    /// </summary>
    Rigidbody2D rb;

    /// <summary>
    /// Реализация движения пули.
    /// </summary>
    protected override void Move()
    {
        rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Установка начальных значений.
    /// </summary>
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    
    /// <summary>
    /// Движение пули.
    /// </summary>
    void Update()
    {
        Move();
    }
}
