using UnityEngine;
using System.Collections;

/// <summary>
/// Абстрактный класс босса.
/// </summary>
public abstract class Boss : Enemy
{    
    /// <summary>
    /// Пуля, которую выпускает босс.
    /// </summary>
    [SerializeField] protected GameObject bullet;

    /// <summary>
    /// Точка, на которую в очередной раз перейдет босс.
    /// </summary>
    protected Vector2 point = new Vector2(-7, -7);

    /// <summary>
    /// Начальная точка нахождения босса.
    /// </summary>
    protected Vector2 init;



    /// <summary>
    /// Задаются начальные значения. Начинается проигрывание анимации и атаки.
    /// </summary>
    protected void SetStartValuesForBoss()
    {
        boss = true;
        init = transform.parent.position;
        SetStartValues();
        animatorController.Play("Idle");
        StartCoroutine("Attack");
    }

    /// <summary>
    /// Управление боссом. Передвижение, атака, проигрывание анимации.
    /// </summary>
    protected void BossController()
    {
        if (target != null)
        {
            if (!dead)
            {
                if (point.x.CompareTo(transform.position.x) == 0 && point.y.CompareTo(transform.position.y) == 0)
                {
                    point = new Vector2(-7, -7);
                    animatorController.Play("Idle");
                    StartCoroutine("Attack");
                }
                else
                {
                    if (point.x != -7)
                        transform.position = Vector2.MoveTowards(transform.position, point, speed * Time.fixedDeltaTime);
                }
            }
        }
    }

    /// <summary>
    /// Реализация атаки босса.
    /// </summary>
    /// <returns> Паузы между действиями. </returns>
    protected override IEnumerator Attack()
    {
        animatorController.Play("Attack");       
        for (int i = 0; i < 5; i++)
        {
            base.MakeQuaternion();
            yield return new WaitForSeconds(1f);
            MakeShot();
        }
        yield return new WaitForSeconds(0.5f);
        MoveOnPoint();
    }

    /// <summary>
    /// Поворот при движении.
    /// </summary>
    protected override void MakeQuaternion()
    {
        if (point.x < transform.position.x)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (point.x > transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    /// <summary>
    /// Передвижение босса на точку.
    /// </summary>
    private void MoveOnPoint()
    {
        do
        {
            point = new Vector2(Random.Range(init.x - 6, init.x + 6), Random.Range(init.y - 2, init.y + 3));
        } while (point.x.CompareTo(transform.position.x) == 0 && point.y.CompareTo(transform.position.y) == 0);
        MakeQuaternion();
        animatorController.Play("Walk");        
    }

    /// <summary>
    /// Сделать выстрел.
    /// </summary>
    protected virtual void MakeShot()
    {
        float k = 1 / Mathf.Sqrt(2);
        var b = Instantiate(bullet, transform.position, Quaternion.identity);
        b.GetComponent<BulletBoss>().direction = new Vector3(1f, 0, 0);
        b = Instantiate(bullet, transform.position, Quaternion.identity);
        b.GetComponent<BulletBoss>().direction = new Vector3(-1f, 0, 0);
        b = Instantiate(bullet, transform.position, Quaternion.identity);
        b.GetComponent<BulletBoss>().direction = new Vector3(0, 1f, 0);
        b = Instantiate(bullet, transform.position, Quaternion.identity);
        b.GetComponent<BulletBoss>().direction = new Vector3(0, -1f, 0);
        b = Instantiate(bullet, transform.position, Quaternion.identity);
        b.GetComponent<BulletBoss>().direction = new Vector3(k, k, 0);
        b = Instantiate(bullet, transform.position, Quaternion.identity);
        b.GetComponent<BulletBoss>().direction = new Vector3(k, -k, 0);
        b = Instantiate(bullet, transform.position, Quaternion.identity);
        b.GetComponent<BulletBoss>().direction = new Vector3(-k, k, 0);
        b = Instantiate(bullet, transform.position, Quaternion.identity);
        b.GetComponent<BulletBoss>().direction = new Vector3(-k, -k, 0);
    }
}
