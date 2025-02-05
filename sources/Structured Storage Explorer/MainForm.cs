﻿#define OLE_PROPERTY

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenMcdf;
using System.IO;
using System.Resources;
using System.Globalization;
using StructuredStorageExplorer.Properties;
using Be.Windows.Forms;
using OpenMcdf.Extensions;
using OpenMcdf.Extensions.Formats;
using System.Linq;
using System.Collections;


namespace StructuredStorageExplorer
{
    public partial class MainForm : Form
    {
        private CompoundFile cf;
        private FileStream fs;
        public MainForm()
        {
            InitializeComponent();

            

#if !OLE_PROPERTY
            tabControl1.TabPages.Remove(tabPage2);
#endif
            //Load images for icons from resx
            Image folderImage = (Image)Properties.Resources.ResourceManager.GetObject("storage");
            Image streamImage = (Image)Properties.Resources.ResourceManager.GetObject("stream");
            //Image olePropsImage = (Image)Properties.Resources.ResourceManager.GetObject("oleprops");

            treeView1.ImageList = new ImageList();
            treeView1.ImageList.Images.Add(folderImage);
            treeView1.ImageList.Images.Add(streamImage);
            //treeView1.ImageList.Images.Add(olePropsImage);

            saveAsToolStripMenuItem.Enabled = false;
            updateCurrentFileToolStripMenuItem.Enabled = false;

        }
        
        private void exportDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //No export if storage
            if (treeView1.SelectedNode == null || !((CFItem)treeView1.SelectedNode.Tag).IsStream)
            {
                MessageBox.Show("Only stream data can be exported", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            CFStream target = (CFStream)treeView1.SelectedNode.Tag;

            // A lot of stream and storage have only non-printable characters.
            // We need to sanitize filename.

            String sanitizedFileName = String.Empty;

            foreach (char c in target.Name)
            {
                if (
                    Char.GetUnicodeCategory(c) == UnicodeCategory.LetterNumber
                    || Char.GetUnicodeCategory(c) == UnicodeCategory.LowercaseLetter
                    || Char.GetUnicodeCategory(c) == UnicodeCategory.UppercaseLetter
                    )

                    sanitizedFileName += c;
            }

            if (String.IsNullOrEmpty(sanitizedFileName))
            {
                sanitizedFileName = "tempFileName";
            }

            saveFileDialog1.FileName = sanitizedFileName + ".bin";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = null;

                try
                {
                    fs = new FileStream(saveFileDialog1.FileName, FileMode.CreateNew, FileAccess.ReadWrite);
                    fs.Write(target.GetData(), 0, (int)target.Size);
                }
                catch (Exception ex)
                {
                    treeView1.Nodes.Clear();
                    MessageBox.Show("Internal error: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Flush();
                        fs.Close();
                        fs = null;
                    }
                }
            }
        }
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode n = treeView1.SelectedNode;
            ((CFStorage)n.Parent.Tag).Delete(n.Name);

            RefreshTree();
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FilterIndex = 2;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                cf.Save(saveFileDialog1.FileName);
            }
        }
        private void updateCurrentFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (canUpdate)
            {
                if (hexEditor.ByteProvider != null && hexEditor.ByteProvider.HasChanges())
                    hexEditor.ByteProvider.ApplyChanges();
                cf.Commit();
            }
            else
                MessageBox.Show("Cannot update a compound document that is not based on a stream or on a file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        private void addStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string streamName = String.Empty;

            if (Utils.InputBox("Add stream", "Insert stream name", ref streamName) == DialogResult.OK)
            {
                CFItem cfs = treeView1.SelectedNode.Tag as CFItem;

                if (cfs != null && (cfs.IsStorage || cfs.IsRoot))
                {
                    try
                    {
                        ((CFStorage)cfs).AddStream(streamName);
                    }
                    catch (CFDuplicatedItemException)
                    {
                        MessageBox.Show("Cannot insert a duplicated item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }


                RefreshTree();
            }
        }
        private void addStorageStripMenuItem1_Click(object sender, EventArgs e)
        {
            string storage = String.Empty;

            if (Utils.InputBox("Add storage", "Insert storage name", ref storage) == DialogResult.OK)
            {
                CFItem cfs = treeView1.SelectedNode.Tag as CFItem;

                if (cfs != null && (cfs.IsStorage || cfs.IsRoot))
                {
                    try
                    {
                        ((CFStorage)cfs).AddStorage(storage);
                    }
                    catch (CFDuplicatedItemException)
                    {
                        MessageBox.Show("Cannot insert a duplicated item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                RefreshTree();
            }
        }
        private void importDataStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fileName = String.Empty;

            if (openDataFileDialog.ShowDialog() == DialogResult.OK)
            {
                CFStream s = treeView1.SelectedNode.Tag as CFStream;

                if (s != null)
                {
                    FileStream f = new FileStream(openDataFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    byte[] data = new byte[f.Length];
                    f.Read(data, 0, (int)f.Length);
                    f.Flush();
                    f.Close();
                    s.SetData(data);

                    RefreshTree();
                }
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cf != null)
                cf.Close();
        }
        private void closeStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.hexEditor.ByteProvider != null && this.hexEditor.ByteProvider.HasChanges())
            {
                if (MessageBox.Show("Do you want to save pending changes ?", "Save changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    this.hexEditor.ByteProvider.ApplyChanges();
                }
            }

            CloseCurrentFile();
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
        private void newStripMenuItem1_Click(object sender, EventArgs e)
        {

            CreateNewFile();
        }
        private void openFileMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    OpenFile();
                }
                catch
                {

                }
            }
        }
        private bool canUpdate = false;
        private void CreateNewFile()
        {
            CloseCurrentFile();

            cf = new CompoundFile();
            canUpdate = false;
            saveAsToolStripMenuItem.Enabled = true;

            updateCurrentFileToolStripMenuItem.Enabled = false;

            RefreshTree();
        }
        private void OpenFile()
        {
            if (!String.IsNullOrEmpty(openFileDialog1.FileName))
            {
                CloseCurrentFile();

                treeView1.Nodes.Clear();
                fileNameLabel.Text = openFileDialog1.FileName;
                LoadFile(openFileDialog1.FileName, true);
                canUpdate = true;
                saveAsToolStripMenuItem.Enabled = true;
                updateCurrentFileToolStripMenuItem.Enabled = true;
            }
        }
        private void LoadFile(string fileName, bool enableCommit)
        {

            FileStream fileStream = new FileStream(fileName, FileMode.Open, enableCommit ? FileAccess.ReadWrite : FileAccess.Read);
            fs = fileStream;

            try
            {
                if (cf != null)
                {
                    cf.Close();
                    cf = null;
                }

                //Load file(loadfilesettings add)
                if (enableCommit)
                {
                    //cf = new CompoundFile(fs, CFSUpdateMode.Update, CFSConfiguration.SectorRecycle | CFSConfiguration.NoValidationException | CFSConfiguration.EraseFreeSectors);
                    cf = new CompoundFile(fs, CFSUpdateMode.Update, CFSConfiguration.SectorRecycle);
                }
                else
                {
                    cf = new CompoundFile(fs);
                }
                //////////////
                //////////////
                //////////////
                
              
                QCN qCn = new QCN();
                qCn.LoadFromCompoundFile(cf);
                


                //////////////
                //////////////
                //////////////
                // burda kaldık


                RefreshTree();
            }
            catch (Exception ex)
            {
                treeView1.Nodes.Clear();
                fileNameLabel.Text = String.Empty;
                MessageBox.Show("Internal error: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }     
        private void RefreshTree()
        {
            treeView1.Nodes.Clear();

            TreeNode root = null;
            root = treeView1.Nodes.Add("Root Entry", "Root");
            root.ImageIndex = 0;
            root.Tag = cf.RootStorage;

            //Recursive function to get all storage and streams
            AddNodes(root, cf.RootStorage);
        }
        private void AddNodes(TreeNode node, CFStorage cfs)
        {
            Action<CFItem> va = delegate (CFItem target)
            {
                TreeNode temp = node.Nodes.Add(
                    target.Name,
                    target.Name + (target.IsStream ? " (" + target.Size + " bytes )" : "")
                    );

                temp.Tag = target;

                if (target.IsStream)
                {

                    //Stream
                    temp.ImageIndex = 1;
                    temp.SelectedImageIndex = 1;
                }
                else
                {
                    //Storage
                    temp.ImageIndex = 0;
                    temp.SelectedImageIndex = 0;

                    //Recursion into the storage
                    AddNodes(temp, (CFStorage)target);
                }
            };

            //Visit NON-recursively (first level only)
            cfs.VisitEntries(va, false);
        }
        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            // Get the node under the mouse cursor.
            // We intercept both left and right mouse clicks
            // and set the selected treenode according.

            TreeNode n = treeView1.GetNodeAt(e.X, e.Y);

            if (n != null)
            {
                if (this.hexEditor.ByteProvider != null && this.hexEditor.ByteProvider.HasChanges())
                {
                    if (MessageBox.Show("Do you want to save pending changes ?", "Save changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        this.hexEditor.ByteProvider.ApplyChanges();
                    }
                }

                treeView1.SelectedNode = n;


                // The tag property contains the underlying CFItem.
                CFItem target = (CFItem)n.Tag;
                CFStream stream = n.Tag as CFStream;

                if (target.IsStream)
                {
                    addStorageStripMenuItem1.Enabled = false;
                    addStreamToolStripMenuItem.Enabled = false;
                    importDataStripMenuItem1.Enabled = true;
                    exportDataToolStripMenuItem.Enabled = true;

#if OLE_PROPERTY    
                    if (target.Name == "Mobile_Property_Info")
                    {
                        PropertySetMobilePropertyInfo mi = new PropertySetMobilePropertyInfo();
                        byte[] by = stream.GetData();
                        Stream sy = new MemoryStream(by);
                        BinaryReader br = new BinaryReader(sy);
                        mi.Read(br);

                        propertyGrid1.SelectedObject = mi;

                        DataTable ds = new DataTable();

                        ds.Columns.Add("Name", typeof(String));
                        ds.Columns.Add("Type", typeof(String));
                        ds.Columns.Add("Value", typeof(String));
                    }
#endif
                }
            }
            else
            {
                addStorageStripMenuItem1.Enabled = true;
                addStreamToolStripMenuItem.Enabled = true;
                importDataStripMenuItem1.Enabled = false;
                exportDataToolStripMenuItem.Enabled = false;
            }


            if (n != null)
            {
                //propertyGrid1.SelectedObject = n.Tag;
                CFStream targetStream = n.Tag as CFStream;
                if (targetStream != null)
                {
                    this.hexEditor.ByteProvider = new StreamDataProvider(targetStream);
                }
                else
                {
                    this.hexEditor.ByteProvider = null;
                }
            }
        }
        private void CloseCurrentFile()
        {
            if (cf != null)
                cf.Close();

            if (fs != null)
                fs.Close();

            treeView1.Nodes.Clear();
            fileNameLabel.Text = String.Empty;
            saveAsToolStripMenuItem.Enabled = false;
            updateCurrentFileToolStripMenuItem.Enabled = false;

            propertyGrid1.SelectedObject = null;
            hexEditor.ByteProvider = null;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Get the node under the mouse cursor.
            // We intercept both left and right mouse clicks
            // and set the selected treenode according.

            TreeNode n = treeView1.SelectedNode;

            if (n != null)
            {
                if (this.hexEditor.ByteProvider != null && this.hexEditor.ByteProvider.HasChanges())
                {
                    if (MessageBox.Show("Do you want to save pending changes ?", "Save changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        this.hexEditor.ByteProvider.ApplyChanges();
                    }
                }

                treeView1.SelectedNode = n;


                // The tag property contains the underlying CFItem.
                CFItem target = (CFItem)n.Tag;
                CFStream stream = n.Tag as CFStream;

                if (target.IsStream)
                {
                    addStorageStripMenuItem1.Enabled = false;
                    addStreamToolStripMenuItem.Enabled = false;
                    importDataStripMenuItem1.Enabled = true;
                    exportDataToolStripMenuItem.Enabled = true;

#if OLE_PROPERTY    
                    if (target.Name == "Mobile_Property_Info")
                    {
                        PropertySetMobilePropertyInfo mi = new PropertySetMobilePropertyInfo();
                        byte[] by = stream.GetData();
                        Stream sy = new MemoryStream(by);
                        BinaryReader br = new BinaryReader(sy);
                        mi.Read(br);

                        propertyGrid1.SelectedObject = mi;

                        DataTable ds = new DataTable();

                        ds.Columns.Add("ID", typeof(Int32));
                        ds.Columns.Add("NAME", typeof(String));
                        ds.Columns.Add("MAPPING", typeof(String));
                        ds.Columns.Add("ENCODING", typeof(Type));
                        ds.Columns.Add("INDEX", typeof(Int16));
                    }
#endif
                }
            }
            else
            {
                addStorageStripMenuItem1.Enabled = true;
                addStreamToolStripMenuItem.Enabled = true;
                importDataStripMenuItem1.Enabled = false;
                exportDataToolStripMenuItem.Enabled = false;
            }


            if (n != null)
            {
                //propertyGrid1.SelectedObject = n.Tag;
                CFStream targetStream = n.Tag as CFStream;
                if (targetStream != null)
                {
                    this.hexEditor.ByteProvider = new StreamDataProvider(targetStream);
                }
                else
                {
                    this.hexEditor.ByteProvider = null;
                }
            }
        }
    }
}
