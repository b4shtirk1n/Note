using AutoUpdaterDotNET;
using DiscordRPC;
using DiscordRPC.Message;
using LightAnimation;
using Note.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Note
{
    public partial class MainWindow : Window
    {
        private string find;
        private bool isChange;
        private TextRange textRange;
        private NoteItem currentItem;
        private DiscordRpcClient client;
        private FrameworkElement currentDiag;
        private const string path = "Documents";
        private readonly BackgroundWorker worker;
        private readonly HeightAnimation moreCtx;
        private readonly HeightAnimation contBtnDel;
        private List<NoteItem> items = new List<NoteItem>();
        private readonly List<Theme> themes = new List<Theme>()
        {
            new Theme (0, "White", "\\Themes\\White\\White.xaml"),
            new Theme (1, "Black", "\\Themes\\Black\\Black.xaml")
        };

        public MainWindow()
        {
            new DirectoryInfo(path).Create();

            int currentTheme = Properties.Settings.Default.Theme;

            ChangeSkin(currentTheme);
            InitializeComponent();
            Fill();
            Update();

            CmbTheme.ItemsSource = themes;
            CmbTheme.SelectedIndex = currentTheme;

            moreCtx = new HeightAnimation(0, 100, MoreCtx);
            contBtnDel = new HeightAnimation(0, 24, ContBtnDel);
            worker = new BackgroundWorker();
            worker.DoWork += InitRichPersence;
            worker.RunWorkerAsync();
        }

        private void EnterEditorDrop(object sender, DragEventArgs e) =>
            new OpacityAnimation(0, 0.5, DragNDrop).StartIn();

        private void EditorDrop(object sender, DragEventArgs e)
        {
            string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            Paragraph paragraph = new Paragraph();
            Image image = new Image();
            Uri uri = new Uri(droppedFiles[0]);

            image.Source = new BitmapImage(uri);
            paragraph.Inlines.Add(new InlineUIContainer(image));
            Editor.Document.Blocks.Add(paragraph);

            new OpacityAnimation(0, 0.5, DragNDrop).StartOut();
        }

        private void WindowStateChanged(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    OwnerScreen.Margin = new Thickness(0);
                    break;
                case WindowState.Maximized:
                    OwnerScreen.Margin = new Thickness(5);
                    break;
            }
        }

        private void NoteListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Editor.Visibility = Visibility.Visible;

            if (ContBtnDel.Visibility == Visibility.Visible)
            {
                if (NoteList.SelectedItems == null)
                    BtnDel.IsEnabled = false;
                else
                    BtnDel.IsEnabled = true;
                return;
            }

            if (NoteList.SelectedItem == currentItem || NoteList.SelectedItem == null)
                return;

            if (currentItem != null)
                Save();

            textRange = new TextRange(Editor.Document.ContentStart,
                Editor.Document.ContentEnd);

            currentItem = (NoteItem)NoteList.SelectedItem;
            FileStream stream = new FileStream(currentItem.Path, FileMode.Open);
            textRange.Load(stream, DataFormats.Rtf);
            worker.RunWorkerAsync();
            stream.Close();
            Fill();
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            if (currentItem != null)
                Save();

            Application.Current.Shutdown();
        }

        private void FullscreenClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void MinimizeClick(object sender, RoutedEventArgs e) =>
            WindowState = WindowState.Minimized;

        private void OpenCreateDocClick(object sender, RoutedEventArgs e)
        {
            OpenDialog(BackDiag, currentDiag = CreateDoc);

            NewFileName.Focus();
        }

        private void CreateDocMouseDown(object sender, MouseButtonEventArgs e) => CloseCreateDoc();

        private void BackgroundKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && CreateDoc.Visibility == Visibility.Visible)
                CreateClick(sender, null);
        }

        private void CreateClick(object sender, RoutedEventArgs e)
        {
            if (NameValidation(NewFileName))
            {
                var doc = new FlowDocument();
                FileStream fileStream = new FileStream($"{path}/{NewFileName.Text}.rtf",
                    FileMode.Create);

                TextRange textRange = new TextRange(doc.ContentStart, doc.ContentEnd);
                textRange.Text = NewFileName.Text;
                textRange.Save(fileStream, DataFormats.Rtf);
                fileStream.Close();

                Fill();
                CloseCreateDoc();
            }
        }

        private void SearchGotFocus(object sender, RoutedEventArgs e) =>
            TbxFocus(Search, "Поиск...", string.Empty);

        private void SearchLostFocus(object sender, RoutedEventArgs e) =>
            TbxFocus(Search, string.Empty, "Поиск...");

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            find = Search.Text;

            if (NoteList != null)
                Fill();
        }

        private void OpenMoreCtxClick(object sender, RoutedEventArgs e) =>
            AnimationState(MoreCtx, moreCtx);

        private void OwnerScreenMouseEnter(object sender, MouseEventArgs e) => moreCtx.StartOut();

        private void DeleteManageClick(object sender, RoutedEventArgs e)
        {
            moreCtx.StartOut();

            AnimationState(ContBtnDel, contBtnDel);
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            contBtnDel.StartOut();

            Editor.Document.Blocks.Clear();

            foreach (var item in NoteList.SelectedItems.Cast<NoteItem>())
                File.Delete(item.Path);

            NoteList.SelectedItem = null;
            currentItem = null;
            worker.RunWorkerAsync();
            Fill();
        }

        private void SettingsDiagClick(object sender, RoutedEventArgs e)
        {
            moreCtx.StartOut();

            OpenDialog(BackDiag, currentDiag = Settings);
        }

        private void CmbThemeClick(object sender, RoutedEventArgs e) => moreCtx.StartIn();

        private void CmbThemeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.Theme = CmbTheme.SelectedIndex;
            Properties.Settings.Default.Save();

            ChangeSkin(CmbTheme.SelectedIndex);

            if (currentItem != null)
                CloseDialog();
        }

        private void OpenAppInfoClick(object sender, RoutedEventArgs e)
        {
            moreCtx.StartOut();

            OpenDialog(BackDiag, currentDiag = AppInfo);
        }

        private void BackDiagMouseDown(object sender, RoutedEventArgs e) =>
            CloseDialog();

        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            CloseDialog();
            Update(true);
        }

        private void OpenRenameDiagClick(object sender, RoutedEventArgs e)
        {
            FileName.Text = currentItem.Name;
            OpenDialog(BackDiag, currentDiag = RenameDoc);
        }

        private void RenameClick(object sender, RoutedEventArgs e)
        {
            if (NameValidation(FileName))
            {
                if (currentItem.Name == FileName.Text)
                    return;

                currentItem.Rename(FileName.Text);
                Fill();
                CloseDialog();
            }
        }

        public void ChangeSkin(int themeId)
        {
            Theme newTheme = themes.Find(x => x.Id == themeId);
            ResourceDictionary theme = Application.LoadComponent(new Uri(newTheme.Path,
                UriKind.Relative)) as ResourceDictionary;

            Resources.Clear();
            Resources.MergedDictionaries.Clear();
            Resources.MergedDictionaries.Add(theme);
        }

        private bool NameValidation(TextBox tbx) =>
            tbx.Text != string.Empty && items.Find(x => x.Name == tbx.Text) == null;

        private void Fill()
        {
            string[] files = Directory.GetFiles(path);

            items.Clear();

            foreach (string file in files)
                items.Add(new NoteItem(file));

            if (find != "Поиск..." && items != null)
                items = items.Where(x => x.Name.Contains(Search.Text) ||
                    x.Text.Contains(Search.Text)).ToList();

            NoteList.ItemsSource = items;
            NoteList.Items.Refresh();
        }

        private void Update(bool error = false)
        {
            var currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);

            AutoUpdater.Mandatory = true;
            AutoUpdater.RunUpdateAsAdmin = false;
            AutoUpdater.InstallationPath = currentDirectory.FullName;
            AutoUpdater.ReportErrors = error;
            AutoUpdater.Start("https://raw.githubusercontent.com/b4shtirk1n/Note/Release/Version.xml");
        }

        private void Save()
        {
            TextRange textRange = new TextRange(Editor.Document.ContentStart,
                Editor.Document.ContentEnd);

            FileStream stream = new FileStream(currentItem.Path, FileMode.Create);
            textRange.Save(stream, DataFormats.Rtf);
            currentItem.Description = textRange.Text;
            stream.Close();
        }

        private void TbxFocus(TextBox tbx, string check, string change)
        {
            if (tbx.Text == check)
                tbx.Text = change;
        }

        private void OpenDialog(FrameworkElement backElement, FrameworkElement element)
        {
            new OpacityAnimation(0, 0.5, backElement).StartIn();
            new OpacityAnimation(0, 1, element).StartIn();
        }

        private void CloseDialog()
        {
            new OpacityAnimation(0, 0.5, BackDiag).StartOut();
            new OpacityAnimation(0, 1, currentDiag).StartOut();
        }

        private void CloseCreateDoc()
        {
            CloseDialog();
            if (FileName.Text != string.Empty)
                NoteList.SelectedItem = items.First(x => x.Name == FileName.Text);

            FileName.Text = string.Empty;
            Editor.Focus();
        }

        private void Edit(DependencyProperty property, object value)
        {
            Editor.Selection.ApplyPropertyValue(property, value);
            isChange = true;
        }

        private void AnimationState(UIElement element, BaseAnimation<double> animation)
        {
            if (element.Visibility == Visibility.Hidden)
                animation.StartIn();
            else
                animation.StartOut();
        }

        private void InitRichPersence(object sender, DoWorkEventArgs e)
        {
            if (client == null)
            {
                client = new DiscordRpcClient("1079356595215011890");
                client.Initialize();
            }

            if (currentItem == null)
            {
                client.SetPresence(NewRichPersence());
                return;
            }

            if (client.CurrentPresence.Details == currentItem.Name)
                return;

            client.SetPresence(NewRichPersence(currentItem.Name, "Edits the Document"));
        }

        private RichPresence NewRichPersence(string detail = null, string state = "In app")
        {
            return new RichPresence()
            {
                Details = detail,
                State = state,
                Assets = new Assets()
                {
                    LargeImageKey = "noteicon"
                },
                Timestamps = Timestamps.Now
            };
        }
    }
}
