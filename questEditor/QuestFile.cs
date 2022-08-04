// QuestFile class reads a text file and lists QuestData objects
// and also generate strings from the list of QuestData objects and write them to a file
using System;
using System.Collections.Generic;
using System.IO;

namespace questEditor
{
    public class QuestFile
    {
        private static readonly string string_start = "a,";
        private static readonly string string_end = "\\0";
        public static List<QuestData> OpenFile(string path) //take a path to questname.txt anв return list of QuestData objects
        {
            List<QuestData> quest_list = new List<QuestData>();
            string s;
            StreamReader f = new StreamReader(path);
            while (!f.EndOfStream)
            {
                s = f.ReadLine(); // read a line from questname.txt
                if (s.Contains("tag_?")) //if its first line with file structure, skeap it
                {
                    continue;
                }
                quest_list.Add(new QuestData(s));//create new oject QuestaData object and add it in list
            }
            f.Close();

            return quest_list;
        }

        public static bool SaveFile(List<QuestData> quest_list, string path)//take a list of QuestData objects and make questname.txt
        {
            bool result = true;
            string main_name;
            string prog_name;
            string description;
            string entity_name;
            string restrictions;
            string short_description;
            string start_s = "tag_?\tquest_id\tquest_prog\tmain_name\tprog_name\tdescription\tcnt1\titems[0]\titems[1]\titems[2]\titems[3]" +
                "\titems[4]\titems[5]\titems[6]\titems[7]\titems[8]\titems[9]\titems[10]\titems[11]\titems[12]\titems[13]\tcnt2\tnum_items[0]" +
                "\tnum_items[1]\tnum_items[2]\tnum_items[3]\tnum_items[4]\tnum_items[5]\tnum_items[6]\tnum_items[7]\tnum_items[8]\tnum_items[9]" +
                "\tnum_items[10]\tnum_items[11]\tnum_items[12]\tnum_items[13]\tquest_x\tquest_y\tquest_z\tlvl_min\tlvl_max\tquest_type\tentity_name" +
                "\tget_item_in_quest\tUNK_1\tUNK_2\tcontact_npc_id\tcontact_npc_x\tcontact_npc_y\tcontact_npc_z\trestricions\tshort_description\tcnt3" +
                "\treq_class[0]\treq_class[1]\treq_class[2]\treq_class[3]\treq_class[4]\treq_class[5]\treq_class[6]\treq_class[7]\treq_class[8]" +
                "\treq_class[9]\treq_class[10]\treq_class[11]\treq_class[12]\treq_class[13]\treq_class[14]\treq_class[15]\treq_class[16]\treq_class[17]" +
                "\treq_class[18]\treq_class[19]\treq_class[20]\treq_class[21]\treq_class[22]\treq_class[23]\treq_class[24]\treq_class[25]\treq_class[26]" +
                "\treq_class[27]\treq_class[28]\treq_class[29]\treq_class[30]\treq_class[31]\treq_class[32]\treq_class[33]\treq_class[34]\treq_class[35]" +
                "\treq_class[36]\treq_class[37]\treq_class[38]\treq_class[39]\treq_class[40]\treq_class[41]\treq_class[42]\treq_class[43]\treq_class[44]" +
                "\treq_class[45]\treq_class[46]\treq_class[47]\treq_class[48]\treq_class[49]\treq_class[50]\treq_class[51]\treq_class[52]\treq_class[53]" +
                "\treq_class[54]\treq_class[55]\treq_class[56]\treq_class[57]\treq_class[58]\treq_class[59]\treq_class[60]\treq_class[61]\treq_class[62]" +
                "\treq_class[63]\treq_class[64]\treq_class[65]\treq_class[66]\treq_class[67]\treq_class[68]\tcnt4\treq_item[0]\treq_item[1]\treq_item[2]" +
                "\treq_item[3]\treq_item[4]\tclan_pet_quest\treq_quest_complete\tUNK_3\tarea_id\tUNK_4\tcnt5\ttab5[0]\ttab5[1]\ttab5[2]\ttab5[3]\ttab5[4]" +
                "\ttab5[5]\ttab5[6]\ttab5[7]\ttab5[8]\ttab5[9]\ttab5[10]\tcnt6\ttab6[0]\ttab6[1]\ttab6[2]\ttab6[3]\ttab6[4]\ttab6[5]\ttab6[6]\ttab6[7]\ttab6[8]" +
                "\ttab6[9]\ttab6[10]\tcnt7\ttab7[0]\ttab7[1]\ttab7[2]\ttab7[3]"; //first string with file structure
            string s;
            try
            {
                StreamWriter f = new StreamWriter(path);
                f.WriteLine(start_s); //write first string
                foreach (var quest in quest_list)
                {
                    main_name = string.IsNullOrEmpty(quest.main_name) ? string_start + quest.main_name : string_start + quest.main_name + string_end; //make a start "a," and end "\0" symbols
                    prog_name = string.IsNullOrEmpty(quest.prog_name) ? string_start + quest.prog_name : string_start + quest.prog_name + string_end; //make a start "a," and end "\0" symbols
                    description = string.IsNullOrEmpty(quest.description)
                        ? string_start + quest.description
                        : quest.description.StartsWith("u,") ? quest.description + string_end : string_start + quest.description + string_end; //make a start "a," and end "\0" symbols and check for string start from "u,"
                    entity_name = string.IsNullOrEmpty(quest.entity_name) ? string_start + quest.entity_name : string_start + quest.entity_name + string_end; //make a start "a," and end "\0" symbols
                    restrictions = string.IsNullOrEmpty(quest.restricions) ? string_start + quest.restricions : string_start + quest.restricions + string_end; //make a start "a," and end "\0" symbols
                    short_description = string.IsNullOrEmpty(quest.short_description)
                        ? string_start + quest.short_description
                        : quest.short_description.StartsWith("u,") ? quest.short_description + string_end : string_start + quest.short_description + string_end; //make a start "a," and end "\0" symbols and check for string start from "u,"

                    s = quest.tag + '\t' + quest.quest_id + '\t' + quest.quest_prog + '\t' + main_name + '\t' + prog_name
                         + '\t' + description + '\t' + quest.cnt1 + '\t' + quest.items[0] + '\t' + quest.items[1] + '\t' + quest.items[2]
                         + '\t' + quest.items[3] + '\t' + quest.items[4] + '\t' + quest.items[5] + '\t' + quest.items[6] + '\t' + quest.items[7]
                         + '\t' + quest.items[8] + '\t' + quest.items[9] + '\t' + quest.items[10] + '\t' + quest.items[11] + '\t' + quest.items[12]
                         + '\t' + quest.items[13] + '\t' + quest.cnt2 + '\t' + quest.num_items[0] + '\t' + quest.num_items[1] + '\t' + quest.num_items[2]
                         + '\t' + quest.num_items[3] + '\t' + quest.num_items[4] + '\t' + quest.num_items[5] + '\t' + quest.num_items[6] + '\t' + quest.num_items[7]
                         + '\t' + quest.num_items[8] + '\t' + quest.num_items[9] + '\t' + quest.num_items[10] + '\t' + quest.num_items[11] + '\t' +
                         quest.num_items[12] + '\t' + quest.num_items[13] + '\t' + quest.quest_x + '\t' + quest.quest_y + '\t' + quest.quest_z + '\t' +
                         quest.lvl_min + '\t' + quest.lvl_max + '\t' + quest.quest_type + '\t' + entity_name + '\t' + quest.get_item_in_quest + '\t' +
                         quest.UNK_1 + '\t' + quest.UNK_2 + '\t' + quest.contact_npc_id + '\t' + quest.contact_npc_x + '\t' + quest.contact_npc_y + '\t' +
                         quest.contact_npc_z + '\t' + restrictions + '\t' + short_description + '\t' + quest.cnt3 + '\t' + quest.req_class[0] + '\t' +
                         quest.req_class[1] + '\t' + quest.req_class[2] + '\t' + quest.req_class[3] + '\t' + quest.req_class[4] + '\t' + quest.req_class[5] + '\t'
                         + quest.req_class[6] + '\t' + quest.req_class[7] + '\t' + quest.req_class[8] + '\t' + quest.req_class[9] + '\t' + quest.req_class[10] + '\t'
                         + quest.req_class[11] + '\t' + quest.req_class[12] + '\t' + quest.req_class[13] + '\t' + quest.req_class[14] + '\t' + quest.req_class[15] + '\t'
                         + quest.req_class[16] + '\t' + quest.req_class[17] + '\t' + quest.req_class[18] + '\t' + quest.req_class[19] + '\t' + quest.req_class[20] + '\t'
                         + quest.req_class[21] + '\t' + quest.req_class[22] + '\t' + quest.req_class[23] + '\t' + quest.req_class[24] + '\t' + quest.req_class[25] + '\t'
                         + quest.req_class[26] + '\t' + quest.req_class[27] + '\t' + quest.req_class[28] + '\t' + quest.req_class[29] + '\t' + quest.req_class[30] + '\t'
                         + quest.req_class[31] + '\t' + quest.req_class[32] + '\t' + quest.req_class[33] + '\t' + quest.req_class[34] + '\t' + quest.req_class[35] + '\t'
                         + quest.req_class[36] + '\t' + quest.req_class[37] + '\t' + quest.req_class[38] + '\t' + quest.req_class[39] + '\t' + quest.req_class[40] + '\t'
                         + quest.req_class[41] + '\t' + quest.req_class[42] + '\t' + quest.req_class[43] + '\t' + quest.req_class[44] + '\t' + quest.req_class[45] + '\t'
                         + quest.req_class[46] + '\t' + quest.req_class[47] + '\t' + quest.req_class[48] + '\t' + quest.req_class[49] + '\t' + quest.req_class[50] + '\t'
                         + quest.req_class[51] + '\t' + quest.req_class[52] + '\t' + quest.req_class[53] + '\t' + quest.req_class[54] + '\t' + quest.req_class[55] + '\t'
                         + quest.req_class[56] + '\t' + quest.req_class[57] + '\t' + quest.req_class[58] + '\t' + quest.req_class[59] + '\t' + quest.req_class[60] + '\t'
                         + quest.req_class[61] + '\t' + quest.req_class[62] + '\t' + quest.req_class[63] + '\t' + quest.req_class[64] + '\t' + quest.req_class[65] + '\t'
                         + quest.req_class[66] + '\t' + quest.req_class[67] + '\t' + quest.req_class[68] + '\t' + quest.cnt4 + '\t' + quest.req_item[0] + '\t' + quest.req_item[1] + '\t' 
                         + quest.req_item[2] + '\t' + quest.req_item[3] + '\t' + quest.req_item[4] + '\t' + quest.clan_pet_quest + '\t' + quest.req_quest_complete
                         + '\t' + quest.UNK_3 + '\t' + quest.area_id + '\t' + quest.UNK_4 + '\t' + quest.cnt5 + '\t' + quest.tab5[0] + '\t' + quest.tab5[1] + '\t' + quest.tab5[2]
                         + '\t' + quest.tab5[3] + '\t' + quest.tab5[4] + '\t' + quest.tab5[5] + '\t' + quest.tab5[6] + '\t' + quest.tab5[7] + '\t' + quest.tab5[8] + '\t' + quest.tab5[9]
                         + '\t' + quest.tab5[10] + '\t' + quest.cnt6 + '\t' + quest.tab6[0] + '\t' + quest.tab6[1] + '\t' + quest.tab6[2] + '\t' + quest.tab6[3] + '\t' + quest.tab6[4] + '\t' + quest.tab6[5]
                         + '\t' + quest.tab6[6] + '\t' + quest.tab6[7] + '\t' + quest.tab6[8] + '\t' + quest.tab6[9] + '\t' + quest.tab6[10] + '\t' + quest.cnt7 + '\t' + quest.tab7[0] + '\t' + quest.tab7[1]
                         + '\t' + quest.tab7[2] + '\t' + quest.tab7[3]; //generate string
                    f.WriteLine(s); // write in file questname.txt
                }
                f.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                result = false;
                return result;
            }
            return result;
        }
    }
}