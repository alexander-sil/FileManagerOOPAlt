using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerOOPAlt.Logic
{
    public static class FileLogic
    {

        // Логика создания файла
        public static void Create(string fname)
        {
            File.Create(fname).Dispose();
            MainLogic.NonSelectiveRedraw(MainLogic.CurrentDir);
        }
        
        // Логика копирования файла
        public static void Copy(string sourceFile, string destDir)
        {
            if (File.Exists(Path.GetFullPath(sourceFile)))
            {
                if (Directory.Exists(destDir))
                {
                    string path = Path.Combine(Path.GetDirectoryName(destDir), Path.GetFileName(sourceFile));
                    File.Create(path).Close();
                    new FileInfo(sourceFile).CopyTo(path, true);

                    MainLogic.NonSelectiveRedraw(MainLogic.CurrentDir);
                }

            }
        }

        // Логика удаления файла
        public static void Delete(string param)
        {
            if (File.Exists(param))
                File.Delete(param);
                MainLogic.NonSelectiveRedraw(MainLogic.CurrentDir);
        }

        // Логика перемещения файла
        public static void Move(string sourceFile, string destDir)
        {
            if (File.Exists(sourceFile))
            {
                if (Directory.Exists(destDir))
                {
                    string path = Path.Combine(Path.GetDirectoryName(destDir), Path.GetFileName(sourceFile));
                    File.Create(path).Close();
                    new FileInfo(sourceFile).CopyTo(path, true);

                    File.Delete(sourceFile);

                    MainLogic.NonSelectiveRedraw(MainLogic.CurrentDir);
                }
            }
        }

        // Логика переименования кода
        public static void Rename(string source, string dest)
        {
            File.Move(source, dest, true);
            MainLogic.NonSelectiveRedraw(MainLogic.CurrentDir);
        }

        // Логика получения статических данных текстовых файлов (да, я использовал чтение всего файла в RAM)
        public static string TextInfo(FileInfo info)
        {
            
            string text = File.ReadAllText(info.FullName);

            return $"{info.FullName}\n{text.Length} characters\n{text.Split(" ").Length + 1} words\n{text.Split(Environment.NewLine).Length} lines";
        }
    }
}
