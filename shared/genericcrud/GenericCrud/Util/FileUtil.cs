using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Util
{
    public static class FileUtil
    {
        public static DirectoryInfo RetornarInfoDiretorio(string path)
        {
            var fileNameArray = path.Split('\\');
            var fileName = fileNameArray[fileNameArray.Length - 1];
            var directoryName = path.Replace(fileName, "");
            DirectoryInfo directory = new DirectoryInfo(directoryName);

            if (!directory.Exists)
            {
                directory.Create();
            }

            return directory;          

        }

        public static void CriarDiretorioPermisaoServicoWin(string fileName)
        {
            var directoryInfo = RetornarInfoDiretorio(fileName);
            DirectorySecurity security = directoryInfo.GetAccessControl();

            int? countErro = 0;
            Exception ex = null;
            try
            {
                security.AddAccessRule(new FileSystemAccessRule("LOCAL SERVICE", FileSystemRights.Modify, AccessControlType.Allow));

            }
            catch(Exception e)
            {
                ex = e;
                countErro++;
            }

            try
            {
                security.AddAccessRule(new FileSystemAccessRule("SERVIÇO LOCAL", FileSystemRights.Modify, AccessControlType.Allow));
            }
            catch(Exception e)
            {
                ex = e;
                countErro++;
            }

            if (countErro > 1)
                throw new Exception("Não é possível adicionar permissão", ex);
        }
        public static bool IsDirectoryWritable(string path, bool throwIfFails = false)
        {
            var fileNameArray = path.Split('\\');
            var fileName = fileNameArray[fileNameArray.Length - 1];
            var dirPath = path.Replace(fileName, "");
            try
            {
                using (FileStream fs = File.Create(
                    Path.Combine(
                        dirPath,
                        Path.GetRandomFileName()
                    ),
                    1,
                    FileOptions.DeleteOnClose)
                )
                { }
                return true;
            }
            catch
            {
                if (throwIfFails)
                    throw;
                else
                    return false;
            }
        }
    }
}
