using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;
using FileManagerOOPAlt.Entries;

namespace FileManagerOOPAlt.Logic
{
    public static class ActionLogic
    {

        // Диалоги

        private static Window _SearchDialog = new Window(new Rect(7, 7, 40, 7), "Search");

        private static Window _CreationDialog = new Window(new Rect(7, 7, 40, 7), "Confirm file creation");

        private static Window _DirectoryCreationDialog = new Window(new Rect(7, 7, 45, 7), "Confirm directory creation");

        private static Window _RenameDialog = new Window(new Rect(7, 7, 45, 7), "Confirm renaming");

        private static Window _CopyDialog = new Window(new Rect(7, 7, 45, 7), "Confirm destination entry");

        private static Window _MoveDialog = new Window(new Rect(7, 7, 45, 7), "Confirm moving");

        private static Window _DirCopyDialog = new Window(new Rect(7, 7, 45, 7), "Confirm copying");

        private static Window _DirMoveDialog = new Window(new Rect(7, 7, 45, 7), "Confirm directory moving");

        // Поля ввода

        private static TextField _SInput = new TextField(1, 2, 38, "");

        private static TextField _FInput = new TextField(1, 2, 38, "");

        private static TextField _DInput = new TextField(1, 2, 43, "");

        private static TextField _DCInput = new TextField(1, 2, 43, "");

        private static TextField _DMInput = new TextField(1, 2, 43, "");

        private static TextField _RInput = new TextField(1, 2, 43, "");

        private static TextField _CInput = new TextField(1, 2, 43, "");

        private static TextField _MInput = new TextField(1, 2, 43, "");

        private static Entry? _RenamingSubject;

        private static FileEntry? _MovingSubject;

        private static DirectoryEntry? _DirMovingSubject;

        private static FileEntry? _CopyingSubject;

        private static DirectoryEntry? _DirCopyingSubject;

        // Вывод информации о файле или каталоге
        public static void ShowInfo(Entry entry)
        {
            if (entry.GetType() == typeof(FileEntry))
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine($"{((FileEntry)entry).Name}");
                sb.AppendLine($"{((FileEntry)entry).Attributes.ToString()}");
                sb.AppendLine($"{((FileEntry)entry).CreationTime.ToString()}");
                sb.AppendLine($"{((FileEntry)entry).LastWriteTime.ToString()}");
                sb.AppendLine($"{((FileEntry)entry).Extension}");
                sb.AppendLine($"{((FileEntry)entry).Size} bytes actual size");

                MessageBox.Query("Info", sb.ToString(), "OK");
                
            } else if (entry.GetType() == typeof(DirectoryEntry))
            {
                StringBuilder sb = new StringBuilder();

                long size = DirectoryLogic.GetSize(((DirectoryEntry)entry).Reference.FullName);

                sb.AppendLine($"{((DirectoryEntry)entry).Name}");
                sb.AppendLine($"{((DirectoryEntry)entry).Attributes.ToString()}");
                sb.AppendLine($"{((DirectoryEntry)entry).CreationTime.ToString()}");
                sb.AppendLine($"{((DirectoryEntry)entry).LastWriteTime.ToString()}");
                sb.AppendLine($"{size} bytes actual size");

                MessageBox.Query("Info", sb.ToString(), "OK");
            }
        }

        // Диалог поиска
        public static void ShowSearchDialog()
        {
            _SearchDialog.Add(new Label(new Rect(1, 1, 38, 2), "Please enter a search term."));

            _SearchDialog.Add(_SInput);

            Button v = new Button(1, 3, "Ok");
            Button x = new Button(10, 3, "Cancel");
            

            _SearchDialog.Add(v);
            _SearchDialog.Add(x);

            WindowLogic.window.Add(_SearchDialog);

            v.Clicked += SrYes;
            x.Clicked += SrNo;

            _SearchDialog.SetFocus();
        }

        // Диалог создания файлов
        public static void CreateFile()
        {

            _CreationDialog.Add(new Label(new Rect(1, 1, 38, 2),"Please enter a name for the new file."));

            _CreationDialog.Add(_FInput);

            Button v = new Button(1, 3, "Ok");
            Button x = new Button(10, 3, "Cancel");

            _CreationDialog.Add(v);
            _CreationDialog.Add(x);

            WindowLogic.window.Add(_CreationDialog);

            v.Clicked += CfYes;
            x.Clicked += CfNo;

            _CreationDialog.SetFocus();
            
        }

        // Диалог создания каталогов
        public static void CreateDir()
        {

            _DirectoryCreationDialog.Add(new Label(new Rect(1, 1, 43, 2), "Please enter a name for the new directory."));

            _DirectoryCreationDialog.Add(_DInput);

            Button v = new Button(1, 3, "Ok");
            Button x = new Button(10, 3, "Cancel");

            _DirectoryCreationDialog.Add(v);
            _DirectoryCreationDialog.Add(x);

            WindowLogic.window.Add(_DirectoryCreationDialog);

            v.Clicked += CdYes;
            x.Clicked += CdNo;

            _DirectoryCreationDialog.SetFocus();
            
        }

        // Диалог копирования файлов
        public static void Copy(FileEntry entry)
        {

            _CopyingSubject = entry;

            _CopyDialog.Add(new Label(new Rect(1, 1, 43, 2), "Please enter a full destination folder name."));

            _CopyDialog.Add(_CInput);

            Button v = new Button(1, 3, "Ok");
            Button x = new Button(10, 3, "Cancel");

            _CopyDialog.Add(v);
            _CopyDialog.Add(x);

            WindowLogic.window.Add(_CopyDialog);


            v.Clicked += CpYes;
            x.Clicked += CpNo;

            _CopyDialog.SetFocus();

        }

        // Диалог копирования каталогов
        public static void CopyDir(DirectoryEntry entry)
        {
            _DirCopyingSubject = entry;

            _DirCopyDialog.Add(new Label(new Rect(1, 1, 43, 2), "Please enter a full destination folder name."));

            _DirCopyDialog.Add(_DCInput);

            Button v = new Button(1, 3, "Ok");
            Button x = new Button(10, 3, "Cancel");

            _DirCopyDialog.Add(v);
            _DirCopyDialog.Add(x);

            WindowLogic.window.Add(_DirCopyDialog);

            v.Clicked += CpdYes;
            x.Clicked += CpdNo;

            _DirCopyDialog.SetFocus();

        }

        // Диалог перемещения файлов
        public static void Move(FileEntry entry)
        {
            _MovingSubject = entry;

            _MoveDialog.Add(new Label(new Rect(1, 1, 43, 2), "Please enter a full destination folder name."));

            _MoveDialog.Add(_MInput);

            Button v = new Button(1, 3, "Ok");
            Button x = new Button(10, 3, "Cancel");

            _MoveDialog.Add(v);
            _MoveDialog.Add(x);

            WindowLogic.window.Add(_MoveDialog);

         
            v.Clicked += MvYes;
            x.Clicked += MvNo;

            _MoveDialog.SetFocus();

        }

        // Диалог перемещения каталогов
        public static void MoveDir(DirectoryEntry entry)
        {
            _DirMovingSubject = entry;

            _DirMoveDialog.Add(new Label(new Rect(1, 1, 43, 2), "Please enter a full destination folder name."));

            _DirMoveDialog.Add(_DMInput);

            Button v = new Button(1, 3, "Ok");
            Button x = new Button(10, 3, "Cancel");

            _DirMoveDialog.Add(v);
            _DirMoveDialog.Add(x);

            WindowLogic.window.Add(_DirMoveDialog);

            v.Clicked += MvdYes;
            x.Clicked += MvdNo;

            _DirMoveDialog.SetFocus();

        }

        // Общий диалог переименования
        public static void Rename(Entry entry)
        {
            _RenamingSubject = entry;

            _RenameDialog.Add(new Label(new Rect(1, 1, 43, 2), "Please enter a new name for the entry (non-absolute)."));

            _RenameDialog.Add(_RInput);

            Button v = new Button(1, 3, "Ok");
            Button x = new Button(10, 3, "Cancel");

            _RenameDialog.Add(v);
            _RenameDialog.Add(x);

            WindowLogic.window.Add(_RenameDialog);


            // Является ли данная конструкция индусским кодом? Ответ: НЕТ
            if (entry.GetType() == typeof(FileEntry))
            {
                v.Clicked += RnYes;
            } else if (entry.GetType() == typeof(DirectoryEntry))
            {
                v.Clicked += RdYes;
            }

            x.Clicked += RnNo;

            _RenameDialog.SetFocus();

        }

        // Защитный метод (чтобы не было NullReferenceException)
        private static string GetText(TextField param)
        {
            try
            {
                return param.Text.ToString();
            } catch
            {
                return "";
            }
        }


        // Методы закрытия диалогов
        #region no
        private static void SrNo()
        {
            _SearchDialog.RemoveAll();
            WindowLogic.window.Remove(_SearchDialog);
        }

        private static void MvdNo()
        {
            _DirMoveDialog.RemoveAll();
            WindowLogic.window.Remove(_DirMoveDialog);
        }

        private static void MvNo()
        {
            _MoveDialog.RemoveAll();
            WindowLogic.window.Remove(_MoveDialog);
        }

        private static void CpdNo()
        {
            _DirCopyDialog.RemoveAll();
            WindowLogic.window.Remove(_DirCopyDialog);
        }

        private static void CpNo()
        {
            _CopyDialog.RemoveAll();
            WindowLogic.window.Remove(_CopyDialog);
        }

        private static void RnNo()
        {
            _RenameDialog.RemoveAll();
            WindowLogic.window.Remove(_RenameDialog);
        }

        private static void CfNo()
        {
            _CreationDialog.RemoveAll();
            WindowLogic.window.Remove(_CreationDialog);
        }

        private static void CdNo()
        {
            _DirectoryCreationDialog.RemoveAll();
            WindowLogic.window.Remove(_DirectoryCreationDialog);
        }

        #endregion

        // Методы подтверждения
        #region yes

        private static void SrYes()
        {
            _SearchDialog.RemoveAll();
            WindowLogic.window.Remove(_SearchDialog);
            MainLogic.SearchDriver(new DirectoryEntry(MainLogic.CurrentDir), GetText(_SInput), true);    
        }

        private static void MvdYes()
        {
            try
            {
                _DirMoveDialog.RemoveAll();
                WindowLogic.window.Remove(_DirMoveDialog);

                string name = GetText(_DMInput);

                if (!Directory.Exists(name))
                    Directory.CreateDirectory(name);
                DirectoryLogic.Move(_DirMovingSubject.Name, name);
            }
            catch (Exception e)
            {
                MessageBox.Query("Error", $"{e.Message}", "Ok");
            }
        }

        private static void MvYes()
        {
            try
            {
                _MoveDialog.RemoveAll();
                WindowLogic.window.Remove(_MoveDialog);

                string name = GetText(_MInput);

                if (!Directory.Exists(name))
                    Directory.CreateDirectory(name);
                FileLogic.Move(_MovingSubject.Name, name + "\\");
            }
            catch (Exception e)
            {
                MessageBox.Query("Error", $"{e.Message}", "Ok");
            }
        }

        private static void CpdYes()
        {
            try
            {
                _DirCopyDialog.RemoveAll();
                WindowLogic.window.Remove(_DirCopyDialog);

                string name = GetText(_DCInput);

                if (!Directory.Exists(name))
                    Directory.CreateDirectory(name);
                DirectoryLogic.Copy(_DirCopyingSubject.Name, name);
            }
            catch (Exception e)
            {
                MessageBox.Query("Error", $"{e.Message}", "Ok");
            }
        }

        private static void CpYes()
        {
            try
            {
                _CopyDialog.RemoveAll();
                WindowLogic.window.Remove(_CopyDialog);

                string name = GetText(_CInput);

                if (!Directory.Exists(name))
                    Directory.CreateDirectory(name);
                FileLogic.Copy(_CopyingSubject.Name, name + "\\");
            }
            catch (Exception e)
            {
                MessageBox.Query("Error", $"{e.Message}", "Ok");
            }
        }

        private static void RnYes()
        {
            try
            {
                _RenameDialog.RemoveAll();
                WindowLogic.window.Remove(_RenameDialog);

                FileLogic.Rename(_RenamingSubject.Name, Path.Combine(Path.GetDirectoryName(_RenamingSubject.Name), GetText(_RInput)));
            } catch (Exception e)
            {
                MessageBox.Query("Error", $"{e.Message}", "Ok");
            }
        }

        private static void RdYes()
        {
            try
            {
                _RenameDialog.RemoveAll();
                WindowLogic.window.Remove(_RenameDialog);

                DirectoryLogic.Rename(_RenamingSubject.Name, Path.Combine(Path.GetDirectoryName(_RenamingSubject.Name), GetText(_RInput)));
            } catch (Exception e)
            {
                MessageBox.Query("Error", $"{e.Message}", "Ok");
            }
}

        private static void CfYes()
        {
            _CreationDialog.RemoveAll();
            WindowLogic.window.Remove(_CreationDialog);

            FileLogic.Create(GetText(_FInput));
        }

        private static void CdYes()
        {
            _DirectoryCreationDialog.RemoveAll();
            WindowLogic.window.Remove(_DirectoryCreationDialog);

            DirectoryLogic.Create(GetText(_DInput));
            
        }

        #endregion
    }
}
