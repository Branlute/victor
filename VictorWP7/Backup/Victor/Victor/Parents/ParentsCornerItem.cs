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

namespace VictorNamespace
{
    public class ParentsCornerItem
    {
        public string Date { get; set; }
        public string Question { get; set; }
        public string Reponse { get; set; }
        public string Couleur { get; set; }

        public ParentsCornerItem(string date, string question, string reponse, string couleur)
        {
            this.Date = date;
            this.Question = question;
            this.Reponse = reponse;
            this.Couleur = couleur;
        }
    }
}
