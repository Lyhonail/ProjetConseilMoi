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

namespace conseilMoi.Resources.Classes
{
    class Produits
    {
        String id_produit;
        String product_name;
        String generic_name;
        List<String> allergene = new List<string>();
        List<String> nutriment = new List<string>();
        List<Allergene> allergene_list = new List<Allergene>();


        //implémenter les autres champs, on ne fait pas de calcul donc on passe tout en String

        //constructeur du produit
        public void SetProduits(String iDp, String pn, String GenName)
        {
            id_produit = iDp;
            product_name = pn;
            generic_name = GenName;
        }

        public String GetProduct_name()
        {
            return product_name;
        }

        public String GetId_Produit()
        {
            return id_produit;
        }

        public String GetGeneric_namet()
        {
            return generic_name;
        }

        //Ajoute un allergene passé en paramètre
        public void AddAllergene(String al)
        {
            allergene.Add(al);
        }

        //Renvoie tous les allergenes dans une chaine
        public String GetAllergenes()
        {
            String Als = "";
            int i = allergene.Count;
            for (int ii = 0; ii < i; ii++)
            {
                Als += allergene[ii] + ", ";
            }
            return Als;
        }

        //ajoute un nutriment passé en paramètre
        public void AddNutriment(String nut)
        {
            nutriment.Add(nut);
        }


        //Renvoie la liste de tous les Nutriments dans une chaine
        public String GetNutriments()
        {
            String Nuts = "";
            int i = nutriment.Count;
            for (int ii = 0; ii < i; ii++)
            {
                Nuts += nutriment[ii] + ", ";
            }
            return Nuts;
        }


        //Ajoute un objet Allergene contenant ID_typeProfil, ID_profil et ID_critère (= ID_allergene) cela permetra de dire quel allergene match avec quel profil en fonction de quel produit
        public void AddCheckAllergene(String al, String idtp, String idp)
        {
            Allergene A = new Allergene();
            A.CreeAllergene(al, idtp, idp);
            allergene_list.Add(A);
        }


        public List<Allergene> GetCheckAllergene()
        {
            return allergene_list;
        }


    }
}