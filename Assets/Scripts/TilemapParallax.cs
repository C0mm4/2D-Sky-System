using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TilemapParallax : MonoBehaviour
{
    private float startPositionX, startPositionY;
    private float cameraStartPosX, cameraStartPosY;
    private Transform cameraTransform;
    public TilemapRenderer MapTile;
    public float parallaxEffectX; // X축 패럴랙스 강도
    public float parallaxEffectY; // Y축 패럴랙스 강도
    public float smoothSpeed = 0.1f;
    public virtual void Awake()
    {
        // Initialize tilemap bound size
        var bounds = GetComponent<TilemapRenderer>().bounds;

        // Initialize Main Camera
        cameraTransform = Camera.main?.transform;
        if (cameraTransform == null)
        {
            Debug.LogError("Main Camera not found!");
        }

        // Initialize Tilemap Position at start
        startPositionX = 0f;
        startPositionY = 0f;


        // Initialize Camera Position at Start - Render Type : Orthographic, Size : 1.7
        cameraStartPosX = 3.04f;
        cameraStartPosY = 1.7f;


    }
    public virtual void Update()
    {
        // Reset Tilemap bound size - for reset tilemap bounds on map inspector
        var bounds = GetComponent<TilemapRenderer>().bounds;

        // if camera is not found, refind camera
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
            return;
        }
        else
        {
            // tilemap position calculate
            float targetPositionX = startPositionX + (cameraTransform.position.x - cameraStartPosX) * parallaxEffectX;
            float targetPositionY = startPositionY + (cameraTransform.position.y - cameraStartPosY) * parallaxEffectY;
            Vector3 smoothPosition = Vector3.Lerp(transform.position, new Vector3(targetPositionX, targetPositionY, transform.position.z), 0.6f);
            transform.position = smoothPosition;
        }
    }

}
