using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MiniTC.View;

namespace MiniTC.Presenter
{
    public class PanelTC
    {
        private readonly IPanelTCView _view;

        public PanelTC(IPanelTCView view)
        {
            _view = view;
            _view.DriveChanged += OnDriveChanged;
            _view.DirectoryChanged += OnDirectoryChanged;

        }

        public void LoadDrives()
        {
            var drives = DriveInfo.GetDrives()
                .Where(d => d.IsReady)
                .Select(d => d.Name)
                .ToList();

            _view.AvailableDrives = drives;

        }

        public void OnDriveChanged()
        {
            var drive = _view.SelectedDrive;
            if (string.IsNullOrEmpty(drive) || !Directory.Exists(drive))
                return;

            _view.CurrentPath = drive;
            LoadDirectoryContent(drive);
        }

        private void OnDirectoryChanged()
        {
            var selected = _view.SelectedPath;
            if (selected == "..")
            {
                var parent = Directory.GetParent(_view.CurrentPath);
                if (parent != null)
                {
                    _view.CurrentPath = parent.FullName;
                    LoadDirectoryContent(parent.FullName);
                }
                else
                {
                    LoadDrives();
                }
            }
            else if (Directory.Exists(selected))
            {
                _view.CurrentPath = selected;
                LoadDirectoryContent(selected);
            }
        }

        public void LoadDirectoryContent(string path)
        {
            try
            {

                if (path.Length == 3 && path[1] == ':' && path[2] == '\\')
                {
                    // root dysku – nic nie robimy
                }
                else
                {
                    path = path.TrimEnd('\\');
                }



                var content = new List<string>();

                if (Path.GetPathRoot(path) != path)
                {
                    content.Add("..");
                }

                foreach (var dir in Directory.GetDirectories(path))
                {
                    content.Add($"<{path[0]}> {Path.GetFileName(dir)}");
                }


                content.AddRange(Directory.GetFiles(path).Select(Path.GetFileName));


                _view.CurrentPath = path;
                _view.CurrentContent = content;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd ładowania zawartości: {ex.Message}", "Błąd",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
