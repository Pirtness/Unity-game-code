using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, показывающий историю, когда начинается новая игра.
/// </summary>
public class ShowHistory : MonoBehaviour
{
    /// <summary>
    /// Массив картинок, включенные в историю.
    /// </summary>
    [SerializeField] Sprite[] sprites = null;

    /// <summary>
    /// Начался ли показ истории.
    /// </summary>
    bool hasStarted = false;

    /// <summary>
    /// Компонента SpriteRenderer
    /// </summary>
    SpriteRenderer sr;

    /// <summary>
    /// Объект, отвечающий за загрузку сцен.
    /// </summary>
    SceneLoader sl;

    /// <summary>
    /// Установка начальных значений, поиск компонент.
    /// </summary>
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = Color.grey;
        sl = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
    }
    
    /// <summary>
    /// Проверка на прерывание показа истории.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && hasStarted)
        {
            hasStarted = false;
            sl.NewGame();
        }
    }

    /// <summary>
    /// Начать показ истории.
    /// </summary>
    public void StartCor()
    {
        StartCoroutine("Show");
    }

    /// <summary>
    /// Реализация показа истории.
    /// </summary>
    /// <returns> Промежутки между сменами картинок. </returns>
    public IEnumerator Show()
    {
        hasStarted = true;
        sr.color = Color.white;
        foreach (Sprite sp in sprites)
        {
            sr.sprite = sp;
            yield return new WaitForSeconds(3f);
        }
        hasStarted = false;
        sl.NewGame();
    }
}
