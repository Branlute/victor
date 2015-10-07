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
using System.Windows.Media.Imaging;

namespace VictorNamespace
{
    static class Init
    {
        public static void InitBDD()
        {
            //Vérification de l'existence de la BDD
            DataBaseContext dbc = new DataBaseContext(DataBaseContext.DBConnectionString);

            dbc.DeleteDatabase();

            if (!dbc.DatabaseExists())
            {
                dbc.CreateDatabase();

                #region Niveau

                Niveau home = new Niveau() { Nom = "home", Fini = false };
                Niveau holiday = new Niveau() { Nom = "holiday", Fini = false };
                
                dbc.Niveaux.InsertOnSubmit(home);
                dbc.Niveaux.InsertOnSubmit(holiday);
                dbc.SubmitChanges();

                #endregion

                #region Scene

                /**
                 * HOME
                 * */
                Scene home_cuisine = new Scene() { Background = "home_cuisine", Nom = "cuisine", NiveauId = home.Id };
                Scene home_salon = new Scene() { Background = "home_salon", Nom = "salon", NiveauId = home.Id };

                dbc.Scenes.InsertOnSubmit(home_cuisine);
                dbc.Scenes.InsertOnSubmit(home_salon);
                dbc.SubmitChanges();

                #endregion

                #region Jeu
                /**
                 * HOME
                 * */
                //Cuisine
                Jeu j1 = new Jeu() { Nom = "casserole", Explication = "TEST", Mp3 = "MP3/Home/Cuisine/fourQuestion.mp3", Debloque = false };
                Jeu j2 = new Jeu() { Nom = "toxique", Explication = "TEST", Mp3 = "MP3/Home/Cuisine/fourQuestion.mp3", Debloque = false };
                Jeu j3 = new Jeu() { Nom = "grillepain", Explication = "TEST", Mp3 = "MP3/Home/Cuisine/fourQuestion.mp3", Debloque = false };
                Jeu j4 = new Jeu() { Nom = "couteau", Explication = "TEST", Mp3 = "MP3/Home/Cuisine/fourQuestion.mp3", Debloque = false };
                Jeu j5 = new Jeu() { Nom = "four", Explication = "TEST", Mp3 = "MP3/Home/Cuisine/fourQuestion.mp3", Debloque = false };

                dbc.Jeux.InsertOnSubmit(j1);
                dbc.Jeux.InsertOnSubmit(j2);
                dbc.Jeux.InsertOnSubmit(j3);
                dbc.Jeux.InsertOnSubmit(j4);
                dbc.Jeux.InsertOnSubmit(j5);
                dbc.SubmitChanges();

                #endregion

                #region Question

                /**
                 * HOME
                 * */
                //Cuisine
                Question q1 = new Question() { Nom = "four", Intitule = "The apple pie in the oven smells good ! Can I open the door and take a piece of cake ?", Vrai = false, Mp3 = "MP3/Home/Cuisine/fourQuestion.mp3", Url = "/Images/Jeu/Home/Cuisine/victorFour.png", Qcm = true, JeuId = j5.Id, SceneId = home_cuisine.Id };
                Question q2 = new Question() { Nom = "casserole", Intitule = "I am sure that one the pieces of my puzzle is in this pan ! Can I take it to verify ?", Vrai = false,  Mp3 = "MP3/Home/Cuisine/casseroleQuestion.mp3", Url = "/Images/Jeu/Home/Cuisine/victorCasserole.png", Qcm = true, JeuId = j1.Id, SceneId = home_cuisine.Id };
                Question q3 = new Question() { Nom = "couteau", Intitule = "I am sure that this knife can be useful to find the piece of my puzzle ! Should I take him with me ?", Vrai = false,  Mp3 = "MP3/Home/Cuisine/couteauQuestion.mp3", Url = "/Images/Jeu/Home/Cuisine/victorCouteau.png", Qcm = true, JeuId = j4.Id, SceneId = home_cuisine.Id };
                Question q4 = new Question() { Nom = "toxique", Intitule = "Oh this is first time that I see this bottle ! I am sure that this new orange juice is delicious ! Can I drink it ?", Vrai = false, Mp3 = "MP3/Home/Cuisine/bottleQuestion.mp3", Url = "/Images/Jeu/Home/Cuisine/victorBidon.png", Qcm = true, JeuId = j2.Id, SceneId = home_cuisine.Id };
                Question q5 = new Question() { Nom = "tasse", Intitule = "I am thirsty ! Reorganize the following photos to help me to drink !", Vrai = false, Mp3 = "MP3/Home/Cuisine/tasseQuestion.mp3", Url = "/Images/Jeu/Home/Cuisine/victorVerre.png", Qcm = false, SceneId = home_cuisine.Id };
                Question q6 = new Question() { Nom = "grillepain", Intitule = "I am sure that one the piece of the puzzle is jammed into the toaster !  Can I check if nothing is jammed into the toaster ?", Vrai = false, Mp3 = "MP3/Home/Cuisine/toasterQuestion.mp3", Url = "/Images/Jeu/Home/Cuisine/victorGrillepain.png", Qcm = true, JeuId = j3.Id, SceneId = home_cuisine.Id };

                dbc.Questions.InsertOnSubmit(q1);
                dbc.Questions.InsertOnSubmit(q2);
                dbc.Questions.InsertOnSubmit(q3);
                dbc.Questions.InsertOnSubmit(q4);
                dbc.Questions.InsertOnSubmit(q5);
                dbc.Questions.InsertOnSubmit(q6);
                dbc.SubmitChanges();

                //Salon
                Question q7 = new Question() { Nom = "cheminee", Intitule = "I am thirsty ! Reorganize the following photos to help me to drink !", Mp3 = "MP3/Home/Cuisine/tasseQuestion.mp3", Url = "/Images/Jeu/Home/Cuisine/victorVerre.png", Qcm = true, SceneId = home_salon.Id };
                Question q8 = new Question() { Nom = "prises", Intitule = "I am thirsty ! Reorganize the following photos to help me to drink !", Mp3 = "MP3/Home/Cuisine/tasseQuestion.mp3", Url = "/Images/Jeu/Home/Cuisine/victorVerre.png", Qcm = true, SceneId = home_salon.Id };
                Question q9 = new Question() { Nom = "radiateur", Intitule = "I am thirsty ! Reorganize the following photos to help me to drink !", Mp3 = "MP3/Home/Cuisine/tasseQuestion.mp3", Url = "/Images/Jeu/Home/Cuisine/victorVerre.png", Qcm = true, SceneId = home_salon.Id };
                Question q10 = new Question() { Nom = "ventilateur", Intitule = "I am thirsty ! Reorganize the following photos to help me to drink !", Mp3 = "MP3/Home/Cuisine/tasseQuestion.mp3", Url = "/Images/Jeu/Home/Cuisine/victorVerre.png", Qcm = true, SceneId = home_salon.Id };
    
                dbc.Questions.InsertOnSubmit(q7);
                dbc.Questions.InsertOnSubmit(q8);
                dbc.Questions.InsertOnSubmit(q9);
                dbc.Questions.InsertOnSubmit(q10);
                dbc.SubmitChanges();

                #endregion

                #region Interaction

                /**
                 * HOME
                 * */

                //Cuisine
                Interaction i1 = new Interaction() { Url = "four", X = 1129, Y = 216, Longueur = 160, Hauteur = 200, SceneId = home_cuisine.Id, QuestionId = q1.Id };
                Interaction i2 = new Interaction() { Url = "casserole", X = 1195, Y = 215, Longueur = 98, Hauteur = 60, SceneId = home_cuisine.Id, QuestionId = q2.Id };
                //Interaction i3 = new Interaction() { Url = "casserole2", X = 2810, Y = 247, Longueur = 35, Hauteur = 8, SceneId = home_cuisine.Id, QuestionId = q2.Id };
                Interaction i4 = new Interaction() { Url = "couteau", X = 1342, Y = 184, Longueur = 40, Hauteur = 80, SceneId = home_cuisine.Id, QuestionId = q3.Id };
                Interaction i5 = new Interaction() { Url = "toxique", X = 1397, Y = 319, Longueur = 70, Hauteur = 90, SceneId = home_cuisine.Id, QuestionId = q4.Id };
                //Interaction i6 = new Interaction() { Url = "verre", X = 1289, Y = 227, Longueur = 25, Hauteur = 35, SceneId = home_cuisine.Id, QuestionId = q5.Id };
                Interaction i7 = new Interaction() { Url = "grillepain", X = 1080, Y = 220, Longueur = 80, Hauteur = 52, SceneId = home_cuisine.Id, QuestionId = q6.Id };

                dbc.Interactions.InsertOnSubmit(i1);
                dbc.Interactions.InsertOnSubmit(i2);
                //dbc.Interactions.InsertOnSubmit(i3);
                dbc.Interactions.InsertOnSubmit(i4);
                dbc.Interactions.InsertOnSubmit(i5);
                //dbc.Interactions.InsertOnSubmit(i6);
                dbc.Interactions.InsertOnSubmit(i7);
                dbc.SubmitChanges();

                //Salon
                Interaction i8 = new Interaction() { Url = "cheminee", X = 119, Y = -100, Longueur = 66, Hauteur = 334, SceneId = home_salon.Id, QuestionId = q7.Id };
                Interaction i9 = new Interaction() { Url = "prises", X = 241, Y = 193, Longueur = 18, Hauteur = 8, SceneId = home_salon.Id, QuestionId = q8.Id };
                Interaction i10 = new Interaction() { Url = "radiateur", X = 289, Y = 125, Longueur = 141, Hauteur = 75, SceneId = home_salon.Id, QuestionId = q9.Id };
                Interaction i11 = new Interaction() { Url = "ventilo", X = 703, Y = 347, Longueur = 99, Hauteur = 110, SceneId = home_salon.Id, QuestionId = q10.Id };

                dbc.Interactions.InsertOnSubmit(i8);
                dbc.Interactions.InsertOnSubmit(i9);
                dbc.Interactions.InsertOnSubmit(i10);
                dbc.Interactions.InsertOnSubmit(i11);
                dbc.SubmitChanges();

                #endregion

                #region Reponse
                /**
                 * HOME
                 * */
                //Cuisine
                Reponse r1 = new Reponse() { Intitule = "Yes I can ! Nobody will never know about this !", Vrai = false, QuestionId = q1.Id };
                Reponse r2 = new Reponse() { Intitule = "No, I cannot ! The oven can be warm  and it is dangerous !", Vrai = true, QuestionId = q1.Id };

                Reponse r4 = new Reponse() { Intitule = "Yes I can ! There is no risk ! My parents take this pan everyday !", Vrai = false, QuestionId = q2.Id };
                Reponse r3 = new Reponse() { Intitule = "No, I cannot ! The pan can be warm or contain something warm and it is dangerous !", Vrai = true, QuestionId = q2.Id };

                Reponse r5 = new Reponse() { Intitule = "Yes I can ! Using this knife to find the piece of the puzzle will be amazing !", Vrai = false, QuestionId = q3.Id };
                Reponse r6 = new Reponse() { Intitule = "No I cannot ! Knives are dangerous ! I can cut myself !", Vrai = true, QuestionId = q3.Id };

                Reponse r8 = new Reponse() { Intitule = "Yes I can ! At worst it is not good !", Vrai = false, QuestionId = q4.Id };
                Reponse r7 = new Reponse() { Intitule = "No I cannot ! It is dangerous to drink something if I do not know what it is !", Vrai = true, QuestionId = q4.Id };

                Reponse r9 = new Reponse() { Intitule = "Yes I can ! Maybe I will find my piece of puzzle and a delicious toast !", Vrai = false, QuestionId = q6.Id };
                Reponse r10 = new Reponse() { Intitule = "No I cannot ! My parents told me several times that I cannot touch the toaster !", Vrai = true, QuestionId = q6.Id };

                dbc.Reponses.InsertOnSubmit(r1);
                dbc.Reponses.InsertOnSubmit(r2);
                dbc.Reponses.InsertOnSubmit(r3);
                dbc.Reponses.InsertOnSubmit(r4);
                dbc.Reponses.InsertOnSubmit(r5);
                dbc.Reponses.InsertOnSubmit(r6);
                dbc.Reponses.InsertOnSubmit(r7);
                dbc.Reponses.InsertOnSubmit(r8);
                dbc.Reponses.InsertOnSubmit(r9);
                dbc.Reponses.InsertOnSubmit(r10);
                dbc.SubmitChanges();

                //Salon
                Reponse r11 = new Reponse() { Intitule = "Yes I can ! Maybe I will find my piece of puzzle and a delicious toast !", Vrai = false, QuestionId = q7.Id };
                Reponse r12 = new Reponse() { Intitule = "No I cannot ! My parents told me several times that I cannot touch the toaster !", Vrai = true, QuestionId = q7.Id };

                Reponse r13 = new Reponse() { Intitule = "Yes I can ! Maybe I will find my piece of puzzle and a delicious toast !", Vrai = false, QuestionId = q8.Id };
                Reponse r14 = new Reponse() { Intitule = "No I cannot ! My parents told me several times that I cannot touch the toaster !", Vrai = true, QuestionId = q8.Id };

                Reponse r15 = new Reponse() { Intitule = "Yes I can ! Maybe I will find my piece of puzzle and a delicious toast !", Vrai = false, QuestionId = q9.Id };
                Reponse r16 = new Reponse() { Intitule = "No I cannot ! My parents told me several times that I cannot touch the toaster !", Vrai = true, QuestionId = q9.Id };

                Reponse r17 = new Reponse() { Intitule = "Yes I can ! Maybe I will find my piece of puzzle and a delicious toast !", Vrai = false, QuestionId = q10.Id };
                Reponse r18 = new Reponse() { Intitule = "No I cannot ! My parents told me several times that I cannot touch the toaster !", Vrai = true, QuestionId = q10.Id };
                
                dbc.Reponses.InsertOnSubmit(r11);
                dbc.Reponses.InsertOnSubmit(r12);
                dbc.Reponses.InsertOnSubmit(r13);
                dbc.Reponses.InsertOnSubmit(r14);
                dbc.Reponses.InsertOnSubmit(r15);
                dbc.Reponses.InsertOnSubmit(r16);
                dbc.Reponses.InsertOnSubmit(r17);
                dbc.Reponses.InsertOnSubmit(r18);
                dbc.SubmitChanges();

                #endregion

                #region Explications
                /**
                 * HOME
                 * */
                //Cuisine
                Explication e1 = new Explication() { Mp3 = "MP3/Home/Cuisine/fourExplication.mp3", Pourquoi = "You cannot touch or open the door of an oven. It can be very warm and it is dangerous ! You can burn yourself ! Only adults can open and touch an oven !", QuestionId = q1.Id };
                Explication e2 = new Explication() { Mp3 = "MP3/Home/Cuisine/casseroleExplication.mp3", Pourquoi = "You cannot touch a pan. It can be very warm and it is dangerous ! You can burn yourself ! Only adults can use it !", QuestionId = q2.Id };
                Explication e3 = new Explication() { Mp3 = "MP3/Home/Cuisine/couteauExplication.mp3", Pourquoi = "You cannot touch a knife. It is very dangerous because it can cut your skin ! Only adults can use it ! This knife is not in plastic, it is a real !", QuestionId = q3.Id };
                Explication e4 = new Explication() { Mp3 = "MP3/Home/Cuisine/bottleExplication.mp3", Pourquoi = "Have you seen this warning symbol on the bottle ? It means that this bottle contains a very dangerous product. You cannot touch or drink it ! You could poison yourself ! ", QuestionId = q4.Id };
                Explication e5 = new Explication() { Mp3 = "MP3/Home/Cuisine/tasseExplication.mp3", Pourquoi = "First, you have to turn on the tap and select cold water without touch it because the water can be warm ! After a while, you have to touch the water with your finger to check if the it is not to warm. Finally, you can drink ! Don't forget to turn off the tap when it is finished !", QuestionId = q5.Id };
                Explication e6 = new Explication() { Mp3 = "MP3/Home/Cuisine/toasterExplication.mp3", Pourquoi = "You cannot touch the toaster ! It can be warm and you can burn yourself ! Moreover, you can be electrocuted if you touch it with a metal object !", QuestionId = q6.Id };

                dbc.Explications.InsertOnSubmit(e1);
                dbc.Explications.InsertOnSubmit(e2);
                dbc.Explications.InsertOnSubmit(e3);
                dbc.Explications.InsertOnSubmit(e4);
                dbc.Explications.InsertOnSubmit(e5);
                dbc.Explications.InsertOnSubmit(e6);
                dbc.SubmitChanges();

                //Salon
                Explication e7 = new Explication() { Mp3 = "MP3/Home/Cuisine/toasterExplication.mp3", Pourquoi = "You cannot touch the toaster ! It can be warm and you can burn yourself ! Moreover, you can be electrocuted if you touch it with a metal object !", QuestionId = q7.Id };
                Explication e8 = new Explication() { Mp3 = "MP3/Home/Cuisine/toasterExplication.mp3", Pourquoi = "You cannot touch the toaster ! It can be warm and you can burn yourself ! Moreover, you can be electrocuted if you touch it with a metal object !", QuestionId = q8.Id };
                Explication e9 = new Explication() { Mp3 = "MP3/Home/Cuisine/toasterExplication.mp3", Pourquoi = "You cannot touch the toaster ! It can be warm and you can burn yourself ! Moreover, you can be electrocuted if you touch it with a metal object !", QuestionId = q9.Id };
                Explication e10 = new Explication() { Mp3 = "MP3/Home/Cuisine/toasterExplication.mp3", Pourquoi = "You cannot touch the toaster ! It can be warm and you can burn yourself ! Moreover, you can be electrocuted if you touch it with a metal object !", QuestionId = q10.Id };
                
                dbc.Explications.InsertOnSubmit(e7);
                dbc.Explications.InsertOnSubmit(e8);
                dbc.Explications.InsertOnSubmit(e9);
                dbc.Explications.InsertOnSubmit(e10);
                dbc.SubmitChanges();

                #endregion

                #region Modele

                Modele shirt = new Modele() { Nom = "shirt" };
                Modele pants = new Modele() { Nom = "pants" };
                Modele head = new Modele() { Nom = "head" };
                Modele accessory = new Modele() { Nom = "accessory" };

                dbc.Modeles.InsertOnSubmit(shirt);
                dbc.Modeles.InsertOnSubmit(pants);
                dbc.Modeles.InsertOnSubmit(head);
                dbc.Modeles.InsertOnSubmit(accessory);
                dbc.SubmitChanges();

                #endregion

                #region Accessoire

                //Les vêtements
                Accessoire a1 = new Accessoire() { Url = "Images/Accessoires/head_plage", Dispo = true, ModeleId = head.Id, NiveauId = holiday.Id, Porte = false};
                Accessoire a2 = new Accessoire() { Url = "Images/Accessoires/pants_plage", Dispo = true, ModeleId = pants.Id, NiveauId = holiday.Id, Porte = false };
                Accessoire a3 = new Accessoire() { Url = "Images/Accessoires/shirt_plage", Dispo = true, ModeleId = shirt.Id, NiveauId = holiday.Id, Porte = false };
                Accessoire a4 = new Accessoire() { Url = "Images/Accessoires/head_home", Dispo = false, ModeleId = head.Id, NiveauId = home.Id, Porte = false };
                Accessoire a5 = new Accessoire() { Url = "Images/Accessoires/pants_home", Dispo = false, ModeleId = pants.Id, NiveauId = home.Id, Porte = false };
                Accessoire a6 = new Accessoire() { Url = "Images/Accessoires/shirt_home", Dispo = false, ModeleId = shirt.Id, NiveauId = home.Id, Porte = false };

                dbc.Accessoires.InsertOnSubmit(a1);
                dbc.Accessoires.InsertOnSubmit(a2);
                dbc.Accessoires.InsertOnSubmit(a3);
                dbc.Accessoires.InsertOnSubmit(a4);
                dbc.Accessoires.InsertOnSubmit(a5);
                dbc.Accessoires.InsertOnSubmit(a6);
                dbc.SubmitChanges();

                #endregion

                //On ferme le connexion
                dbc.Dispose();
            }
        }
    }
}
