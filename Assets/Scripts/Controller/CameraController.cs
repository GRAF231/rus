using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    GameObject _player = null;
    PixelPerfectCamera camera;
    Vector3 _delta = new Vector3(0, 0, -10f);

    public void SetPlayer(GameObject player) { _player = player; }

    public void Start()
    {
        camera = gameObject.GetComponent<PixelPerfectCamera>();
        if (Util.isMobile())
        {
            camera.assetsPPU = 45;
        }
        else
        {
            camera.assetsPPU = 25;
        }
    }



    void Update()
    {
        if (object.ReferenceEquals(_player, null))
            return;

        transform.position = Vector3.Slerp(transform.position, _player.transform.position + _delta, 0.1f);
    }
}
