using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                MessageBox.Show("Error copying image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}