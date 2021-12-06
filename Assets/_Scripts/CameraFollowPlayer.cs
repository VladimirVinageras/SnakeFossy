using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
     
    private Rigidbody _player;
    [SerializeField]
    private Vector3 _offset = new Vector3(0,20,-20);

    private float _deltaXpos;
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (!_player)
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        }

        _deltaXpos = transform.position.x - _player.transform.position.x;
        this.transform.position = _player.transform.position + _offset;
        transform.Translate(Vector3.right * _deltaXpos);
    }

}
