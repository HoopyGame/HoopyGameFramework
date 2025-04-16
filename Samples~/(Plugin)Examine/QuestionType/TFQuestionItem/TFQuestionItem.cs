/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：判断题对象
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TFQuestionItem : MonoBehaviour,IJudgeAnswer
{
    private TextMeshProUGUI _topic_Txt;
    private Toggle _t, _f;  //正确和错误的选项
    private Image _tGraphic, _fGraphic;
    //是否做对了
    public bool IsRight { get; private set; }
    //private TextMeshProUGUI _tips;//有提示再用这个

    private Sprite _rightAnswerSprite, _errorAnswerSprite;
    private TFQuestion _tfQuestion;

    public void OnInit(TFQuestion tfQuestion, Sprite rightAnswer, Sprite errorAnswer)
    {
        _t = transform.FindComponentFromChild<Toggle>("T");
        _tGraphic = _t.graphic.GetComponent<Image>();
        _f = transform.FindComponentFromChild<Toggle>("F");
        _fGraphic = _f.graphic.GetComponent<Image>();
        //_tips = transform.GetComponentFromChild<TextMeshProUGUI>("Tips");
        _rightAnswerSprite = rightAnswer;
        _errorAnswerSprite = errorAnswer;
        _tfQuestion = tfQuestion;
    }
    public bool JudgeAnswer()
    {
        _t.interactable = false;
        _f.interactable = false;
        IsRight = _tfQuestion.answer ? _t.isOn : _f.isOn;
        ShowResult();
        return IsRight;
    }
    private void ShowResult()
    {
        if (IsRight)
        {
            _tGraphic.sprite = _t.isOn ? _rightAnswerSprite : _tGraphic.sprite;
            _fGraphic.sprite = _f.isOn ? _rightAnswerSprite : _fGraphic.sprite;
        }
        else
        {
            _tGraphic.sprite = _t.isOn ? _errorAnswerSprite : _tGraphic.sprite;
            _fGraphic.sprite = _f.isOn ? _errorAnswerSprite : _fGraphic.sprite;
        }
    }
}
