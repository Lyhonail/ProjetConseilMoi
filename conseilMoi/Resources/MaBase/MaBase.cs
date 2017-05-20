using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;
using System.IO;
using Android.Database.Sqlite;
using Android.Util;
using conseilMoi.Resources.Classes;

namespace conseilMoi.Resources.MaBase
{
    class MaBase
    {
        //déclaration des variables de la classe
        String path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); //chemin d'enregistrement de la base
        String maBase; //contient 'path' + le nom de la base - utilisé pour la connexion -
        SqliteConnection connexion; //variable de connexion à la base
                                    //Fin de déclarartion des variables


        public string ExistBase() //On vérifie si la base existe ( cette fonction est optionnelle car ne sert à rien d'autre)
        {
            try //si pas d'erreur
            {
                maBase = Path.Combine(path, "data4.sqlite"); ;
                //on regarde si le fichier existe déjà
                if (File.Exists(maBase)) { return maBase + " Existe déjà"; }
                else { return maBase + " - à été créé"; }
            }
            catch
            { //si erreur
                return " ERREUR";
            }
        }//fin creerBase

        public void ConnexionOpen() // Ouverture de la connexion (si la base n'existe pas elle est automatiquement créée)
        {
            this.connexion = new SqliteConnection("Data Source=" + maBase + ";Version=3;");
            this.connexion.Open();
        }//fin ConnexionOpen

        public void ConnexionClose() //fermeture de la connexion
        {
            connexion.Close();
        }//fin ConnexionClose


        public string CreerTableUtilisateur()//création d'une table
        {
            try
            {
                this.ConnexionOpen();
                SqliteCommand command = connexion.CreateCommand();
                command.CommandText = "create table UTILISATEUR		" +
                                        " ( " +
                                        " ID_utilisateur integer PRIMARY KEY ASC, " +
                                        " login          text, " +
                                        " password       text, " +
                                        " nom            text, " +
                                        " prenom         text, " +
                                        " Adresse1       text, " +
                                        " Adresse2       text, " +
                                        " Ville          text, " +
                                        " CP             text, " +
                                        " TEL            text " +
                                        " ); ";
                command.ExecuteNonQuery();
                connexion.Close();
                return "Ok creer table utilisateur";
            }
            //Retourne le message d'erreur SQL
            catch (SqliteException ex)
            {
                return ex.Message;
            }
            //Fermeture de la connexion
            finally
            {
                this.ConnexionClose();
            }
        }// fin CreerTable

        //création de la table PRofil
        public string CreerTableProfil()
        {
            try
            {
                this.ConnexionOpen();
                SqliteCommand command = connexion.CreateCommand();
                command.CommandText = "create table PROFIL		" +
                                        " ( " +
                                        " ID_profil PRIMARY KEY ASC, " +
                                        " Lib_profil          text, " +
                                        " ); ";
                command.ExecuteNonQuery();
                connexion.Close();
                return "Ok creer table profil";
            }
            //Retourne le message d'erreur SQL
            catch (SqliteException ex)
            {
                return ex.Message;
            }
            //Fermeture de la connexion
            finally
            {
                this.ConnexionClose();
            }
        }// fin CreerTableProfil

        //chargement du produit
        public Produits SelectIdProduit(String p)
        {
            try
            {
                this.ConnexionOpen();
                //Selection du produit
                string sql = "select id_produit, product_name, generic_name from produit where id_produit = " + p + "; ";
                SqliteCommand commanda = new SqliteCommand(sql, connexion);
                SqliteDataReader result = commanda.ExecuteReader();
                result.Read();
                Produits produits = new Produits();
                produits.SetProduits(result.GetString(0).ToString(), result.GetString(1).ToString(), result.GetString(2).ToString());
                result.Close();

                //recherche de tous les allergenes qui composent le produit
                string sql_allergene = "select id_allergene from compo_allergene where id_produit  = '" + p + "'; ";
                SqliteCommand command_allergene = new SqliteCommand(sql_allergene, connexion);
                SqliteDataReader result_allergene = command_allergene.ExecuteReader();
                while (result_allergene.Read())
                {
                    produits.AddAllergene(result_allergene.GetInt32(0).ToString());
                    //produits.AddAllergene("100");
                }
                result_allergene.Close();

                //recherche de tous les nutriments qui composent le produit
                string sql_nutriment = "select id_nutriment from compo_nutriment where id_produit  = '" + p + "'; ";
                SqliteCommand command_nutriment = new SqliteCommand(sql_nutriment, connexion);
                SqliteDataReader result_nutriment = command_nutriment.ExecuteReader();
                while (result_nutriment.Read())
                {
                    produits.AddNutriment(result_nutriment.GetString(0));
                }
                result_nutriment.Close();

                //Recherche d'allergène qui matchent avec les critères du profil
                string sql_recherche_allergene =
                    " select PU.ID_typeProfil, PU.ID_profil, PU.ID_critere " +
                    " from profil_utilisateur PU, compo_allergene CA " +
                    " where PU.ID_critere = CA.ID_allergene " +
                    " AND CA.id_produit = '" + p + "'; ";
                SqliteCommand command_recherche_allergene = new SqliteCommand(sql_recherche_allergene, connexion);
                SqliteDataReader result_recherche_allergene = command_recherche_allergene.ExecuteReader();
                while (result_recherche_allergene.Read())
                {
                    produits.AddCheckAllergene(result_recherche_allergene.GetString(2).ToString(),
                                               result_recherche_allergene.GetString(0).ToString(),
                                               result_recherche_allergene.GetString(1).ToString());
                }
                //Fin recherche d'allergene
                result_recherche_allergene.Close();
                //on retourne le produit en entier
                return produits;
            }
            //Retourne le message d'erreur SQL
            catch
            {
                //pas de résultat, on va donc créer un produit vide qui renvoie l'information "aucun produit"
                Produits produits = new Produits();
                produits.SetProduits("000", "erreur", "erreur");
                return produits;
            }
            //Fermeture de la connexion
            finally
            {
                this.ConnexionClose();
            }
        }// fin CreerTableProfil


        //Scann d'un produit --> Ajout dans l'historique
        public string InsertIntoHistorique(String id_typeProfil, String id_produit)
        {
            //On tente d'abbord de modifier un enregistrement qui existerai déjà : la date et les produit  de substitution
            try
            {
                this.ConnexionOpen();
                SqliteCommand command = connexion.CreateCommand();
                command.CommandText = "update historique set  date = datetime() where id_typeProfil='" + id_typeProfil + "' and id_produit='" + id_produit + "' ";

                command.ExecuteNonQuery();
                connexion.Close();
                return "Ok";
            }
            //Si le couple id_produit et id_typeProfil n'existe pas = on le créer
            catch
            {
                try
                {
                    this.ConnexionOpen();
                    SqliteCommand command = connexion.CreateCommand();
                    command.CommandText = "insert into historique (id_typeProfil, id_produit, date) values ( '" + id_typeProfil + "', '" + id_produit + "', datetime() );";

                    command.ExecuteNonQuery();
                    connexion.Close();
                    return "modifie";
                }
                catch
                {
                    return "Erreur";
                }

            }
            //Fermeture de la connexion
            finally
            {
                this.ConnexionClose();
            }
        }// fin CreerTableProfil
















    }
}