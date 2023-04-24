using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass6_0
	{
		public string name;

		internal bool _003CPlayUIs_003Eb__0(Sound sound)
		{
			return sound.name == name;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_0
	{
		public string name;

		internal bool _003CStopPlayUIS_003Eb__0(Sound sound)
		{
			return sound.name == name;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass8_0
	{
		public string name;

		internal bool _003CPlayPlayerS_003Eb__0(Sound sound)
		{
			return sound.name == name;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_0
	{
		public string name;

		internal bool _003CPlayMusicS_003Eb__0(Sound sound)
		{
			return sound.name == name;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_0
	{
		public string name;

		internal bool _003CStopPlayMusicS_003Eb__0(Sound sound)
		{
			return sound.name == name;
		}
	}

	public static AudioManager instance;

	[NonReorderable]
	public Sound[] UISounds;

	[NonReorderable]
	public Sound[] playerSounds;

	[NonReorderable]
	public Sound[] musicS;

	[Range(0f, 1f)]
	public float volume;

	private void Awake()
	{
		instance = this;
		Sound[] uISounds = UISounds;
		foreach (Sound sound in uISounds)
		{
			sound.source = base.gameObject.AddComponent<AudioSource>();
			sound.source.clip = sound.clip;
			sound.source.volume = sound.volume;
			sound.source.pitch = sound.pitch;
			sound.source.loop = sound.loop;
		}
		uISounds = playerSounds;
		foreach (Sound sound2 in uISounds)
		{
			sound2.source = base.gameObject.AddComponent<AudioSource>();
			sound2.source.clip = sound2.clip;
			sound2.source.volume = sound2.volume;
			sound2.source.pitch = sound2.pitch;
			sound2.source.loop = sound2.loop;
		}
		uISounds = musicS;
		foreach (Sound sound3 in uISounds)
		{
			sound3.source = base.gameObject.AddComponent<AudioSource>();
			sound3.source.clip = sound3.clip;
			sound3.source.volume = sound3.volume;
			sound3.source.pitch = sound3.pitch;
			sound3.source.loop = sound3.loop;
		}
	}

	public void PlayUIs(string name)
	{
		_003C_003Ec__DisplayClass6_0 _003C_003Ec__DisplayClass6_ = new _003C_003Ec__DisplayClass6_0();
		_003C_003Ec__DisplayClass6_.name = name;
		Sound sound = Array.Find(UISounds, _003C_003Ec__DisplayClass6_._003CPlayUIs_003Eb__0);
		if (sound == null)
		{
			Debug.Log("Sound " + _003C_003Ec__DisplayClass6_.name + "not found");
		}
		else
		{
			sound.source.Play();
		}
	}

	public void StopPlayUIS(string name)
	{
		_003C_003Ec__DisplayClass7_0 _003C_003Ec__DisplayClass7_ = new _003C_003Ec__DisplayClass7_0();
		_003C_003Ec__DisplayClass7_.name = name;
		Sound sound = Array.Find(UISounds, _003C_003Ec__DisplayClass7_._003CStopPlayUIS_003Eb__0);
		if (sound == null)
		{
			Debug.Log("Sound " + _003C_003Ec__DisplayClass7_.name + "not found");
		}
		else
		{
			sound.source.Stop();
		}
	}

	public void PlayPlayerS(string name)
	{
		_003C_003Ec__DisplayClass8_0 _003C_003Ec__DisplayClass8_ = new _003C_003Ec__DisplayClass8_0();
		_003C_003Ec__DisplayClass8_.name = name;
		Sound sound = Array.Find(playerSounds, _003C_003Ec__DisplayClass8_._003CPlayPlayerS_003Eb__0);
		if (sound == null)
		{
			Debug.Log("Sound " + _003C_003Ec__DisplayClass8_.name + "not found");
		}
		else
		{
			sound.source.Play();
		}
	}

	public void PlayMusicS(string name)
	{
		_003C_003Ec__DisplayClass9_0 _003C_003Ec__DisplayClass9_ = new _003C_003Ec__DisplayClass9_0();
		_003C_003Ec__DisplayClass9_.name = name;
		Sound sound = Array.Find(musicS, _003C_003Ec__DisplayClass9_._003CPlayMusicS_003Eb__0);
		if (sound == null)
		{
			Debug.Log("Sound " + _003C_003Ec__DisplayClass9_.name + "not found");
		}
		else
		{
			sound.source.Play();
		}
	}

	public void StopPlayMusicS(string name)
	{
		_003C_003Ec__DisplayClass10_0 _003C_003Ec__DisplayClass10_ = new _003C_003Ec__DisplayClass10_0();
		_003C_003Ec__DisplayClass10_.name = name;
		Sound sound = Array.Find(musicS, _003C_003Ec__DisplayClass10_._003CStopPlayMusicS_003Eb__0);
		if (sound == null)
		{
			Debug.Log("Sound " + _003C_003Ec__DisplayClass10_.name + "not found");
		}
		else
		{
			sound.source.Stop();
		}
	}
}
