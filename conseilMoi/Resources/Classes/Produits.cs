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


        //implémenter les autres champs, on ne fait pas de calcul donc on passe tout en String

       //constructeur du produit
        public void SetProduits(String iDp, String pn)
        {
            id_produit = iDp;
            product_name = pn;
        }

        public String GetProduct_name()
        {
            return product_name;
        }

        public String GetId_Produit()
        {
            return id_produit;
        }
    }
}