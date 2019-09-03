using System.Collections;
using UnityEngine;

/// <summary>
/// Класс, управляющий боссом третьего уровня.
/// </summary>
public class Boss_L3 : Boss
{
    /// <summary>
    /// Противник, который будет появляться рядом с игроком.
    /// </summary>
    [SerializeField] GameObject enemy;

    /// <summary>
    /// Задаются начальные значения. Начинается проигрывание анимации.
    /// </summary>
    void Start()
    {
        SetStartValuesForBoss();
        StartCoroutine("Spawn");
    }

    /// <summary>
    /// Управление боссом. Передвижение, атака, проигрывание анимации.
    /// </summary>
    void FixedUpdate()
    {
        BossController();
    }
    
    /// <summary>
    /// Появление врагов каждые 10 секунд рядом с персонажем.
    /// </summary>
    /// <returns> Интервалы между появлениями. </returns>
    IEnumerator Spawn()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        yield return new WaitForSeconds(10f);
        while(!dead)
        {
            Instantiate(enemy, player.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(10f);
        }
    }
}
