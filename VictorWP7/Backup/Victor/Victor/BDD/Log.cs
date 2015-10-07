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
    public class Log : INotifyPropertyChanged, INotifyPropertyChanging
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

        //Date
        private DateTime date;

        [Column()]
        public DateTime Date
        {
            get
            {
                return date;
            }

            set
            {
                if (date != value)
                {
                    OnNotifyPropertyChanging("Date");
                    date = value;
                    OnNotifyPropertyChanged("Date");
                }
            }
        }

        //Réponse - Clef étrangère
        private int reponseId;

        [Column()]
        public int ReponseId
        {
            get
            {
                return reponseId;
            }

            set
            {
                if (reponseId != value)
                {
                    OnNotifyPropertyChanging("ReponseId");
                    reponseId = value;
                    OnNotifyPropertyChanged("ReponseId");
                }
            }
        }

        //Propriété sur le niveau
        private EntityRef<Reponse> reponse;

        [Association(OtherKey = "Id", ThisKey = "ReponseId", Storage = "reponse")]
        public Reponse Reponse
        {
            get
            {
                return reponse.Entity;
            }

            set
            {
                reponse.Entity = value;
                ReponseId = value.Id;
            }
        }
    }
}
