using UnityEngine;

namespace OmNomNomtek.Domain
{
  public class ThingyEater : MonoBehaviour
  {
    [SerializeField]
    private float _speed = 1.0f;

    private GameObject _gameObjectToFollow;

    private void FixedUpdate()
    {
      if (_gameObjectToFollow != null)
      {
        Vector3 direction = _gameObjectToFollow.transform.position - this.transform.position;

        this.transform.position += direction.normalized * _speed * Time.fixedDeltaTime;

        this.transform.rotation = Quaternion.LookRotation(direction);
      }
    }

    public void StartFollowing(GameObject gameObject)
    {
      Debug.Log($"Start following {gameObject.name}!");

      _gameObjectToFollow = gameObject;
    }

    public void StopFollowing()
    {
      Debug.Log($"Stop following!");

      _gameObjectToFollow = null;
    }
  }
}
