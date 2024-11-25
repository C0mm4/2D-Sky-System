using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CloudLayer : TilemapParallax
{
    public List<GameObject> clouds;
    public float cloudSpd;
    public float tilemapRightMax;
    public float minY, maxY;

    public GameObject cloudPrefab;

    public Material cloudMaterial;

    float lastCloudT;
    float nextT;

    public List<Sprite> cloudSprites;

    public InGameTime gameTime;

    public override void Awake()
    {
        base.Awake();
        // cloud generate position X - out of screen + 1 tile
        tilemapRightMax = transform.position.x + 22 * 0.32f;

        // set cloud generate position Y range - top of Tilemap bound size to half
        maxY = Math.Max(GetComponent<Tilemap>().cellBounds.size.y - 11, 0) * 0.32f * (1 - parallaxEffectY) + 11 * 0.32f;
        minY = maxY / 2;
        nextT = 0;
        CloudInitialize();
    }

    public override void Update()
    {
        base.Update();

        tilemapRightMax = transform.position.x + 22 * 0.32f;
        moveClouds();

        lastCloudT += Time.deltaTime;

        // if cloud generate time over nextT, Create new cloud
        if(lastCloudT > nextT)
        {
            CreateCloud();
            lastCloudT = 0;
            // next cloud generate time set
            nextT = UnityEngine.Random.Range(1.5f, 2);
        }

        // After clouds move, checking out of position cloud and delete it
        DeleteCloud();

        // Set Material time
        cloudMaterial.SetFloat("_CustomTime", gameTime.time / 1440f);
    }

    public void moveClouds()
    {
        // move clouds at speed;
        foreach (var cloud in clouds)
        {
            cloud.transform.position -= new Vector3(cloudSpd, 0, 0) * Time.deltaTime;
        }
    }

    public void CloudInitialize()
    {
        float xMin, xMax;
        xMin = 0;
        xMax = tilemapRightMax + 0.64f;

        for (int i = 0; i < GetComponent<Tilemap>().size.x; i += 2)
        {
            int attempt = 0;
            bool isPositionValid;

            // Random Cloud Sprite Image
            int randomIndex = UnityEngine.Random.Range(0, cloudSprites.Count);
            isPositionValid = true;

            float randomX, randomY;
            Vector3 pos;

            do
            {
                randomX = UnityEngine.Random.Range(xMin, xMax);
                randomY = UnityEngine.Random.Range(minY, maxY);


                pos = new Vector3(randomX, randomY, 9);

                // if clouds so close, refind new position
                foreach (var cloud in clouds)
                {
                    if (Vector2.Distance(cloud.transform.position, pos) <= 0.48f)
                    {
                        isPositionValid = false;
                        break;
                    }
                }
                attempt++;

            } while (!isPositionValid && attempt < 10);

            // if find valid new position, create new cloud
            if (isPositionValid)
            {
                GameObject go = Instantiate(cloudPrefab, pos, Quaternion.identity, transform);
                go.GetComponent<SpriteRenderer>().sprite = cloudSprites[randomIndex];
                clouds.Add(go);
            }
        }

    }

    public void CreateCloud()
    {
        float newCloudX = tilemapRightMax + 0.64f;

        // Random Cloud Sprite Image
        int randomIndex = UnityEngine.Random.Range(0, cloudSprites.Count);

        bool isPositionValid;
        int attempt = 0;

        float randomY;
        Vector3 pos;

        do
        {
            randomY = UnityEngine.Random.Range(minY, maxY);
            pos = new Vector3(newCloudX, randomY, 9);

            isPositionValid = true;

            // if clouds so close, refind new position
            foreach (var cloud in clouds)
            {
                if (Vector2.Distance(cloud.transform.position, pos) <= 0.48f)
                {
                    isPositionValid = false;
                    break;
                }
            }
            attempt++;

        } while (attempt < 10 && isPositionValid);

        // is find valid new position, create new cloud
        if (isPositionValid)
        {

            GameObject go = Instantiate(cloudPrefab, pos, Quaternion.identity, transform);
            go.GetComponent<SpriteRenderer>().sprite = cloudSprites[randomIndex];
            clouds.Add(go);
        }
    }

    public void DeleteCloud()
    {
        List<GameObject> removeCloue = new();

        // if cloud is out of screen, Add cloud delete list
        foreach(var cloud in clouds)
        {
            if(cloud.transform.position.x < transform.position.x - 0.64f)
            {
                removeCloue.Add(cloud);
            }
        }

        // Remove clouds in delete list
        foreach(var cloud in removeCloue)
        {
            clouds.Remove(cloud);
            
            Destroy(cloud);
        }
    }
}
