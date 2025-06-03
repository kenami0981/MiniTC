using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MiniTC.View
{
    public class PanelWrapper : IPanelTCView
    {
        private readonly Form1 _form;
        private readonly bool _isLeftPanel;
        public PanelWrapper(Form1 form, bool isLeftPanel)
        {
            _form = form;
            _isLeftPanel = isLeftPanel;

            if (_isLeftPanel)
            {
                _form.comboBoxLeft.SelectedIndexChanged += (s, e) => DriveChanged?.Invoke();
               
                _form.listBoxLeft.DoubleClick += (s, e) => DirectoryChanged?.Invoke();
            }
            else
            {
                _form.comboBoxRight.SelectedIndexChanged += (s, e) => DriveChanged?.Invoke();
                _form.listBoxRight.DoubleClick += (s, e) => DirectoryChanged?.Invoke();
            }
        }

        public string SelectedDrive
        {
            get
            {
                var selected = _isLeftPanel ? _form.comboBoxLeft.SelectedItem?.ToString()
                                            : _form.comboBoxRight.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selected)) return "";
                return selected.EndsWith("\\") ? selected : selected + "\\";
            }
        }

        public string SelectedPath
        {
            get
            {
                var listBox = _isLeftPanel ? _form.listBoxLeft : _form.listBoxRight;
                var pathBox = _isLeftPanel ? _form.textBoxLeft : _form.textBoxRight;
                var selected = listBox.SelectedItem?.ToString();
                if (selected == "..")
                    return "..";
                if (selected?.StartsWith($"<C> ") == true)
                    return Path.Combine(pathBox.Text, selected.Substring(4));
                return Path.Combine(pathBox.Text, selected);
            }
        }

        public string CurrentPath
        {
            get
            {
                if (_isLeftPanel)
                    return _form.textBoxLeft.Text;
                else
                    return _form.textBoxRight.Text;
            }
            set
            {
                if (_isLeftPanel)
                    _form.textBoxLeft.Text = value;
                else
                    _form.textBoxRight.Text = value;
            }
        }

        public List<string> AvailableDrives
        {
            set
            {
                var combo = _isLeftPanel ? _form.comboBoxLeft : _form.comboBoxRight;
                combo.Items.Clear();
                combo.Items.AddRange(value.ToArray());
            }
        }

        public List<string> CurrentContent
        {
            set
            {
                var listBox = _isLeftPanel ? _form.listBoxLeft : _form.listBoxRight;
                listBox.Items.Clear();
                listBox.Items.AddRange(value.ToArray());
            }
        }

        public string SelectedItem
        {
            get
            {
                if (_isLeftPanel)
                {
                    if (_form.comboBoxLeft.Focused)
                        return _form.comboBoxLeft.SelectedItem?.ToString();
                    return Path.Combine(_form.textBoxLeft.Text,
                                        _form.listBoxLeft.SelectedItem?.ToString()?.Substring(4) ?? "");
                }
                else
                {
                    if (_form.comboBoxRight.Focused)
                        return _form.comboBoxRight.SelectedItem?.ToString();
                    return Path.Combine(_form.textBoxRight.Text,
                                        _form.listBoxRight.SelectedItem?.ToString()?.Substring(4) ?? "");
                }
            }
        }

        public event Action DriveChanged;
        public event Action DirectoryChanged;
    }
}
