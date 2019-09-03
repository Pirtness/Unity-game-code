using UnityEngine;
using System.Collections;

/// <summary>
/// Класс, управляющий противником дальнего боя.
/// </summary>
public class EnemyRange : Enemy
{
    /// <summary>
    /// Пуля, которую выпускает противник.
    /// </summary>
    [SerializeField] GameObject bullet = null;
    /// <summary>
    /// Расстояние, с которого противник начинает убегать.
    /// </summary>
    [SerializeField] float distOfLeaving = 0;

    /// <summary>
    /// Установка начальных значений.
    /// </summary>
    void Start()
    {
        SetStartValues();
    }

    /// <summary>
    /// Управление противником. Передвижение, атака, проигрывание анимации.
    /// </summary>
    void FixedUpdate()
    {
        ControlEnemy();
    }

    /// <summary>
    /// Управление противником.
    /// </summary>
    private void ControlEnemy()
    {
        if (target == null)
            animatorController.Play("Attack");
        else
        {
            if (dead)
                animatorController.Play("Dead");
            else
            {
                dist = Vector2.Distance(transform.position, target.transform.position);
                MakeQuaternion();

                if (dist >= distOfHitting)
                {
                    transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
                    animatorController.Play("Walk");
                }
                else if (dist < distOfHitting && dist >= distOfLeaving)
                {
                    animatorController.Play("Attack");
                }
                else if (dist < distOfLeaving)
                {
                    transform.position = Vector2.MoveTowards(transform.position, target.transform.position, -speed * Time.deltaTime);
                    animatorController.Play("Walk");
                }
                if (!inAttack)
                    StartCoroutine("Attack");
            }
        }
    }

    /// <summary>
    /// Реализация атаки.
    /// </summary>
    /// <returns> Промежутки между действиями. </returns>
    protected override IEnumerator Attack()
    {
        inAttack = true;
        Instantiate(bullet, transform.position + new Vector3(0, -0.7f, 0f), Quaternion.identity);
        yield return new WaitForSeconds(3f);
        inAttack = false;
    }
}

