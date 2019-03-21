using UnityEngine;
using UnityEngine.UI;

public class AchieveInterface : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        //内部计数器
        int i = 0;

        //遍历每个成就
        for (i = 0; i < MyClass.achievementCompletedState.Length; i++)
        {
            //从玩家偏好中读取该成就是否已达成
            MyClass.achievementCompletedState[i] = PlayerPrefs.GetInt("achievementCompletedState" + i, 0);

            //根据该成就是否达成，来显示该成就Logo对应的图片
            transform.GetChild(i + 1).GetComponent<Image>().sprite = Resources.Load<Sprite>("AchieveSprites/" + (16 * (1 - MyClass.localizationLanguageIndex) + 2 * i + MyClass.achievementCompletedState[i]));
        }
    }

    //方法，执行成就界面入场动画结束之后的操作
    public void AchieveInterfaceStartCompleted()
    {
        //成就界面响应点击
        MenuController.Instance.achieveInterfaceClickEnable = true;
    }

    //方法，执行成就界面离场动画结束之后的操作
    public void AchieveInterfaceEndCompleted()
    {
        //成就界面暂时禁用
        gameObject.SetActive(false);

        //首页界面允许点击
        MenuController.Instance.menuInterfaceClickEnable = true;
    }
}
