using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
     
    private Rigidbody player;
    [SerializeField]
    private Vector3 offset = new Vector3(0,20,-20);

    private float deltaXpos;
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        }

        deltaXpos = transform.position.x - player.transform.position.x;
        this.transform.position = player.transform.position + offset;
        transform.Translate(Vector3.right * deltaXpos);
    }

}
