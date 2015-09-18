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
using System.Data.Linq;

namespace VictorNamespace
{
    public class DataBaseContext : DataContext
    {
        public static string DBConnectionString = "Data Source=isostore:/Victor.sdf";

        public DataBaseContext(string connectionString) : base(connectionString) { }

        public Table<Niveau> Niveaux;
        public Table<Question> Questions;
        public Table<Reponse> Reponses;
        public Table<Explication> Explications;
        public Table<Modele> Modeles;
        public Table<Accessoire> Accessoires;
        public Table<Log> Logs;
        public Table<Scene> Scenes;
        public Table<Interaction> Interactions;
        public Table<Jeu> Jeux;
        public Table<Photo> Photos;
        public Table<InteractionPhoto> InteractionsPhotos;
    }
}