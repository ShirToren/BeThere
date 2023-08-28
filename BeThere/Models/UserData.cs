using System;
using System.ComponentModel;

namespace BeThere.Models
{
    public class UserData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string m_UserName;

        public string Username
        {
            get { return m_UserName; }
            set { m_UserName = value; OnPropertyChanged(nameof(Username)); }
        }

        private string m_Password;

        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; OnPropertyChanged(nameof(Password)); }
        }

        private int m_Age;

        public int Age
        {
            get { return m_Age; }
            set { m_Age = value; OnPropertyChanged(nameof(Age)); }
        }

        private string m_Email;

        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; OnPropertyChanged(nameof(m_Email)); }
        }

        private string m_Gender;

        public string Gender
        {
            get { return m_Gender; }
            set { m_Gender = value; OnPropertyChanged(nameof(m_Gender)); }
        }

        private string m_FullName;

        public string FullName
        {
            get { return m_FullName; }
            set { m_FullName = value; OnPropertyChanged(nameof(m_FullName)); }
        }


        void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void ClearAllFeilds()
        {
            Username = string.Empty;
            Password = string.Empty;
            Age = 0;
            FullName = string.Empty;
            Email = string.Empty;
            Gender = string.Empty;
        }

    }

}
