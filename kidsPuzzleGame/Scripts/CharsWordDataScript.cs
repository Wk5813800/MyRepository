using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class CharsWordDataScript : MonoBehaviour
{
    [SerializeField]
    private Text charsText;
    [HideInInspector]
    public char charsValue;
    private  Button button;
    public AudioSource audioSource;
    public AudioClip audioClip;

    private void Awake()
    {
        // audioSource = GetComponent<AudioSource>();
        button = GetComponent<Button>();
        if(button)
        {
            button.onClick.AddListener( () => ClickedChar());
        }
    }
    public void SetAndDisplayCharacter(char characterValue)
    {
        charsText.text = characterValue + "";
        charsValue = characterValue;
    }

    void ClickedChar()
    {
             audioSource.PlayOneShot(audioClip);
        QuizGameManager.instance.SelectedCharFromOptions(this);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
