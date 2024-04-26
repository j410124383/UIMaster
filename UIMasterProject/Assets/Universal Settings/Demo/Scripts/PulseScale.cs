using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalSettings.Demo {
    public class PulseScale : MonoBehaviour {

		public float speed = 2f;

		private float pos = 0;
		private AudioSource audioSource;

		private void Awake() {
			audioSource = GetComponent<AudioSource>();
		}

		void Update() {
			pos += speed * Time.deltaTime;
			float alpha = Mathf.Sin(pos);

			float scale = 0.3f + (1f+alpha)*0.5f;
			transform.localScale = new Vector3(scale, scale, scale);	

			if (alpha >= 0.9 && audioSource.isPlaying == false) {
				audioSource.Play();
			}
        }
    }

}
