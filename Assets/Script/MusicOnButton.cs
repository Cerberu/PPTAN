using UnityEngine;

public class MusicOnButton : MonoBehaviour {

    //MusicOff按钮
    public GameObject musicOffButton;

    //方法，执行该按钮被点击之后的操作
    public void MusicOnButtonClick()
    {
        //如果设置界面响应点击
        if (MenuController.Instance.settingInterfaceClickEnable)
        {
            #if (IOS_SDK)

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("MusicOnButton");

            #endif

            //播放按钮音效
            MyClass.AudioPlay(GameObject.Find("SoundPlayer").GetComponent<AudioSource>(),
                              Resources.Load<AudioClip>("Audio/button"),
                              MyClass.soundEnable);

            //背景音乐开关关闭
            MyClass.musicEnable = 0;

            //将背景音乐开关状态存入玩家偏好中
            PlayerPrefs.SetInt("musicEnable", MyClass.musicEnable);

            //背景音乐停止
            MyClass.AudioStop(GameObject.Find("MusicPlayer").GetComponent<AudioSource>());

            //MusicOff按钮激活
            musicOffButton.SetActive(true);

            //本按钮禁用
            gameObject.SetActive(false);
        }
    }
}
