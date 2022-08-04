// Class QuestData it's a class represent questname-e.txt string structure.

namespace questEditor
{
    public class QuestData
    {
        private readonly char[] charsToTrim = { 'a', ',' }; //chars[] for string.TrimStart
        public QuestData(string line)
        {
            string[] questLine = line.Split('\t'); //split the line

            tag = questLine[0];
            quest_id = questLine[1];
            quest_prog = questLine[2];
            if (questLine[3].Contains("\\0"))
            {
                questLine[3] = questLine[3].Remove(questLine[3].Length - 2, 2); //remove \0 symbol from end of string
                main_name = questLine[3].TrimStart(charsToTrim); //remove a, symbol from start of string
            }
            if (questLine[4].Contains("\\0"))
            {
                questLine[4] = questLine[4].Remove(questLine[4].Length - 2, 2); //remove \0 symbol from end of string
                prog_name = questLine[4].TrimStart(charsToTrim); //remove a, symbol from start of string
            }
            if (questLine[5].Contains("\\0"))
            {
                questLine[5] = questLine[5].Remove(questLine[5].Length - 2, 2); //remove \0 symbol from end of string
                description = questLine[5].TrimStart(charsToTrim); //remove a, symbol from start of string
            }
            cnt1 = questLine[6];
            items[0] = questLine[7];
            items[1] = questLine[8];
            items[2] = questLine[9];
            items[3] = questLine[10];
            items[4] = questLine[11];
            items[5] = questLine[12];
            items[6] = questLine[13];
            items[7] = questLine[14];
            items[8] = questLine[15];
            items[9] = questLine[16];
            items[10] = questLine[17];
            items[11] = questLine[18];
            items[12] = questLine[19];
            items[13] = questLine[20];
            cnt2 = questLine[21];
            num_items[0] = questLine[22];
            num_items[1] = questLine[23];
            num_items[2] = questLine[24];
            num_items[3] = questLine[25];
            num_items[4] = questLine[26];
            num_items[5] = questLine[27];
            num_items[6] = questLine[28];
            num_items[7] = questLine[29];
            num_items[8] = questLine[30];
            num_items[9] = questLine[31];
            num_items[10] = questLine[32];
            num_items[11] = questLine[33];
            num_items[12] = questLine[34];
            num_items[13] = questLine[35];
            quest_x = questLine[36];
            quest_y = questLine[37];
            quest_z = questLine[38];
            lvl_min = questLine[39];
            lvl_max = questLine[40];
            quest_type = questLine[41];
            if (questLine[42].Contains("\\0"))
            {
                questLine[42] = questLine[42].Remove(questLine[42].Length - 2, 2); //remove \0 symbol from end of string
                entity_name = questLine[42].TrimStart(charsToTrim); //remove a, symbol from start of string
            }
            get_item_in_quest = questLine[43];
            UNK_1 = questLine[44];
            UNK_2 = questLine[45];
            contact_npc_id = questLine[46];
            contact_npc_x = questLine[47];
            contact_npc_y = questLine[48];
            contact_npc_z = questLine[49];
            if (questLine[50].Contains("\\0"))
            {
                questLine[50] = questLine[50].Remove(questLine[50].Length - 2, 2); //remove \0 symbol from end of string
                restricions = questLine[50].TrimStart(charsToTrim); //remove a, symbol from start of string
            }
            if (questLine[51].Contains("\\0"))
            {
                questLine[51] = questLine[51].Remove(questLine[51].Length - 2, 2); //remove \0 symbol from end of string
                short_description = questLine[51].TrimStart(charsToTrim); //remove a, symbol from start of string
            };
            cnt3 = questLine[52];
            req_class[0] = questLine[53];
            req_class[1] = questLine[54];
            req_class[2] = questLine[55];
            req_class[3] = questLine[56];
            req_class[4] = questLine[57];
            req_class[5] = questLine[58];
            req_class[6] = questLine[59];
            req_class[7] = questLine[60];
            req_class[8] = questLine[61];
            req_class[9] = questLine[62];
            req_class[10] = questLine[63];
            req_class[11] = questLine[64];
            req_class[12] = questLine[65];
            req_class[13] = questLine[66];
            req_class[14] = questLine[67];
            req_class[15] = questLine[68];
            req_class[16] = questLine[69];
            req_class[17] = questLine[70];
            req_class[18] = questLine[71];
            req_class[19] = questLine[72];
            req_class[20] = questLine[73];
            req_class[21] = questLine[74];
            req_class[22] = questLine[75];
            req_class[23] = questLine[76];
            req_class[24] = questLine[77];
            req_class[25] = questLine[78];
            req_class[26] = questLine[79];
            req_class[27] = questLine[80];
            req_class[28] = questLine[81];
            req_class[29] = questLine[82];
            req_class[30] = questLine[83];
            req_class[31] = questLine[84];
            req_class[32] = questLine[85];
            req_class[33] = questLine[86];
            req_class[34] = questLine[87];
            req_class[35] = questLine[88];
            req_class[36] = questLine[89];
            req_class[37] = questLine[90];
            req_class[38] = questLine[91];
            req_class[39] = questLine[92];
            req_class[40] = questLine[93];
            req_class[41] = questLine[94];
            req_class[42] = questLine[95];
            req_class[43] = questLine[96];
            req_class[44] = questLine[97];
            req_class[45] = questLine[98];
            req_class[46] = questLine[99];
            req_class[47] = questLine[100];
            req_class[48] = questLine[101];
            req_class[49] = questLine[102];
            req_class[50] = questLine[103];
            req_class[51] = questLine[104];
            req_class[52] = questLine[105];
            req_class[53] = questLine[106];
            req_class[54] = questLine[107];
            req_class[55] = questLine[108];
            req_class[56] = questLine[109];
            req_class[57] = questLine[110];
            req_class[58] = questLine[111];
            req_class[59] = questLine[112];
            req_class[60] = questLine[113];
            req_class[61] = questLine[114];
            req_class[62] = questLine[115];
            req_class[63] = questLine[116];
            req_class[64] = questLine[117];
            req_class[65] = questLine[118];
            req_class[66] = questLine[119];
            req_class[67] = questLine[120];
            req_class[68] = questLine[121];
            cnt4 = questLine[122];
            req_item[0] = questLine[123];
            req_item[1] = questLine[124];
            req_item[2] = questLine[125];
            req_item[3] = questLine[126];
            req_item[4] = questLine[127];
            clan_pet_quest = questLine[128];
            req_quest_complete = questLine[129];
            UNK_3 = questLine[130];
            area_id = questLine[131];
            UNK_4 = questLine[132];
            cnt5 = questLine[133];
            tab5[0] = questLine[134];
            tab5[1] = questLine[135];
            tab5[2] = questLine[136];
            tab5[3] = questLine[137];
            tab5[4] = questLine[138];
            tab5[5] = questLine[139];
            tab5[6] = questLine[140];
            tab5[7] = questLine[141];
            tab5[8] = questLine[142];
            tab5[9] = questLine[143];
            tab5[10] = questLine[144];
            cnt6 = questLine[145];
            tab6[0] = questLine[146];
            tab6[1] = questLine[147];
            tab6[2] = questLine[148];
            tab6[3] = questLine[149];
            tab6[4] = questLine[150];
            tab6[5] = questLine[151];
            tab6[6] = questLine[152];
            tab6[7] = questLine[153];
            tab6[8] = questLine[154];
            tab6[9] = questLine[155];
            tab6[10] = questLine[156];
            cnt7 = questLine[157];
            tab7[0] = questLine[158];
            tab7[1] = questLine[159];
            tab7[2] = questLine[160];
            tab7[3] = questLine[161];
        }

        public string tag;
        public string quest_id { get; set; }
        public string quest_prog { get; set; }
        public string main_name { get; set; }
        public string prog_name;
        public string description;
        public string cnt1;
        public string[] items = new string[14];
        public string cnt2;
        public string[] num_items = new string[14];
        public string quest_x;
        public string quest_y;
        public string quest_z;
        public string lvl_min;
        public string lvl_max;
        public string quest_type;
        public string entity_name;
        public string get_item_in_quest;
        public string UNK_1;
        public string UNK_2;
        public string contact_npc_id;
        public string contact_npc_x;
        public string contact_npc_y;
        public string contact_npc_z;
        public string restricions;
        public string short_description;
        public string cnt3;
        public string[] req_class = new string[69];
        public string cnt4;
        public string[] req_item = new string[5];
        public string clan_pet_quest;
        public string req_quest_complete;
        public string UNK_3;
        public string area_id;
        public string UNK_4;
        public string cnt5;
        public string[] tab5 = new string[11];
        public string cnt6;
        public string[] tab6 = new string[11];
        public string cnt7;
        public string[] tab7 = new string[4];
    }
}
