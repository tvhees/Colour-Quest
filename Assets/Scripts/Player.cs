using UnityEngine;
using System.Collections;
using System;

public class Player : MovingObject<Player> {
	public float moveDistance;
    public MeshRenderer childRenderer;
    public CameraScript cameraScript;

    public override void Reset()
    {
        transform.position = startLocation;
        moveDistance = 2.5f;
        childRenderer.enabled = true;
        cameraScript.Reset(transform);
    }
}
