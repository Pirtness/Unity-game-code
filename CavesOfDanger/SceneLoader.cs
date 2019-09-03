using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Класс, отвечающий за смену сцен.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Дезактивация копки "Продолжить" в случае, если не была начата игра.
    /// </summary>
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (!PlayerPrefs.HasKey("Level"))
            {
                GameObject.Find("ContinueButton").SetActive(false);
            }
        }
    }

    /// <summary>
    /// Загрузка сцены.
    /// </summary>
    /// <param name="levelNum"> Индекс сцены, которая будет загружена. </param>
    public void LoadLevel(int levelNum)
    {
        int n = SceneManager.sceneCountInBuildSettings;
        if (levelNum < n)
            SceneManager.LoadScene(levelNum);
        else
            GameObject.Find("LevelInformer").GetComponent<LevelInformator>().Win();
    }

    /// <summary>
    /// Перезапуск уровня.
    /// </summary>
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Продолжить игру с последнего открытого уровня.
    /// </summary>
    public void Continue()
    {        
        if (PlayerPrefs.HasKey("Level"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
        }
    }

     /// <summary>
     /// Начать игру заново. Сброс всех сохранений.
     /// </summary>
    public void NewGame()
    {
        float a = -1;
        if (PlayerPrefs.HasKey("Volume"))
        {
            a = PlayerPrefs.GetFloat("Volume");
        }
        PlayerPrefs.DeleteAll();
        if (a != -1)
        {
            PlayerPrefs.SetFloat("Volume", a);
        }
        SceneManager.LoadScene(1);      
    }

    /// <summary>
    /// Выйти из игры.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Загрузка следующего уровня при входе игрока в область объекта. Сохранение прогресса.
    /// </summary>
    /// <param name="collision"> Объект, вошечший в область. </param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController pl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            PlayerPrefs.SetFloat("Health", pl.currHealthPoints);
            PlayerPrefs.SetInt("Mines", pl.numberOfMines);
            PlayerPrefs.SetInt("Defence", pl.defence);
            PlayerPrefs.SetInt("Attack", pl.attackForce);

            LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
