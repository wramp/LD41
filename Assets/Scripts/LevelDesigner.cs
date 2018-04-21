﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDesigner : MonoBehaviour
{

    void Start()
    {
        var count = transform.childCount;

        for (int i = 0; i < count; i++)
        {
            var platform = transform.GetChild(i);
            var tr = platform.transform;


            var whiteplatform = Instantiate(platform);
            whiteplatform.gameObject.layer = 1; //transparentFX layer
            whiteplatform.GetComponent<SpriteRenderer>().color = Color.white;
            whiteplatform.position = new Vector3(whiteplatform.position.x, whiteplatform.position.y, whiteplatform.position.z - 0.1f);
            whiteplatform.SetParent(tr);


        }

    }
}
