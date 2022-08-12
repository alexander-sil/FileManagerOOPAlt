using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerOOPAlt.Logic
{
    public static class DirectoryLogic
    {
        // Логика создания каталога
        public static void Create(string dir)
        {
            try
            {
                Directory.CreateDirectory(Path.GetFullPath(dir));

                MainLogic.NonSelectiveRedraw(MainLogic.CurrentDir);
            } catch
            {
                MainLogic.NonSelectiveRedraw(MainLogic.CurrentDir);
            }
        }

        // Логика вычисления размера каталога
        public static long GetSize(string param)
        {
            long length = 0L;

            var dir = new DirectoryInfo(Path.GetFullPath(param));

            foreach (FileInfo file in dir.EnumerateFiles("*.*", new EnumerationOptions() { IgnoreInaccessible = true, RecurseSubdirectories = true }))
            {
                length += file.Length;
            }

            return length;
        }

        // Логика копирования каталога
        public static void Copy(string sourceDir, string destDir)
        {

            var dir = new DirectoryInfo(sourceDir);


            if (!dir.Exists)
            {
                return;
            }

            DirectoryInfo[] dirs = dir.EnumerateDirectories("*", new EnumerationOptions() { IgnoreInaccessible = true, RecurseSubdirectories = false }).ToArray();


            Directory.CreateDirectory(destDir);


            foreach (FileInfo file in dir.EnumerateFiles("*.*", new EnumerationOptions() { IgnoreInaccessible = true, RecurseSubdirectories = false }))
            {
                string targetFilePath = Path.Combine(destDir, file.Name);
                file.CopyTo(targetFilePath);
            }


            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destDir, subDir.Name);
                Copy(subDir.FullName, newDestinationDir);
            }
            
        }

        // Логика перемещения каталога
        public static void Move(string sourceDir, string destDir)
        {
            Copy(sourceDir, destDir);
            Delete(sourceDir);

            MainLogic.NonSelectiveRedraw(MainLogic.CurrentDir);
        }

        // Логика удаления каталога
        public static void Delete(string dir)
        {
            Directory.Delete(Path.GetFullPath(dir), true);



            MainLogic.NonSelectiveRedraw(MainLogic.CurrentDir);
        }

        // Логика переименования каталога
        public static void Rename(string name1, string name2)
        {
            Directory.Move(Path.GetFullPath(name1), Path.GetFullPath(name2));

            MainLogic.NonSelectiveRedraw(MainLogic.CurrentDir);
        }
    }
}
