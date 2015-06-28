using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SkypeAdBlock.NativeWindowOperations
{
    /// <summary>
    /// Цепочка окон (поиск по именам классов окон).
    /// </summary>
    public class WindowChain
    {
        /// <summary>
        /// Имена классов окон.
        /// </summary>
        public List<string> ClassNamesList { get; set; }

        public List<IntPtr> HandleList { get; set; }

        /// <summary>
        /// Last window handle.
        /// </summary>
        public IntPtr Handle { get { return HandleList[HandleList.Count - 1]; } }



        /// <summary>
        /// Найти цепочку окон по именам классов.
        /// </summary>
        /// <returns>Вся цепочка найдена.</returns>
        public bool Find()
        {
            HandleList = new List<IntPtr>();
            IntPtr handle = IntPtr.Zero;
            for (int n = 0; n < ClassNamesList.Count; n++)
            {
                var className = ClassNamesList[n];
                if (n == 0)
                {
                    handle = Window.Find(className, null);
                }
                else
                {
                    handle = Window.Find(handle, IntPtr.Zero, className, null);
                }
                if (handle == IntPtr.Zero)
                {
                    return false;
                }
                HandleList.Add(handle);
            }
            return true;
        }

        /// <summary>
        /// Ненайденная часть отображается в квадратных скобках.
        /// </summary>
        public override string ToString()
        {
            var s = "";
            bool openBracket = false;
            for (int i = 0; i < ClassNamesList.Count; i++)
            {
                if (i == HandleList.Count)
                {
                    s += "[";
                    openBracket = true;
                }
                s += (i > 0 ? "/" : "") + ClassNamesList[i];
            }
            if (openBracket)
            {
                s += "]";
            }
            return s;
        }
    }
}
