using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerOOPAlt.Entries
{
    public abstract class Entry
    {
        // Ссылка на запись в файловой системе
        public abstract FileSystemInfo Reference { get; set; }

        // Полное имя записи
        public abstract string Name { get; set; }

        // Атрибуты формата DOS
        public abstract FileAttributes Attributes { get; set; }

        // Изменен:
        public abstract DateTime LastWriteTime { get; set; }

        // Создан:
        public abstract DateTime CreationTime { get; set; }

        // Данный метод вызывается при обновлении списка файлов в GUI
        public override string ToString()
        {
            return this.Name;
        }
    }
}
