using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using TableSwitchWebApplication.Helper;

namespace TableSwitchWebApplication.HangFireTask
{
    public class DeleteLogFile
    {
        public static void DeleteFile()
        {
            try
            {
                foreach (var filePath in GetFile())
                {
                    if (CheckDateToDelete(filePath)) // File date greater or equal 30 days...
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers(); //Force Delete
                        File.Delete(filePath);
                    }
                }
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
        public static string[] GetFile()
        {
            try
            {
                // Get all the files in the folder with the .txt extension
                string[] filePaths = Directory.GetFiles(AppCon.LogFilePath, "*.txt");
                return filePaths;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private static bool CheckDateToDelete(string filePath)
        {
            DateTime fileCreatedDate = File.GetCreationTime(filePath);
            DateTime cutoffDate = DateTime.Now.AddDays(-30);
            if (fileCreatedDate <= cutoffDate) return true;
            else return false;
        }
    }
}
