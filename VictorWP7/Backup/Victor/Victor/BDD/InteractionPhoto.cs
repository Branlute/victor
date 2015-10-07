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
    public class InteractionPhoto : INotifyPropertyChanged, INotifyPropertyChanging
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

        //x
        private int x;

        [Column()]
        public int X
        {
            get
            {
                return x;
            }

            set
            {
                if (x != value)
                {
                    OnNotifyPropertyChanging("X");
                    x = value;
                    OnNotifyPropertyChanged("X");
                }
            }
        }

        //y
        private int y;

        [Column()]
        public int Y
        {
            get
            {
                return y;
            }

            set
            {
                if (y != value)
                {
                    OnNotifyPropertyChanging("Y");
                    y = value;
                    OnNotifyPropertyChanged("Y");
                }
            }
        }

        //longueur
        private int longueur;

        [Column()]
        public int Longueur
        {
            get
            {
                return longueur;
            }

            set
            {
                if (longueur != value)
                {
                    OnNotifyPropertyChanging("Longueur");
                    longueur = value;
                    OnNotifyPropertyChanged("Longueur");
                }
            }
        }

        //hauteur
        private int hauteur;

        [Column()]
        public int Hauteur
        {
            get
            {
                return hauteur;
            }

            set
            {
                if (hauteur != value)
                {
                    OnNotifyPropertyChanging("Hauteur");
                    hauteur = value;
                    OnNotifyPropertyChanged("Hauteur");
                }
            }
        }

        //Photo - Clef étrangère
        private int photoId;

        [Column()]
        public int PhotoId
        {
            get
            {
                return photoId;
            }

            set
            {
                if (photoId != value)
                {
                    OnNotifyPropertyChanging("PhotoId");
                    photoId = value;
                    OnNotifyPropertyChanged("PhotoId");
                }
            }
        }

        //Propriété sur la photo
        private EntityRef<Photo> photo;

        [Association(OtherKey = "Id", ThisKey = "PhotoId", Storage = "photo")]
        public Photo Photo
        {
            get
            {
                return photo.Entity;
            }

            set
            {
                photo.Entity = value;
                PhotoId = value.Id;
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

        //Propriété sur la scène
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
