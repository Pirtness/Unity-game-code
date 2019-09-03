using System.Collections;
using UnityEngine;

/// <summary>
/// Класс, управляющий противником ближнего боя.
/// </summary>
public class EnemyMelee : Enemy
{
    /// <summary>
    /// Область поражения.
    /// </summary>
    [SerializeField] GameObject hitArea;

    /// <summary>
    /// Установка начальных значений.
    /// </summary>
    private void Start()
    {
        SetStartValues();
        hitArea = transform.Find("HitArea").gameObject;
        hitArea.SetActive(false);
    }
    /// <summary>
    /// Управление противником. Передвижение, атака, проигрывание анимации.
    /// </summary>
    private void FixedUpdate()
    {
        ControlEnemy();
    }
    
    /// <summary>
    /// Управление противником.
    /// </summary>
    private void ControlEnemy()
    {
        if (target == null)
            animatorController.Play("Win");
        else
        {
            if (dead)
                animatorController.Play("Dead");
            else if (!inAttack)
            {
                dist = Vector2.Distance(transform.position, target.transform.position);
                MakeQuaternion();

                if (dist >= distOfHitting)
                {
                    transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
                    animatorController.Play("Walk");
                }
                else if (dist < distOfHitting)
                {
                    StartCoroutine("Attack");
                }
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
        animatorController.Play("Attack");
        yield return new WaitForSeconds(0.1f);
        hitArea.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        
        hitArea.SetActive(false);
        animatorController.Play("Rest");
        yield return new WaitForSeconds(0.3f);
        inAttack = false;
    }
}
