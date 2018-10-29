using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	private float time;
	private int points;
	private float nextUpdate;
	private CubeData lastCube;
	private int multiplier;

	public static int state;
	private static GameManager instance;

	public const int STANDBY	= 0;
	public const int PLAYING	= 1;
	public const int TIMEOVER	= 2;
	public const int SUCCESS	= 3;
	public const int DIE		= 4;
	public const int GAMEOVER	= 5;
	public const string BESTSCORE_KEY = "bs";


	public static void ConsumeCube( CubeData cube ) {
		Vector2 _pos;

		if ( cube.color == instance.lastCube.color ) {
			instance.multiplier++;
			_pos = Camera.main.WorldToScreenPoint( cube.objTrans.position );
			MultiplierControl.ShowMultiplier( cube.color, instance.multiplier, _pos );
			AudioControl.PlayFX( "multiplier" );
		} else {
			instance.multiplier = 1;
		}
		instance.lastCube = cube;

		instance.points += cube.points * instance.multiplier;
		instance.StartCoroutine( instance.destroyCube(cube) );
		ScoreControl.Set( instance.points );
		if ( (state == PLAYING) && (SpawnManager.RemainCubes() == 0) ) {
			state = SUCCESS;
			TitleControl.Animate( "GOOD JOB!", TitleControl.SUCCESS_ANIM );
			instance.nextUpdate = Time.time + 2;
		}
	}

	void Start() {
		instance = this;
		time = 60;
		points = 0;
		TimerControl.Set( (int)time );
		ScoreControl.Set( points );
		state = STANDBY;
		nextUpdate = Time.time + 3;
		FaderControl.FadeOut( 2 );
	}

	void Update() {
		switch ( state ) {
			case STANDBY:
				if ( Time.time > nextUpdate ) {
					state = PLAYING;
				}
				break;
			case PLAYING:
				time -= Time.deltaTime;
				if ( time <= 0 ) {
					time = 0;
					state = TIMEOVER;
					TitleControl.Animate( "TIME OVER", TitleControl.GAMEOVER_ANIM );
					FaderControl.FadeIn( 2 );
					nextUpdate = Time.time + 3;
				}
				TimerControl.Set( Mathf.CeilToInt(time) );
				break;
			case SUCCESS:
				if ( Time.time > nextUpdate ) {
					nextUpdate = Time.time + ( 0.002f * time );
					time -= 1;
					AudioControl.PlayFX( "point" );
					if ( time <= 0 ) {
						time = 0;
						state = GAMEOVER;
						FaderControl.FadeIn( 2 );
						nextUpdate = Time.time + 3;
						AudioControl.PlayFX( "pointend" );
					}
					TimerControl.Set( Mathf.CeilToInt(time) );
					points++;
					ScoreControl.Set( points );
				}
				break;
			case DIE:
				state = GAMEOVER;
				TitleControl.Animate( "GAME OVER", TitleControl.GAMEOVER_ANIM );
				FaderControl.FadeIn( 2 );
				nextUpdate = Time.time + 3;
				break;
			case TIMEOVER:
			case GAMEOVER:
				if ( Time.time > nextUpdate ) {
					if ( points > PlayerPrefs.GetInt(BESTSCORE_KEY, 0) ) {
						PlayerPrefs.SetInt( BESTSCORE_KEY, points );
						PlayerPrefs.Save();
					}
					Application.LoadLevel( "Menu" );
				}
				break;
		}
	}

	IEnumerator destroyCube( CubeData cube ) {
		Transform _trans;
		float _scale, _target;

		_trans = cube.objTrans;
		_scale = _trans.localScale.x;
		_target = _scale * 4f;
		do {
			_trans.Rotate( Random.value * 360, Random.value * 360, Random.value * 360 );
			_scale = Mathf.Lerp( _scale, _target, Time.deltaTime );
			if ( _scale < 0 ) _scale = 0;
			_trans.localScale = new Vector3( _scale, _scale, _scale );
			_target -= Time.deltaTime;
			yield return false;
		} while ( _scale > 0 );
	}
}
