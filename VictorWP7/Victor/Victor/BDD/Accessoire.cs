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
    public class Accessoire : INotifyPropertyChanged, INotifyPropertyChanging
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
        private string url;

        [Column()]
        public string Url
        {
            get
            {
                return url;
            }

            set
            {
                if (url != value)
                {
                    OnNotifyPropertyChanging("Url");
                    url = value;
                    OnNotifyPropertyChanged("Url");
                }
            }
        }

        //Dispo
        private bool dispo;

        [Column()]
        public bool Dispo
        {
            get
            {
                return dispo;
            }

            set
            {
                if (dispo != value)
                {
                    OnNotifyPropertyChanging("Dispo");
                    dispo = value;
                    OnNotifyPropertyChanged("Dispo");
                }
            }
        }

        //Porte
        private bool porte;

         [Column()]
        public bool Porte
        {
            get
            {
                return porte;
            }

            set
            {
                if (porte != value)
                {
                    OnNotifyPropertyChanging("Porte");
                    porte = value;
                    OnNotifyPropertyChanged("Porte");
                }
            }
        }

        //Type - Clef étrangère
        private int modeleId;

        [Column()]
        public int ModeleId
        {
            get
            {
                return modeleId;
            }

            set
            {
                if (modeleId != value)
                {
                    OnNotifyPropertyChanging("ModeleId");
                    modeleId = value;
                    OnNotifyPropertyChanged("ModeleId");
                }
            }
        }

        //Propriété sur le type
        private EntityRef<Modele> modele;

        [Association(OtherKey = "Id", ThisKey = "ModeleId", Storage = "modele")]
        public Modele Modele
        {
            get
            {
                return modele.Entity;
            }

            set
            {
                modele.Entity = value;
                ModeleId = value.Id;
            }
        }

        //Niveau - Clef étrangère
        private int niveauId;

        [Column()]
        public int NiveauId
        {
            get
            {
                return niveauId;
            }

            set
            {
                if (niveauId != value)
                {
                    OnNotifyPropertyChanging("NiveauId");
                    niveauId = value;
                    OnNotifyPropertyChanged("NiveauId");
                }
            }
        }

        //Propriété sur le type
        private EntityRef<Niveau> niveau;

        [Association(OtherKey = "Id", ThisKey = "NiveauId", Storage = "niveau")]
        public Niveau Niveau
        {
            get
            {
                return niveau.Entity;
            }

            set
            {
                niveau.Entity = value;
                NiveauId = value.Id;
            }
        }
    }
}
