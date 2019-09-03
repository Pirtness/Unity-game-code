using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Класс, информирующий игрока о номере уровня и победе.
/// </summary>
public class LevelInformator : MonoBehaviour
{
    /// <summary>
    /// Объект, на котором отображается номер уровня.
    /// </summary>
    [SerializeField] GameObject LevelInfo;

    /// <summary>
    /// Текст с номером уровня.
    /// </summary>
    [SerializeField] Text Message;

    /// <summary>
    /// Победный экран.
    /// </summary>
    [SerializeField] GameObject WinScreen;

    /// <summary>
    /// Начать показ номера уровня.
    /// </summary>
    void Start()
    {
        StartCoroutine("Show");
    }

    /// <summary>
    /// Показать победный экран.
    /// </summary>
    public void Win()
    {
        WinScreen.SetActive(true);
    }

    /// <summary>
    /// Показывать номер уровня в течении трех секунд.
    /// </summary>
    /// <returns></returns>
    IEnumerator Show()
    {
        int a = SceneManager.GetActiveScene().buildIndex;
        Message.text = "Уровень " + a.ToString();
        LevelInfo.SetActive(true);
        yield return new WaitForSeconds(3f);
        LevelInfo.SetActive(false);
    }
}
