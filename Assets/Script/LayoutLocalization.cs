using UnityEngine;

public class LayoutLocalization : MonoBehaviour {

    //该游戏体中文版本下的本地位置
    public Vector3 chineseLocalPosition;

    //该游戏体非中文版本下的本地位置
    public Vector3 englishLocalPosition;

	// Use this for initialization
	void Start () {

         //如果为中文版本
        if (MyClass.localizationLanguageIndex == 0)
        {
            //设置该游戏体中文版本下的本地位置
            GetComponent<RectTransform>().anchoredPosition3D = chineseLocalPosition;
        }

        //否则
        else
        {
            //设置该游戏体中文版本下的本地位置
            GetComponent<RectTransform>().anchoredPosition3D = englishLocalPosition;
        }	
	}
}
