using UnityEngine;

/// <summary>
/// Класс, управляющий боссом первого уровня.
/// </summary>
public class Boss_L1 : Boss
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
}
