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
using ZXing.Mobile;
using conseilMoi.Resources.MaBase;
using System.IO;
using Mono.Data.Sqlite;
using conseilMoi.Resources.Classes;

namespace conseilMoi
{
    [Activity(Label = "Avertissement")]
    public class Avertissement : Activity
    {
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            MaBase db = new MaBase();
            db.ExistBase();
            SetContentView(Resource.Layout.Avertissement);
            MobileBarcodeScanner.Initialize(Application);

            var menuProfil = FindViewById<ImageView>(Resource.Id.imageViewAvertissementProfil);
            var menuHistorique = FindViewById<ImageView>(Resource.Id.imageViewAvertissementHistorique);
            var menuScanner = FindViewById<ImageView>(Resource.Id.imageViewAvertissementScann);
            var menuConseil = FindViewById<ImageView>(Resource.Id.imageViewAvertissementConseil);
            var menuAvertissement = FindViewById<ImageView>(Resource.Id.imageViewAvertissementAvertissement);
            var btnMajBase = FindViewById<Button>(Resource.Id.buttonMajBase);


            var txtIdProduit = FindViewById<TextView>(Resource.Id.textViewIdProduitAv);



            btnMajBase.Click += delegate
            {

                var docFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                //Console.WriteLine("Data path:" + Database.DatabaseFilePath);
                var dbFile = Path.Combine(docFolder, "data4.sqlite"); // FILE NAME TO USE WHEN COPIED
                SqliteConnection.CreateFile(dbFile);

                var s = Resources.OpenRawResource(Resource.Raw.data);  // DATA FILE RESOURCE ID
                    FileStream writeStream = new FileStream(dbFile, FileMode.OpenOrCreate, FileAccess.Write);
                    ReadWriteStream(s, writeStream);
                
            };

            Produits produits = new Produits();
            produits = db.SelectIdProduit("3250390503101");
            txtIdProduit.Text = produits.GetId_Produit();



            menuProfil.Click += delegate
            {
                StartActivity(typeof(Profil));
            };
            menuHistorique.Click += delegate
            {
                StartActivity(typeof(Historique));
            };
           
            menuConseil.Click += delegate
            {
                StartActivity(typeof(Conseil));
            };

            //Clik sur le bouton scanner
            menuScanner.Click += async (sender, e) =>
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await scanner.Scan();
                if (result != null)
                {
                    //Intent garde la variable ID Produit et la transmet à l'activité Produit
                    Intent produit = new Intent(this, typeof(Produit));
                    produit.PutExtra("IDproduit", result.Text);
                    StartActivity(produit);
                }
                else { }
            };


            

    }


        private void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = readStream.Read(buffer, 0, Length);
            // write the required bytes
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, Length);
            }
            readStream.Close();
            writeStream.Close();
        }
    }
}