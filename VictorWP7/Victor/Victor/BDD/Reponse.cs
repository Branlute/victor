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
    public class Reponse : INotifyPropertyChanged, INotifyPropertyChanging
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

        //Intitulé
        private string intitule;

        [Column()]
        public string Intitule
        {
            get
            {
                return intitule;
            }

            set
            {
                if (intitule != value)
                {
                    OnNotifyPropertyChanging("Intitule");
                    intitule = value;
                    OnNotifyPropertyChanged("Intitule");
                }
            }
        }

        //Bonne réponse ?
        private bool vrai;

        [Column()]
        public bool Vrai
        {
            get
            {
                return vrai;
            }

            set
            {
                if (vrai != value)
                {
                    OnNotifyPropertyChanging("Vrai");
                    vrai = value;
                    OnNotifyPropertyChanged("Vrai");
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
