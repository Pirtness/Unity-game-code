using UnityEngine;

/// <summary>
/// Класс, управляющий боссом второго уровня.
/// </summary>
public class Boss_L2 : Boss
{
    /// <summary>
    /// Задаются начальные значения. Начинается проигрывание анимации.
    /// </summary>
    void Start()
    {
        SetStartValuesForBoss();
    }

    /// <summary>
    /// Управление боссом. Передвижение, атака, проигрывание анимации.
    /// </summary>
    void FixedUpdate()
    {
        BossController();
    }

    /// <summary>
    /// Сделать выстрел.
    /// </summary>
    protected override void MakeShot()
    {
        var b = Instantiate(bullet, transform.position, Quaternion.identity);
    }
}
