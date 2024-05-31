using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// This component has to be put on a canvas set to render world space objects!
/// </summary>
public class WorldSpaceText : CanPause
{
    [SerializeField] private int amountOfWorldSpaceTextCached;
    [SerializeField] private GameObject textTemplate;
    
    private static Camera mCam;

    private List<WorldText> worldTexts;

    private List<WorldText> activeTexts;
    private List<WorldText> inActiveTexts;

    /// <summary>
    /// Only use it to access the object with your Id.
    /// </summary>
    public List<WorldText> WorldTexts => worldTexts;

    private void Awake()
    {
        worldTexts = new List<WorldText>();

        for (int i = 0; i < amountOfWorldSpaceTextCached; i++)
        {
            WorldText worldText = new WorldText(textTemplate, transform);
            worldTexts.Add(worldText);
        }

        inActiveTexts = worldTexts.ToList();
        activeTexts = new List<WorldText>();
    }

    private void Start()
    {
        mCam = Camera.main;
    }

    /// <summary>
    /// It returns -1 when no in active world text is available. If you're going to use the Id make sure it's not -1!
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="lifeTime"></param>
    /// <param name="movementSpeed"></param>
    /// <returns>Id is the index position of the instantiated WorldText in the worldTexts list.</returns>
    public int Instantiate(Vector3 pos, float lifeTime = Mathf.Infinity, float movementSpeed = 0)
    {
        if (activeTexts.Count < 1) return -1;

        var worldText = inActiveTexts[0];
        worldText.gameObject.transform.position = pos;
        worldText.lifeTime = lifeTime;
        worldText.movementSpeed = movementSpeed;

        inActiveTexts.Remove(worldText);
        activeTexts.Add(worldText);
        int Id = worldTexts.IndexOf(worldText);
        return Id;
    }

    /// <summary>
    /// This functions assumes that you have checked if Id is -1!
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="newText"></param>
    public void ChangeText(int Id, string newText)
    {
        worldTexts[Id].text.text = newText;
    }

    /// <summary>
    /// text object wont be null, but do not use it either way!
    /// </summary>
    /// <param name="text"></param>
    public void DestroyText(WorldText text)
    {
        activeTexts.Remove(text);
        inActiveTexts.Add(text);
    }

    protected override void FrameTick()
    {
        foreach (var worldText in activeTexts)
        {
            if (worldText.Update(Time.deltaTime)) DestroyText(worldText);
        }
    }

    public struct WorldText
    {
        public readonly TextMeshProUGUI text;
        public readonly GameObject gameObject;
        
        public float lifeTime;
        public float movementSpeed;
        
        public float currentLifeTime;

        public WorldText(GameObject obj, Transform parent)
        {
            gameObject = Instantiate(obj, parent);
            text = gameObject.GetComponent<TextMeshProUGUI>();
            
            gameObject.SetActive(false);

            lifeTime = 0;
            movementSpeed = 0;
            currentLifeTime = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <returns>Returns true if it should be destroyed.</returns>
        public bool Update(float deltaTime)
        {
            gameObject.transform.rotation = mCam.transform.rotation;
            gameObject.transform.Translate(0, movementSpeed * deltaTime, 0);

            currentLifeTime += deltaTime;
            return currentLifeTime > lifeTime;
        }
    }
}