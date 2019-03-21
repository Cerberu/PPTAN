using UnityEngine;

public class PauseInterface : MonoBehaviour {

    //方法，执行暂停界面入场动画结束之后的操作
    public void PauseInterfaceStartCompleted()
    {
        //暂停界面响应点击
        GameController.Instance.pauseInterfaceClickEnable = true;
    }

    //方法，执行暂停界面出场动画结束之后的操作
    public void PauseInterfaceEndCompleted()
    {
        //暂停界面禁用
        gameObject.SetActive(false);

        //游戏状态变为playing
        MyClass.gameState = GameState.Playing;
    }
}
