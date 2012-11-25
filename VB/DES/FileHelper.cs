using System;
using System.IO;

namespace DES
{
	/// <summary>
	/// Summary description for FileHelper.
	/// </summary>
	public abstract class FileHelper
	{
		public static string ReadFile(string sFile)
		{
			// START PARSING AND INSERTING RECORDS
            /*
			FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read);
			StreamReader rw = new StreamReader(fs);
			string sContent = rw.ReadToEnd();
			rw.Close();
			fs.Close();
            */

            //string sContent = null;

            using (StreamReader r = new StreamReader(sFile))
            {
                return r.ReadToEnd();
            }

			//return sContent;
		}

		public static bool WriteFile(string sContent, string sFile)
		{
            using (FileStream fs = new FileStream(sFile, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter rw = new StreamWriter(fs))
                {
                    rw.Write(sContent);
                }
            }

			return true;
		}
	}
}
