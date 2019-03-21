using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour {

    //记录目标场景的名字
    string destinationScene;

	//方法，切换场景
    public void SwitchScene(string targetScene)
    {
        //播放当前场景的渐出动画
        GetComponent<Animator>().SetTrigger("FadeOut");

        //获得目标场景的名字
        destinationScene = targetScene;

        //0.5秒之后，切换至目标场景
        Invoke("SwitchToDestinationScene", 0.5F);
    }

    //方法，从当前场景切换到目标场景
    void SwitchToDestinationScene()
    {
        //加载目标场景
        SceneManager.LoadScene(destinationScene);
    }
}
