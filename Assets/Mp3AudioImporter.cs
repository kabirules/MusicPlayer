 using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;
 using System.IO;
 
 [RequireComponent(typeof(AudioSource))]
 public class Mp3AudioImporter : MonoBehaviour {
     
     private List<AudioClip> audioClips;
     private const string musicDir = "/";

	 public AudioSource audio;
         
     void Start()
     {
		this.audio = GetComponent<AudioSource>();
		audioClips = new List<AudioClip>();
		StartCoroutine("PlayAudioList");
     }
         
	IEnumerator DownloadPlaylist()
	{
		string[] playlist = Directory.GetFiles(@musicDir, "*.mp3", SearchOption.TopDirectoryOnly);
		
		foreach(string song in playlist)
		{
			Debug.Log("file://" + song);
			WWW audioLoader = new WWW("file:///" + song);
			
			while( !audioLoader.isDone )
				yield return null;
			
			audioClips.Add(audioLoader.GetAudioClip(false));
		}    
	}
         
         
	IEnumerator PlayAudioList()
	{    
		yield return StartCoroutine("DownloadPlaylist");

		foreach(AudioClip song in audioClips)
		{
			this.audio.clip = song;
			this.audio.Play();
			Debug.Log("Here I am -> " + song.name);
			yield return new WaitForSeconds(song.length + 1.0f);
		}
	}
 }