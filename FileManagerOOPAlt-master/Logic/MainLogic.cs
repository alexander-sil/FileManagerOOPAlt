using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FileManagerOOPAlt.Entries;
using Terminal.Gui;

namespace FileManagerOOPAlt.Logic
{
    public class MainLogic
    {

        // Поля

        private static List<Entry> _View = new List<Entry>();

        private static DirectoryInfo _CurrentDir = new DirectoryInfo(@"C:\");

        private static Dictionary<string, List<Entry>> _Entries = new Dictionary<string, List<Entry>>();


        // Свойства

        public static DirectoryInfo CurrentDir => _CurrentDir;

        public static List<Entry> View => _View;


        // Заполнение списка записей
        private static void FillEntries(string dir)
        {
            DirectoryInfo[] dirs = _CurrentDir.EnumerateDirectories("*", new EnumerationOptions() { IgnoreInaccessible = true, RecurseSubdirectories = false }).ToArray();
            FileInfo[] files = _CurrentDir.EnumerateFiles("*.*", new EnumerationOptions() { IgnoreInaccessible = true, RecurseSubdirectories = false }).ToArray();


            List<Entry> temp = new List<Entry>();

            foreach (DirectoryInfo j in dirs)
            {
                temp.Add(new DirectoryEntry(j));
            }

            foreach (FileInfo i in files)
            {
                temp.Add(new FileEntry(i));
            }

            _Entries.Add(dir, temp);

        }

        // Метод-обертка над поиском
        public static void SearchDriver(DirectoryEntry startDir, string searchTerm, bool recursive)
        {
            _View.Clear();

            Search(startDir, searchTerm, recursive);

            WindowLogic.fileList.MoveEnd();
            WindowLogic.fileList.MoveHome();
        }

        // Итеративный поиск (включая подпапки)
        public static void Search(DirectoryEntry start, string term, bool rec)
        {
            foreach (FileInfo i in ((DirectoryInfo)start.Reference).EnumerateFiles("*.*", new EnumerationOptions() { IgnoreInaccessible = true, RecurseSubdirectories = false }))
            {
                if (i.Name.Contains(term))
                {
                    _View.Add(new FileEntry(i));
                }
                else
                {
                    ;
                }
            }

            foreach (DirectoryInfo j in ((DirectoryInfo)start.Reference).EnumerateDirectories("*", new EnumerationOptions() { IgnoreInaccessible = true, RecurseSubdirectories = rec }))
            {
                foreach (FileInfo k in j.EnumerateFiles("*.*", new EnumerationOptions() { IgnoreInaccessible = true, RecurseSubdirectories = false }))
                {
                    if (k.Name.Contains(term))
                    {
                        _View.Add(new FileEntry(k));
                    }
                    else
                    {
                        ;
                    }
                }
            }
        }

        // Стартовый метод
        public static void Execute()
        {

            Redraw(new DirectoryInfo(_CurrentDir.FullName));
        }
        
        // Обновление списка записей
        private static void Redraw(DirectoryInfo dir)
        {
            _View.Clear();

            
            if (!_Entries.ContainsKey(dir.FullName)) {
                FillEntries(dir.FullName);
                _View.AddRange(_Entries[dir.FullName]);
            }
            else
            {
                _View.AddRange(_Entries[dir.FullName]);
            }

            WindowLogic.fileList.MoveEnd();
            WindowLogic.fileList.MoveHome();
        }

        // Принудительное обновление списка записей
        public static void NonSelectiveRedraw(DirectoryInfo dir)
        {
            _View.Clear();
            
            
            _Entries.Remove(dir.FullName);

            FillEntries(dir.FullName);
            _View.AddRange(_Entries[dir.FullName]);

            WindowLogic.fileList.MoveEnd();
            WindowLogic.fileList.MoveHome();
        }

        // Подъем на один каталог вверх
        public static void GoUp()
        {
            try
            {
                string needle = _CurrentDir.Name;
                string hstack = _CurrentDir.FullName;

                GoTop();
                if (hstack != _CurrentDir.Root.FullName)
                {

                    string newPath = hstack.Remove(hstack.LastIndexOf(needle));

                    Directory.SetCurrentDirectory(newPath);
                    _CurrentDir = new DirectoryInfo(newPath);

                    Redraw(_CurrentDir);
                    WindowLogic.fileList.SetFocus();
                } 
                else
                {
                    GoTop();
                }
            } catch {
                MessageBox.Query("", "The specified folder has no parent directory", "OK");
            }
        }

        // Переход в коренной каталог
        public static void GoTop()
        {
            DirectoryInfo? root = _CurrentDir.Root;
            Directory.SetCurrentDirectory(root.FullName);
            Redraw(root);
            WindowLogic.fileList.SetFocus();  
        }

        // Подсветить кнопку при наведении
        public static void Highlight(View.MouseEventArgs e)
        {
            WindowLogic.goUpButton.SetFocus();
        }

        // Убрать подсветку кнопки
        public static void Deactivate(View.MouseEventArgs e)
        {
            WindowLogic.fileList.SetFocus();
        }

        // Логика перемещения по каталогам и выполнения действий
        public static void PerformAction(ListViewItemEventArgs e)
        {
            // Логика каталогов
            if (e.Value.GetType() == typeof(DirectoryEntry))
            {

                // Запрос действия
                var answer = MessageBox.Query("Action", "Select the desired action to perform: ", "Show info", "Navigate", "Rename", "Move", "Copy", "Delete");

                // Исполнение действий
                if (answer == 0)
                {
                    // Информация о каталоге
                    ActionLogic.ShowInfo((DirectoryEntry)e.Value);
                }
                else if (answer == 1)
                {
                    // Переход в выбранный каталог
                    try
                    {
                        _CurrentDir = (DirectoryInfo)((DirectoryEntry)e.Value).Reference;

                        Redraw((DirectoryInfo)((DirectoryEntry)e.Value).Reference);

                        Directory.SetCurrentDirectory(_CurrentDir.FullName);

                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Query("", "Access error", "OK");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Query("Unidentified error", ex.Message, "OK");
                    }
                }
                else if (answer == 2)
                {
                    // Переименование
                    ActionLogic.Rename((DirectoryEntry)e.Value);

                } else if (answer == 3)
                {
                    // Перемещение
                    ActionLogic.MoveDir((DirectoryEntry)e.Value);

                } else if (answer == 4)
                {
                    // Копирование
                    ActionLogic.CopyDir((DirectoryEntry)e.Value);

                }
                else if (answer == 5)
                {
                    int x = MessageBox.Query("Confirmation", "Are you sure you want to delete this directory?", "Yes", "No");

                    if (x == 0)
                    {
                        // Удаление
                        DirectoryLogic.Delete(((DirectoryEntry)e.Value).Reference.FullName);
                    }

                }
            }
            else if (e.Value.GetType() == typeof(FileEntry))
            {
                var answer = (((FileEntry)e.Value).Reference.Extension != ".txt") ? MessageBox.Query("Action", "Select the desired action to perform: ", "Show info", "Rename", "Move", "Copy", "Delete") : MessageBox.Query("Action", "Select the desired action to perform: ", "Show info", "Rename", "Move", "Copy", "Delete", "Text info");

                if (answer == 0)
                {
                    ActionLogic.ShowInfo((FileEntry)e.Value);
                } 
                else if (answer == 1)
                {
                    ActionLogic.Rename((FileEntry)e.Value);
                }
                else if (answer == 2)
                {
                    ActionLogic.Move((FileEntry)e.Value);
                }
                else if (answer == 3)
                {
                    ActionLogic.Copy((FileEntry)e.Value);
                }
                else if (answer == 4)
                {
                    int x = MessageBox.Query("Confirmation", "Are you sure you want to delete this file?", "Yes", "No");

                    if (x == 0)
                        FileLogic.Delete(((FileEntry)e.Value).Reference.FullName);
                } else if (answer == 5)
                {
                    MessageBox.Query("Text info", FileLogic.TextInfo((FileInfo)((FileEntry)e.Value).Reference), "OK");
                }
            }

            // Перерисовка
            Redraw(_CurrentDir);
            WindowLogic.fileList.SetFocus();
        }


    }
}
