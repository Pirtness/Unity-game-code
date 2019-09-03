using UnityEngine;

/// <summary>
/// Класс, управляющий бонусами.
/// </summary>
public class Item : MonoBehaviour
{
    /// <summary>
    /// Игрок, на которого направлено действие бонусов.
    /// </summary>
    GameObject player;

    /// <summary>
    /// Тип бонуса
    /// 0 - жизнь+, 1 - защита+, 2 - атака+,
    /// 3 - жизнь-, 4 - защита-, 5 - атака-,
    /// 6 - монета, 7 - случайное
    /// </summary>
    [SerializeField] int type;

    /// <summary>
    /// Поиск игрока и определение эффекта случайного бонуса.
    /// </summary>
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (type == 7)
        {
            type = Random.Range(0, 7);
        }
    }

    /// <summary>
    /// Применение эффекта при вхождении игрока в область бонуса.
    /// </summary>
    /// <param name="collision"> Объект, вошедший в область бонуса. </param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            if (collision.gameObject.tag == "Player" || collision.transform.parent.tag == "Player")
            {
                player.GetComponent<PlayerController>().GetItem(type);
                Destroy(gameObject);
            }
        }
        catch { }
    }
}
