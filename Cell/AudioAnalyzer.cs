/* Stewart Gray III - CELL Project
 * January 6, 2017
 * Script for reading and manipulating audio input from Audio Sources in the scene
 */
using UnityEngine;
using System.Collections;

public class AudioAnalyzer : MonoBehaviour {

// Constant used to scale the magnitude of the spectrum's output	
private const float SCALE = 32f;
// Constant used to declare the number of audio samples to be captured
private const int SAMPLES = 1024;	
private const int BANDS = 3;
private int seed;
private float[] audioSpectrum;
private float[] freq;
private float scaleMultiplier;
private Vector3 startScale; 



	void Awake(){
		changeFreqAlloc();
		// Initializing an array containing 1024 audio samples
		audioSpectrum = new float [SAMPLES];
		freq = new float [BANDS];
		visualizeScale();
		visualizeHeight();
	}

	void Start(){
		// Changes the seed so different terrain elements can have different frequencies every 2 seconds
		InvokeRepeating("changeFreqAlloc", 0.0f, 2f);
	}

	void Update(){
		// Populating the array with samples from channel 0
		// The BlackmanHarris Window is used to produce a clean, usable signal from the audio
		AudioListener.GetSpectrumData (audioSpectrum, 1, FFTWindow.BlackmanHarris);

		// Averaging the low frequencies into one band (0-172Hz)
		for(int i=0; i<4; i++){
			freq[0] = audioSpectrum[i]; 
		}
		//Averaging the Mid frequencies into one band (172-3014Hz)
		for(int i=0; i<70; i++){
			freq[1] = audioSpectrum[i + 5];
		}
		//Averaging the High frequencies into one band (3014 - 44100Hz)
		for(int i=0; i<950; i++){
			freq[2] = audioSpectrum[i + 74];
		}

	}

	/* Picks a random frequency band from an averaged amount of the samples defined above */
	void changeFreqAlloc(){
		seed = Random.Range(0, SAMPLES / 128);
	}
	/* Scales the y+ component of a terrain element in proportion to its assigned averaged frequency band */
	void FixedUpdate(){
		transform.localScale = new Vector3(1f, Mathf.Lerp(transform.localScale.y, (audioSpectrum[seed] * SCALE) + 1f, Time.deltaTime * 3f), 1f);	
	}

	/* Randomizes the height of each terrain element once at the start of each track */
	public void visualizeHeight(){
		transform.localPosition= new Vector3 (transform.localPosition.x,(Random.Range(1f, 2f)+startScale.y),transform.localPosition.z);

	}
	/* Randomizes the scale of each terrain element */
	public void visualizeScale(){
		scaleMultiplier=Random.Range(0.6f, 1f);
		transform.localScale= new Vector3 (scaleMultiplier, transform.localScale.y,scaleMultiplier);

	}
}	

	
		
			
	

	

