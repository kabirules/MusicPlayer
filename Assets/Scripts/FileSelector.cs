using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

public class FileSelector : MonoBehaviour {

	private string[] directories;
	private string[] files;
	private string upDirectory;	
	private string currentDir;
	public AudioSource audioSource;

	public Button fileButton; // Prefab
	public GameObject scrollViewContent;
	public Text currentFolderText;

	public GameObject fileLoadPanel;
	public GameObject playerPanel;
	public Text songText;
	public Slider slider;

	// Particles
	public ParticleSystem particle1;	
	public ParticleSystem particle2;
	public ParticleSystem particle3;
	public ParticleSystem particle4;

	private int nextUpdate = 1;

	// Use this for initialization
	void Start () {
#if UNITY_ANDROID
		Debug.Log("Unity_Android");
		this.currentDir = "/storage/emulated/0/";
#endif
#if UNITY_EDITOR
		Debug.Log("Unity_Editor");
		this.currentDir = "c:/";
#endif
		this.slider.normalizedValue = 0;
		this.audioSource = GetComponent<AudioSource>();
		this.BuildFileSystem();
	}

	
	// Update is called once per frame
	void Update () {
		if (this.audioSource &&
			this.audioSource.isPlaying) {

			if (Time.time >= nextUpdate){
				// Debug.Log(Time.time+">="+nextUpdate);
				// Change the next update (current second+1)
				nextUpdate=Mathf.FloorToInt(Time.time)+1;
				// Call your fonction
				this.UpdateEverySecond();
			}
		} else {
			this.StopAllParticles();
		}
		
	}

	private void BuildFileSystem() {
		currentFolderText.text = this.currentDir;
		this.directories = Directory.GetDirectories(this.currentDir, "*");
		this.files = Directory.GetFiles(this.currentDir, "*.mp3");
		if (!this.currentDir.Equals("c:/")) {
			Button button = Instantiate(fileButton) as Button;
			button.transform.SetParent(scrollViewContent.transform,false);
			button.GetComponentInChildren<Text>().text = "..";
			button.onClick.AddListener(UpDirectory);
		}
		foreach (string directory in this.directories) {
			string directoryModded = directory.Replace("\\", "/");
			Button button = Instantiate(fileButton) as Button;
			button.transform.SetParent(scrollViewContent.transform,false);
			button.GetComponentInChildren<Text>().text = directoryModded.Substring(directoryModded.IndexOf("/")+1);
			button.onClick.AddListener(delegate {EnterDirectory(directoryModded); });
		}
		foreach (string file in this.files) {
			Button button = Instantiate(fileButton) as Button;
			button.transform.SetParent(scrollViewContent.transform,false);
			button.GetComponentInChildren<Text>().text = file.Substring(file.IndexOf("/")+1);
			button.onClick.AddListener(delegate{StartCoroutine(ManageFile(file)); });
		}
		this.scrollViewContent.transform.position = new Vector2(this.scrollViewContent.transform.position.x, -99999f);
	}

	public void UpDirectory() {
		string newDir = this.currentDir.Substring(0, this.currentDir.LastIndexOf("/"));
		if (newDir.IndexOf("/") == -1) {
			newDir = newDir + "/";
		}
		this.currentDir = newDir;
		this.EmptyScrollView();
		this.BuildFileSystem();
	}

	public void EnterDirectory(string directory) {
		try {
			string newDir = directory;
			Debug.Log(newDir);
			this.currentDir = newDir;
			this.EmptyScrollView();
			this.BuildFileSystem();
		} catch (Exception e) {
			Debug.Log(e); //TODO Show error message somehow to the user
			this.UpDirectory();
		}
	}

	IEnumerator ManageFile(string file) {
		Debug.Log(file); //TODO Do something with the file
		this.fileLoadPanel.SetActive(false);
		this.playerPanel.SetActive(true);
		this.songText.text = file.Substring(file.LastIndexOf("/")+1,file.LastIndexOf(".")-3);
		// As it has to be an mp3, load the AudioSource.
		WWW audioLoader = new WWW("file:///" + file);
		while( !audioLoader.isDone )
			yield return null;
		this.audioSource.clip = audioLoader.GetAudioClip(false);
	}	

	public void EmptyScrollView()
	{
		foreach (Transform child in scrollViewContent.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}

	public void PlaySong() {
		this.slider.normalizedValue = 0f;
		this.PlayRandomParticle();
		this.audioSource.Play();
	}

	public void StopSong() {
		this.slider.normalizedValue = 0f;
		this.StopAllParticles();
		this.audioSource.Stop();
	}

	public void PlayRandomParticle() {
		this.StopAllParticles();
		int rndInteger =  UnityEngine.Random.Range(1, 5);
		Debug.Log(rndInteger);
		if (rndInteger == 1) {
			this.particle1.Play();	
		} else if (rndInteger == 2) {
			this.particle2.Play();
		} else if (rndInteger == 3) {
			this.particle3.Play();
		} else if (rndInteger == 4) {
			this.particle4.Play();
		} else {
			Debug.Log("I shouldn't be here...");
		}
	}

	private void StopAllParticles() {
		this.particle1.Stop();
		this.particle2.Stop();
		this.particle3.Stop();
		this.particle4.Stop();
	}

	void UpdateEverySecond() {
		Debug.Log(1f / this.audioSource.clip.length);
		this.slider.normalizedValue = this.slider.normalizedValue + 1f / this.audioSource.clip.length;
		Debug.Log(this.slider.normalizedValue);
	}
}
