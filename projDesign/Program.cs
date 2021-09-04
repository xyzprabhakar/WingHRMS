using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using projContext;
using projContext.DB;

//using projContext;
//using projContext.DB;

namespace projDesign
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();


            // code to transfer xml ddata to tables

            //var tblmenumaster = new tbl_menu_master();

            //var tblrolmaster = new tbl_role_master();

            //here



            //XElement menu = XElement.Load("../projContext/XMLData/Menu.xml");

            //XElement role = XElement.Load("../projContext/XMLData/role.xml");

            //this
            //XElement formualemasterxml = XElement.Load("../projContext/XMLData/FormulaeMaster.xml");

            //IEnumerable<XElement> formulaelst = formualemasterxml.Elements();//contains array of all question elements from xml

            //IEnumerable<XElement> tblmenumasters = menu.Elements();//all key elements

            //IEnumerable<XElement> rolez = role.Elements();//all fx elements

            //foreach (var tblmenu in tblmenumasters)
            //{

            //    //    tblfxmstr.function_id = Convert.ToInt16(fxmst.Element("fid").Value);
            //    //    tblfxmstr.function_name = fxmst.Element("name").Value;
            //    //    tblfxmstr.display_name = fxmst.Element("displayname").Value;
            //    //    tblfxmstr.calling_function_name = fxmst.Element("CallingFxName").Value;
            //    //    tblfxmstr.description = fxmst.Element("description").Value;
            //    //    tblfxmstr.function_category = Convert.ToInt16(fxmst.Element("category").Value);
            //    //    tblfxmstr.computation_order = Convert.ToInt16(fxmst.Element("order").Value);
            //    //    //remove the underneath coment to add to tabl

            //    tblmenumaster.menu_id = Convert.ToInt16(tblmenu.Element("id").Value);
            //    tblmenumaster.menu_name = tblmenu.Element("name").Value;
            //    tblmenumaster.urll = tblmenu.Element("link").Value;
            //    tblmenumaster.IconUrl = tblmenu.Element("icon").Value;
            //    tblmenumaster.parent_menu_id = Convert.ToInt16(tblmenu.Element("parentid").Value);
            //    ctx.tbl_menu_master.Add(tblmenumaster);
            //    ctx.SaveChanges();

            //}


            //foreach (var cmp in compdet)
            //{
            //    //tblkymstr.key_id = Convert.ToInt16(ky.Element("myky").Value);
            //    //tblkymstr.key_name = ky.Element("name").Value;
            //    //tblkymstr.description = ky.Element("description").Value;
            //    //tblkymstr.display_name = ky.Element("displayname").Value;
            //    //tblkymstr.calling_function_name = ky.Element("CallingFxName").Value;
            //    //tblkymstr.type = Convert.ToInt16(ky.Element("type").Value);
            //    //tblkymstr.defaultvalue = ky.Element("defaultvalue").Value;
            //    //tblkymstr.is_active = 1;
            //    //tblkymstr.data_type = ky.Element("data_type").Value;
            //    //remove the underneath coment to add to table
            //    tblcompdet.component_id = Convert.ToInt16(cmp.Element("component_id").Value);
            //    tblcompdet.company_id = Convert.ToInt16(cmp.Element("company_id").Value);
            //    tblcompdet.salary_group_id = Convert.ToInt16(cmp.Element("salary_group_id").Value);
            //    tblcompdet.component_type = Convert.ToInt16(cmp.Element("component_type").Value);
            //    tblcompdet.is_salary_comp = Convert.ToByte(cmp.Element("is_salary_comp").Value);
            //    tblcompdet.is_tds_comp = Convert.ToByte(cmp.Element("is_tds_comp").Value);
            //    tblcompdet.is_data_entry_comp = Convert.ToByte(cmp.Element("is_data_entry_comp").Value);
            //    tblcompdet.payment_type = Convert.ToByte(cmp.Element("payment_type").Value);
            //    tblcompdet.formula = cmp.Element("formula").Value;
            //    tblcompdet.function_calling_order = Convert.ToInt16(cmp.Element("function_calling_order").Value);
            //    tblcompdet.is_user_interface = Convert.ToInt16(cmp.Element("is_user_interface").Value);
            //    //tblcompdet.sno = Convert.ToInt16(cmp.Element("sno").Value);
            //    ctx.tbl_component_property_details.Add(tblcompdet);
            //    ctx.SaveChanges();
            //}
            //remove comments to add data to tbl ques master
            //foreach (var roll in rolez)
            //{
            //    //remove comments to ad data to epa ques master
            //    //tblquesmaster.question_name = ques.Element("question_name").Value;
            //    //var qt = ques.Element("question_type").Value;
            //    //tblquesmaster.question_type = Convert.ToByte(qt);
            //    //var tid = ques.Element("tab_id").Value;
            //    //tblquesmaster.tab_id = Convert.ToInt16(tid);
            //    //var at = ques.Element("answer_type").Value;
            //    //tblquesmaster.answer_type = Convert.ToByte(at);
            //    //tblquesmaster.have_remarks = Convert.ToByte(ques.Element("have_remarks").Value);
            //    //tblquesmaster.question_id = Convert.ToInt16(ques.Element("Question").Attribute("id").Value);

            //    tblrolmaster.role_id = Convert.ToInt16(roll.Element("id").Value);
            //    tblrolmaster.role_name = roll.Element("name").Value;
            //    ctx.tbl_role_master.Add(tblrolmaster);
            //    ctx.SaveChanges();

            //}
            #region----------------Uncomment region code to update db component & Detail & REMEMBER TO COMMENT -  CreateWebHostBuilder(args).Build().Run(); -----------------
            //tbl_component_property_details tblcompdet = new tbl_component_property_details();
            //var tblcomp = new tbl_component_master();
            //var ctx = new Context();

            //XElement formualemasterxml = XElement.Load("../projContext/XMLData/FormulaeMaster.xml");
            //IEnumerable<XElement> formulaelst = formualemasterxml.Elements();//contains array of all question elements from xml

            //    foreach(var fx in formulaelst)
            //    {
            //        tblcomp.component_id = Convert.ToInt32(fx.Element("component_id").Value);
            //        tblcomp.component_name = fx.Element("component_name").Value;
            //        tblcomp.defaultvalue = fx.Element("defaultvalue").Value;
            //        tblcomp.is_system_key = Convert.ToByte(fx.Element("is_system_key").Value);
            //        tblcomp.property_details = fx.Element("component_name").Value;
            //        tblcomp.parentid = Convert.ToInt32(fx.Element("parentid").Value);
            //        tblcomp.System_function = fx.Element("System_function").Value;
            //        // table formuale
            //        //tblcompdet.company_id = 1;
            //        tblcompdet.sno = Convert.ToInt32(fx.Element("component_id").Value);
            //        tblcompdet.component_id = Convert.ToInt32(fx.Element("component_id").Value);
            //        tblcompdet.formula = fx.Element("formula").Value;                
            //        tblcompdet.function_calling_order = Convert.ToInt32(fx.Element("priority").Value);
            //        tblcompdet.is_tds_comp = 0;
            //        tblcompdet.is_salary_comp = 0;
            //        tblcompdet.is_data_entry_comp = 0;
            //        tblcompdet.is_payslip = 0;
            //        //ctx.tbl_component_master.Add(tblcomp);
            //        ctx.tbl_component_property_details.Add(tblcompdet);
            //        ctx.SaveChanges();
            //    }
            #endregion

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
