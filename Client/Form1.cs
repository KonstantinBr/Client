using Client.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        IMemoAPI memoApi;
        public Form1()
        {
            InitializeComponent();
            //memoApi = RestService.For<IMemoAPI>(ConfigurationSettings.AppSettings["MemoApiURL"]);
            //UpdateAllMemoList().Wait();
        }

        async Task UpdateAllMemoList()
        {
            List<MemoResponse> memoResponses = await memoApi.GetAllMemos();
            AllMemosGridView.Rows.Clear();
            int rowIndex = 0;
            foreach(var memo in memoResponses)
            {
                AllMemosGridView.Rows.Add();
                AllMemosGridView[rowIndex, 0].Value = memo.Id;
                AllMemosGridView[rowIndex, 1].Value = memo.SubjectName;
                AllMemosGridView[rowIndex, 2].Value = memo.LectureHours;
                AllMemosGridView[rowIndex, 3].Value = memo.LabHours;
                AllMemosGridView[rowIndex, 4].Value = controlTupeToString(memo.ControlType);
                AllMemosGridView[rowIndex, 5].Value = memo.StudentsCount;
            }
        }

        string controlTupeToString(int type)
        {
            if (type == 1)
                return "Экзамен";
            else
                return "Зачет";
        }

        string getControlType()
        {
            if (AddExam.Checked)
                return "Exam";
            else
                return "Test";
        }

        void setContentType(int type)
        {
            if(type == 1)
            {
                AddExam.Checked = false;
                AddOffset.Checked = true;
            }
            else
            {
                AddOffset.Checked = false;
                AddExam.Checked = true;
            }
        }

        void setAllFields(MemoResponse memoResponse)
        {
            IDText.Text = memoResponse.Id.ToString();
            AddSubject.Text = memoResponse.SubjectName;
            AddLectureHours.Text = memoResponse.LectureHours.ToString();
            AddLabHours.Text = memoResponse.LabHours.ToString();
            AddStudentsCount.Text = memoResponse.StudentsCount.ToString();
            setContentType(memoResponse.ControlType);
        }

        private async void RefreshList_Click(object sender, EventArgs e)
        {
            await UpdateAllMemoList();
        }

        private async void AddButton_Click(object sender, EventArgs e)
        {
            CreateMemoReqest createMemoReqest = new CreateMemoReqest
            {
                SubjectName = AddSubject.Text,
                LectureHours = int.Parse(AddLectureHours.Text),
                LabHours = int.Parse(AddLabHours.Text),
                StudentsCount = int.Parse(AddStudentsCount.Text),
                ControlType = getControlType()
            };

            MemoResponse memoResponse = await memoApi.CreateMemo(createMemoReqest);

            setAllFields(memoResponse);
            await UpdateAllMemoList();
        }

        private async void GetButton_Click(object sender, EventArgs e)
        {
            MemoResponse memoResponse = await memoApi.GetMemo(int.Parse(IDText.Text));
            setAllFields(memoResponse);

        }

        private async void UpdateButton_Click(object sender, EventArgs e)
        {
            UpdateMemoReqest updateMemoReqest = new UpdateMemoReqest
            {
                Id = int.Parse(IDText.Text),
                SubjectName = AddSubject.Text,
                LectureHours = int.Parse(AddLectureHours.Text),
                LabHours = int.Parse(AddLabHours.Text),
                StudentsCount = int.Parse(AddStudentsCount.Text),
                ControlType = getControlType()
            };
            MemoResponse memoResponse = await memoApi.UpdateMemo(updateMemoReqest);
            setAllFields(memoResponse);
            await UpdateAllMemoList();
        }

        private async void DeleteButton_Click(object sender, EventArgs e)
        {
            await memoApi.DeleteMemo(int.Parse(IDText.Text));
            await UpdateAllMemoList();
        }

        private async void RangeButton_Click(object sender, EventArgs e)
        {

            List<MemoResponse> memoResponses = await memoApi.GetMemosRange(int.Parse(minLec.Text),int.Parse(MaxLec.Text));
            RangeGridView.Rows.Clear();
            int rowIndex = 0;
            foreach (var memo in memoResponses)
            {
                RangeGridView.Rows.Add();
                RangeGridView[rowIndex, 0].Value = memo.Id;
                RangeGridView[rowIndex, 1].Value = memo.SubjectName;
                RangeGridView[rowIndex, 2].Value = memo.LectureHours;
                RangeGridView[rowIndex, 3].Value = memo.LabHours;
                RangeGridView[rowIndex, 4].Value = controlTupeToString(memo.ControlType);
                RangeGridView[rowIndex, 5].Value = memo.StudentsCount;
            }
        }
    }
}
