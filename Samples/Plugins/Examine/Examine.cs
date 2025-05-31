/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：考核
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using HoopyGame.UIF;

public class Examine : BasePanel<IUIDataBase>
{
    public string examineName;
    //总分
    private float _currentScore;
    //读取所有的题目 题目名 题目Item  每次加载获取一下名字
    ExamineQuestion question;
    private Dictionary<int, ChoiceQuestion> _choiceIdMap;
    private Dictionary<int, TFQuestion> _tfIdMap;

    //是否确定了答案  确定->提交
    private bool _sureAnswer;

    //答对答错的UI
    [SerializeField] private Sprite _rightAnswerSprite, _wroningAnswerSprite;

    private ScrollRect _scrollrect;
    private Transform _content;

    //题目
    [SerializeField] private ChoiceQuestionItem _choiceQstItem;
    [SerializeField] private TFQuestionItem _tfQstItem;
    private List<IJudgeAnswer> _totalQuestionItems = new();
    [SerializeField] private TextMeshProUGUI littleTitle_Txt;


    public override void OnInitComponent()
    {
        _scrollrect = transform.FindComponentFromChild<ScrollRect>("QuestionContent");
        _content = _scrollrect.content;
        _choiceIdMap = new Dictionary<int, ChoiceQuestion>();
        _tfIdMap = new Dictionary<int, TFQuestion>();
        try
        {
            question = FileUtils.LoadDataFile<ExamineQuestion>(examineName);
            //这个的目的是为了能通过id拿取题，如果有单独取题的需求，可以在这里取
            foreach (ChoiceQuestion item in question.ChoiceQuestion)
            {
                _choiceIdMap.Add(item.id, item);
            }
            foreach (TFQuestion item in question.TFQuestion)
            {
                _tfIdMap.Add(item.id, item);
            }
        }
        catch (System.Exception)
        {
            throw;
        }
    }
    public override void OnStart()
    {
        SpanQuestionItem();
    }

    //其余复杂类型请参考此写法扩充
    private void SpanQuestionItem()
    {
        //单选多选题
        if (question.ChoiceQuestion.Count > 0)
        {
            var tmpChoice = TakeQuestion(question.ChoiceQuestion, 5, true, "选择题.");
            foreach (ChoiceQuestion item in tmpChoice)
            {
                ChoiceQuestionItem tmp = Instantiate(_choiceQstItem, _content);
                tmp.OnInit(item, _rightAnswerSprite, _wroningAnswerSprite);
                _totalQuestionItems.Add(tmp);
            }
        }
        //判断题
        if (question.TFQuestion.Count > 0)
        {
            var tmpTF = TakeQuestion(question.TFQuestion, 5, true, "判断题.");
            foreach (TFQuestion item in tmpTF)
            {
                GameObject go = Instantiate(_tfQstItem.gameObject, _content);
                TFQuestionItem tmp = go.GetComponent<TFQuestionItem>();
                tmp.OnInit(item, _rightAnswerSprite, _wroningAnswerSprite);
                _totalQuestionItems.Add(tmp);
            }
        }
    }
    public List<T> TakeQuestion<T>(List<T> arrays,int Count,bool isRandom,string queTypeTitle) where T : struct
    {
        Instantiate(littleTitle_Txt, _content).SetText(queTypeTitle);
        return arrays.OrderBy(x => isRandom ? Random.value : 0).Take(Count).ToList();
    }



    /// <summary>
    /// 判断答案
    /// </summary>
    public override void OnCloseBtnClick()
    {
        if (!_sureAnswer)
        {
            _sureAnswer = true;
            foreach (var item in _totalQuestionItems)
            {
                item.JudgeAnswer();
            }
        }
        else
        {
            //提交

        }
    }

}
