using System;਍ഀ
using System.Collections.Generic;਍ഀ
using System.Text;਍ഀ
using System.Data.SqlClient;਍ഀ
using System.Data;਍ഀ
using System.IO;਍ഀ
using System.Collections;਍ഀ
਍ഀ
namespace NpcUiTypeExport਍ഀ
{਍ഀ
    public class Class1਍ഀ
    {਍ഀ
        public Class1()਍ഀ
        {਍ഀ
਍ഀ
        }਍ഀ
਍ഀ
        public bool Export(SqlConnection Conn, string RootDir)਍ഀ
        {਍ഀ
            StringBuilder sb = new StringBuilder();਍ഀ
            sb.Append("TemplateID\tTypeID\r\n");਍ഀ
਍ഀ
            string sql = "select id,name,title,shopoptiontext,craftmasterid,masterid,hasbank,hasmailbox from npctemplate where (shopoptiontext is not null and shopoptiontext!='0') or craftmasterid!=0 or masterid!=0 or hasbank!=0 or hasmailbox!=0 or title like '%车夫%' or title like '%船夫%' or title like '%·守卫%' order by id";਍ഀ
            DataTable tbl = Helper.GetDataTable(sql, Conn);਍ഀ
            ਍ഀ
            string strGuard = "·守卫";਍ഀ
਍ഀ
            foreach (DataRow row in tbl.Rows)਍ഀ
            {਍ഀ
                int npctype = 0;਍ഀ
਍ഀ
                if (row["title"].ToString().Contains("车夫") || row["title"].ToString().Contains("船夫"))਍ഀ
                {਍ഀ
                    npctype = 17;਍ഀ
                }਍ഀ
                else if (!IsLogicallyNull(row["CraftMasterID"]))਍ഀ
                {਍ഀ
                    npctype = 13;਍ഀ
                }਍ഀ
                else if (!IsLogicallyNull(row["MasterID"]))਍ഀ
                {਍ഀ
                    npctype = 14;਍ഀ
                }਍ഀ
                else if (!IsLogicallyNull(row["HasBank"]))਍ഀ
                {਍ഀ
                    npctype = 15;਍ഀ
                }਍ഀ
                else if (!IsLogicallyNull(row["HasMailBox"]))਍ഀ
                {਍ഀ
                    npctype = 16;਍ഀ
                }਍ഀ
                else if (row["title"].ToString().Contains(strGuard))਍ഀ
                {਍ഀ
                    if (row["title"].ToString().Contains(strGuard))਍ഀ
                        npctype = 18;਍ഀ
                }਍ഀ
                else if (!IsLogicallyNull(row["ShopOptionText"]))਍ഀ
                {਍ഀ
                    npctype = GetShopTypeByShopOptionText(row["ShopOptionText"].ToString().Trim());਍ഀ
                }਍ഀ
਍ഀ
                if (npctype != 0)਍ഀ
                {਍ഀ
                    int npctemplateid = Convert.ToInt32(row["ID"]);਍ഀ
                    sb.Append(npctemplateid.ToString());਍ഀ
                    sb.Append("\t");਍ഀ
                    sb.Append(npctype.ToString());਍ഀ
                    sb.Append("\r\n");਍ഀ
                }਍ഀ
            }਍ഀ
਍ഀ
            // 写文件਍ഀ
            string targetFileName = RootDir + "/ui/scheme/case/npc.txt";਍ഀ
            Helper.StringToFile(sb.ToString(), targetFileName, Encoding.GetEncoding("gb2312"));਍ഀ
਍ഀ
            return true;਍ഀ
        }਍ഀ
਍ഀ
        private int GetShopTypeByShopOptionText(string shopoptiontext)਍ഀ
        {਍ഀ
            if (shopoptiontext.Contains("武器店"))਍ഀ
                return 1;਍ഀ
            else if (shopoptiontext.Contains("਍앧鞈≞⤀⤀ഀ
਍                爀攀琀甀爀渀 ㈀㬀ഀ
਍            攀氀猀攀 椀昀 ⠀猀栀漀瀀漀瀀琀椀漀渀琀攀砀琀⸀䌀漀渀琀愀椀渀猀⠀∀瀀솙靔≞⤀⤀ഀ
਍                爀攀琀甀爀渀 ㌀㬀ഀ
਍            攀氀猀攀 椀昀 ⠀猀栀漀瀀漀瀀琀椀漀渀琀攀砀琀⸀䌀漀渀琀愀椀渀猀⠀∀䈀❧鞍≞⤀⤀ഀ
਍                爀攀琀甀爀渀 㐀㬀ഀ
਍            攀氀猀攀 椀昀 ⠀猀栀漀瀀漀瀀琀椀漀渀琀攀砀琀⸀䌀漀渀琀愀椀渀猀⠀∀漀솃靔≞⤀⤀ഀ
਍                爀攀琀甀爀渀 㔀㬀ഀ
਍            攀氀猀攀 椀昀 ⠀猀栀漀瀀漀瀀琀椀漀渀琀攀砀琀⸀䌀漀渀琀愀椀渀猀⠀∀찀ր靓≞⤀⤀ഀ
਍                爀攀琀甀爀渀 ㄀　㬀ഀ
਍            攀氀猀攀 椀昀 ⠀猀栀漀瀀漀瀀琀椀漀渀琀攀砀琀⸀䌀漀渀琀愀椀渀猀⠀∀�솘靔≞⤀⤀ഀ
਍                爀攀琀甀爀渀 㘀㬀ഀ
਍            攀氀猀攀 椀昀 ⠀猀栀漀瀀漀瀀琀椀漀渀琀攀砀琀⸀䌀漀渀琀愀椀渀猀⠀∀Ԁﮖ傋饧魥鑏非≞⤀⤀ഀ
਍                爀攀琀甀爀渀 㜀㬀ഀ
਍            攀氀猀攀 椀昀 ⠀猀栀漀瀀漀瀀琀椀漀渀琀攀砀琀⸀䌀漀渀琀愀椀渀猀⠀∀倀饧魥鑏非≞⤀ 簀簀 ⠀猀栀漀瀀漀瀀琀椀漀渀琀攀砀琀⸀䌀漀渀琀愀椀渀猀⠀∀豈橰⊙⤀ ☀☀ 猀栀漀瀀漀瀀琀椀漀渀琀攀砀琀⸀䌀漀渀琀愀椀渀猀⠀∀需≞⤀⤀⤀ഀ
਍                爀攀琀甀爀渀 ㄀㈀㬀ഀ
਍            攀氀猀攀 椀昀 ⠀猀栀漀瀀漀瀀琀椀漀渀琀攀砀琀⸀䌀漀渀琀愀椀渀猀⠀∀氀ꦚ≓⤀⤀ഀ
਍                爀攀琀甀爀渀 㠀㬀ഀ
਍            攀氀猀攀 椀昀 ⠀猀栀漀瀀漀瀀琀椀漀渀琀攀砀琀⸀䌀漀渀琀愀椀渀猀⠀∀ ॺ楧셲靔≞⤀⤀ഀ
਍                爀攀琀甀爀渀 㤀㬀ഀ
਍            攀氀猀攀 椀昀 ⠀猀栀漀瀀漀瀀琀椀漀渀琀攀砀琀⸀䌀漀渀琀愀椀渀猀⠀∀椀䑲颍≛⤀⤀ഀ
਍                爀攀琀甀爀渀 ㄀㄀㬀ഀ
਍ഀ
਍ഀ
਍            爀攀琀甀爀渀 　㬀ഀ
਍        紀ഀ
਍ഀ
਍        瀀爀椀瘀愀琀攀 戀漀漀氀 䤀猀䰀漀最椀挀愀氀氀礀一甀氀氀⠀漀戀樀攀挀琀 䐀戀䘀椀攀氀搀⤀ഀ
਍        笀ഀ
਍            椀昀 ⠀䐀戀䘀椀攀氀搀 㴀㴀 渀甀氀氀 簀簀 䐀戀䘀椀攀氀搀 㴀㴀 䐀䈀一甀氀氀⸀嘀愀氀甀攀 簀簀 䐀戀䘀椀攀氀搀⸀吀漀匀琀爀椀渀最⠀⤀⸀吀爀椀洀⠀⤀ 㴀㴀 猀琀爀椀渀最⸀䔀洀瀀琀礀 簀簀 䐀戀䘀椀攀氀搀⸀吀漀匀琀爀椀渀最⠀⤀⸀吀爀椀洀⠀⤀ 㴀㴀 ∀　∀ 簀簀 䐀戀䘀椀攀氀搀⸀吀漀匀琀爀椀渀最⠀⤀⸀吀爀椀洀⠀⤀ 㴀㴀 ∀䘀愀氀猀攀∀⤀ ⼀⼀ ꑎ㪋⽎穦葺灶湥ൣ
਍                爀攀琀甀爀渀 琀爀甀攀㬀ഀ
਍            攀氀猀攀ഀ
਍                爀攀琀甀爀渀 昀愀氀猀攀㬀ഀ
਍        紀ഀ
਍    紀ഀ
਍紀ഀ
਍�