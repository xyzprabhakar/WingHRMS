using projContext.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace projContext
{
    public enum LoginType
    {
        SuperAdmin = 0, // For Super Admin
        Admin = 1, // For Company Admin
        Employer = 2, // Employer Like HR 
        Employee = 3 // Normal User
    }

    public enum EmployeeType
    {
        [Description("Temporary")]
        Temporary = 1,
        [Description("Probation")]
        Probation = 2,
        [Description("Confirmed")]
        Confirmend = 3,
        [Description("Contract")]
        Contract = 4,
        [Description("Confirmation Extended")]
        Confirmation_Extended = 5,
        [Description("Notice")]
        Notice = 10,
        [Description("FNF")]
        FNF = 99,
        //[Description("Terminate")]
        [Description("Separated")]
        Terminate = 100

    }
    public  class CommonClass
    {
        /// <summary>
        /// Get Current Year
        /// </summary>
        /// <returns></returns>
        public static string GetYear()
        {
            return Convert.ToString(DateTime.Now.Year);
        }

        #region Encryption/ Decryption
        /// <summary>
        /// Encrypting params here
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string EncryptString(string Password, string salt)
        {

            if (Password == null)
                return null;
            if (Password == "")
                return "";

            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(salt));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(Password);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(Results);
        }


        /// <summary>
        /// password generation - forgot password
        /// </summary>
        /// <param name="p"></param>
        /// <param name="_salt"></param>
        /// <param name="_password"></param>
        public static void GeneratePassword(string p, ref string _salt, ref string _password)
        {
            _salt = "";// CommonLibClass.FetchRandStr(3);
            _password = EncryptString(p, _salt);
        }


        /// <summary>
        /// Decrypting string here
        /// </summary>
        /// <param name="EncryptedString"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string DecryptString(string EncryptedString, string salt)
        {
            if (EncryptedString == null)
                return null;
            if (EncryptedString == "")
                return "";

            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(salt));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToDecrypt = Convert.FromBase64String(EncryptedString);

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }

        #endregion

        #region Password
        /// <summary>
        /// Get password to match entered pwd
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetPassword(string p, string s)
        {
            return EncryptString(p, s);
        }

        /// <summary>
        /// Password decryption- log in
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string DecryptPassword(string p, string s)
        {
            return DecryptString(p, s);
        }
        #endregion

        /// <summary>
        /// Login Type Or User Type
        /// </summary>
        

        /*-----set description attribute enum for location type
        created by: vibhav 
        Created on: 10 dec 2018
        */
        public enum LocationType
        {
            [Description("Warehouse")]
            Warehouse = 1,
            [Description("Head Office")]
            HeadOffice = 2,
            [Description("Branch")]
            Branch = 3,
            [Description("Plant")]
            Plant = 4,
            [Description("Virtual Branch")]
            VirtualBranch = 5

        }

        /*-----set description attribute enum for Employee type
        created by: vibhav 
        Created on: 19 dec 2018
        ---1 temporary,2 Probation,3 Confirmend, 4 Contract, 10 notice,99 FNF,(100 terminate     no entry coreposnding to 100   )
        */
        


        public enum MaritalStatus
        {
            [Description("Married")]
            Married = 1,
            [Description("Single")]
            Single = 2,
            [Description("Divorced")]
            Divorced = 2,
            [Description("Widowed")]
            Widowed = 2,
            [Description("Separated")]
            Separated = 2,
        }

        /*-----set description attribute enum for Frequency type
        created by: vibhav 
        Created on: 19 dec 2018
        ---1-monthly,2 Quaterly,3 Half yealry,4 yearly
        */
        public enum FrequencyType
        {
            [Description("Monthly")]
            Monthaly = 1,
            [Description("Quaterly")]
            Quaterly = 2,
            [Description("Half Yearly")]
            HalfYearly = 3,
            [Description("Yearly")]
            Yearly = 4,
            [Description("Mannually")]
            Mannually = 5
        }

        //deserialize data from json to class object
        public  T ToObjectFromJSON<T>(string jsonString)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            var memoryStream = new System.IO.MemoryStream(System.Text.Encoding.Unicode.GetBytes(jsonString));
            var newObject = (T)serializer.ReadObject(memoryStream);
            memoryStream.Close();
            return newObject;
        }
    }

    public class Save_xml
    {
        public void Xml_save()
        {
            //var tblquesmaster = new tbl_epa_ques_mstr();
            //var tblfxmstr = new tbl_function_master();
            //tbl_key_master tblkymstr = new tbl_key_master();
            //var ctx = new Context();

            ////XElement epaques = XElement.Load("../XMLData/epaQuestionMaster.xml");
            //XElement payrollkeymstr = XElement.Load("../projContext/XMLData/KeyMstr.xml");
            //XElement payrollfxmstr = XElement.Load("../projContext/XMLData/PayrollFxMstr.xml");
            ////IEnumerable<XElement> questions = epaques.Elements();//contains array of all question elements from xml
            //IEnumerable<XElement> keymstr = payrollkeymstr.Elements();//all key elements
            //IEnumerable<XElement> fxmstr = payrollfxmstr.Elements();//all fx elements
            ////foreach (var fxmst in fxmstr)
            ////{
            ////    //tblfxmstr.function_id = Convert.ToInt16(fxmst.Element("function").Attribute("id").Value);
            ////    tblfxmstr.function_name = fxmst.Element("name").Value;
            ////    tblfxmstr.display_name = fxmst.Element("displayname").Value;
            ////    tblfxmstr.calling_function_name = fxmst.Element("CallingFxName").Value;
            ////    tblfxmstr.description = fxmst.Element("description").Value;
            ////    tblfxmstr.function_category = Convert.ToInt16(fxmst.Element("category").Value);
            ////    tblfxmstr.computation_order = Convert.ToInt16(fxmst.Element("order").Value);
            ////    //remove the underneath coment to add to table
            ////    ctx.tbl_function_master.Add(tblfxmstr);
            ////    ctx.SaveChanges();

            ////}
            //foreach (var ky in keymstr)
            //{
            //    //tblkymstr.key_id = Convert.ToInt16(ky.Element("KeyMstr").Attribute("id").Value);
            //    tblkymstr.key_name = ky.Element("name").Value;
            //    tblkymstr.display_name = ky.Element("displayname").Value;
            //    tblkymstr.calling_function_name = ky.Element("CallingFxName").Value;
            //    tblkymstr.type = Convert.ToInt16(ky.Element("type").Value);
            //    tblkymstr.defaultvalue = ky.Element("defaultvalue").Value;
            //    //remove the underneath coment to add to table
            //    ctx.tbl_key_master.Add(tblkymstr);
            //    //ctx.SaveChanges();
            //}
            ////remove comments to add data to tbl ques master
            ////foreach (var ques in questions)
            ////{
            ////    //remove comments to ad data to epa ques master
            ////    tblquesmaster.question_name = ques.Element("question_name").Value;
            ////    var qt = ques.Element("question_type").Value;
            ////    tblquesmaster.question_type = Convert.ToByte(qt);
            ////    var tid = ques.Element("tab_id").Value;
            ////    tblquesmaster.tab_id = Convert.ToInt16(tid);
            ////    var at = ques.Element("answer_type").Value;
            ////    tblquesmaster.answer_type = Convert.ToByte(at);
            ////    tblquesmaster.have_remarks = Convert.ToByte(ques.Element("have_remarks").Value);
            ////    tblquesmaster.question_id = Convert.ToInt16(ques.Element("Question").Attribute("id").Value);
            ////    ctx.tbl_epa_ques_mstr.Add(tblquesmaster);
            ////    ctx.SaveChanges();

            ////}
        }


    }

    #region  ******************************CREATED BY AMARJEET, CREATED DATE 29-04-2019, PAYROLL ***********************************

    //Enum Functions
    public enum enm_payroll_functions
    {
        [Description("ABS(x)"), DisplayName("ABS(x)")]
        ABS = 1,
        [Description("CEILING(x)"), DisplayName("CEILING(x)")]
        CEILING = 2,
        [Description("FLOOR(x)"), DisplayName("FLOOR(x)")]
        FLOOR = 3,
        [Description("GREATEST(a1,a2,...,an)"), DisplayName("GREATEST(a1,a2,...,an)")]
        GREATEST = 5,
        [Description("IF(exp,True,False)"), DisplayName("IF(exp,True,False)")]
        IF = 6,
        [Description("IIF(exp,True,False)"), DisplayName("IIF(exp,True,False)")]
        IIF = 7,
        [Description("LEAST(a1,a2,...,an)"), DisplayName("LEAST(a1,a2,...,an)")]
        LEAST = 8,
        [Description("MOD(x,y)"), DisplayName("MOD(x,y)")]
        MOD = 9,
        [Description("ROUND(exp,0)"), DisplayName("ROUND(exp,0)")]
        ROUND = 10,
        [Description("SQRT(x)"), DisplayName("SQRT(x)")]
        SQRT = 11,
        [Description("SUM()"), DisplayName("SUM()")]
        SUM = 12,
        [Description("NOT()"), DisplayName("NOT()")]
        NOT = 13,

    }

    //Enum Operator
    public enum enm_payroll_operator
    {

        [Description("^"), DisplayName("^")]
        Power = 1,
        [Description("*"), DisplayName("*")]
        Multiply = 2,
        [Description("/"), DisplayName("/")]
        Devide = 3,
        [Description("%"), DisplayName("%")]
        Mod = 4,
        [Description("+"), DisplayName("+")]
        Add = 5,
        [Description("-"), DisplayName("-")]
        Subtract = 6,
        [Description("<"), DisplayName("<")]
        Smlr_Thn = 7,
        [Description("<="), DisplayName("<=")]
        Smlr_Thn_Eql_To = 8,
        [Description(">"), DisplayName(">")]
        Grtr_Thn = 9,
        [Description(">="), DisplayName(">=")]
        Grtr_Thn_Eql_To = 10,
        [Description("=="), DisplayName("==")]
        Eql_To_Eql_to = 11,
        [Description("!="), DisplayName("!=")]
        Not_Eql_To = 12,
        [Description("&&"), DisplayName("AND")]
        AND = 13,
        [Description("||"), DisplayName("OR")]
        OR = 14,
        //[Description("!"), DisplayName("!")]
        //Not_Operator = 15,

    }
    #endregion

    #region ************************** Created by : Rajesh Yati on : 15-may-2019 ************************************************

    public enum enum_interest_rate
    {
        [Description("0"), DisplayName("0%")]
        a = 0,
        [Description("3"), DisplayName("3%")]
        b = 3,
        [Description("5"), DisplayName("5%")]
        c = 5,
        [Description("8"), DisplayName("8%")]
        d = 8,
        [Description("12"), DisplayName("12%")]
        e = 12,


    }

    #endregion


    #region ** STARTED BY SUPRIYA ON 24-06-2019 **
    public enum RelationType
    {
        [Description("Father")]
        Father=1,
        [Description("Mother")]
        Mother=2,
        [Description("Husband")]
        Husband=3,
        [Description("Wife")]
        Wife=4,
        [Description("Brother")]
        Brother=5,
        [Description("Sister")]
        Sister=6,
        [Description("Child")]
        Child = 7



    }
    #endregion ** END BY SUPRIYA ON 24-06-2019

    #region ** STARTED BY SUPRIYA ON 18-07-2019

    public enum ApproverType
    {
        [Description("Loan")]
        Loan = 1,

        [Description("Assets")]
        Assets = 2

    }

    public enum EducationLevel
    {
        // 1 Not Educated, 2 Primary Education, 3 Secondary, 4 Sr Secondary, 5 Graduation, 6 Post Graduation, 7 Doctorate 
        [Description("Not Educated")]
        Not_Educated=1,

        [Description("Primary Education")]
        Primary_Education=2,

        [Description("Secondary Education")]
        Secondary_Education=3,

        [Description("Sr. Secondary")]
        Senior_Secondary=4,

        [Description("Graduation")]
        Graduation=5,

        [Description("Post Graduation")]
        Post_Graduation=6,

        [Description("Doctorate")]
        Doctorate=7,

        [Description("Diploma")]
        Diploma=8,

        [Description("Certificate")]
        Certificate=9,
    }
    #endregion ** END BY SUPRIYA ON 18-07-2019
}
