using UnityEngine;

/// <summary>
/// Класс, отвечающий за нанесение урона игроку противником ближнего боя.
/// </summary>
public class MeleeAttack : MonoBehaviour
{
    /// <summary>
    /// Объект игрока.
    /// </summary>
    GameObject player;

    /// <summary>
    /// Поиск объекта игрока.
    /// </summary>
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Нанесение урона, если игрок попадает в область удара.
    /// </summary>
    /// <param name="collision"> Объект, попавший в область удара. </param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject == player.transform.Find("PlayerField").gameObject)
        {
            player.GetComponent<PlayerController>().TakeDamage();
            gameObject.SetActive(false);
        }
    }
}
