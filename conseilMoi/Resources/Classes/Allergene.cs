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
    class Allergene
    {
        String ID_alergene;
        String ID_typeProfil;
        String ID_Profil;


        public void CreeAllergene(String idp, String idtp, String idA)
        {
            ID_alergene = idA;
            ID_typeProfil = idtp;
            ID_Profil = idp;
        }

        public String GetIdAlergene()
        {
            return ID_alergene;
        }

        public String GetIdProfil()
        {
            return ID_Profil;
        }

        public String GetIdTypeProfil()
        {
            return ID_typeProfil;


        }

    }
}