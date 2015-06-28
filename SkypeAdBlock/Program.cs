using System;
using System.Threading;
using SkypeAdBlock.NativeWindowOperations;

namespace SkypeAdBlock
{

    /// <summary>
    /// Удаляет рекламные баннеры из скайпа.
    /// </summary>
    internal class Program
    {
        // Mutex для поиска запущенного экземпляра программы
        static Mutex mutex = new Mutex(true, "{146E25F0-006A-4CFF-B1C3-5C20CD5BDD21}");
        const string hiddenWindowTitle = "* Skype Ad Block *";

        private static void Main(string[] args)
        {
            // ищем запущенный экземпляр программы
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                mutex.ReleaseMutex();
            }
            else
            {
                // уже есть запущенная программа
                // находим ее окно и показываем
                var handle = Window.Find("ConsoleWindowClass", hiddenWindowTitle);
                Window.Show(handle);
                Window.SetForeground(handle);

                // после этого завершаем свою работу
                return;
            }


            Console.WriteLine("Skype AdBlock 1.0");           
            Console.WriteLine("Andrey Svetlichny 2014");
            Console.WriteLine("Press <Esc> to hide window");

            bool showInfo = false;
            while (true)
            {
                // нажата клавиша
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.Escape:
                            // скрываем собственное окно, установив приметный заголовок
                            Console.Title = hiddenWindowTitle;
                            var consoleHandle = Window.GetConsoleHandle();
                            Window.Hide(consoleHandle);
                            break;

                        case ConsoleKey.I:
                            showInfo = true;
                            break;
                    }
                }

                Thread.Sleep(1000);
                SkypeAdBlock.RemoveAds(showInfo);
                showInfo = false;
            }
       }
    }
}
