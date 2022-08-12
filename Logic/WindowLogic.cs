using Terminal.Gui;

namespace FileManagerOOPAlt.Logic
{
    public static class WindowLogic
    {

        // Общедоступные компоненты

        public static ListView fileList = new ListView(new Rect(1, 2, Console.WindowWidth - 2, Console.WindowHeight - 3), MainLogic.View);

        public static Button goUpButton = new Button("Back");

        public static Window window = new Window("FileManagerOOP")
        {
            X = 0,
            Y = 1,

            Width = Dim.Fill(),
            Height = Dim.Fill()
        };


        // Стартовый метод

        public static void Execute()
        {

            Toplevel top = Application.Top;

            Console.SetWindowSize(90, 30);

            Console.SetBufferSize(90, 30);

            Console.Title = "FileManagerOOP";

            Application.Init();

            top = Application.Top;


            MenuBarItem[] menu = new MenuBarItem[] { new MenuBarItem("Tools", new MenuItem[] {
                 new MenuItem("New File", "", ActionLogic.CreateFile, () => true),
                 new MenuItem("New Directory", "", ActionLogic.CreateDir, () => true),
                 new MenuItem("Search", "", ActionLogic.ShowSearchDialog, () => true),

            }) };


            top.Add(new MenuBar(menu));

            fileList.OpenSelectedItem += MainLogic.PerformAction;

            goUpButton.Clicked += MainLogic.GoUp;

            goUpButton.MouseEnter += MainLogic.Highlight;
            
            goUpButton.MouseLeave += MainLogic.Deactivate;

            window.Add(fileList);

            window.Add(goUpButton);

            top.Add(window);

            MainLogic.Execute();

            Application.Run();

        }
    }
}
