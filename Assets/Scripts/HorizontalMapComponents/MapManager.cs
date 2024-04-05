using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private string _mapName = null;
    [Header("References")]
    [SerializeField] private CanvasPlayController _controller = null;
    [SerializeField] private Joystick _joystick = null;
    [Header("Next Scene")]
    [Tooltip("Next scene comes after completing this map.")]
    [SerializeField] private string _nextSceneName = null;
    [SerializeField] private Image _blackScreen = null;
    [SerializeField] private TextMeshProUGUI _mapNameText = null;
    private GameDelegate _delegate = null;
    private List<EnemyBehaviour> _enemies = new List<EnemyBehaviour>();
    private float _timer = 0.0f;

    public static MapManager Instance
    {
        get;
        private set;
    }

    public static ObjectPool ObjectPool
    {
        get;
        private set;
    }

    public string MapName
    {
        get
        {
            return _mapName;
        }
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// Game pause. Hides play controller UI.
    /// </summary>
    public void PauseGame(bool pause)
    {
        // Play controller UI
        _controller.SetControllerActive(!pause);

        // Player
        HorizontalPlayerControl.Instance.PausePlayerControl(pause);

        // Enemy
        foreach (EnemyBehaviour enemy in _enemies)
        {
            enemy.SetEnemyPause(pause);
        }
    }


    public void UrgentAlert()
    {
        foreach (EnemyBehaviour enemy in _enemies)
        {
            enemy.ReceiveUrgentAlert();
        }
    }


    public void EnemyDeathReport(EnemyBehaviour reporter)
    {
        _enemies.Remove(reporter);
    }


    public void LoadNextScene()
    {
        PauseGame(true);
        _timer = 1.0f;
        _delegate += BlackScreenFadeIn;
    }



    /* ==================== Private Methods ==================== */

    private void BlackScreenFadeOut()
    {
        if (_timer < 1.0f)
        {
            _timer += Time.deltaTime;
            if (_timer >= 1.0f)
            {
                _blackScreen.color = new Color(
                    0.0f,
                    0.0f,
                    0.0f,
                    0.0f
                );
                PauseGame(false);
            }
            else
            {
                _blackScreen.color = new Color(
                    0.0f,
                    0.0f,
                    0.0f,
                    Mathf.Cos(_timer * Mathf.PI) * 0.5f + 0.5f
                );
            }
        }
        else if (_timer < 2.0f)
        {
            _timer += Time.deltaTime;
            if (_timer >= 2.0f)
            {
                _mapNameText.color = new Color(
                    1.0f,
                    1.0f,
                    1.0f,
                    1.0f
                );
            }
            else
            {
                _mapNameText.color = new Color(
                    1.0f,
                    1.0f,
                    1.0f,
                    Mathf.Cos(_timer * Mathf.PI) * 0.5f + 0.5f
                );
            }
        }
        else if (_timer > 3.0f)
        {
            _timer += Time.deltaTime;
            if (_timer >= 4.0f)
            {
                Destroy(_mapNameText.gameObject);
                _delegate -= BlackScreenFadeOut;
            }
            else
            {
                _mapNameText.color = new Color(
                    1.0f,
                    1.0f,
                    1.0f,
                    Mathf.Cos((_timer + 1.0f) * Mathf.PI) * 0.5f + 0.5f
                );
            }
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }


    private void BlackScreenFadeIn()
    {
        if (_timer < 2.0f)
        {
            _timer += Time.deltaTime;
            if (_timer >= 2.0f)
            {
                _blackScreen.color = new Color(
                    0.0f,
                    0.0f,
                    0.0f,
                    1.0f
                );
                _delegate = null;
                SceneManager.LoadScene(_nextSceneName);
            }
            else
            {
                _blackScreen.color = new Color(
                    0.0f,
                    0.0f,
                    0.0f,
                    Mathf.Cos(_timer * Mathf.PI) * 0.5f + 0.5f
                );
            }
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }


    private void Awake()
    {
        // Singletop pattern
        Instance = this;

        // ObjectPool
        ObjectPool = new ObjectPool(transform.Find("ObjectPool"));

        // Find all enemies
        foreach (EnemyBehaviour enemy in transform.Find("Enemies").GetComponentsInChildren<EnemyBehaviour>())
        {
            _enemies.Add(enemy);
        }

        // Black screen
        _blackScreen.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        _mapNameText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        _mapNameText.text = MapName;

        // Delegate
        _delegate += _joystick.JoystickUpdate;
        _delegate += BlackScreenFadeOut;
    }


    private void Update()
    {
        _delegate.Invoke();
    }
}
