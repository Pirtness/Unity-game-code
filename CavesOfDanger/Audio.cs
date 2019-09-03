using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Класс для управления звуком.
/// </summary>
public class Audio : MonoBehaviour
{
    /// <summary>
    /// Источник звука.
    /// </summary>
    AudioSource audS;

    /// <summary>
    /// Слайдер, отвечающий за громкость звука.
    /// </summary>
    [SerializeField] Slider vol;
    
    
    /// <summary>
    /// Установка начальной громкости звука или загрузка сохраненной.
    /// </summary>
    void Start()
    {
        audS = gameObject.GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("Volume"))
        {
            audS.volume = PlayerPrefs.GetFloat("Volume");
            vol.value = PlayerPrefs.GetFloat("Volume");
        }
    }

    /// <summary>
    /// Обновление и сохранение громкости.
    /// </summary>
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 && gameObject.tag == "Audio")
        {
            Destroy(gameObject);
        }
        else if (SceneManager.GetActiveScene().buildIndex != 0 && gameObject.tag != "Audio")
            Destroy(gameObject);
        audS.volume = vol.value;
        PlayerPrefs.SetFloat("Volume", vol.value);
    }
}
