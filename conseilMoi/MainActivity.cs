using Android.App;
using Android.Widget;
using Android.OS;
using conseilMoi.Resources.MaBase;
using System.IO;
using System;


namespace conseilMoi
{
    [Activity(Label = "conseilMoi", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var docFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            //Console.WriteLine("Data path:" + Database.DatabaseFilePath);
            var dbFile = Path.Combine(docFolder, "data4.sqlite"); // FILE NAME TO USE WHEN COPIED
            if (!System.IO.File.Exists(dbFile))
            {
                var s = Resources.OpenRawResource(Resource.Raw.data);  // DATA FILE RESOURCE ID
                FileStream writeStream = new FileStream(dbFile, FileMode.OpenOrCreate, FileAccess.Write);
                ReadWriteStream(s, writeStream);
            }

            MaBase db = new MaBase();
            db.ExistBase();
            db.ConnexionOpen();
            db.ConnexionClose();

            StartActivity(typeof(Avertissement));
            
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

