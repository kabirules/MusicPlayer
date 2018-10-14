using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(AudioSource))]
 public class Mp3AudioImporter : MonoBehaviour {
     
     private List<AudioClip> audioClips;
     // private const string musicDir = "/storage/emulated/0/Ringtones";
	 private const string musicDir = "c:/";

	 public AudioSource audio;
	 public Text songText;
	 public Button songButton;
	 public GameObject scrollViewContent;
	 private string song; 
         
     void Start()
     {
		songText.text = Directory.GetCurrentDirectory();
		this.audio = GetComponent<AudioSource>();
		audioClips = new List<AudioClip>();
		StartCoroutine("PlayAudioList");
     }
         
	IEnumerator DownloadPlaylist()
	{
		string[] playlist = Directory.GetFiles(@musicDir, "*.mp3", SearchOption.TopDirectoryOnly);
		
		foreach(string song in playlist)
		{
			songText.text = song;
			this.song = song;
			Button button = Instantiate(songButton) as Button;
			button.transform.SetParent(scrollViewContent.transform,false);
			button.GetComponentInChildren<Text>().text = song.Substring(song.IndexOf("/")+1);
			button.onClick.AddListener(delegate{StartCoroutine(LoadSong(song)); });
			WWW audioLoader = new WWW("file:///" + song);
			
			while( !audioLoader.isDone )
				yield return null;
			
			audioClips.Add(audioLoader.GetAudioClip(false));
		}
	}
         
         
	IEnumerator PlayAudioList()
	{    
		yield return StartCoroutine("DownloadPlaylist");

/*
		foreach(AudioClip song in audioClips)
		{
			this.audio.clip = song;
			this.audio.Play();
			Debug.Log("Here I am -> " + song.name);
			yield return new WaitForSeconds(song.length + 1.0f);
		}
*/		
	}

	public IEnumerator LoadSong(string song) {
		yield return StartCoroutine(SetSongInAudioList(song));
		Debug.Log(song);
		this.audio.clip = audioClips[0];
		this.audio.Play();		
	}

	IEnumerator SetSongInAudioList(string song) {
		audioClips.Clear();
		WWW audioLoader = new WWW("file:///" + song);
		while( !audioLoader.isDone )
			yield return null;
		audioClips.Add(audioLoader.GetAudioClip(false));
	}

	public void EmptyScrollView()
	{
		foreach (Transform child in scrollViewContent.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}

	public void GetDirectories() {
		var directories = Directory.GetDirectories("c:/", "*");
		foreach (string currentDir in directories)
		{
			Debug.Log(currentDir);
		}
	}
 }