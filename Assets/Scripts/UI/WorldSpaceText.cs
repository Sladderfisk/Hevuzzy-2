using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldSpaceText : MonoBehaviour
{
    private float lifeTime = Mathf.Infinity;
    private float movementSpeed;

    private float currentLifeTime;

    private static Camera mCam;
    private TextMeshProUGUI myText;
    
    private void Awake()
    {
        myText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        mCam = Camera.main;
    }

    public static WorldSpaceText Instantiate(WorldSpaceText worldSpaceTextPrefab, Vector3 pos, float lifeTime = Mathf.Infinity, float movementSpeed = 0)
    {
        var text = Instantiate(worldSpaceTextPrefab, pos, Quaternion.identity);
        text.lifeTime = lifeTime;
        text.movementSpeed = movementSpeed;
        return text;
    }

    public void ChangeText(string newText)
    {
        myText.text = newText;
    }

    private void Update()
    {
        transform.rotation = mCam.transform.rotation;
        transform.Translate(0, movementSpeed * Time.deltaTime, 0);

        currentLifeTime += Time.deltaTime;
        if (currentLifeTime > lifeTime) Destroy(gameObject);
    }
}