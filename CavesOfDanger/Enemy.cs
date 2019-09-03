using System.Collections;
using UnityEngine;

/// <summary>
/// Абстрактный класс противника.
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    /// <summary>
    /// Спрайт полоски жизни.
    /// </summary>
    [SerializeField] protected Texture2D healthBar;
    /// <summary>
    /// Координаты полоски жизни относительно противника.
    /// </summary>
    [SerializeField] protected int barX;
    [SerializeField] protected int barY;

    /// <summary>
    /// Скорость противника.
    /// </summary>
    [SerializeField] protected float speed = 5f;
    /// <summary>
    /// Максимальное количество жизни противника.
    /// </summary>
    public float health = 20;
    /// <summary>
    /// Текущее количество жизни противника.
    /// </summary>
    protected float curHealth;
    /// <summary>
    /// Количество очков за убийство противника.
    /// </summary>
    [SerializeField] protected int points = 100;

    /// <summary>
    /// Расстояние, с которого противник может атаковать.
    /// </summary>
    [SerializeField] protected float distOfHitting = 1f;
    /// <summary>
    /// Время удара.
    /// </summary>
    [SerializeField] protected float timeOfShot = 1f;
    /// <summary>
    /// Время между ударами.
    /// </summary>
    [SerializeField] protected float timeBetweenShots = 1f;
    /// <summary>
    /// Персонаж, на которого направлена атака.
    /// </summary>
    protected GameObject target;
    /// <summary>
    /// Время, прошедшее с момента удара.
    /// </summary>
    protected float currentTime;

    /// <summary>
    /// Компонента Rigidbody2D противника.
    /// </summary>
    protected Rigidbody2D rb;
    
    /// <summary>
    /// Управление анимацией.
    /// </summary>
    protected Animator animatorController;

    /// <summary>
    /// Совершает ли противник удар.
    /// </summary>
    protected bool inAttack = false;
    /// <summary>
    /// Жив ли противник.
    /// </summary>
    public bool dead = false;
    /// <summary>
    /// Расстояние до игрока.
    /// </summary>
    protected float dist;

    /// <summary>
    /// Босс ли это.
    /// </summary>
    protected bool boss = false;

    /// <summary>
    /// Предметы, которые могут выпасть после смерти.
    /// </summary>
    [SerializeField] protected GameObject[] items = new GameObject[0];
    
    /// <summary>
    /// Установка начальных значений.
    /// </summary>
    protected void SetStartValues()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        currentTime = -timeBetweenShots;
        curHealth = health;
        animatorController = GetComponent<Animator>();

        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), target.GetComponent<Collider2D>());
    }

    /// <summary>
    /// Поворот в сторону игрока.
    /// </summary>
    protected virtual void MakeQuaternion()
    {
        try
        {
            if (transform.position.x > target.transform.position.x)
                transform.rotation = Quaternion.Euler(0, 180, 0);
            else
                transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        catch { }
    }

    /// <summary>
    /// Получение урона и проверка на смерть.
    /// </summary>
    protected void TakeDamageEnemy()
    {
        curHealth -= 1.0f*target.gameObject.GetComponent<PlayerController>().attackForce / 100;
        if (curHealth <= 0)
        {
            if (!dead)
                StartCoroutine("Dead");
            target.GetComponent<PlayerController>().AddPointsToScore(points);
            Destroy(gameObject, 1.5f);            
        }
    }

    /// <summary>
    /// Смерть противника.
    /// </summary>
    /// <returns> Промежутки между действиями. </returns>
    IEnumerator Dead()
    {
        dead = true;
        animatorController.Play("Dead");
        yield return new WaitForSeconds(1.3f);
        Instantiate(items[Random.Range(0, items.Length)], gameObject.transform.position + new Vector3(0, -1f, 0), gameObject.transform.rotation);
        yield return new WaitForSeconds(0.1f);
        if (boss)
            PlayerController.KillThemAll();
        else
            yield return new WaitForSeconds(0.1f);

    }

    /// <summary>
    /// Получить урон при соприкосновении с миной.
    /// </summary>
    /// <param name="collision"> Объект, с которым было соприкосновение. </param>
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Mine")
        {
            Destroy(collision.gameObject);           
            TakeDamageEnemy();
        }
    }

    /// <summary>
    /// Атака.
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator Attack();

    /// <summary>
    /// Реализация полоски жизни над противником.
    /// </summary>
    protected void OnGUI()
    {
        if (curHealth > 0)
        {
            Vector3 posSqr = Camera.main.WorldToScreenPoint(transform.position);
            GUI.Box(new Rect(posSqr.x - barX, Screen.height - posSqr.y - barY, (health * 10), 10), "");
            GUI.DrawTexture(new Rect(posSqr.x - barX, Screen.height - posSqr.y - barY, (curHealth) * 10, 10), healthBar);
        }
    }
}
