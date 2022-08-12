using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerOOPAlt.Entries
{
    public class FileEntry : Entry
    {
        // Конструктор
        public FileEntry(FileInfo info)
        {
            Reference = info;

            Size = info.Length;

            Name = info.FullName;

            Extension = info.Extension;

            Attributes = info.Attributes;

            LastWriteTime = info.LastWriteTime;

            CreationTime = info.CreationTime;
        }

        // Ccылка на FileSystemInfo
        public override FileSystemInfo Reference { get; set; }

        // Имя
        public override string Name { get; set; }

        // Размер файла
        public long Size { get; set; }

        // Расширение файла
        public string Extension { get; set; }

        // Атрибуты файла формата DOS
        public override FileAttributes Attributes { get; set; }

        // Изменен:
        public override DateTime LastWriteTime { get; set; }

        // Создан:
        public override DateTime CreationTime { get; set; }

    }
}
