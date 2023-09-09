
using System;
using System.ComponentModel;

namespace BeThere.Models
{
    public class QuestionToAsk : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string m_ChatRoomId;

        public string ChatRoomId
        {
            get { return m_ChatRoomId; }
            set { m_ChatRoomId = value; OnPropertyChanged(nameof(ChatRoomId)); }
        }

        private string m_Question;

        public string Question
        {
            get { return m_Question; }
            set { m_Question = value; OnPropertyChanged(nameof(Question)); }
        }

        private string m_QuestionId;

        public string QuestionId
        {
            get { return m_QuestionId; }
            set { m_QuestionId = value; OnPropertyChanged(nameof(QuestionId)); }
        }

        private int? m_MinimumAgeRange;

        public int? MinimumAgeRange
        {
            get { return m_MinimumAgeRange; }
            set { m_MinimumAgeRange = value; OnPropertyChanged(nameof(MinimumAgeRange)); }
        }

        private int? m_MaximumAgeRange;

        public int? MaximumAgeRange
        {
            get { return m_MaximumAgeRange; }
            set { m_MaximumAgeRange = value; OnPropertyChanged(nameof(MaximumAgeRange)); }
        }

        private string m_Gender;

        public string Gender
        {
            get { return m_Gender; }
            set { m_Gender = value; OnPropertyChanged(nameof(Gender)); }
        }

        public LocationData Location { get; set; }

        private double? m_Radius;

        public double? Radius
        {
            get { return m_Radius; }
            set { m_Radius = value; OnPropertyChanged(nameof(Radius)); }
        }

        private string m_Date;

        public string Date
        {
            get { return m_Date; }
            set { m_Date = value; OnPropertyChanged(nameof(Date)); }
        }

        private bool m_IsCityQuestion;

        public bool IsCityQuestion
        {
            get { return m_IsCityQuestion; }
            set { m_IsCityQuestion = value; }
        }

        private string m_Time;

        public string Time
        {
            get { return m_Time; }
            set { m_Time = value; OnPropertyChanged(nameof(Time)); }
        }

        private int m_NumOfAnswers;

        public int NumOfAnswers
        {
            get { return m_NumOfAnswers; }
            set { m_NumOfAnswers = value; OnPropertyChanged(nameof(NumOfAnswers)); }
        }

        void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void ClearAllFeilds()
        {
            Question = string.Empty;
            MinimumAgeRange = null;
            MaximumAgeRange = null;
            Gender = string.Empty;
            Location = null;
            Radius = null;
            Date = string.Empty;
            Time = string.Empty;
        }
    }
}

