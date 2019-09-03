using UnityEngine;

/// <summary>
/// Абстрактный класс пули.
/// </summary>
public abstract class Bullet : MonoBehaviour
{
    /// <summary>
    /// Скорость полета пули.
    /// </summary>
    [SerializeField] protected float speed = 9f;

    /// <summary>
    /// Игрок, в которого полетит пуля.
    /// </summary>
    protected GameObject player;

    /// <summary>
    /// Компонента transform игрока.
    /// </summary>
    protected Transform target;

    /// <summary>
    /// Движение пули.
    /// </summary>
    protected abstract void Move();

    /// <summary>
    /// Событие, срабатывающее, когда пуля входит в область другого объекта. Взаимодействие с другими игровыми объектами.
    /// Если это игрок, ему наносится урон и пуля исчезает. Если это стена, пуля просто исчезает.
    /// </summary>
    /// <param name="collision"> Объект, в область которого вошла пуля. </param>
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            if (collision.gameObject == GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().TakeDamage();
                Destroy(gameObject);
            }
            else if (collision.gameObject.tag == "Wall")
            {
                Destroy(gameObject);
            }
        }
        catch { }
    }
}
