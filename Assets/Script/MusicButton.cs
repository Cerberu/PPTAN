using UnityEngine;

public class MusicButton : MonoBehaviour {

    //当该游戏体激活时
    void OnEnable()
    {
        //根据背景音乐的开关状态，激活或禁用相关的按钮
        transform.GetChild(1 - MyClass.musicEnable).gameObject.SetActive(true);
        transform.GetChild(MyClass.musicEnable).gameObject.SetActive(false);
    }
}
