using UnityEngine;
using DG.Tweening;

public class AudioPlay : MonoBehaviour
{
    // BGM再生用のAudioSourceを2つ用意
    [SerializeField]
    private AudioSource _source0;

    [SerializeField]
    private AudioSource _source1;


    public void CrossFade(float maxVolume, float fadingTime)
    {
        var fadeInSource =
            _source0.isPlaying ?
            _source1 :
            _source0;

        var fadeOutSource =
            _source0.isPlaying ?
            _source0 :
            _source1;

        fadeInSource.Play();
        fadeInSource.DOKill();
        fadeInSource.DOFade(maxVolume, fadingTime);

        fadeOutSource.DOKill();
        fadeOutSource.DOFade(0, fadingTime);
    }

}