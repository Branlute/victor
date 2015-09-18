using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Data.Linq;

namespace VictorNamespace
{
    [Table]
    public class Explication : INotifyPropertyChanged, INotifyPropertyChanging
    {
        //Implémentation de l'interface INotifyPropertyChanging
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        public void OnNotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void OnNotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        //Identifiant
        private int id;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                if (id != value)
                {
                    OnNotifyPropertyChanging("Id");
                    id = value;
                    OnNotifyPropertyChanged("Id");
                }
            }
        }

        //Explication
        private string pourquoi;

        [Column()]
        public string Pourquoi
        {
            get
            {
                return pourquoi;
            }

            set
            {
                if (pourquoi != value)
                {
                    OnNotifyPropertyChanging("Pourquoi");
                    pourquoi = value;
                    OnNotifyPropertyChanged("Pourquoi");
                }
            }
        }

        //MP3
        private string mp3;

        [Column()]
        public string Mp3
        {
            get
            {
                return mp3;
            }

            set
            {
                if (mp3 != value)
                {
                    OnNotifyPropertyChanging("Mp3");
                    mp3 = value;
                    OnNotifyPropertyChanged("Mp3");
                }
            }
        }

        //Question - Clef étrangère
        private int questionId;

        [Column()]
        public int QuestionId
        {
            get
            {
                return questionId;
            }

            set
            {
                if (questionId != value)
                {
                    OnNotifyPropertyChanging("QuestionId");
                    questionId = value;
                    OnNotifyPropertyChanged("QuestionId");
                }
            }
        }

        //Propriété sur la question
        private EntityRef<Question> question;

        [Association(OtherKey = "Id", ThisKey = "QuestionId", Storage = "question")]
        public Question Question
        {
            get
            {
                return question.Entity;
            }

            set
            {
                question.Entity = value;
                QuestionId = value.Id;
            }
        }
    }
}