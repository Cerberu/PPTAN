using UnityEngine;
using System.Collections;

public class Surprise : MonoBehaviour {

	//Surprise爆炸
    void SurpriseExplode()
    {
        //在Surpise的周围，产生4枚爆炸效果
        BlockDisappearPool.GetBlockDisappearParticle(transform.position + Vector3.up);
        BlockDisappearPool.GetBlockDisappearParticle(transform.position + Vector3.down);
        BlockDisappearPool.GetBlockDisappearParticle(transform.position + 1.5F * Vector3.left);
        BlockDisappearPool.GetBlockDisappearParticle(transform.position + 1.5F * Vector3.right);

        //播放礼花爆炸音效
        MyClass.AudioPlay(GameObject.Find("SoundPlayer").GetComponent<AudioSource>(),
                          Resources.Load<AudioClip>("Audio/Pop" + Random.Range(0, 4)),
                          MyClass.soundEnable);
    }

    //Surprise入场动画结束之后的操作
    void SurpriseStartCompleted()
    {
        //Surprise禁用
        gameObject.SetActive(false);
    }
}
