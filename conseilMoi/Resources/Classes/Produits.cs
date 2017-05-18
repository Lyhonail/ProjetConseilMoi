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
        List<String> allergene;
        List<String> nutriment;
        List<Allergene> allergene_list;


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

        public void AddAllergene(String al)
        {
            allergene.Add(al);
        }

        public void AddNutriment(String nut)
        {
            nutriment.Add(nut);
        }

        public void AddCheckAllergene(String al, String idtp, String idp)
        {
            Allergene A = new Allergene();
            A.CreeAllergene(al, idtp, idp);
            allergene_list.Add(A);
        }


    }
}