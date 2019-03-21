using UnityEngine;

public class MusicOffButton : MonoBehaviour {

    //MusicOn按钮
    public GameObject musicOnButton;

    //方法，执行该按钮被按下之后的操作
    public void MusicOffButtonClick()
    {
        //如果设置界面响应点击
        if (MenuController.Instance.settingInterfaceClickEnable)
        {
            #if (IOS_SDK)

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("MusicOffButton");

            #endif

            //背景音乐开关打开
            MyClass.musicEnable = 1;

            //将背景音乐开关状态存入玩家偏好中
            PlayerPrefs.SetInt("musicEnable", MyClass.musicEnable);

            //背景音乐开始播放
            MyClass.AudioPlay(GameObject.Find("MusicPlayer").GetComponent<AudioSource>(), MyClass.musicEnable);

            //MusicOn按钮激活
            musicOnButton.SetActive(true);

            //本按钮禁用
            gameObject.SetActive(false);
        }
    }
}
