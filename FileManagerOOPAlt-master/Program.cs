using FileManagerOOPAlt.Logic;
using Terminal.Gui;
using System;
using System.Threading;


namespace FileManagerOOPAlt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Запуск
            try
            {
                WindowLogic.Execute();

            } catch (Exception e)
            {
                MessageBox.Query("Error", $"{e.Message}\nExiting application in 10 seconds", "OK");

                Thread.Sleep(10000);
                Environment.Exit(0);
            }
        }
    }
}