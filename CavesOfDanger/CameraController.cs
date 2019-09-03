using UnityEngine;

/// <summary>
/// Класс, управляющий движением камеры.
/// </summary>
public class CameraController : MonoBehaviour
{
    //ширина и высота экрана
    float width = 1920 / 90f;
    float height = 1080 / 90f;

    /// <summary>
    /// Игрок, за которым следует камера.
    /// </summary>
    GameObject player;

    /// <summary>
    /// Установка начальных значений.
    /// </summary>
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Проверка позиции игрока и передвижение камеры.
    /// </summary>
    void Update()
    {
        if (player != null)
            CheckPlayerPosition();
    }

    /// <summary>
    /// Реализация передвижения камеры при переходе игрока в другую комнату.
    /// </summary>
    private void CheckPlayerPosition()
    {
        if (player.transform.position.x > transform.position.x + 9.2)
            moveCameraHorizontal(1);
        else if (player.transform.position.x < transform.position.x - 9.5)
            moveCameraHorizontal(-1);
        else if (player.transform.position.y > transform.position.y + 5.2)
            moveCameraVertical(1);
        else if (player.transform.position.y < transform.position.y - 5.3)
            moveCameraVertical(-1);
    }

    /// <summary>
    /// Горизонтальное передвижение камеры.
    /// </summary>
    /// <param name="i"> Направление передвижения. </param>
    public void moveCameraHorizontal(int i)
    {
        if (i > 0)
        {
            transform.position += new Vector3(width, 0f, 0f);
            player.transform.position += new Vector3(3f, 0f, 0f);
        }
        else
        {
            transform.position -= new Vector3(width, 0f, 0f);
            player.transform.position += new Vector3(-3f, 0f, 0f);
        }
    }

    /// <summary>
    /// Вертикальное передвижение камеры.
    /// </summary>
    /// <param name="i"> Направление передвижения. </param>
    public void moveCameraVertical(int i)
    {
        if (i > 0)
        {
            transform.position += new Vector3(0f, height, 0f);
            player.transform.position += new Vector3(0f, 2f, 0f);
        }
        else
        {
            transform.position -= new Vector3(0f, height, 0f);
            player.transform.position += new Vector3(0f, -2f, 0f);
        }
    }

}