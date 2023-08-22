﻿
using System;
using System.ComponentModel;

namespace BeThere.Models
{
    public class QuestionToAsk 
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        private string m_ChatRoomId;

        public string ChatRoomId
        {
            get { return m_ChatRoomId; }
            set { m_ChatRoomId = value; }
        }

        private string m_Question;

        public string Question
        {
            get { return m_Question; }
            set { m_Question = value;}
        }

        private string m_QuestionId;

        public string QuestionId
        {
            get { return m_QuestionId; }
            set { m_QuestionId = value; }
        }

        private int? m_MinimumAgeRange;

        public int? MinimumAgeRange
        {
            get { return m_MinimumAgeRange; }
            set { m_MinimumAgeRange = value; }
        }

        private int? m_MaximumAgeRange;

        public int? MaximumAgeRange
        {
            get { return m_MaximumAgeRange; }
            set { m_MaximumAgeRange = value; }
        }

        private string m_Gender;

        public string Gender
        {
            get { return m_Gender; }
            set { m_Gender = value; }
        }

        public LocationData Location { get; set; }

        private double? m_Radius;

        public double? Radius
        {
            get { return m_Radius; }
            set { m_Radius = value;}
        }

        private string m_Date;

        public string Date
        {
            get { return m_Date; }
            set { m_Date = value;  }
        }

        private string m_Time;

        public string Time
        {
            get { return m_Time; }
            set { m_Time = value; }
        }


        //void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

       
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

