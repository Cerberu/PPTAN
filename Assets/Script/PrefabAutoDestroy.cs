using UnityEngine;

public class PrefabAutoDestroy : MonoBehaviour {

    //该预置的寿命
    public float lifeTime;

    //该预置的存活计时
    float timer;

	// Use this for initialization
	void Start () {

        //初始时计时为0
        timer = 0;	
	}
	
	// Update is called once per frame
	void Update () {
	
        //计时递增
        timer += Time.deltaTime;

        //如果计时到达寿命期限
        if (timer >= lifeTime)
        {
            //计时归0
            timer = 0;

            //自身禁用
            gameObject.SetActive(false);
        }
	}
}
