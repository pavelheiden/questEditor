using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace questEditor
{
    public partial class MainWindow : Window
    {
        private List<QuestData> quest_list = new List<QuestData>();
        private QuestData selected_quest;
        private string questname_dat_path = Directory.GetCurrentDirectory() + "\\Tools\\mxencdec\\questname-e.dat";
        private string mxencdec_dir_path = Directory.GetCurrentDirectory() + "\\Tools\\mxencdec\\";
        private string questname_ddf_path = Directory.GetCurrentDirectory() + "\\Tools\\mxencdec\\l2asm-disasm\\dats\\questname-e.ddf";
        private string questname_new_ddf_path = Directory.GetCurrentDirectory() + "\\Tools\\mxencdec\\l2asm-disasm\\newdats\\questname-e-new.ddf";
        private string questname_bat_path = Directory.GetCurrentDirectory() + "\\Tools\\mxencdec\\questname-e.bat";
        private string l2asm_path = Directory.GetCurrentDirectory() + "\\Tools\\mxencdec\\l2asm-disasm\\l2asm.exe";
        private string l2disasm_path = Directory.GetCurrentDirectory() + "\\Tools\\mxencdec\\l2asm-disasm\\l2disasm.exe";
        private string decrypt_in;
        private string decrypt_out;
        private string encrypt_in;
        private string encrypt_out;


        public MainWindow()
        {
            InitializeComponent();

        }

        private void act_About_Click(object sender, RoutedEventArgs e)
        {
            _ = MessageBox.Show("Thank you for download it!\nUse with love.", "About");
        }

        private void act_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Text file(*.txt)|*.txt"
            };

            if ((bool)openFileDialog.ShowDialog())
            {
                try
                {
                    Stream checkStream;
                    if ((checkStream = openFileDialog.OpenFile()) != null)
                    {
                        quest_list = QuestFile.OpenFile(openFileDialog.FileName);
                        Reload_All();
                        _ = MessageBox.Show("File load successfully.", "Info");
                        act_Save.IsEnabled = true;
                        new_Quest_ID.IsEnabled = true;
                        new_Quest_Prog.IsEnabled = true;
                        new_Quest_btn.IsEnabled = true;
                        quest_Filter.IsEnabled = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _ = MessageBox.Show("Error: Could not read file from disk or file corrupted.", "Error");
                }

            }
            else
            {

                _ = MessageBox.Show("No questname.txt files selected.", "Info");

            }
        }

        private void act_Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Text file(*.txt)|*.txt"
            };

            if ((bool)saveFileDialog.ShowDialog())
            {
                try
                {
                    bool result = QuestFile.SaveFile(quest_list, saveFileDialog.FileName);
                    if (result)
                    {
                        _ = MessageBox.Show("File saved successfully.", "Info");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _ = MessageBox.Show("Error: Could not save file on disk.", "Error");
                }

            }
            else
            {

                _ = MessageBox.Show("Could not save file.", "Info");

            }
        }

        private void act_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void list_Quest_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Reload_Req_Items();
            Reload_Rew_Items();
            Reload_Req_Class();
            selected_quest = (QuestData)list_Quest_List.SelectedItem;
            if (selected_quest != null)
            {
                save_Quest_btn.IsEnabled = true;
                del_Quest_btn.IsEnabled = true;
                ed_Quest_Type_combo.IsEnabled = true;
                ed_Quest_Req_Item_combo.IsEnabled = true;
                ed_Quest_Rew_Item_combo.IsEnabled = true;

                ed_Quest_ID.Text = selected_quest.quest_id;
                ed_Quest_ID.IsEnabled = true;
                ed_Prog_ID.Text = selected_quest.quest_prog;
                ed_Prog_ID.IsEnabled = true;
                ed_Quest_Name.Text = selected_quest.main_name;
                ed_Quest_Name.IsEnabled = true;
                ed_Prog_Name.Text = selected_quest.prog_name;
                ed_Prog_Name.IsEnabled = true;
                ed_Min_Level.Text = selected_quest.lvl_min;
                ed_Min_Level.IsEnabled = true;
                ed_Max_Level.Text = selected_quest.lvl_max;
                ed_Max_Level.IsEnabled = true;
                ed_Restrictions.Text = selected_quest.restricions;
                ed_Restrictions.IsEnabled = true;
                ed_Description.Text = selected_quest.description;
                ed_Description.IsEnabled = true;
                ed_Short_Description.Text = selected_quest.short_description;
                ed_Short_Description.IsEnabled = true;
                ed_NPC_ID.Text = selected_quest.contact_npc_id;
                ed_NPC_ID.IsEnabled = true;
                switch (selected_quest.quest_type)
                {
                    case "2":
                        ed_Quest_Type_combo.SelectedItem = Repeatable;
                        break;

                    case "3":
                        ed_Quest_Type_combo.SelectedItem = Onetime;
                        break;
                }
                ed_Entity_Name.Text = selected_quest.entity_name;
                ed_Entity_Name.IsEnabled = true;
                ed_Quest_X.Text = selected_quest.quest_x;
                ed_Quest_X.IsEnabled = true;
                ed_Quest_Y.Text = selected_quest.quest_y;
                ed_Quest_Y.IsEnabled = true;
                ed_Quest_Z.Text = selected_quest.quest_z;
                ed_Quest_Z.IsEnabled = true;
                ed_NPC_X.Text = selected_quest.contact_npc_x;
                ed_NPC_X.IsEnabled = true;
                ed_NPC_Y.Text = selected_quest.contact_npc_y;
                ed_NPC_Y.IsEnabled = true;
                ed_NPC_Z.Text = selected_quest.contact_npc_z;
                ed_NPC_Z.IsEnabled = true;
                switch (selected_quest.cnt1)
                {
                    case "0":
                        ed_Quest_Req_Item_combo.SelectedItem = Zero;
                        break;
                    case "1":
                        ed_Quest_Req_Item_combo.SelectedItem = One;
                        ed_Req_Item1_ID_txt.Text = selected_quest.items[0];
                        ed_Req_Item1_cnt_txt.Text = selected_quest.num_items[0];
                        ed_Req_Item1_ID_txt.IsEnabled = true;
                        ed_Req_Item1_cnt_txt.IsEnabled = true;
                        break;
                    case "2":
                        ed_Quest_Req_Item_combo.SelectedItem = Two;
                        ed_Req_Item1_ID_txt.Text = selected_quest.items[0];
                        ed_Req_Item1_cnt_txt.Text = selected_quest.num_items[0];
                        ed_Req_Item1_ID_txt.IsEnabled = true;
                        ed_Req_Item1_cnt_txt.IsEnabled = true;
                        ed_Req_Item2_ID_txt.Text = selected_quest.items[1];
                        ed_Req_Item2_cnt_txt.Text = selected_quest.num_items[1];
                        ed_Req_Item2_ID_txt.IsEnabled = true;
                        ed_Req_Item2_cnt_txt.IsEnabled = true;
                        break;
                    case "3":
                        ed_Quest_Req_Item_combo.SelectedItem = Three;
                        ed_Req_Item1_ID_txt.Text = selected_quest.items[0];
                        ed_Req_Item1_cnt_txt.Text = selected_quest.num_items[0];
                        ed_Req_Item1_ID_txt.IsEnabled = true;
                        ed_Req_Item1_cnt_txt.IsEnabled = true;
                        ed_Req_Item2_ID_txt.Text = selected_quest.items[1];
                        ed_Req_Item2_cnt_txt.Text = selected_quest.num_items[1];
                        ed_Req_Item2_ID_txt.IsEnabled = true;
                        ed_Req_Item2_cnt_txt.IsEnabled = true;
                        ed_Req_Item3_ID_txt.Text = selected_quest.items[2];
                        ed_Req_Item3_cnt_txt.Text = selected_quest.num_items[2];
                        ed_Req_Item3_ID_txt.IsEnabled = true;
                        ed_Req_Item3_cnt_txt.IsEnabled = true;
                        break;
                    case "4":
                        ed_Quest_Req_Item_combo.SelectedItem = Four;
                        ed_Req_Item1_ID_txt.Text = selected_quest.items[0];
                        ed_Req_Item1_cnt_txt.Text = selected_quest.num_items[0];
                        ed_Req_Item1_ID_txt.IsEnabled = true;
                        ed_Req_Item1_cnt_txt.IsEnabled = true;
                        ed_Req_Item2_ID_txt.Text = selected_quest.items[1];
                        ed_Req_Item2_cnt_txt.Text = selected_quest.num_items[1];
                        ed_Req_Item2_ID_txt.IsEnabled = true;
                        ed_Req_Item2_cnt_txt.IsEnabled = true;
                        ed_Req_Item3_ID_txt.Text = selected_quest.items[2];
                        ed_Req_Item3_cnt_txt.Text = selected_quest.num_items[2];
                        ed_Req_Item3_ID_txt.IsEnabled = true;
                        ed_Req_Item3_cnt_txt.IsEnabled = true;
                        ed_Req_Item4_ID_txt.Text = selected_quest.items[3];
                        ed_Req_Item4_cnt_txt.Text = selected_quest.num_items[3];
                        ed_Req_Item4_ID_txt.IsEnabled = true;
                        ed_Req_Item4_cnt_txt.IsEnabled = true;
                        break;
                    case "5":
                        ed_Quest_Req_Item_combo.SelectedItem = Five;
                        ed_Req_Item1_ID_txt.Text = selected_quest.items[0];
                        ed_Req_Item1_cnt_txt.Text = selected_quest.num_items[0];
                        ed_Req_Item1_ID_txt.IsEnabled = true;
                        ed_Req_Item1_cnt_txt.IsEnabled = true;
                        ed_Req_Item2_ID_txt.Text = selected_quest.items[1];
                        ed_Req_Item2_cnt_txt.Text = selected_quest.num_items[1];
                        ed_Req_Item2_ID_txt.IsEnabled = true;
                        ed_Req_Item2_cnt_txt.IsEnabled = true;
                        ed_Req_Item3_ID_txt.Text = selected_quest.items[2];
                        ed_Req_Item3_cnt_txt.Text = selected_quest.num_items[2];
                        ed_Req_Item3_ID_txt.IsEnabled = true;
                        ed_Req_Item3_cnt_txt.IsEnabled = true;
                        ed_Req_Item4_ID_txt.Text = selected_quest.items[3];
                        ed_Req_Item4_cnt_txt.Text = selected_quest.num_items[3];
                        ed_Req_Item4_ID_txt.IsEnabled = true;
                        ed_Req_Item4_cnt_txt.IsEnabled = true;
                        ed_Req_Item5_ID_txt.Text = selected_quest.items[4];
                        ed_Req_Item5_cnt_txt.Text = selected_quest.num_items[4];
                        ed_Req_Item5_ID_txt.IsEnabled = true;
                        ed_Req_Item5_cnt_txt.IsEnabled = true;
                        break;
                }
                switch (selected_quest.cnt5)
                {
                    case "0":
                        ed_Quest_Rew_Item_combo.SelectedItem = Zero1;
                        break;
                    case "1":
                        ed_Quest_Rew_Item_combo.SelectedItem = One1;
                        ed_Rew_Item1_ID_txt.Text = selected_quest.tab5[0];
                        ed_Rew_Item1_cnt_txt.Text = selected_quest.tab6[0];
                        ed_Rew_Item1_ID_txt.IsEnabled = true;
                        ed_Rew_Item1_cnt_txt.IsEnabled = true;
                        break;
                    case "2":
                        ed_Quest_Rew_Item_combo.SelectedItem = Two1;
                        ed_Rew_Item1_ID_txt.Text = selected_quest.tab5[0];
                        ed_Rew_Item1_cnt_txt.Text = selected_quest.tab6[0];
                        ed_Rew_Item1_ID_txt.IsEnabled = true;
                        ed_Rew_Item1_cnt_txt.IsEnabled = true;
                        ed_Rew_Item2_ID_txt.Text = selected_quest.tab5[1];
                        ed_Rew_Item2_cnt_txt.Text = selected_quest.tab6[1];
                        ed_Rew_Item2_ID_txt.IsEnabled = true;
                        ed_Rew_Item2_cnt_txt.IsEnabled = true;
                        break;
                    case "3":
                        ed_Quest_Rew_Item_combo.SelectedItem = Three1;
                        ed_Rew_Item1_ID_txt.Text = selected_quest.tab5[0];
                        ed_Rew_Item1_cnt_txt.Text = selected_quest.tab6[0];
                        ed_Rew_Item1_ID_txt.IsEnabled = true;
                        ed_Rew_Item1_cnt_txt.IsEnabled = true;
                        ed_Rew_Item2_ID_txt.Text = selected_quest.tab5[1];
                        ed_Rew_Item2_cnt_txt.Text = selected_quest.tab6[1];
                        ed_Rew_Item2_ID_txt.IsEnabled = true;
                        ed_Rew_Item2_cnt_txt.IsEnabled = true;
                        ed_Rew_Item3_ID_txt.Text = selected_quest.tab5[2];
                        ed_Rew_Item3_cnt_txt.Text = selected_quest.tab6[2];
                        ed_Rew_Item3_ID_txt.IsEnabled = true;
                        ed_Rew_Item3_cnt_txt.IsEnabled = true;
                        break;
                    case "4":
                        ed_Quest_Rew_Item_combo.SelectedItem = Four1;
                        ed_Rew_Item1_ID_txt.Text = selected_quest.tab5[0];
                        ed_Rew_Item1_cnt_txt.Text = selected_quest.tab6[0];
                        ed_Rew_Item1_ID_txt.IsEnabled = true;
                        ed_Rew_Item1_cnt_txt.IsEnabled = true;
                        ed_Rew_Item2_ID_txt.Text = selected_quest.tab5[1];
                        ed_Rew_Item2_cnt_txt.Text = selected_quest.tab6[1];
                        ed_Rew_Item2_ID_txt.IsEnabled = true;
                        ed_Rew_Item2_cnt_txt.IsEnabled = true;
                        ed_Rew_Item3_ID_txt.Text = selected_quest.tab5[2];
                        ed_Rew_Item3_cnt_txt.Text = selected_quest.tab6[2];
                        ed_Rew_Item3_ID_txt.IsEnabled = true;
                        ed_Rew_Item3_cnt_txt.IsEnabled = true;
                        ed_Rew_Item4_ID_txt.Text = selected_quest.tab5[3];
                        ed_Rew_Item4_cnt_txt.Text = selected_quest.tab6[3];
                        ed_Rew_Item4_ID_txt.IsEnabled = true;
                        ed_Rew_Item4_cnt_txt.IsEnabled = true;
                        break;
                    case "5":
                        ed_Quest_Rew_Item_combo.SelectedItem = Five1;
                        ed_Rew_Item1_ID_txt.Text = selected_quest.tab5[0];
                        ed_Rew_Item1_cnt_txt.Text = selected_quest.tab6[0];
                        ed_Rew_Item1_ID_txt.IsEnabled = true;
                        ed_Rew_Item1_cnt_txt.IsEnabled = true;
                        ed_Rew_Item2_ID_txt.Text = selected_quest.tab5[1];
                        ed_Rew_Item2_cnt_txt.Text = selected_quest.tab6[1];
                        ed_Rew_Item2_ID_txt.IsEnabled = true;
                        ed_Rew_Item2_cnt_txt.IsEnabled = true;
                        ed_Rew_Item3_ID_txt.Text = selected_quest.tab5[2];
                        ed_Rew_Item3_cnt_txt.Text = selected_quest.tab6[2];
                        ed_Rew_Item3_ID_txt.IsEnabled = true;
                        ed_Rew_Item3_cnt_txt.IsEnabled = true;
                        ed_Rew_Item4_ID_txt.Text = selected_quest.tab5[3];
                        ed_Rew_Item4_cnt_txt.Text = selected_quest.tab6[3];
                        ed_Rew_Item4_ID_txt.IsEnabled = true;
                        ed_Rew_Item4_cnt_txt.IsEnabled = true;
                        ed_Rew_Item5_ID_txt.Text = selected_quest.tab5[4];
                        ed_Rew_Item5_cnt_txt.Text = selected_quest.tab6[4];
                        ed_Rew_Item5_ID_txt.IsEnabled = true;
                        ed_Rew_Item5_cnt_txt.IsEnabled = true;
                        break;
                }
                switch (selected_quest.cnt3)
                {
                    case "0":
                        ed_Quest_Req_Class_combo.SelectedItem = Zero2;
                        break;
                    case "1":
                        ed_Quest_Req_Class_combo.SelectedItem = One2;
                        ed_Req_Class1_ID_txt.Text = selected_quest.req_class[0];
                        ed_Req_Class1_ID_txt.IsEnabled = true;
                        break;
                    case "2":
                        ed_Quest_Req_Class_combo.SelectedItem = Two2;
                        ed_Req_Class1_ID_txt.Text = selected_quest.req_class[0];
                        ed_Req_Class1_ID_txt.IsEnabled = true;
                        ed_Req_Class2_ID_txt.Text = selected_quest.req_class[1];
                        ed_Req_Class2_ID_txt.IsEnabled = true;
                        break;
                    case "3":
                        ed_Quest_Req_Class_combo.SelectedItem = Three2;
                        ed_Req_Class1_ID_txt.Text = selected_quest.req_class[0];
                        ed_Req_Class1_ID_txt.IsEnabled = true;
                        ed_Req_Class2_ID_txt.Text = selected_quest.req_class[1];
                        ed_Req_Class2_ID_txt.IsEnabled = true;
                        ed_Req_Class3_ID_txt.Text = selected_quest.req_class[2];
                        ed_Req_Class3_ID_txt.IsEnabled = true;
                        break;
                    case "4":
                        ed_Quest_Req_Class_combo.SelectedItem = Four2;
                        ed_Req_Class1_ID_txt.Text = selected_quest.req_class[0];
                        ed_Req_Class1_ID_txt.IsEnabled = true;
                        ed_Req_Class2_ID_txt.Text = selected_quest.req_class[1];
                        ed_Req_Class2_ID_txt.IsEnabled = true;
                        ed_Req_Class3_ID_txt.Text = selected_quest.req_class[2];
                        ed_Req_Class3_ID_txt.IsEnabled = true;
                        ed_Req_Class4_ID_txt.Text = selected_quest.req_class[3];
                        ed_Req_Class4_ID_txt.IsEnabled = true;
                        break;
                    case "5":
                        ed_Quest_Req_Class_combo.SelectedItem = Five2;
                        ed_Req_Class1_ID_txt.Text = selected_quest.req_class[0];
                        ed_Req_Class1_ID_txt.IsEnabled = true;
                        ed_Req_Class2_ID_txt.Text = selected_quest.req_class[1];
                        ed_Req_Class2_ID_txt.IsEnabled = true;
                        ed_Req_Class3_ID_txt.Text = selected_quest.req_class[2];
                        ed_Req_Class3_ID_txt.IsEnabled = true;
                        ed_Req_Class4_ID_txt.Text = selected_quest.req_class[3];
                        ed_Req_Class4_ID_txt.IsEnabled = true;
                        ed_Req_Class5_ID_txt.Text = selected_quest.req_class[4];
                        ed_Req_Class5_ID_txt.IsEnabled = true;
                        break;
                }
                ed_Area_ID.Text = selected_quest.area_id;
                ed_Area_ID.IsEnabled = true;
                ed_Req_Quest_Complete.Text = selected_quest.req_quest_complete;
                ed_Req_Quest_Complete.IsEnabled = true;
                switch (selected_quest.get_item_in_quest)
                {
                    case "0":
                        cb_Quest_Get_Item.IsChecked = false;
                        cb_Quest_Get_Item.IsEnabled = true;
                        break;
                    case "1":
                        cb_Quest_Get_Item.IsChecked = true;
                        cb_Quest_Get_Item.IsEnabled = true;
                        break;
                }

            }
        }

        private void Reload_List()
        {
            list_Quest_List.ItemsSource = null;
            list_Quest_List.ItemsSource = quest_list;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(quest_list);
            view.Filter = QuestFilter;
        }

        private void new_Quest_btn_Click(object sender, RoutedEventArgs e)
        {
            string quest_id = new_Quest_ID.Text;
            string quest_prog = new_Quest_Prog.Text;
            if (quest_id != "" && quest_prog != "")
            {
                QuestData new_quest = new QuestData("1\t" + quest_id + "\t" + quest_prog + "\ta,New Quest Name\\0\ta,New Prog Name\\0\ta,New Descriprion\\0\t0\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t0\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t0.00000000\t0.00000000\t0.00000000\t0\t0\t3\ta,New Entity\\0\t0\t1\t1\t0\t0.00000000\t0.00000000\t0.00000000\ta,New Restriction\\0\ta,New Short Description.\\0\t0\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t0\t\t\t\t\t\t0\t0\t0\t0\t0\t0\t\t\t\t\t\t\t\t\t\t\t\t0\t\t\t\t\t\t\t\t\t\t\t\t1\t0\t\t\t");
                quest_list.Add(new_quest);
                Reload_List();
                list_Quest_List.SelectedItem = quest_list.Last();
                list_Quest_List.ScrollIntoView(list_Quest_List.SelectedItem);
                list_Quest_List.Focus();
            }
            else
            {
                _ = MessageBox.Show("Please input new quest ID and Prog!", "Info");
            }
        }

        private void del_Quest_btn_Click(object sender, RoutedEventArgs e)
        {
            if (list_Quest_List.SelectedItem != null)
            {
                quest_list.Remove((QuestData)list_Quest_List.SelectedItem);
                Reload_All();
                selected_quest = null;
                _ = MessageBox.Show("Quest deleted.", "Info");
            }
            else
            {
                _ = MessageBox.Show("Please select a quest!", "Info");
            }
        }

        private void save_Quest_btn_Click(object sender, RoutedEventArgs e)
        {
            selected_quest.quest_id = ed_Quest_ID.Text;
            selected_quest.quest_prog = ed_Prog_ID.Text;
            selected_quest.main_name = ed_Quest_Name.Text;
            selected_quest.prog_name = ed_Prog_Name.Text;
            selected_quest.lvl_min = ed_Min_Level.Text;
            selected_quest.lvl_max = ed_Max_Level.Text;
            selected_quest.restricions = ed_Restrictions.Text;
            selected_quest.description = ed_Description.Text;
            selected_quest.short_description = ed_Short_Description.Text;
            selected_quest.contact_npc_id = ed_NPC_ID.Text;
            selected_quest.contact_npc_x = ed_NPC_X.Text;
            selected_quest.contact_npc_y = ed_NPC_Y.Text;
            selected_quest.contact_npc_z = ed_NPC_Z.Text;
            selected_quest.quest_x = ed_Quest_X.Text;
            selected_quest.quest_y = ed_Quest_Y.Text;
            selected_quest.quest_z = ed_Quest_Z.Text;
            selected_quest.entity_name = ed_Entity_Name.Text;
            switch (selected_quest.cnt1)
            {
                case "1":
                    selected_quest.items[0] = ed_Req_Item1_ID_txt.Text;
                    selected_quest.num_items[0] = ed_Req_Item1_cnt_txt.Text;
                    break;
                case "2":
                    selected_quest.items[0] = ed_Req_Item1_ID_txt.Text;
                    selected_quest.num_items[0] = ed_Req_Item1_cnt_txt.Text;
                    selected_quest.items[1] = ed_Req_Item2_ID_txt.Text;
                    selected_quest.num_items[1] = ed_Req_Item2_cnt_txt.Text;
                    break;
                case "3":
                    selected_quest.items[0] = ed_Req_Item1_ID_txt.Text;
                    selected_quest.num_items[0] = ed_Req_Item1_cnt_txt.Text;
                    selected_quest.items[1] = ed_Req_Item2_ID_txt.Text;
                    selected_quest.num_items[1] = ed_Req_Item2_cnt_txt.Text;
                    selected_quest.items[2] = ed_Req_Item3_ID_txt.Text;
                    selected_quest.num_items[2] = ed_Req_Item3_cnt_txt.Text;
                    break;
                case "4":
                    selected_quest.items[0] = ed_Req_Item1_ID_txt.Text;
                    selected_quest.num_items[0] = ed_Req_Item1_cnt_txt.Text;
                    selected_quest.items[1] = ed_Req_Item2_ID_txt.Text;
                    selected_quest.num_items[1] = ed_Req_Item2_cnt_txt.Text;
                    selected_quest.items[2] = ed_Req_Item3_ID_txt.Text;
                    selected_quest.num_items[2] = ed_Req_Item3_cnt_txt.Text;
                    selected_quest.items[3] = ed_Req_Item4_ID_txt.Text;
                    selected_quest.num_items[3] = ed_Req_Item4_cnt_txt.Text;
                    break;
                case "5":
                    selected_quest.items[0] = ed_Req_Item1_ID_txt.Text;
                    selected_quest.num_items[0] = ed_Req_Item1_cnt_txt.Text;
                    selected_quest.items[1] = ed_Req_Item2_ID_txt.Text;
                    selected_quest.num_items[1] = ed_Req_Item2_cnt_txt.Text;
                    selected_quest.items[2] = ed_Req_Item3_ID_txt.Text;
                    selected_quest.num_items[2] = ed_Req_Item3_cnt_txt.Text;
                    selected_quest.items[3] = ed_Req_Item4_ID_txt.Text;
                    selected_quest.num_items[3] = ed_Req_Item4_cnt_txt.Text;
                    selected_quest.items[4] = ed_Req_Item5_ID_txt.Text;
                    selected_quest.num_items[4] = ed_Req_Item5_cnt_txt.Text;
                    break;
            }
            switch (selected_quest.cnt4)
            {
                case "1":
                    selected_quest.tab5[0] = ed_Rew_Item1_ID_txt.Text;
                    selected_quest.tab6[0] = ed_Rew_Item1_cnt_txt.Text;
                    break;
                case "2":
                    selected_quest.tab5[0] = ed_Rew_Item1_ID_txt.Text;
                    selected_quest.tab6[0] = ed_Rew_Item1_cnt_txt.Text;
                    selected_quest.tab5[1] = ed_Rew_Item2_ID_txt.Text;
                    selected_quest.tab6[1] = ed_Rew_Item2_cnt_txt.Text;
                    break;
                case "3":
                    selected_quest.tab5[0] = ed_Rew_Item1_ID_txt.Text;
                    selected_quest.tab6[0] = ed_Rew_Item1_cnt_txt.Text;
                    selected_quest.tab5[1] = ed_Rew_Item2_ID_txt.Text;
                    selected_quest.tab6[1] = ed_Rew_Item2_cnt_txt.Text;
                    selected_quest.tab5[2] = ed_Rew_Item3_ID_txt.Text;
                    selected_quest.tab6[2] = ed_Rew_Item3_cnt_txt.Text;
                    break;
                case "4":
                    selected_quest.tab5[0] = ed_Rew_Item1_ID_txt.Text;
                    selected_quest.tab6[0] = ed_Rew_Item1_cnt_txt.Text;
                    selected_quest.tab5[1] = ed_Rew_Item2_ID_txt.Text;
                    selected_quest.tab6[1] = ed_Rew_Item2_cnt_txt.Text;
                    selected_quest.tab5[2] = ed_Rew_Item3_ID_txt.Text;
                    selected_quest.tab6[2] = ed_Rew_Item3_cnt_txt.Text;
                    selected_quest.tab5[3] = ed_Rew_Item4_ID_txt.Text;
                    selected_quest.tab6[3] = ed_Rew_Item4_cnt_txt.Text;
                    break;
                case "5":
                    selected_quest.tab5[0] = ed_Rew_Item1_ID_txt.Text;
                    selected_quest.tab6[0] = ed_Rew_Item1_cnt_txt.Text;
                    selected_quest.tab5[1] = ed_Rew_Item2_ID_txt.Text;
                    selected_quest.tab6[1] = ed_Rew_Item2_cnt_txt.Text;
                    selected_quest.tab5[2] = ed_Rew_Item3_ID_txt.Text;
                    selected_quest.tab6[2] = ed_Rew_Item3_cnt_txt.Text;
                    selected_quest.tab5[3] = ed_Rew_Item4_ID_txt.Text;
                    selected_quest.tab6[3] = ed_Rew_Item4_cnt_txt.Text;
                    selected_quest.tab5[4] = ed_Rew_Item5_ID_txt.Text;
                    selected_quest.tab6[4] = ed_Rew_Item5_cnt_txt.Text;
                    break;
            }
            switch (selected_quest.cnt3)
            {
                case "1":
                    selected_quest.req_class[0] = ed_Req_Class1_ID_txt.Text;
                    break;
                case "2":
                    selected_quest.req_class[0] = ed_Req_Class1_ID_txt.Text;
                    selected_quest.req_class[1] = ed_Req_Class2_ID_txt.Text;
                    break;
                case "3":
                    selected_quest.req_class[0] = ed_Req_Class1_ID_txt.Text;
                    selected_quest.req_class[1] = ed_Req_Class2_ID_txt.Text;
                    selected_quest.req_class[2] = ed_Req_Class3_ID_txt.Text;
                    break;
                case "4":
                    selected_quest.req_class[0] = ed_Req_Class1_ID_txt.Text;
                    selected_quest.req_class[1] = ed_Req_Class2_ID_txt.Text;
                    selected_quest.req_class[2] = ed_Req_Class3_ID_txt.Text;
                    selected_quest.req_class[3] = ed_Req_Class4_ID_txt.Text;
                    break;
                case "5":
                    selected_quest.req_class[0] = ed_Req_Class1_ID_txt.Text;
                    selected_quest.req_class[1] = ed_Req_Class2_ID_txt.Text;
                    selected_quest.req_class[2] = ed_Req_Class3_ID_txt.Text;
                    selected_quest.req_class[3] = ed_Req_Class4_ID_txt.Text;
                    selected_quest.req_class[4] = ed_Req_Class5_ID_txt.Text;
                    break;
            }
            selected_quest.area_id = ed_Area_ID.Text;
            selected_quest.req_quest_complete = ed_Req_Quest_Complete.Text;
            selected_quest.cnt7 = "1";
            if (selected_quest.quest_prog != "-1")
            {
                selected_quest.tab7[0] = (int.Parse(selected_quest.quest_prog) - 1).ToString();
            }
            else
            {
                int count = 0;
                foreach (var quest in quest_list)
                {
                    if (quest.quest_id == selected_quest.quest_id)
                    {
                        count++;
                    }
                }
                selected_quest.tab7[0] = count.ToString();
            }
            Reload_List();
            _ = MessageBox.Show("Quest saved.", "Info");
        }

        private void quest_Popup_btn_Click(object sender, RoutedEventArgs e)
        {
            popup_Info.IsOpen = true;
        }

        private void ed_Quest_Type_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            if (selectedItem != null)
            {
                switch (selectedItem.Name)
                {
                    case "Onetime":
                        selected_quest.quest_type = "3";
                        break;
                    case "Repeatable":
                        selected_quest.quest_type = "2";
                        break;
                }
            }
        }

        private bool QuestFilter(object item)
        {
            if (String.IsNullOrEmpty(quest_Filter.Text))
                return true;
            else
                return ((item as QuestData).quest_id.IndexOf(quest_Filter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void quest_Filter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(list_Quest_List.ItemsSource).Refresh();
        }

        private void ed_Quest_Req_Item_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            if (selectedItem != null)
            {
                switch (selectedItem.Name)
                {
                    case "Zero":
                        selected_quest.cnt1 = "0";
                        selected_quest.cnt2 = "0";
                        Reload_Req_Items();
                        break;
                    case "One":
                        selected_quest.cnt1 = "1";
                        selected_quest.cnt2 = "1";
                        ed_Req_Item1_ID_txt.IsEnabled = true;
                        ed_Req_Item1_cnt_txt.IsEnabled = true;
                        break;
                    case "Two":
                        selected_quest.cnt1 = "2";
                        selected_quest.cnt2 = "2";
                        ed_Req_Item1_ID_txt.IsEnabled = true;
                        ed_Req_Item1_cnt_txt.IsEnabled = true;
                        ed_Req_Item2_ID_txt.IsEnabled = true;
                        ed_Req_Item2_cnt_txt.IsEnabled = true;
                        break;
                    case "Three":
                        selected_quest.cnt1 = "3";
                        selected_quest.cnt2 = "3";
                        ed_Req_Item1_ID_txt.IsEnabled = true;
                        ed_Req_Item1_cnt_txt.IsEnabled = true;
                        ed_Req_Item2_ID_txt.IsEnabled = true;
                        ed_Req_Item2_cnt_txt.IsEnabled = true;
                        ed_Req_Item3_ID_txt.IsEnabled = true;
                        ed_Req_Item3_cnt_txt.IsEnabled = true;
                        break;
                    case "Four":
                        selected_quest.cnt1 = "4";
                        selected_quest.cnt2 = "4";
                        ed_Req_Item1_ID_txt.IsEnabled = true;
                        ed_Req_Item1_cnt_txt.IsEnabled = true;
                        ed_Req_Item2_ID_txt.IsEnabled = true;
                        ed_Req_Item2_cnt_txt.IsEnabled = true;
                        ed_Req_Item3_ID_txt.IsEnabled = true;
                        ed_Req_Item3_cnt_txt.IsEnabled = true;
                        ed_Req_Item4_ID_txt.IsEnabled = true;
                        ed_Req_Item4_cnt_txt.IsEnabled = true;
                        break;
                    case "Five":
                        selected_quest.cnt1 = "5";
                        selected_quest.cnt2 = "5";
                        ed_Req_Item1_ID_txt.IsEnabled = true;
                        ed_Req_Item1_cnt_txt.IsEnabled = true;
                        ed_Req_Item2_ID_txt.IsEnabled = true;
                        ed_Req_Item2_cnt_txt.IsEnabled = true;
                        ed_Req_Item3_ID_txt.IsEnabled = true;
                        ed_Req_Item3_cnt_txt.IsEnabled = true;
                        ed_Req_Item4_ID_txt.IsEnabled = true;
                        ed_Req_Item4_cnt_txt.IsEnabled = true;
                        ed_Req_Item5_ID_txt.IsEnabled = true;
                        ed_Req_Item5_cnt_txt.IsEnabled = true;
                        break;
                }
            }
        }

        private void ed_Quest_Rew_Item_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            if(selectedItem != null)
            {
                switch (selectedItem.Name)
                {
                    case "Zero1":
                        selected_quest.cnt4 = "0";
                        selected_quest.cnt5 = "0";
                        Reload_Rew_Items();
                        break;
                    case "One1":
                        selected_quest.cnt4 = "1";
                        selected_quest.cnt5 = "1";
                        ed_Rew_Item1_ID_txt.IsEnabled = true;
                        ed_Rew_Item1_cnt_txt.IsEnabled = true;
                        break;
                    case "Two1":
                        selected_quest.cnt4 = "2";
                        selected_quest.cnt5 = "2";
                        ed_Rew_Item1_ID_txt.IsEnabled = true;
                        ed_Rew_Item1_cnt_txt.IsEnabled = true;
                        ed_Rew_Item2_ID_txt.IsEnabled = true;
                        ed_Rew_Item2_cnt_txt.IsEnabled = true;
                        break;
                    case "Three1":
                        selected_quest.cnt4 = "3";
                        selected_quest.cnt5 = "3";
                        ed_Rew_Item1_ID_txt.IsEnabled = true;
                        ed_Rew_Item1_cnt_txt.IsEnabled = true;
                        ed_Rew_Item2_ID_txt.IsEnabled = true;
                        ed_Rew_Item2_cnt_txt.IsEnabled = true;
                        ed_Rew_Item3_ID_txt.IsEnabled = true;
                        ed_Rew_Item3_cnt_txt.IsEnabled = true;
                        break;
                    case "Four1":
                        selected_quest.cnt4 = "4";
                        selected_quest.cnt5 = "4";
                        ed_Rew_Item1_ID_txt.IsEnabled = true;
                        ed_Rew_Item1_cnt_txt.IsEnabled = true;
                        ed_Rew_Item2_ID_txt.IsEnabled = true;
                        ed_Rew_Item2_cnt_txt.IsEnabled = true;
                        ed_Rew_Item3_ID_txt.IsEnabled = true;
                        ed_Rew_Item3_cnt_txt.IsEnabled = true;
                        ed_Rew_Item4_ID_txt.IsEnabled = true;
                        ed_Rew_Item4_cnt_txt.IsEnabled = true;
                        break;
                    case "Five1":
                        selected_quest.cnt4 = "5";
                        selected_quest.cnt5 = "5";
                        ed_Rew_Item1_ID_txt.IsEnabled = true;
                        ed_Rew_Item1_cnt_txt.IsEnabled = true;
                        ed_Rew_Item2_ID_txt.IsEnabled = true;
                        ed_Rew_Item2_cnt_txt.IsEnabled = true;
                        ed_Rew_Item3_ID_txt.IsEnabled = true;
                        ed_Rew_Item3_cnt_txt.IsEnabled = true;
                        ed_Rew_Item4_ID_txt.IsEnabled = true;
                        ed_Rew_Item4_cnt_txt.IsEnabled = true;
                        ed_Rew_Item5_ID_txt.IsEnabled = true;
                        ed_Rew_Item5_cnt_txt.IsEnabled = true;
                        break;
                }
            }
        }

        private void ed_Quest_Req_Class_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            if (selectedItem != null)
            {
                switch (selectedItem.Name)
                {
                    case "Zero2":
                        selected_quest.cnt3 = "0";
                        Reload_Req_Class();
                        break;
                    case "One2":
                        selected_quest.cnt3 = "1";
                        ed_Req_Class1_ID_txt.IsEnabled = true;
                        break;
                    case "Two2":
                        selected_quest.cnt3 = "2";
                        ed_Req_Class1_ID_txt.IsEnabled = true;
                        ed_Req_Class2_ID_txt.IsEnabled = true;
                        break;
                    case "Three2":
                        selected_quest.cnt3 = "3";
                        ed_Req_Class1_ID_txt.IsEnabled = true;
                        ed_Req_Class2_ID_txt.IsEnabled = true;
                        ed_Req_Class3_ID_txt.IsEnabled = true;
                        break;
                    case "Four2":
                        selected_quest.cnt3 = "4";
                        ed_Req_Class1_ID_txt.IsEnabled = true;
                        ed_Req_Class2_ID_txt.IsEnabled = true;
                        ed_Req_Class3_ID_txt.IsEnabled = true;
                        ed_Req_Class4_ID_txt.IsEnabled = true;
                        break;
                    case "Five2":
                        selected_quest.cnt3 = "5";
                        ed_Req_Class1_ID_txt.IsEnabled = true;
                        ed_Req_Class2_ID_txt.IsEnabled = true;
                        ed_Req_Class3_ID_txt.IsEnabled = true;
                        ed_Req_Class4_ID_txt.IsEnabled = true;
                        ed_Req_Class5_ID_txt.IsEnabled = true;
                        break;
                }
            }
        }

        private void Reload_Req_Items()
        {
            ed_Req_Item1_ID_txt.Clear();
            ed_Req_Item1_cnt_txt.Clear();
            ed_Req_Item2_ID_txt.Clear();
            ed_Req_Item2_cnt_txt.Clear();
            ed_Req_Item3_ID_txt.Clear();
            ed_Req_Item3_cnt_txt.Clear();
            ed_Req_Item4_ID_txt.Clear();
            ed_Req_Item4_cnt_txt.Clear();
            ed_Req_Item5_ID_txt.Clear();
            ed_Req_Item5_cnt_txt.Clear();
            ed_Req_Item1_ID_txt.IsEnabled = false;
            ed_Req_Item1_cnt_txt.IsEnabled = false;
            ed_Req_Item2_ID_txt.IsEnabled = false;
            ed_Req_Item2_cnt_txt.IsEnabled = false;
            ed_Req_Item3_ID_txt.IsEnabled = false;
            ed_Req_Item3_cnt_txt.IsEnabled = false;
            ed_Req_Item4_ID_txt.IsEnabled = false;
            ed_Req_Item4_cnt_txt.IsEnabled = false;
            ed_Req_Item5_ID_txt.IsEnabled = false;
            ed_Req_Item5_cnt_txt.IsEnabled = false;
        }

        private void Reload_Rew_Items()
        {
            ed_Rew_Item1_ID_txt.Clear();
            ed_Rew_Item1_cnt_txt.Clear();
            ed_Rew_Item2_ID_txt.Clear();
            ed_Rew_Item2_cnt_txt.Clear();
            ed_Rew_Item3_ID_txt.Clear();
            ed_Rew_Item3_cnt_txt.Clear();
            ed_Rew_Item4_ID_txt.Clear();
            ed_Rew_Item4_cnt_txt.Clear();
            ed_Rew_Item5_ID_txt.Clear();
            ed_Rew_Item5_cnt_txt.Clear();
            ed_Rew_Item1_ID_txt.IsEnabled = false;
            ed_Rew_Item1_cnt_txt.IsEnabled = false;
            ed_Rew_Item2_ID_txt.IsEnabled = false;
            ed_Rew_Item2_cnt_txt.IsEnabled = false;
            ed_Rew_Item3_ID_txt.IsEnabled = false;
            ed_Rew_Item3_cnt_txt.IsEnabled = false;
            ed_Rew_Item4_ID_txt.IsEnabled = false;
            ed_Rew_Item4_cnt_txt.IsEnabled = false;
            ed_Rew_Item5_ID_txt.IsEnabled = false;
            ed_Rew_Item5_cnt_txt.IsEnabled = false;
        }

        private void Reload_Req_Class()
        {
            ed_Req_Class1_ID_txt.Clear();
            ed_Req_Class2_ID_txt.Clear();
            ed_Req_Class3_ID_txt.Clear();
            ed_Req_Class4_ID_txt.Clear();
            ed_Req_Class5_ID_txt.Clear();
            ed_Req_Class1_ID_txt.IsEnabled = false;
            ed_Req_Class2_ID_txt.IsEnabled = false;
            ed_Req_Class3_ID_txt.IsEnabled = false;
            ed_Req_Class4_ID_txt.IsEnabled = false;
            ed_Req_Class5_ID_txt.IsEnabled = false;
        }

        private void Reload_All()
        {
            Reload_List();
            Reload_Req_Items();
            Reload_Rew_Items();
            Reload_Req_Class();
            ed_Description.Clear();
            ed_Description.IsEnabled = false;
            ed_Entity_Name.Clear();
            ed_Entity_Name.IsEnabled = false;
            ed_Max_Level.Clear();
            ed_Max_Level.IsEnabled = false;
            ed_Min_Level.Clear();
            ed_Min_Level.IsEnabled = false;
            ed_NPC_ID.Clear();
            ed_NPC_ID.IsEnabled = false;
            ed_NPC_X.Clear();
            ed_NPC_X.IsEnabled = false;
            ed_NPC_Y.Clear();
            ed_NPC_Y.IsEnabled = false;
            ed_NPC_Z.Clear();
            ed_NPC_Z.IsEnabled = false;
            ed_Prog_ID.Clear();
            ed_Prog_ID.IsEnabled = false;
            ed_Prog_Name.Clear();
            ed_Prog_Name.IsEnabled = false;
            ed_Quest_ID.Clear();
            ed_Quest_ID.IsEnabled = false;
            ed_Quest_Name.Clear();
            ed_Quest_Name.IsEnabled = false;
            ed_Quest_Req_Item_combo.SelectedItem = null;
            ed_Quest_Req_Item_combo.IsEnabled = false;
            ed_Quest_Rew_Item_combo.SelectedItem = null;
            ed_Quest_Rew_Item_combo.IsEnabled = false;
            ed_Quest_Type_combo.SelectedItem = null;
            ed_Quest_Type_combo.IsEnabled = false;
            ed_Quest_X.Clear();
            ed_Quest_X.IsEnabled = false;
            ed_Quest_Y.Clear();
            ed_Quest_Y.IsEnabled = false;
            ed_Quest_Z.Clear();
            ed_Quest_Z.IsEnabled = false;
            ed_Restrictions.Clear();
            ed_Restrictions.IsEnabled = false;
            ed_Short_Description.Clear();
            ed_Short_Description.IsEnabled = false;
            save_Quest_btn.IsEnabled = false;
            ed_Area_ID.Clear();
            ed_Area_ID.IsEnabled = false;
            ed_Req_Quest_Complete.Clear();
            ed_Req_Quest_Complete.IsEnabled = false;
            cb_Quest_Get_Item.IsChecked = false;
            cb_Quest_Get_Item.IsEnabled = false;
        }

        private void act_Decrypt_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "DAT file (*.dat)|*.dat"
            };

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Txt file (*.txt*)|*.txt"
            };

            if ((bool)openFileDialog.ShowDialog())
            {
                if (openFileDialog.FileName != string.Empty)
                {
                    decrypt_in = openFileDialog.FileName;
                    if ((bool)saveFileDialog.ShowDialog())
                    {
                        if (saveFileDialog.FileName != string.Empty)
                        {
                            decrypt_out = saveFileDialog.FileName;
                            File_Decrypt();
                        }
                        else
                        {
                            _ = MessageBox.Show("Output file not selected.", "Info");
                        }
                    }
                    else
                    {
                        _ = MessageBox.Show("Output file not selected.", "Info");
                    }
                }
                else
                {
                    _ = MessageBox.Show("Input file not selected.", "Info");
                }
            }
            else
            {
                _ = MessageBox.Show("Input file not selected.", "Info");
            }
        }

        private void act_Encrypt_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Txt file (*.txt*)|*.txt"
            };

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "DAT file (*.dat)|*.dat"
            };

            if ((bool)openFileDialog.ShowDialog())
            {
                if (openFileDialog.FileName != string.Empty)
                {
                    encrypt_in = openFileDialog.FileName;
                    if ((bool)saveFileDialog.ShowDialog())
                    {
                        if (saveFileDialog.FileName != string.Empty)
                        {
                            encrypt_out = saveFileDialog.FileName;
                            File_Encrypt();
                        }
                        else
                        {
                            _ = MessageBox.Show("Output file not selected.", "Info");
                        }
                    }
                    else
                    {
                        _ = MessageBox.Show("Output file not selected.", "Info");
                    }
                }
                else
                {
                    _ = MessageBox.Show("Input file not selected.", "Info");
                }
            }
            else
            {
                _ = MessageBox.Show("Input file not selected.", "Info");
            }
        }

        private void File_Decrypt()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoDecrypt;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += decrypt_Complete;

            pbStatus.IsIndeterminate = true;
            pb_Encrypt_text.Visibility = Visibility.Visible;
            pb_Encrypt_text.Text = "Decrypting...";
            worker.RunWorkerAsync();
        }

        private void File_Encrypt()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoEncrypt;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += encrypt_Complete;

            pbStatus.IsIndeterminate = true;
            pb_Encrypt_text.Visibility = Visibility.Visible;
            pb_Encrypt_text.Text = "Encrypting...";
            worker.RunWorkerAsync();
        }

        void worker_DoDecrypt(object sender, DoWorkEventArgs e)
        {
            try
            {
                File.Copy(decrypt_in, questname_dat_path, true);
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.WorkingDirectory = mxencdec_dir_path;
                    myProcess.StartInfo.FileName = questname_bat_path;
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                    myProcess.WaitForExit();
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.FileName = l2disasm_path;
                    myProcess.StartInfo.Arguments = "-d " + questname_ddf_path + " " +
                        "-e " + questname_new_ddf_path + " " + questname_dat_path + " " + decrypt_out;
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                    myProcess.WaitForExit();
                }
            }
            catch (Exception exx)
            {
                Console.WriteLine(exx.Message);
            }
        }

        void worker_DoEncrypt(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.FileName = l2asm_path;
                    myProcess.StartInfo.Arguments = "-d " + questname_new_ddf_path +
                        " " + encrypt_in + " " + questname_dat_path;
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                    myProcess.WaitForExit();
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.WorkingDirectory = mxencdec_dir_path;
                    myProcess.StartInfo.FileName = questname_bat_path;
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                    myProcess.WaitForExit();
                }
                File.Copy(questname_dat_path, encrypt_out, true);
            }
            catch (Exception exx)
            {
                Console.WriteLine(exx.Message);
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbStatus.Value = e.ProgressPercentage;
        }

        void decrypt_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            pb_Encrypt_text.Text = "";
            pb_Encrypt_text.Visibility = Visibility.Hidden;
            pbStatus.IsIndeterminate = false;
            if (e.Error != null)
            {
                Remove_Questname_File();
                _ = MessageBox.Show("Decryption failed.", "Error");
            }
            else
            {
                Remove_Questname_File();
                _ = MessageBox.Show("File decrypted successfully.", "Info");
            }
        }

        void encrypt_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            pb_Encrypt_text.Text = "";
            pb_Encrypt_text.Visibility = Visibility.Hidden;
            pbStatus.IsIndeterminate = false;
            if (e.Error != null)
            {
                Remove_Questname_File();
                _ = MessageBox.Show("Encryption failed.", "Error");
            }
            else
            {
                Remove_Questname_File();
                _ = MessageBox.Show("File encrypted successfully.", "Info");
            }
        }

        private void CheckForNumeric(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cb_Quest_Get_Item_Checked(object sender, RoutedEventArgs e)
        {
            switch (cb_Quest_Get_Item.IsChecked)
            {
                case false:
                    selected_quest.get_item_in_quest = "0";
                    break;
                case true:
                    selected_quest.get_item_in_quest = "1";
                    break;
            }
        }

        private void Remove_Questname_File()
        {
            if (File.Exists(questname_dat_path))
            {
                File.Delete(questname_dat_path);
            }
        }
    }
}