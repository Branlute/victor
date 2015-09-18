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
    public class Question : INotifyPropertyChanged, INotifyPropertyChanging
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

        //booleen
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

        //Intitulé
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

        //URL
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

        //QCM ou pas ?
        private bool qcm;

        [Column()]
        public bool Qcm
        {
            get
            {
                return qcm;
            }

            set
            {
                if (qcm != value)
                {
                    OnNotifyPropertyChanging("Qcm");
                    qcm = value;
                    OnNotifyPropertyChanged("Qcm");
                }
            }
        }

        //Jeu - Clef étrangère
        private int jeuId;

        [Column()]
        public int JeuId
        {
            get
            {
                return jeuId;
            }

            set
            {
                if (jeuId != value)
                {
                    OnNotifyPropertyChanging("JeuId");
                    jeuId = value;
                    OnNotifyPropertyChanged("JeuId");
                }
            }
        }

        //Propriété sur le Jeu
        private EntityRef<Jeu> jeu;

        [Association(OtherKey = "Id", ThisKey = "JeuId", Storage = "jeu")]
        public Jeu Jeu
        {
            get
            {
                return jeu.Entity;
            }

            set
            {
                jeu.Entity = value;
                JeuId = value.Id;
            }
        }

        //Scene - Clef étrangère
        private int sceneId;

        [Column()]
        public int SceneId
        {
            get
            {
                return sceneId;
            }

            set
            {
                if (sceneId != value)
                {
                    OnNotifyPropertyChanging("SceneId");
                    sceneId = value;
                    OnNotifyPropertyChanged("SceneId");
                }
            }
        }

        //Propriété sur la Scène
        private EntityRef<Scene> scene;

        [Association(OtherKey = "Id", ThisKey = "SceneId", Storage = "scene")]
        public Scene Scene
        {
            get
            {
                return scene.Entity;
            }

            set
            {
                scene.Entity = value;
                SceneId = value.Id;
            }
        }
    }
}