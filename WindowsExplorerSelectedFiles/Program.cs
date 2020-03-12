using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shell32;
using System.IO;
using SHDocVw;

namespace WindowsExplorerSelectedFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename;
            ArrayList selected = new ArrayList();
            ShellWindows winClass = new ShellWindows();
            int cnt = 1;
            foreach (InternetExplorer window in winClass)
            {
                filename = Path.GetFileNameWithoutExtension(window.FullName).ToLower();
                if (filename.ToLowerInvariant() == "explorer")
                {
                    Console.WriteLine("Windows Explorer Instance #{0}:", cnt++);
                    Console.WriteLine(((IShellFolderViewDual2)window.Document).Folder.Title.ToString());
                    FolderItems items = ((IShellFolderViewDual2)window.Document).SelectedItems();
                    foreach (FolderItem item in items)
                        selected.Add(item.Path);
                    if (selected.Count > 0)
                        foreach (var path in selected)
                            Console.WriteLine(path);
                    else
                        Console.WriteLine("No files selected");

                    Console.WriteLine();
                }
                selected.Clear();
            }

            Console.ReadKey();
        }
    }
}
