using UnityEngine;

/// <summary>
/// Класс, управляющий минами.
/// </summary>
public class MineController : MonoBehaviour
{
    /// <summary>
    /// Объект игрока.
    /// </summary>
    GameObject player;

    /// <summary>
    /// Находится ли игрок в области мины.
    /// </summary>
    bool inBomb = true;

    /// <summary>
    /// Поиск объекта игрока.
    /// </summary>
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Проверка, поднимает ли игрок мину.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && inBomb)
        {
            inBomb = false;
            player.GetComponent<PlayerController>().GetMine();
            Destroy(gameObject, .2f);
        }
    }

    /// <summary>
    /// Вхождение объекта в область мины.
    /// </summary>
    /// <param name="collision"> Объект, вошедщий в область мины. </param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            if (collision.gameObject.tag == "Player" || collision.transform.parent.tag == "Player")
                inBomb = true;
        }
        catch { }
    }

    /// <summary>
    /// Выход объекта из области мины.
    /// </summary>
    /// <param name="collision"> Объект, который вышел из области мины. </param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        try
        {
            if (collision.gameObject.tag == "Player" || collision.transform.parent.tag == "Player")
            {
                inBomb = false;
            }
        }
        catch { }
    }
}
