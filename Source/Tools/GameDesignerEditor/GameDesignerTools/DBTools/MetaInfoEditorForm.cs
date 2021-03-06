﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
//using System.Data.DataRowBuilder;

namespace GameDesingerTools
{
    public partial class MetainfoEditorForm : Form
    {
        public MetainfoEditorForm()
        {
            InitializeComponent();
            groupBox2.Left = groupBox1.Left;
            groupBox2.Top = groupBox1.Top;
            groupBox3.Left = groupBox1.Left;
            groupBox3.Top = groupBox1.Top;
            log.Log(TimeLog.enumLogType.ltstart, "表元编辑器", "工具模块启动", null);
        }
        private TimeLog log = new TimeLog();
        private System.Data.SqlClient.SqlDataAdapter m_adp;
        public System.Data.SqlClient.SqlConnection m_conn;
        public System.Data.DataTable m_dic_meta_info_Table, m_columnTable;
        private string[] EditorTypeEn = { "text", "pathname", "filename", "bool", "lookupcombo", "textcombo", "customeditor"};
        // private string[] EditorTypeCn = { "文本","文件路径浏览","文件名浏览","真假值","下拉框" }; 

        private void Form1_Load(object sender, EventArgs e)
        {
            string SQLtxt = "SELECT name FROM sysobjects WHERE (xtype = 'u') order by name";
            GetDataConn(Program.ConnetionString,SQLtxt);
            for (int i = 0; i < dataGridView1.Columns.Count;i++ )
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        public void FillCatNames(string tblname)
        {
            string sqltxt = string.Format("SELECT catname FROM sys_meta_info WHERE tablename='{0}' GROUP BY catname", tblname);
            /*
            SqlDataAdapter adpt = new SqlDataAdapter(sqltxt, m_conn);
            System.Data.DataSet DS = new System.Data.DataSet();
            adpt.Fill(DS);
            System.Data.DataTable tbl = DS.Tables[0];
            */

            DataTable tbl = Helper.GetDataTableWithSqlProxy("sys_meta_info", sqltxt, MainForm.conn);

            this.catname_comboBox.Items.Clear();
            foreach (System.Data.DataRow row in tbl.Rows)
            {
                string str = row[0].ToString().Trim();
                if (str != "")
                {
                    this.catname_comboBox.Items.AddRange(new object[] { str });
                }
            }
        }

        //初始化数据库连接，及相关的元数据读取
        private void GetDataConn(string strconn, string strsql)
        {
            m_conn = new SqlConnection(strconn);
            
            SqlDataAdapter adp = new SqlDataAdapter(strsql, m_conn);
            System.Data.DataSet DS = new System.Data.DataSet();

            DataTable table = Helper.GetDataTableWithSqlProxy("sysobjects", "SELECT name FROM sysobjects WHERE (xtype = 'u') order by name", m_conn);

            this.tablename_comboBox.DataSource = table;
            this.tablename_comboBox.DisplayMember = "sysobjects.name";

            System.Data.DataSet DS2 = new System.Data.DataSet();
            adp.Fill(DS2, "sysobjects");
            this.listtable_comboBox.DataSource = DS2;
            this.listtable_comboBox.DisplayMember = "sysobjects.name";
            //m_tbl_MainData = DS.Tables[0];
            //tablename_comboBox.Text = "tbl_skill_caster";
                     
            DataGridViewShow();
             
        }

        private void DataGridViewShow()
        {
            string sqltxt = "SELECT * FROM sys_meta_info WHERE tablename = '" + tablename_comboBox.Text + "'";

            m_adp = new SqlDataAdapter(sqltxt, m_conn);
           
            System.Data.DataSet DS = new System.Data.DataSet();

            m_adp.Fill(DS);

            m_dic_meta_info_Table = DS.Tables[0];
            /*
            if ( false && m_dic_meta_info_Table.Rows.Count == 0) //暂时屏蔽
            {
                if (DialogResult.Yes == MessageBox.Show("没有任何关于" + tablename_comboBox.Text + "表的相关数据！是否要自动添加所有记录", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1))
                {
                    for (int index = 0; index < fieldname_comboBox.Items.Count; index++)
                    {
                            System.Data.DataRow dataRow;
                            dataRow = m_dic_meta_info_Table.NewRow();
                            dataRow.BeginEdit();
                            dataRow["tablename"] = (tablename_comboBox.Text.Trim() == "" ? null : tablename_comboBox.Text.Trim());
                            dataRow["fieldname"] = fieldname_comboBox.Items[index].ToString().Trim();
                            dataRow["fieldcnname"] = null;
                            dataRow["editortype"] = "text";
                            dataRow["catname"] = null;
                            dataRow["orderno"] = "0";
                            dataRow["description"] = null;
                            dataRow["listtable"] = null;
                            dataRow["listcondition"] = null;
                            dataRow["keyfield"] = null;
                            dataRow["listfield"] = null;
                            dataRow["listvalues"] = null;
                            dataRow["relativepath"] = null;
                            dataRow["visible"] = 1;
                            dataRow["readonly"] = 0;
                            m_dic_meta_info_Table.Rows.Add(dataRow);
                            dataRow.EndEdit();
                    }
                    try
                    {
                            //使用SqlCommandBuilder  对像填充SqlDataAdapter 的InsertCommand、DeleteCommand、UpdateCommand对像
                            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(m_adp);
                            m_adp.UpdateCommand = cmdBuilder.GetUpdateCommand();

                            int val = m_adp.Update(m_dic_meta_info_Table);
                            m_dic_meta_info_Table.AcceptChanges();

                     }
                     catch (Exception ex)
                     {
                          //  m_dic_meta_info_Table.Rows[m_dic_meta_info_Table.Rows.Count - 1].Delete();
                            MessageBox.Show(ex.Message);
                      }
                 }
            }
             */
                //DataColumn[] primarKey = new DataColumn[1];
                //primarKey[0] = m_dic_meta_info_Table.Columns["fieldname"];
                //m_dic_meta_info_Table.PrimaryKey = primarKey;
                dataGridView1.DataSource = m_dic_meta_info_Table;
           
        }

        private System.Data.DataTable GetDataSQL(string strconn, string strsql)
        {
            SqlDataAdapter adp = new SqlDataAdapter(strsql, m_conn);
            System.Data.DataSet DS1 = new System.Data.DataSet();
            adp.Fill(DS1);
            
            return DS1.Tables[0];
        }
     
        private void RefreshFieldname_ListBox()
        {
            string sql = "SELECT * FROM " + tablename_comboBox.Text;
            m_columnTable = GetDataSQL(txtConn.Text, sql);
            for(int i = 0; i<m_columnTable.Columns.Count;i++)
            {
                fieldname_comboBox.Items.Add(m_columnTable.Columns[i].ColumnName);
            }
            if (fieldname_comboBox.Items.Count > 0)
                fieldname_comboBox.SelectedIndex = 0;
        }
       
        private bool isLoad = true;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!isLoad)
            {
                FillCatNames(this.tablename_comboBox.Text);
                DataGridViewShow();
                fieldname_comboBox.Items.Clear();
                RefreshFieldname_ListBox();
            }
            else 
               isLoad = false;
        }
       
        private void RefreshKeyfiedAndListfield()
        {
            if (listtable_comboBox.Text != "")
            {
                try
                {
                    string sql = "SELECT * FROM " + listtable_comboBox.Text;
                    System.Data.DataTable Table = GetDataSQL(txtConn.Text, sql);
                    for (int i = 0; i < Table.Columns.Count; i++)
                    {
                        keyfield_comboBox.Items.Add(Table.Columns[i].ColumnName);
                        listfield_comboBox.Items.Add(Table.Columns[i].ColumnName);
                    }
                    if (keyfield_comboBox.Items.Count > 0)
                        keyfield_comboBox.SelectedIndex = 0;
                    if (listfield_comboBox.Items.Count > 1)
                        listfield_comboBox.SelectedIndex = 1;
                }
                catch //(Exception ex)
                {

                }
            }
        }
        
        private void listtable_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            keyfield_comboBox.Items.Clear();
            listfield_comboBox.Items.Clear();
            RefreshKeyfiedAndListfield();
        }

        private void add_button_Click(object sender, EventArgs e)
        {
           // int editortypeID = editortype_comboBox.SelectedIndex;       
            System.Data.DataRow dataRow;
            dataRow = m_dic_meta_info_Table.NewRow();
            dataRow.BeginEdit();
            dataRow["tablename"] = (tablename_comboBox.Text.Trim() == "" ? null : tablename_comboBox.Text.Trim());
            dataRow["fieldname"] = fieldname_comboBox.Text.Trim();
            dataRow["fieldcnname"] = fieldcnnametxt.Text.Trim() == "" ? null : fieldcnnametxt.Text.Trim();
            dataRow["editortype"] = EditorTypeEn[editortype_comboBox.SelectedIndex];
            dataRow["catname"] = catname_comboBox.Text.Trim() == "" ? null : catname_comboBox.Text.Trim();
            dataRow["orderno"] = "0";
            dataRow["description"] = descriptiontxt.Text.Trim() == "" ? null : descriptiontxt.Text.Trim();
            dataRow["listtable"] = listtable_comboBox.Text.Trim() == "" ? null : listtable_comboBox.Text.Trim();
            dataRow["listcondition"] = listconditiontxt.Text.Trim() == "" ? null : listconditiontxt.Text.Trim(); 
            dataRow["keyfield"] = keyfield_comboBox.Text.Trim() == "" ? null : keyfield_comboBox.Text.Trim();
            dataRow["listfield"] = listfield_comboBox.Text.Trim() == "" ? null : listfield_comboBox.Text.Trim();
            dataRow["listvalues"] = txtBoxListValues.Text.Trim();
            dataRow["relativepath"] = txtBoxRelativePath.Text.Trim();
            dataRow["visible"] = visible_checkBox.Checked ? 1 : 0;
            dataRow["readonly"] = readonly_checkBox.Checked ? 1 : 0;
            m_dic_meta_info_Table.Rows.Add(dataRow);
            dataRow.EndEdit();
            
            try
            {
                SqlDataAdapter adapter = m_adp;

                ////使用SqlCommandBuilder  对像填充SqlDataAdapter 的InsertCommand、DeleteCommand、UpdateCommand对像
                SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(adapter);
                adapter.InsertCommand = cmdBuilder.GetInsertCommand();

                int val = adapter.Update(m_dic_meta_info_Table);
                m_dic_meta_info_Table.AcceptChanges();
                this.dataGridView1.CurrentCell = this.dataGridView1.Rows[0].Cells[0];

                // 更新类别名下拉框
                FillCatNames(this.tablename_comboBox.Text);
            }
            catch (Exception ex)
            {
                m_dic_meta_info_Table.Rows[m_dic_meta_info_Table.Rows.Count - 1].Delete();
                MessageBox.Show(ex.Message);
                
            }
        }

        private void addField_button_Click(object sender, EventArgs e)
        {
            paraMetaInfo param = new paraMetaInfo();
            param.m_strEditType = EditorTypeEn[editortype_comboBox.SelectedIndex];
            param.m_strFieldNameCN = fieldcnnametxt.Text.Trim();
            param.m_strCatName = catname_comboBox.Text.Trim();
            param.m_strDescription = descriptiontxt.Text.Trim();
            param.m_bVisible = visible_checkBox.Checked;
            param.m_bReadOnly = readonly_checkBox.Checked;
            switch (EditorTypeEn[editortype_comboBox.SelectedIndex])
            {
                case "filename":
                    param.m_strRelativePath = txtBoxRelativePath.Text.Trim();
                    break;
                case "lookupcombo":
                    param.m_strListTable = listtable_comboBox.Text.Trim();
                    param.m_strKeyField = keyfield_comboBox.Text.Trim();
                    param.m_strListField = listfield_comboBox.Text.Trim();
                    param.m_strCondition = listconditiontxt.Text.Trim();
                    break;
                case "textcombo":
                    param.m_strTextCombo = txtBoxListValues.Text.Trim();
                    break;
                default:
                    break;
            }

            addField form = new addField(tablename_comboBox.Text, editortype_comboBox.Text, this, param);
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                RefreshFieldname_ListBox();
                DataGridViewShow();
            }
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("是否确定要删除该数据！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2))
            {
                try
                {
                    int RowID = dataGridView1.CurrentRow.Index;
                    

                    string tablename = m_dic_meta_info_Table.Rows[RowID][0].ToString();
                    string fieldname = m_dic_meta_info_Table.Rows[RowID][1].ToString();
                    string sqltxt = "DELETE FROM sys_meta_info WHERE (tablename = '" + tablename + "') AND (fieldname = '" + fieldname + "')";
                    m_adp.DeleteCommand = new SqlCommand(sqltxt, m_conn);
                    m_dic_meta_info_Table.Rows[RowID].Delete();
                    m_adp.Update(m_dic_meta_info_Table);
                  
                    MessageBox.Show("数据删除成功！", "删除信息");
                    m_dic_meta_info_Table.AcceptChanges();

                    // 更新类别名下拉框
                    FillCatNames(this.tablename_comboBox.Text);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                int RowID = dataGridView1.CurrentRow.Index;
                string editortype = m_dic_meta_info_Table.Rows[RowID]["editortype"].ToString().Trim();
                int editortypeID ;
                //editortype = EditortypeConversion(editortype, true);
                for (editortypeID = 0; editortypeID < EditorTypeEn.Length; editortypeID++)
                {
                    if (string.Compare(editortype, EditorTypeEn[editortypeID]) == 0)
                        break;
                    
                }
                if (editortypeID >= EditorTypeEn.Length)
                    editortypeID = 0;
                  
                tablename_comboBox.Text = m_dic_meta_info_Table.Rows[RowID]["tablename"].ToString().Trim();
                fieldname_comboBox.Text = m_dic_meta_info_Table.Rows[RowID]["fieldname"].ToString().Trim();
                fieldcnnametxt.Text = m_dic_meta_info_Table.Rows[RowID]["fieldcnname"].ToString().Trim();
                editortype_comboBox.SelectedIndex = editortypeID;
                catname_comboBox.Text = m_dic_meta_info_Table.Rows[RowID]["catname"].ToString().Trim();
                descriptiontxt.Text = m_dic_meta_info_Table.Rows[RowID]["description"].ToString().Trim();
                listtable_comboBox.Text = m_dic_meta_info_Table.Rows[RowID]["listtable"].ToString().Trim();
                listconditiontxt.Text = m_dic_meta_info_Table.Rows[RowID]["listcondition"].ToString().Trim();
                keyfield_comboBox.Text = m_dic_meta_info_Table.Rows[RowID]["keyfield"].ToString().Trim();
                listfield_comboBox.Text = m_dic_meta_info_Table.Rows[RowID]["listfield"].ToString().Trim();
                txtBoxListValues.Text = m_dic_meta_info_Table.Rows[RowID]["listvalues"].ToString().Trim();
                txtBoxRelativePath.Text = m_dic_meta_info_Table.Rows[RowID]["relativepath"].ToString().Trim();
                //string a = m_dic_meta_info_Table.Rows[dataGridView1.CurrentRow.Index][11].ToString();
                //visible_checkBox.CheckState = (string.Compare(m_dic_meta_info_Table.Rows[RowID]["visible"].ToString(), "True") == 0) ? CheckState.Checked : CheckState.Unchecked;
                visible_checkBox.Checked = (string.Compare(m_dic_meta_info_Table.Rows[RowID]["visible"].ToString(), "True") == 0) ? true : false;
                //readonly_checkBox.CheckState = (string.Compare(m_dic_meta_info_Table.Rows[RowID]["readonly"].ToString(), "True") == 0) ? CheckState.Checked : CheckState.Unchecked;
                readonly_checkBox.Checked = (string.Compare(m_dic_meta_info_Table.Rows[RowID]["readonly"].ToString(), "True") == 0) ? true : false;
            }
            catch //(Exception ex)
            {
            //    MessageBox.Show(ex.Message);
            }
        }

        private void modification_button_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("是否确定要修改该数据！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2))
            {
                             
                System.Data.DataRow dataRow;
                dataRow = m_dic_meta_info_Table.Rows[dataGridView1.CurrentRow.Index];
                dataRow.BeginEdit();
                dataRow["tablename"] = (tablename_comboBox.Text.Trim() == "" ? null : tablename_comboBox.Text.Trim());
                dataRow["fieldname"] = fieldname_comboBox.Text.Trim();
                dataRow["fieldcnname"] = fieldcnnametxt.Text.Trim() == "" ? null : fieldcnnametxt.Text.Trim();
                dataRow["editortype"] = EditorTypeEn[editortype_comboBox.SelectedIndex];
                dataRow["catname"] = catname_comboBox.Text.Trim() == "" ? null : catname_comboBox.Text.Trim();
                dataRow["orderno"] = "0";
                dataRow["description"] = descriptiontxt.Text.Trim() == "" ? null : descriptiontxt.Text.Trim();
                dataRow["listtable"] = listtable_comboBox.Text.Trim() == "" ? null : listtable_comboBox.Text.Trim();
                dataRow["listcondition"] = listconditiontxt.Text.Trim() == "" ? null : listconditiontxt.Text.Trim();
                dataRow["keyfield"] = keyfield_comboBox.Text.Trim() == "" ? null : keyfield_comboBox.Text.Trim();
                dataRow["listfield"] = listfield_comboBox.Text.Trim() == "" ? null : listfield_comboBox.Text.Trim();
                dataRow["listvalues"] = txtBoxListValues.Text.Trim() == "" ? null : txtBoxListValues.Text.Trim();
                dataRow["relativepath"] = txtBoxRelativePath.Text.Trim() == "" ? null : txtBoxRelativePath.Text.Trim();
                dataRow["visible"] = visible_checkBox.Checked ? 1 : 0;
                dataRow["readonly"] = readonly_checkBox.Checked ? 1 : 0;
                // m_dic_meta_info_Table.Rows.Add(dataRow);
                dataRow.EndEdit();
             
                try
                {
                    //使用SqlCommandBuilder  对像填充SqlDataAdapter 的InsertCommand、DeleteCommand、UpdateCommand对像
                    SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(m_adp);
                    m_adp.UpdateCommand = cmdBuilder.GetUpdateCommand();

                    int val = m_adp.Update(m_dic_meta_info_Table);
                    m_dic_meta_info_Table.AcceptChanges();
                  //  MessageBox.Show("数据修改成功！", "修改信息");

                    // 更新类别名下拉框
                    FillCatNames(this.tablename_comboBox.Text);
                }
                catch (Exception ex)
                {
                  //  m_dic_meta_info_Table.Rows[m_dic_meta_info_Table.Rows.Count - 1].Delete();
                    MessageBox.Show(ex.Message);

                    // throw;
                }
            }
        }

        private void editortype_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(EditorTypeEn[editortype_comboBox.SelectedIndex])
            {
                case "lookupcombo":
                    groupBox1.Enabled = true;
                    groupBox1.Visible = true;
                    groupBox2.Enabled = false;
                    groupBox2.Visible = false;
                    groupBox3.Enabled = false;
                    groupBox3.Visible = false;
                    break;
                case "textcombo":
                    groupBox2.Visible = true;
                    groupBox2.Enabled = true;
                    groupBox1.Enabled = false;
                    groupBox1.Visible = false;
                    groupBox3.Enabled = false;
                    groupBox3.Visible = false;
                    break;
                case "filename":
                    groupBox3.Visible = true;
                    groupBox3.Enabled = true;
                    groupBox1.Enabled = false;
                    groupBox1.Visible = false;
                    groupBox2.Enabled = false;
                    groupBox2.Visible = false;
                    break;
                default:
                    groupBox1.Enabled = false;
                    groupBox1.Visible = false;
                    groupBox2.Enabled = false;
                    groupBox2.Visible = false;
                    groupBox3.Enabled = false;
                    groupBox3.Visible = false;
                    break;
            }
            /*
            if (string.Compare(EditorTypeEn[editortype_comboBox.SelectedIndex] ,"lookupcombo",true)==0)
            {
                groupBox1.Enabled = true;
                listtable_comboBox.Enabled = true;
                listfield_comboBox.Enabled = true;
                keyfield_comboBox.Enabled = true;
                listconditiontxt.Enabled = true;
                
            }
            else
            {
                groupBox1.Enabled = false;
                listtable_comboBox.Enabled = false;
                listfield_comboBox.Enabled = false;
                keyfield_comboBox.Enabled = false;
                listconditiontxt.Enabled = false;
                listconditiontxt.Text = null;
            }
            */
        }
      
        private void fieldname_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchDataInDataGridView();  
        }

        private void SearchDataInDataGridView()
        {
            try
            {
                string a = fieldname_comboBox.Text.Trim();
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {

                    string b = dataGridView1.Rows[i].Cells[1].Value.ToString().Trim();
                    if (string.Compare(a, b, true) == 0)
                    {
                        
                        this.dataGridView1.CurrentCell = this.dataGridView1.Rows[i].Cells[0];

                        break;
                    }

                }
             
                dataGridView1_SelectionChanged(this, null);
                
            }
            catch //(Exception ex)
            {
                              
                fieldcnnametxt.Text = null;
                descriptiontxt.Text = null;
                catname_comboBox.Text = null;
                listfield_comboBox.Text = null;
                keyfield_comboBox.Text = null;
                listtable_comboBox.Text = null;
                listconditiontxt.Text = null;
                editortype_comboBox.SelectedIndex = 0;
                //this.visible_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
                this.visible_checkBox.Checked = true;
                //this.readonly_checkBox.CheckState = System.Windows.Forms.CheckState.Unchecked;
                this.readonly_checkBox.Checked = false;
            }
           
        }

        private void MetainfoEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Visible = false;
            log.Log(TimeLog.enumLogType.ltend, null, null, null);
        }

        public void GotoField(string strTableName, string strFieldName)
        {
            this.tablename_comboBox.SelectedText = strTableName;
            foreach (System.Data.DataRowView item in this.tablename_comboBox.Items)
            {
                string text = item.Row.ItemArray[0].ToString();
                if (text == strTableName)
                {
                    tablename_comboBox.SelectedItem = item;
                    tablename_comboBox.SelectedText = strFieldName;
                    foreach (string feilditem in this.fieldname_comboBox.Items)
                    {
                        if (feilditem.ToLower() == strFieldName.ToLower())
                            fieldname_comboBox.SelectedItem = feilditem;
                    }
                }
            }          
        }

        private void MetainfoEditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}