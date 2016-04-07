using UnityEngine;
using System.Collections;
using System;

public class Player : MovingObject {
	public float moveDistance;
    public MeshRenderer childRenderer;
    public CameraScript cameraScript;

    public override void Reset()
    {
        Master.Instance.player = this;
        transform.position = startLocation;
        moveDistance = 2.5f;
        childRenderer.enabled = true;
        cameraScript.Reset(transform);
    }
}
