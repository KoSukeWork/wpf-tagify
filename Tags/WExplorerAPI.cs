using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Shell32;
using System.IO;
using SHDocVw;

namespace Tags {
    public class ExplorerPaths {
        public int Index { get; set; }
        public string Folder { get; set; }
        public List<string> Paths { get; set; }

        public ExplorerPaths(int ind, string folder = "", List<string> paths = null) {
            Index = ind;
            Folder = folder;
            Paths = paths == null ? new List<string>() : paths;
        }
    }
    public static class WExplorerAPI {
        public static List<ExplorerPaths> GetExplorerFiles() {
            string windowName;
            List<ExplorerPaths> ret = new List<ExplorerPaths>();
            ShellWindows winClass = new ShellWindows();
            int cnt = 0;
            foreach (InternetExplorer window in winClass) {
                windowName = Path.GetFileNameWithoutExtension(window.FullName).ToLower();

                if (windowName.ToLowerInvariant() == "explorer") {
                    var doc = (IShellFolderViewDual2)window.Document;
                    ExplorerPaths expl = new ExplorerPaths(cnt++, doc.Folder.Title);
                    FolderItems items = doc.SelectedItems();

                    foreach (FolderItem item in items)
                        expl.Paths.Add(item.Path);

                    ret.Add(expl);
                }
            }

            return ret;
        }
    }
}
