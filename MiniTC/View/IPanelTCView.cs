using System;
using System.Collections.Generic;

namespace MiniTC.View
{
    public interface IPanelTCView
    {
        string SelectedDrive { get; }
        string SelectedPath { get; }

        string CurrentPath { get; set; }
        List<string> AvailableDrives { set; }
        List<string> CurrentContent { set; }
        string SelectedItem { get; }

        event Action DriveChanged;
        event Action DirectoryChanged;
    }
}
