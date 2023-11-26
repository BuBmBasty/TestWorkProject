using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private float _visibleDistance, _newStageDistance;
    [SerializeField] private GameObject _balls;
    [SerializeField] private TMP_Text _distanceText;

    private Transform _playerOne, _playerTwo;

    private float _distance;
    
    void Start()
    {
        _playerOne = GameObject.FindGameObjectWithTag("PlayerOne").transform;
        _playerTwo = GameObject.FindGameObjectWithTag("PlayerTwo").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _distance = Vector3.Distance(_playerOne.position, _playerTwo.position);
        _distanceText.text = _distance.ToString();
        if (_distance <= _newStageDistance)
        {
            _balls.SetActive(true);
            SceneManager.LoadScene(1);
        }
        else if (_distance <= _visibleDistance)
        {
            _balls.SetActive(true);
        }
        else
        {
            _balls.SetActive(false);
        }
    }
}
