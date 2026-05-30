using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace DVLD
{
    public static class clsUtil
    {
        public static string ReplaceFileNameWithGUID(string sourceImageFile)
        {
            string FileName = sourceImageFile;
            FileInfo fi = new FileInfo(FileName);
            string ext = fi.Extension;
            return Guid.NewGuid().ToString() + ext;
        }

        public static bool CopyImageToProjectFolder(ref string sourceImageFile)
        {
            string destinationFolder = @"C:\DVLD-People-Images\";

            try
            {
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                string ext = Path.GetExtension(sourceImageFile);
                string newFileName = ReplaceFileNameWithGUID(ext);
                string destinationFile = Path.Combine(destinationFolder, newFileName);

                File.Copy(sourceImageFile, destinationFile, true);

                sourceImageFile = destinationFile;

                return true;
            }
            catch (Exception ex)
            {
                clsGlobalSettings.LogError(ex);

                MessageBox.Show("Error copying image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create()) //SHA = Secure Hash Algorithms.
            {
                // Computing hash value from the UTF-8 encoded input string
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert the byte array to a lowercase hexadecimal string
                return BitConverter.ToString(hashBytes).Replace("-", "");
            }
        }
    }
}
