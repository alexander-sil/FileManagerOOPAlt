using FileManagerOOPAlt.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerOOPAlt.Entries
{
    public class DirectoryEntry : Entry
    {

        // Конструктор
        public DirectoryEntry(DirectoryInfo info)
        {
            Reference = info;

            Name = info.FullName;

            Attributes = info.Attributes;

            LastWriteTime = info.LastWriteTime;

            CreationTime = info.CreationTime;
        }

        // Ccылка на FileSystemInfo
        public override FileSystemInfo Reference { get; set; }

        // Имя каталога
        public override string Name { get; set; }

        // Атрибуты каталога в формате DOS
        public override FileAttributes Attributes { get; set; }

        // Изменен:
        public override DateTime LastWriteTime { get; set; }

        // Создан:
        public override DateTime CreationTime { get; set; }

    }
}
