using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public SpriteRenderer targetSprite;
    private Vector2 lastScreenSize;
    private float offset;

    void Start()
    {
        AdjustCameraSize();
        lastScreenSize = new Vector2(Screen.width, Screen.height);
    }

    void Update()
    {
        // Check if the screen size has changed
        if (Screen.width != lastScreenSize.x || Screen.height != lastScreenSize.y)
        {
            AdjustCameraSize();
            lastScreenSize = new Vector2(Screen.width, Screen.height);
        }
    }

    void AdjustCameraSize()
    {
        if (targetSprite == null) return;

        float spriteWidth = targetSprite.bounds.size.x;
        float screenAspect = (float)Screen.width / Screen.height;
        float targetOrthoSize = spriteWidth / (2f * screenAspect);

        if(Screen.width>=Screen.height)
        {
            // landscape
            offset = 3;
        }
        else
            offset = 1;

        Camera.main.orthographicSize = targetOrthoSize+offset;
    }
}
