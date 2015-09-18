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
    static class Session
    {
        #region Maison
        //Cuisine
        public static bool cuisine_casserole = false;
        public static bool cuisine_couteau = false;
        public static bool cuisine_toxique = false;
        public static bool cuisine_tasse = false;
        public static bool cuisine_grillepain = false;
        public static bool cuisine_four = false;

        public static bool cuisine_puzzle = false;

        //Salon
        public static bool salon_cheminee = false;
        public static bool salon_prises = false;
        public static bool salon_ventilateur = false;
        public static bool salon_radiateur = false;

        public static bool salon_puzzle = false;

        #endregion

       

        public static bool verification_item(string item)
        {
            switch(item)
            {
                case "casserole" :
                    return cuisine_casserole;
                case "couteau" :
                    return cuisine_couteau;
                case "toxique" :
                    return cuisine_toxique;
                case "tasse" :
                    return cuisine_tasse;
                case "grillepain" :
                    return cuisine_grillepain;
                case "four" :
                    return cuisine_four;
                case "radiateur":
                    return salon_radiateur;
                case "prises":
                    return salon_prises;
                case "ventilateur":
                    return salon_ventilateur;
                case "cheminee":
                    return salon_cheminee;
                default :
                    return true;
            }
        }


        public static void isPuzzleCuisineAvailable()
        {
            //Vérification de la pièce du puzzle Cuisine
            if (cuisine_casserole && cuisine_couteau && cuisine_toxique && cuisine_grillepain && cuisine_four)
            {
                cuisine_puzzle = true;
            }
        }
    }
}
