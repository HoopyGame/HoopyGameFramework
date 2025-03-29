/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：选择题对象，单选多选
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceQuestionItem : MonoBehaviour, IJudgeAnswer
{
    //标题
    private TextMeshProUGUI _topic_Txt;
    //选项
    private Transform _choiceParentTrans;
    private ToggleGroup _answersParent_TogGP;
    private List<Toggle> _answers_Tog = new List<Toggle>();
    private List<TextMeshProUGUI> _answerTopic_Txt = new List<TextMeshProUGUI>();
    //答案
    private List<int> _answers = new List<int>();

    //是否做对了
    public bool IsCorrect { get; private set; }
    public bool IsSingleChoice { get; private set; }//是否单选题

    private Sprite _rightAnswerSprite,_errorAnswerSprite;

    private ChoiceQuestion _choiceQuestionData;


    public void OnInit(ChoiceQuestion choiceQuestion, Sprite rightAnswer, Sprite errorAnswer)
    {
        _topic_Txt = transform.FindComponentFromChild<TextMeshProUGUI>("Topic_Txt");
        _choiceParentTrans = transform.FindTransFromChild("Answers");
        _answersParent_TogGP = _choiceParentTrans.gameObject.AddComponent<ToggleGroup>();
        _answersParent_TogGP.allowSwitchOff = true;
        _choiceQuestionData = choiceQuestion;
        _rightAnswerSprite = rightAnswer;
        _errorAnswerSprite = errorAnswer;
        foreach (Transform item in _choiceParentTrans)
        {
            try
            {
                Transform choiceToggle = item.GetChild(0);
                _answers_Tog.Add(choiceToggle.GetComponent<Toggle>());
                _answerTopic_Txt.Add(choiceToggle.GetChild(1).GetComponent<TextMeshProUGUI>());
            }
            catch
            {
                DebugUtils.Print("获取选择Toggle失败");
            }
        }
        SetData();
    }

    private void SetData()
    {
        string[] tempAnswers = _choiceQuestionData.answer.Split(',');
        for (int i = 0; i < tempAnswers.Length; i++)
        {
            _answers.Add(int.Parse(tempAnswers[i]));
        }
        //是否单选和题目内容
        if (_answers.Count <= 1)
        {
            _topic_Txt.SetText("(单选)" + _choiceQuestionData.topic);
            IsSingleChoice = true;
            foreach (var item in _answers_Tog)
            {
                item.group = _answersParent_TogGP;
            }
        }
        else
        {
            IsSingleChoice = false;
            _topic_Txt.SetText("(多选)" + _choiceQuestionData.topic);
        }
        //设置答案内容  这里规定了只有四个答案
        _answerTopic_Txt[0].text = _choiceQuestionData.answerOne;
        _answerTopic_Txt[1].text = _choiceQuestionData.answerTwo;
        _answerTopic_Txt[2].text = _choiceQuestionData.answerThree;
        _answerTopic_Txt[3].text = _choiceQuestionData.answerFour;

    }
    public bool JudgeAnswer()
    {
        IsCorrect = false;
        for (int i = 0; i < _answers_Tog.Count; i++)
        {
            if (_answers.Contains(i))
            {
                if (_answers_Tog[i].isOn)
                {
                    IsCorrect = true;
                    continue;
                }
                else
                {
                    IsCorrect = false;
                    break;
                }
            }
            else
            {
                if (_answers_Tog[i].isOn)
                {
                    IsCorrect = false;
                    //错误的被选择了
                    break;
                }
            }
        }
        SetResult();
        return IsCorrect;
    }
    private void SetResult()
    {
        for (int i = 0; i < _answers_Tog.Count; i++)
        {
            _answers_Tog[i].interactable = false;
            _answers_Tog[i].group = null;

            if (IsCorrect)
            {
                if (_answers_Tog[i].isOn)
                    _answers_Tog[i].graphic.GetComponent<Image>().sprite = _rightAnswerSprite;
            }
            else
            {
                if (_answers_Tog[i].isOn)
                {
                    _answers_Tog[i].graphic.GetComponent<Image>().sprite = _errorAnswerSprite;
                    if (_answers.Contains(i))
                    {
                        _answerTopic_Txt[i].color = Color.green;
                    }
                }
                else
                {
                    if (_answers.Contains(i))
                    {
                        _answerTopic_Txt[i].color = Color.green;
                    }
                }
            }
        }
    }
}
