#region Using Directives
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Org.Vesic.WinForms;
using System.Drawing;
#endregion

//TODO
//ACTIVATE WEBPAGE METHODS IN OTHER TABS
//ADD FUNCTIONS FOR FORWARD/BACK MENUS
//HISTORY MENU
//FAVORITES
//LOGTIME STRING
//NOTE TAB FUNCTIONALITY
//LOOK AT CLIPBOARD APP
//NEW TAB METHOD - CREATE NEW TAB WITHOUT CREATING WEB BROWSER INSTANTLY, WAIT FOR USER INPUT
//DETECT RIGHT CLICK
//MDI CONTAINER


//show timestamp of when page was loaded

//<link rel="shortcut icon" href="favicon.ico" />

//2.07mb 206kb

namespace Voyager
{
    public partial class Form1 : Form
    {
        #region Strings

        public string versiontext = "4.0.0.0";

        int current_tab_count = 0;

        //string ienodisplay = "Internet Explorer cannot display the webpage";
        string time = "HH:mm tt";
        string date = "MMMM dd, yyyy";
        string currentTime = DateTime.Now.ToString();

        //Search Engines
        string Google = "http://www.google.com/search?q=";
        string Live = "http://search.msn.com/results.aspx?FORM=SMCRT&q=";
        string Wikipedia = "http://en.wikipedia.org/wiki/Special:Search?search=";
        string Yahoo = "http://search.yahoo.com/search?p=";
        string YouTube = "http://www.youtube.com/results?search_query=";

        #endregion

        FormState formState = new FormState();
        ArrayList tabpages = new ArrayList();
        ArrayList webpages = new ArrayList();

        #region Constructor

        public Form1()
        {
            InitializeComponent();

            UpdatingTimer.Start();

            this.splitContainer2.Panel2Collapsed = true;

            tabControl1.TabPages.Clear();
            Create_a_new_tab();

            WebBrowser webpage = GetCurrentWebBrowser();
            webpage.ScriptErrorsSuppressed = true;
            webpage.GoHome();

            fswFavorites.Path = Environment.GetFolderPath(Environment.SpecialFolder.Favorites);

            LoadFavoriteMenuItems();

            webpage.ProgressChanged += new WebBrowserProgressChangedEventHandler(webpage_ProgressChanged);
            webpage.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webpage_LocationChanged);
            webpage.LocationChanged += new EventHandler(webpage_LocationChanged);
            webpage.DocumentTitleChanged += new EventHandler(webpage_LocationChanged);
            webpage.Navigating += new WebBrowserNavigatingEventHandler(webpage_Navigating);
            webpage.Navigated += new WebBrowserNavigatedEventHandler(webpage_Navigated);
            webpage.NewWindow += new CancelEventHandler(webpage_NewWindow);
            webpage.StatusTextChanged += new EventHandler(webpage_StatusTextChanged);

            tsDate.Text = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString();
            TrayIcon.Text = "Voyager\r\n" + versiontext;

            DateTime startTime = DateTime.Now;

            LogInformation.Text = "Log Information:  [Voyager " + versiontext + "] [" + Environment.OSVersion.ToString() + "][" + startTime.ToString() + "]";
        }

        #endregion

        #region Helper Methods

        private void NewTab()
        {
            Create_a_new_tab();

            WebBrowser thiswebpage = GetCurrentWebBrowser();
            thiswebpage.ScriptErrorsSuppressed = true;
            //thiswebpage.Document.Title = "(Untitled)";
            AddressBar.Focus();

            UpdateTabButtons();
        }

        private void CloseTab()
        {
            if (current_tab_count < 2) return;

            TabPage current_tab = tabControl1.SelectedTab;
            WebBrowser thiswebpage = (WebBrowser)webpages[tabpages.IndexOf(current_tab)];
            thiswebpage.Dispose();
            tabpages.Remove(current_tab);
            current_tab.Dispose();
            tabControl1.TabPages.Remove(current_tab);
            current_tab_count--;

            UpdateTabButtons();
        }

        private void ClearHistory()
        {
            try
            {
                HistoryMenu.DropDownItems.Clear();
                HistoryMenu.DropDownItems.Add(goBackToolStripMenuItem);
                HistoryMenu.DropDownItems.Add(goForwardToolStripMenuItem);
                HistoryMenu.DropDownItems.Add(goHomeToolStripMenuItem);
                HistoryMenu.DropDownItems.Add(clearToolStripMenuItem);
                HistoryMenu.DropDownItems.Add(HistorySeparator);
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Clear History\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void KeyLocks()
        {
            if (IsKeyLocked(Keys.CapsLock))
            {
                caps.Enabled = true;
            }

            else
            {
                caps.Enabled = false;
            }

            if (IsKeyLocked(Keys.NumLock))
            {
                num.Enabled = true;
            }

            else
            {
                num.Enabled = false;
            }

            if (IsKeyLocked(Keys.Insert))
            {
                ins.Enabled = true;
            }

            else
            {
                ins.Enabled = false;
            }
        }

        private void FullScreen()
        {
            MenuToolBar.Visible = false;
            AddressToolBar.Visible = true;
            StatusToolBar.Visible = false;
            searchtext.Visible = false;
            searchoption.Visible = false;
            fullclosebtn.Visible = true;
            fullclosebtn.Enabled = true;
            fullclosebtn.Text = "Switch to Normal View";
            fullScreenToolStripMenuItem.Enabled = false;
            formState.Maximize(this);
            splitContainer2.Panel2Collapsed = true;
        }

        private void Create_a_new_tab()
        {
            TabPage newpage = new TabPage("(Untitled)");
            tabpages.Add(newpage);

            tabControl1.TabPages.Add(newpage);

            current_tab_count++;

            WebBrowser webpage = new WebBrowser();

            webpages.Add(webpage);
            webpage.Parent = newpage;
            webpage.Dock = DockStyle.Fill;

            webpage.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webpage_DocumentCompleted);

            tabControl1.SelectedTab = newpage;
        }


        private WebBrowser GetCurrentWebBrowser()
        {
            TabPage current_tab = tabControl1.SelectedTab;
            WebBrowser thiswebpage = (WebBrowser)webpages[tabpages.IndexOf(current_tab)];
            return thiswebpage;
        }

        private void UpdateTip(TabPage tb)
        {
            try
            {
                TabPage current_tab = tb;
                WebBrowser thiswebpage = (WebBrowser)webpages[tabpages.IndexOf(current_tab)];

                if (thiswebpage.Url.ToString() == "")
                {
                    current_tab.Text = "(Untitled)";
                }

                else
                {
                    current_tab.ToolTipText = thiswebpage.Url.ToString();
                }
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Update Tip\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void UpdateName(TabPage tb)
        {
            try
            {
                TabPage current_tab = tb;
                WebBrowser thiswebpage = (WebBrowser)webpages[tabpages.IndexOf(current_tab)];

                if (thiswebpage.DocumentTitle == "")
                {
                    current_tab.Text = "(Untitled)";
                }

                else
                {
                    current_tab.Text = thiswebpage.DocumentTitle;
                }

                if (tabControl1.ItemSize.Width > 250)
                {

                }
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Update Name\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void UpdateAllNames()
        {
            try
            {
                foreach (TabPage tb in tabControl1.TabPages)
                {
                    UpdateName(tb);
                    UpdateTip(tb);
                }
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Update All Names\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void UpdateAddress()
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();

                if (thiswebpage.DocumentTitle == "")
                {
                    AddressBar.Text = "";
                }

                else
                {
                    AddressBar.Text = thiswebpage.Url.ToString();
                }
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Update Address\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void UpdateTitle()
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();

                if (thiswebpage.DocumentTitle == "")
                {
                    this.Text = "(Untitled)";
                }

                if (thiswebpage.DocumentTitle == "(Untitled)")
                {
                    this.Text = "(Untitled)";
                }

                else
                {
                    this.Text = thiswebpage.DocumentTitle + " - Voyager";
                }
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Update Title\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void UpdateTabButtons()
        {
            if (current_tab_count > 1) closeTabToolStripMenuItem.Enabled = true;
            else closeTabToolStripMenuItem.Enabled = false;

            if (current_tab_count > 1) closeTabContextMenuItem.Enabled = true;
            else closeTabContextMenuItem.Enabled = false;
        }

        private void UpdateBackForwardButtons()
        {
            WebBrowser thiswebpage = GetCurrentWebBrowser();

            if (thiswebpage.CanGoBack) backbtn.Enabled = true;
            else backbtn.Enabled = false;

            if (thiswebpage.CanGoBack) goBackToolStripMenuItem.Enabled = true;
            else goBackToolStripMenuItem.Enabled = false;

            if (thiswebpage.CanGoForward) forwardbtn.Enabled = true;
            else forwardbtn.Enabled = false;

            if (thiswebpage.CanGoForward) goForwardToolStripMenuItem.Enabled = true;
            else goForwardToolStripMenuItem.Enabled = false;
        }

        public static bool AskForConfirmation(string Msg, IWin32Window Win)
        {
            const string CONFIRMATIONREQUESTED = "Confirmation Requested";
            DialogResult result = MessageBox.Show(Win, Msg, CONFIRMATIONREQUESTED, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return (result == DialogResult.Yes) ? true : false;
        }

        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                this.Show();
                this.TrayIcon.Visible = false;
                this.WindowState = FormWindowState.Maximized;
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Tray Icon Click\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                AddressBar.Size = new System.Drawing.Size(530, 29);
            }

            if (this.WindowState == FormWindowState.Normal)
            {
                AddressBar.Size = new System.Drawing.Size(270, 29);
            }
        }

        #endregion

        #region File Menu

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Open File - Voyager";
                ofd.AddExtension = true;
                ofd.Filter = "HTML Files (*.htm;*.html)|*.htm;*.html|Image Files (*.gif;*.jpg;*.jpeg)|*.gif;*.jpg;*.jpeg";

                if (ofd.ShowDialog() != DialogResult.Cancel)
                {
                    thiswebpage.Navigate(ofd.FileName);
                }

                else
                {
                    return;
                }
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Open File\r\n" + ee.ToString() + "\r\n");
            }
        }


        private void savetoolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                thiswebpage.ShowSaveAsDialog();
            }
            catch (Exception ee)
            {
                LogEntry.AppendText("Save\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void printtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                thiswebpage.ShowPrintDialog();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Print\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void printpreviewtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                thiswebpage.ShowPrintPreviewDialog();
            }


            catch (Exception ee)
            {
                LogEntry.AppendText("Print Preview\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                thiswebpage.ShowPageSetupDialog();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Print\r\n" + ee.ToString() + "\r\n");
            }

        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                thiswebpage.ShowPropertiesDialog();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Print\r\n" + ee.ToString() + "\r\n");
            }

        }

        public void newTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                NewTab();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("New Tab\r\n" + ee.ToString() + "\r\n");
            }
        }

        public void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CloseTab();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Close Tab\r\n" + ee.ToString() + "\r\n");
            }
        }

        public void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Form1 myForm1 = new Form1();
                myForm1.Show();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("New Window\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Restart();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Restart\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Exit\r\n" + ee.ToString() + "\r\n");
            }
        }

        #endregion

        #region Edit Menu

        private void undotoolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                thiswebpage.Document.ExecCommand("Undo", true, null);
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Undo\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void redotoolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                thiswebpage.Document.ExecCommand("Redo", true, null);
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Redo\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void cuttoolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                thiswebpage.Document.ExecCommand("Cut", true, null);
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Cut\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void copytoolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                thiswebpage.Document.ExecCommand("Copy", true, null);
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Copy\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void pastetoolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                thiswebpage.Document.ExecCommand("Paste", true, null);
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Paste\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                thiswebpage.Document.ExecCommand("Delete", true, null);
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Delete\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void selectalltoolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                thiswebpage.Document.ExecCommand("SelectAll", true, null);
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Select All\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void findInPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                thiswebpage.Focus();
                SendKeys.Send("^f");
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Find in Page\r\n" + ee.ToString() + "\r\n");
            }
        }

        #endregion

        #region View Menu

        private void addressBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                if (AddressToolBar.Visible == true)
                {
                    addressBarToolStripMenuItem.Checked = false;
                    AddressToolBar.Visible = false;
                }

                else
                {
                    addressBarToolStripMenuItem.Checked = true;
                    AddressToolBar.Visible = true;
                }
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Address Bar\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (StatusToolBar.Visible == true)
                {
                    statusBarToolStripMenuItem.Checked = false;
                    StatusToolBar.Visible = false;
                }

                else
                {
                    statusBarToolStripMenuItem.Checked = true;
                    StatusToolBar.Visible = true;
                }
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Status Bar\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void fullScreenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                FullScreen();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Full Screen\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void searchBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (searchtext.Visible == true)
            {
                searchBarToolStripMenuItem.Checked = false;
                searchtext.Visible = false;
                searchoption.Visible = false;
            }

            else
            {
                searchBarToolStripMenuItem.Checked = true;
                searchtext.Visible = true;
                searchoption.Visible = true;
            }
        }

        private void pageSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                sourceCode.Text = thiswebpage.DocumentText;
                sourceInfo.Text = "Source for:  " + "(" + thiswebpage.DocumentTitle + ")  " + thiswebpage.Url.ToString();
                sourceCode.ZoomFactor = 1;
                splitContainer2.Panel2Collapsed = false;
                SourceTabControl.SelectedIndex = 0;
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Page Source\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                stopbtn.PerformClick();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Stop\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                refreshbtn.PerformClick();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Refresh\r\n" + ee.ToString() + "\r\n");
            }
        }

        #endregion

        #region History Menu

        private void goHomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                homebtn.PerformClick();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Go Home\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void goBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                backbtn.PerformClick();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Go Back\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void goForwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                forwardbtn.PerformClick();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Go Forward\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ClearHistory();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Clear History\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void HistoryItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                ToolStripMenuItem HistoryItem = new ToolStripMenuItem();
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                thiswebpage.Navigate(HistoryItem.Tag.ToString());
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("History Item\r\n" + ee.ToString() + "\r\n");
            }
        }

        #endregion

        #region Tools Menu

        private void webSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WebBrowser thiswebpage = GetCurrentWebBrowser();

            if (searchtext.Visible == true)
            {
                searchtext.Focus();
            }

            else
            {
                thiswebpage.GoSearch();
            }
        }

        private void internetOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"inetcpl.cpl");
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Internet Options\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void folderOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("Control", "folders");
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Folder Options\r\n" + ee.ToString() + "\r\n");
            }
        }

        #endregion

        #region Help Menu

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }

        private void homepageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                thiswebpage.Navigate("http://www.nateshoffner.tk");
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Homepage\r\n" + ee.ToString() + "\r\n");
            }

        }

        #endregion

        #region Favorites Handling

        //Use a FileSystemWatcher to determine whether to reload favorites upon
        //dropping down or not.
        //To be more effeicent, I would have, in case of
        //Create, insert a new menu item in the appropriate index
        //delete, remove the menu item
        //renamed, modify the text
        //changed, modify text and/or url

        private bool m_FavNeedReload = false;

        private void fswFavorites_Created(object sender, FileSystemEventArgs e)
        {
            m_FavNeedReload = true;
        }

        private void fswFavorites_Deleted(object sender, FileSystemEventArgs e)
        {
            m_FavNeedReload = true;
        }

        private void fswFavorites_Renamed(object sender, RenamedEventArgs e)
        {
            m_FavNeedReload = true;
        }

        private void fswFavorites_Changed(object sender, FileSystemEventArgs e)
        {
            m_FavNeedReload = true;
        }

        private void LoadFavoriteMenuItems()
        {
            {
                this.Cursor = Cursors.WaitCursor;
                DirectoryInfo objDir = new DirectoryInfo(fswFavorites.Path);
                //Recurse, starting from main dir
                LoadFavoriteMenuItems(objDir, FavoritesMenu);
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Recursive function
        /// </summary>
        /// <param name="objDir"></param>
        private void LoadFavoriteMenuItems(DirectoryInfo objDir, ToolStripDropDownButton ToolStripMenuItem)
        {
            {
                string strName = string.Empty;
                string strUrl = string.Empty;

                DirectoryInfo[] dirs = objDir.GetDirectories();
                foreach (DirectoryInfo dir in dirs)
                {

                    ToolStripMenuItem diritem = new ToolStripMenuItem(dir.Name, openToolStripMenuItem.Image);
                    ToolStripMenuItem.DropDownItems.Add((ToolStripItem)diritem);
                    LoadFavoriteMenuItems(dir, FavoritesMenu);
                }

                FileInfo[] urls = objDir.GetFiles("*.url");
                foreach (FileInfo url in urls)
                {
                    WebBrowser thiswebpage = GetCurrentWebBrowser();

                    //(url.FullName);
                    strName = Path.GetFileNameWithoutExtension(url.Name);
                    //  strUrl = thiswebpage.Navigate(url.FullName);
                    ToolStripMenuItem item = new ToolStripMenuItem(strName);
                    item.Tag = strUrl;
                    item.Click += new EventHandler(ToolStripFavoritesMenuClickHandler);
                    ToolStripMenuItem.DropDownItems.Add((ToolStripItem)item);
                }
            }
        }

        private void tsFavoritesMnu_DropDownOpening(object sender, EventArgs e)
        {
            if (!m_FavNeedReload)
                return;
            m_FavNeedReload = false;

            {
                //Reload favorites
                if (FavoritesMenu.DropDownItems.Count > 3)
                {
                    //Remove from back to front except the original items
                    for (int i = FavoritesMenu.DropDownItems.Count - 1; i > 2; i--)
                    {
                        if ((FavoritesMenu.DropDownItems[i] != FavoritesMenuAddToFavorites) &&
                            (FavoritesMenu.DropDownItems[i] != FavoritesMenuOrganizeFavorites) &&
                            (FavoritesMenu.DropDownItems[i] != FavoritesMenuSeparator))
                        {
                            FavoritesMenu.DropDownItems.Remove(FavoritesMenu.DropDownItems[i]);
                        }
                    }
                }
                //Load favorites
                LoadFavoriteMenuItems();
            }
        }

        private void ToolStripFavoritesMenuClickHandler(object sender, EventArgs e)
        {
            WebBrowser thiswebpage = GetCurrentWebBrowser();

            {
                if (sender == FavoritesMenuAddToFavorites)
                {
                    //m_CurWB.AddToFavorites();
                }
                else if (sender == FavoritesMenuOrganizeFavorites)
                {
                    //m_CurWB.OrganizeFavorites();
                }
                ToolStripItem item = (ToolStripItem)sender;
                if (item.Tag != null)
                    thiswebpage.Navigate(item.Tag.ToString());
            }
        }

        #endregion

        #region Address Bar

        private void AddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                gobtn.PerformButtonClick();
            }
        }

        private void AddressBar_SelectedIndexChanged(object sender, EventArgs e)
        {
            gobtn.PerformButtonClick();
        }

        private void gobtn_ButtonClick(object sender, EventArgs e)
        {

            try
            {
                if (newTabToolStripMenuItem1.Checked == true)
                {
                    string url = AddressBar.Text;
                    if (url == "") return;
                    Create_a_new_tab();
                    WebBrowser thiswebpage = GetCurrentWebBrowser();
                    thiswebpage.Navigate(url);
                }

                if (currentTabToolStripMenuItem.Checked == true)
                {
                    string url = AddressBar.Text;
                    if (url == "") return;
                    WebBrowser thiswebpage = GetCurrentWebBrowser();
                    thiswebpage.Navigate(url);
                }
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Navigate\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void currentTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                currentTabToolStripMenuItem.Checked = true;
                newTabToolStripMenuItem1.Checked = false;
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Current Tab\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void newTabToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                newTabToolStripMenuItem1.Checked = true;
                currentTabToolStripMenuItem.Checked = false;
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("New Tab\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();

                if (fbd.ShowDialog() != DialogResult.Cancel)
                {
                    string fbdurl = fbd.SelectedPath.ToString();
                    WebBrowser thiswebpage = GetCurrentWebBrowser();
                    thiswebpage.Navigate(fbdurl);
                }

                else
                {
                    return;
                }
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Open Folder\r\n" + ee.ToString() + "\r\n");
            }
        }

        #endregion

        #region Search Functions

        private void searchoption_ButtonClick(object sender, EventArgs e)
        {
            WebBrowser thiswebpage = GetCurrentWebBrowser();

            if (googleToolStripMenuItem.Checked == true)
            {
                thiswebpage.Url = new System.Uri(Google + searchtext.Text);
            }

            if (livesearchToolStripMenuItem.Checked == true)
            {
                thiswebpage.Url = new System.Uri(Live + searchtext.Text);
            }

            if (wikipediaToolStripMenuItem.Checked == true)
            {
                thiswebpage.Url = new System.Uri(Wikipedia + searchtext.Text);
            }

            if (yahooToolStripMenuItem.Checked == true)
            {
                thiswebpage.Url = new System.Uri(Yahoo + searchtext.Text);
            }

            if (youtubeToolStripMenuItem.Checked == true)
            {
                thiswebpage.Url = new System.Uri(YouTube + searchtext.Text);
            }
        }

        private void searchtext_KeyDown(object sender, KeyEventArgs e)
        {
            WebBrowser thiswebpage = GetCurrentWebBrowser();
            if (e.KeyCode == Keys.Enter)
            {
                searchoption.PerformButtonClick();
            }
        }

        private void googleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            googleToolStripMenuItem.Checked = true;
            livesearchToolStripMenuItem.Checked = false;
            wikipediaToolStripMenuItem.Checked = false;
            yahooToolStripMenuItem.Checked = false;
            youtubeToolStripMenuItem.Checked = false;
        }

        private void livesearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            livesearchToolStripMenuItem.Checked = true;
            googleToolStripMenuItem.Checked = false;
            wikipediaToolStripMenuItem.Checked = false;
            yahooToolStripMenuItem.Checked = false;
            youtubeToolStripMenuItem.Checked = false;
        }

        private void wikipediaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wikipediaToolStripMenuItem.Checked = true;
            livesearchToolStripMenuItem.Checked = false;
            googleToolStripMenuItem.Checked = false;
            yahooToolStripMenuItem.Checked = false;
            youtubeToolStripMenuItem.Checked = false;
        }

        private void yahooToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yahooToolStripMenuItem.Checked = true;
            livesearchToolStripMenuItem.Checked = false;
            googleToolStripMenuItem.Checked = false;
            wikipediaToolStripMenuItem.Checked = false;
            youtubeToolStripMenuItem.Checked = false;
        }

        private void youtubeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            youtubeToolStripMenuItem.Checked = true;
            livesearchToolStripMenuItem.Checked = false;
            googleToolStripMenuItem.Checked = false;
            wikipediaToolStripMenuItem.Checked = false;
            yahooToolStripMenuItem.Checked = false;
        }

        private void defaultSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WebBrowser thiswebpage = GetCurrentWebBrowser();
            thiswebpage.GoSearch();
        }

        #endregion

        #region Dashboard

        private void homebtn_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                thiswebpage.GoHome();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Go Home\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void backbtn_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                //backbtn.DropDownItems.Add(thiswebpage.DocumentTitle);
                if (thiswebpage.CanGoBack)
                {
                    thiswebpage.GoBack();
                }
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Go Back\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void forwardbtn_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                //forwardbtn.DropDownItems.Add(thiswebpage.DocumentTitle);
                if (thiswebpage.CanGoForward)
                {
                    thiswebpage.GoForward();
                }
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Go Forward\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void refreshbtn_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                thiswebpage.Refresh();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Refresh\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void stopbtn_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();
                thiswebpage.Stop(); ;
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Stop\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void newtabtoolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                NewTab();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("New Tab\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void closetabtoolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                CloseTab();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Close Tab\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void fullclosebtn_Click(object sender, EventArgs e)
        {
            try
            {
                MenuToolBar.Visible = true;
                StatusToolBar.Visible = true;
                searchtext.Visible = true;
                searchoption.Visible = true;
                fullclosebtn.Visible = false;
                fullclosebtn.Enabled = false;
                fullclosebtn.Text = "";
                fullScreenToolStripMenuItem.Enabled = true;
                formState.Restore(this);
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.TopMost = false;
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Close Full Screen\r\n" + ee.ToString() + "\r\n");
            }
        }

        #endregion

        #region Source/Log

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel2Collapsed = false;
            SourceTabControl.SelectedIndex = 1;
        }

        private void sourcecopybtn_Click(object sender, EventArgs e)
        {
            sourceCode.Copy();
        }

        private void sourceselectallbtn_Click(object sender, EventArgs e)
        {
            sourceCode.SelectAll();
        }

        private void wordwrapcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (sourceCode.WordWrap == true)
            {
                wordwrapcheckBox.Checked = false;
                sourceCode.WordWrap = false;
            }

            else
            {
                wordwrapcheckBox.Checked = true;
                sourceCode.WordWrap = true;
            }
        }

        private void detecturlscheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (sourceCode.DetectUrls == true)
            {
                detecturlscheckBox.Checked = false;
                sourceCode.DetectUrls = false;
            }

            else
            {
                detecturlscheckBox.Checked = true;
                sourceCode.DetectUrls = true;
            }
        }

        private void sourcehidebtn_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel2Collapsed = true;
        }

        private void logclearbtn_Click(object sender, EventArgs e)
        {
            LogEntry.Text = "";
        }

        private void logsavebtn_Click(object sender, EventArgs e)
        {
            try
            {
                Stream myStream;
                SaveFileDialog sfd = new SaveFileDialog();

                sfd.Filter = "TE LOG (*.telog)|*.telog";
                sfd.FilterIndex = 2;
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = sfd.OpenFile()) != null)
                    {
                        StreamWriter wText = new StreamWriter(myStream);
                        wText.Write(LogInformation.Text + "\r\n");
                        wText.Write(LogEntry.Text);
                        myStream.Close();
                    }
                }
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Save Log\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void loghidebtn_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel2Collapsed = true;
        }

        #endregion

        #region Control Events

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            UpdateAllNames();
            UpdateBackForwardButtons();
            UpdateAddress();
            UpdateTitle();
            UpdateTabButtons();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAllNames();
            UpdateBackForwardButtons();
            UpdateAddress();
            UpdateTitle();
            UpdateTabButtons();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                WebBrowser thiswebpage = GetCurrentWebBrowser();

                if ((e.CloseReason == CloseReason.ApplicationExitCall)
                    || (e.CloseReason == CloseReason.UserClosing))
                {
                    if (!AskForConfirmation("Proceed to exit application?", this))
                    {
                        e.Cancel = true;

                        {
                            if (thiswebpage != null)
                                thiswebpage.Focus();
                        }
                    }
                }
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Form Closing\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void searchtext_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (searchtext.Text == "< Search Here >")
                {
                    this.searchtext.Text = "";
                    this.searchtext.ForeColor = System.Drawing.SystemColors.WindowText;
                }

                else
                {
                    return;
                }
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Search Focus\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void searchtext_Leave(object sender, EventArgs e)
        {
            try
            {
                if (searchtext.Text == "")
                {
                    this.searchtext.ForeColor = System.Drawing.SystemColors.InactiveBorder;
                    this.searchtext.Text = "< Search Here >";
                }

                else
                {
                    return;
                }
            }
            catch (Exception ee)
            {
                LogEntry.AppendText("Search Unfocus\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void UpdatingTimer_Tick(object sender, EventArgs e)
        {
            KeyLocks();
            tsDate.Text = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString();
        }

        #endregion

        #region WebBrowser Events

        void webpage_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            refreshbtn.Enabled = false;
            stopbtn.Enabled = true;
            ProgressBar.Visible = true;
            ProgressSeparator.Visible = true;
            UpdateAddress();
        }

        void webpage_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            WebBrowser thiswebpage = GetCurrentWebBrowser();

            refreshbtn.Enabled = true;
            stopbtn.Enabled = false;
            ProgressBar.Visible = false;
        }

        private void webpage_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            WebBrowser thiswebpage = GetCurrentWebBrowser();
            if (sender != thiswebpage)
                return;

            ProgressBar.Value = (int)(((double)e.CurrentProgress / e.MaximumProgress) * 100);
        }

        private void webpage_StatusTextChanged(object sender, EventArgs e)
        {
            WebBrowser thiswebpage = GetCurrentWebBrowser();

            if (sender != thiswebpage)
                return;

            tsStatus.Text = thiswebpage.StatusText;
        }

        void webpage_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            WebBrowser thiswebpage = GetCurrentWebBrowser();
            thiswebpage.DocumentText = "Pop-ups Disabled";
        }

        //private void webpage_NewWindow2(object sender, WebBrowserEvents2_NewWindow2Event e)
        //{
        /*
        Form newWindow = new Form();
        newWindow.Width = 450;
        newWindow.Height = 350;
        newWindow.StartPosition = FormStartPosition.CenterParent;
        newWindow.ShowIcon = false;
        newWindow.ShowInTaskbar = true;
        WebBrowser newWeb = new WebBrowser();
        newWeb.Dock = DockStyle.Fill;
        newWindow.Text = newWeb.DocumentTitle;
        newWeb.RegisterAsBrowser = true;
        newWindow.Controls.Add(newWeb);
        newWindow.Show(this);
        */

        //WebBrowser thiswebpage = GetCurrentWebBrowser();

        /*
        System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
        messageBoxCS.AppendFormat("{0} = {1}", "Cancel", e.Cancel);
        messageBoxCS.AppendLine();
        MessageBox.Show(messageBoxCS.ToString(), "NewWindow Event");
        */

        /*
        string url = AddressBar.Text;
        if (url == "") return;
        Create_a_new_tab();
        WebBrowser thiswebpage = GetCurrentWebBrowser();
        thiswebpage.Navigate(url);
        */
        //}

        void webpage_LocationChanged(object sender, EventArgs e)
        {
            WebBrowser thiswebpage = GetCurrentWebBrowser();
            this.Text = thiswebpage.DocumentTitle + " - Voyager";
        }

        void webpage_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser thiswebpage = GetCurrentWebBrowser();

            UpdateAllNames();
            UpdateBackForwardButtons();
            UpdateAddress();
            UpdateTitle();
            UpdateTabButtons();

            stopbtn.Enabled = false;
            refreshbtn.Enabled = true;
            ProgressBar.Visible = false;
            ProgressSeparator.Visible = false;

            richTextBox1.Text = thiswebpage.DocumentText;

            // Create empty menu item objects.
            ToolStripMenuItem HistoryItem = new ToolStripMenuItem();

            // Set the caption of the menu items.
            HistoryItem.Text = thiswebpage.DocumentTitle;
            HistoryItem.Tag = thiswebpage.Url.ToString();
            HistoryItem.ToolTipText = thiswebpage.Document.Url.ToString();

            // Add the menu items to the main menu.
            HistoryMenu.DropDownItems.Add(HistoryItem);

            // Add functionality to the menu items using the Click event. 
            HistoryItem.Click += new System.EventHandler(this.HistoryItem_Click);

            int historycount = HistoryMenu.DropDownItems.Count;

            //If the HistoryMenu item count is 10 items or more, define the variables.
            if (historycount > 10)
            {
                //Enter HistoryMenu Layout Variables Here
            }
        }

        #endregion

        #region Tab Menu

        private void newTabContextMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                NewTab();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("New Tab\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void closeTabContextMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CloseTab();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Close Tab\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void closeOtherTabsContextMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            WebBrowser thiswebpage = GetCurrentWebBrowser();

            HtmlDocument rememberme = thiswebpage.Document;
            rememberme.GetElementById("remBox").SetAttribute("checked", "false");
            thiswebpage.Stop();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            WebBrowser thiswebpage = GetCurrentWebBrowser();
            thiswebpage.Navigate("javascript:void(document.getElementById('remBox').checked = false);");

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            WebBrowser thiswebpage = GetCurrentWebBrowser();
            HtmlDocument submit = thiswebpage.Document;
            submit.GetElementById("user-login-form").InvokeMember("submit", "submit");
        }



        private void TrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Options().ShowDialog();
        }

        private void AddressToolBar_Layout(object sender, LayoutEventArgs e)
        {
            int width = AddressToolBar.DisplayRectangle.Width;
            for (int index = 0; index < AddressToolBar.Items.Count; ++index)
            {
                ToolStripItem curItem = AddressToolBar.Items[index];
                if (curItem != AddressBar)
                {
                    width -= curItem.Width;
                    width -= curItem.Margin.Horizontal;
                }
            }
            AddressBar.Width = Math.Max(0, width - AddressBar.Margin.Horizontal);
        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FullScreen();
            }

            catch (Exception ee)
            {
                LogEntry.AppendText("Full Screen\r\n" + ee.ToString() + "\r\n");
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            WebBrowser thiswebpage = GetCurrentWebBrowser();
            thiswebpage.DocumentText = richTextBox1.Text;
        }

        private void closeTabContextMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void closeOtherTabsContextMenuItem_Click_1(object sender, EventArgs e)
        {

        }
    }
}

