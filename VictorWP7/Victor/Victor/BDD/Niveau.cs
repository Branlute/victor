﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;  
using System.Data.Linq.Mapping;  
using System.ComponentModel;  
using System.Collections.ObjectModel; 

namespace VictorNamespace
{
    [Table]
    public class Niveau : INotifyPropertyChanged, INotifyPropertyChanging
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

        //Nom
        private string nom;

        [Column()]
        public string Nom
        {
            get
            {
                return nom;
            }

            set
            {
                if (nom != value)
                {
                    OnNotifyPropertyChanging("Nom");
                    nom = value;
                    OnNotifyPropertyChanged("Nom");
                }
            }
        }

        //Nom
        private bool fini;

        [Column()]
        public bool Fini
        {
            get
            {
                return fini;
            }

            set
            {
                if (fini != value)
                {
                    OnNotifyPropertyChanging("Fini");
                    fini = value;
                    OnNotifyPropertyChanged("Fini");
                }
            }
        }
    }
}
