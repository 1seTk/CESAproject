// ---------------------------------------
// Brief : 一時停止ボタン
// 
// Date  : 2016/06/26
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
	enum Direction
	{
		Neutral,
		Up,
		Down,
		Right,
		Left
	}

	private Vector2 m_touchStart;
	private Vector2 m_touchEnd;

	private ReactiveProperty<Direction> m_directionRP = new ReactiveProperty<Direction>();

	[SerializeField]
	private GameObject m_prefab;

	private GameObject m_instance;

	[SerializeField, Tooltip("フリック判定にする距離"), Range(0.0f, 100.0f)]
	private float m_flickDistance = 30.0f;

    private void Start()
    {
        var core = GameObject.Find("Player").GetComponentInChildren<PlayerCore>();
        var enter = core.GetComponent<PlayerEnter>();

        // フリック開始
        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ =>
            {
                m_touchStart = Input.mousePosition;

                // フリック方向のリセット
                m_directionRP.Value = Direction.Neutral;
            });

        // フリック終了
        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonUp(0))
            .Subscribe(_ =>
            {
                m_touchEnd = Input.mousePosition;

                // フリックの生成
                m_directionRP.Value = GetDirection();
            });

        // フリック検知後処理(下方向)
        m_directionRP
            .Where(_ => core != null)               // プレイヤーが死んでいない
            .Where(_ => enter.IsPlayerEnter == true)    // 登場してる
            .Where(_ => ShunLib.ClearDirection.instance.isClear() == false)    // ゴールしていない
            .Where(_ => !(Time.timeScale < 1.0f))   // ゴール演出が始まってない
            .Where(_ => m_instance == null)         // メニューが生成済みでない
            .Where(x => x == Direction.Down)        // 下方向にフリックされたら
            .Subscribe(_ =>
			{
				// 時を止める
				Time.timeScale = 0.0f;


				// ボタンの生成
				m_instance = Instantiate(m_prefab)as GameObject;

				// ポーズのフェードイン
				var canvasGroup = m_instance.GetComponent<CanvasGroup>();
				canvasGroup.alpha = 0.0f;
				canvasGroup.DOFade(1.0f, 0.5f).SetUpdate(true);

				m_instance.transform.parent = GetComponentInParent<Canvas>().transform;
				m_instance.transform.localScale = Vector3.one;
				m_instance.transform.localPosition = Vector3.zero;

                // ステージ名の設定
                SetStageNumber();

                PopButtons();
			});

		// 下以外なら消す
		m_directionRP
			.Where(_ => m_instance != null)
			.Where(x => x != Direction.Down)
			.Subscribe(_ =>
			{
				BackButton();
			});
	}

	private Direction GetDirection()
	{
		float dX = m_touchEnd.x - m_touchStart.x;
		float dY = m_touchEnd.y - m_touchStart.y;

		Direction direction = Direction.Neutral;

		if (Mathf.Abs(dY) < Mathf.Abs(dX))
		{
			// 右向きのフリック
			if (30 < dX) {
				direction = Direction.Right;
			}
			//左向きにフリック
			else if (-30 > dX) {
				direction = Direction.Left;
			}
		}
		else if (Mathf.Abs(dX) < Mathf.Abs(dY))
		{
			//上向きにフリック
			if (30 < dY){
				direction = Direction.Up;
			}
			//下向きのフリック
			else if (-30 > dY){
				direction = Direction.Down;
			}
		}

		// フリック方向を返す
		return direction;
	}

	void PopButtons()
	{
		if (m_instance == null) return;

		var buttons = m_instance.GetComponentsInChildren<Button>();

		foreach (var item in buttons)
		{
			Vector3 target = item.transform.position;
			item.transform.position = m_touchEnd;
			item.transform.DOMove(target, 0.2f).SetUpdate(true);
		}
	}

	void CollectButtons()
	{
		if (m_instance == null) return;

		var buttons = m_instance.GetComponentsInChildren<Button>();

		foreach (var item in buttons)
		{
			item.transform.DOMove(m_touchEnd, 0.2f);
		}
	}

	public void BackButton()
	{
		CollectButtons();

		// ポーズのフェードアウト
		m_instance.GetComponent<CanvasGroup>()
			.DOFade(0.0f, 0.2f)
			.SetUpdate(true)
			.OnKill(() => Destroy(m_instance));

		Time.timeScale = 1.0f;
	}

	void SetStageNumber()
	{
		if(YamagenLib.PlayInstructor.instance != null)
			transform.parent.GetComponentInChildren<Text>().text = YamagenLib.PlayInstructor.instance.GetLoadStage().ToString();
	}
}
