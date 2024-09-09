using System.Data;
using Ede.Uof.WKF.ExternalUtility;
using System.Xml.Linq;

namespace LYV.Supplier.PO
{
    internal class SupplierPO : Ede.Uof.Utility.Data.BasePersistentObject
    {
        internal void InsertTaskData(Ede.Uof.WKF.ExternalUtility.ApplyTask applyTask, string USERID)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            XElement XElement = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Form"].FieldValue);

            string LYV = applyTask.FormNumber;
            string Type = XElement.Attribute("Type").Value;
            string SupplierID = XElement.Attribute("SupplierID").Value;
            string SupplierName = XElement.Attribute("SupplierName").Value;
            string CompanyAddress = XElement.Attribute("CompanyAddress").Value;
            string FactoryAddress = XElement.Attribute("FactoryAddress").Value;
            string Product = XElement.Attribute("Product").Value;
            string Established = XElement.Attribute("Established").Value;
            string License = XElement.Attribute("License").Value;
            string PersonInCharge = XElement.Attribute("PersonInCharge").Value;
            string ContactPerson = XElement.Attribute("ContactPerson").Value;
            string Tel = XElement.Attribute("Tel").Value;
            string Fax = XElement.Attribute("Fax").Value;
            string Email = XElement.Attribute("Email").Value;
            string Designated = XElement.Attribute("Designated").Value;
            string Cooperated = XElement.Attribute("Cooperated").Value;
            string C_Qualified = XElement.Attribute("C_Qualified").Value;
            string New = XElement.Attribute("New").Value;
            string N_Qualified = XElement.Attribute("N_Qualified").Value;
            string Price = XElement.Attribute("Price").Value;
            string Effective = XElement.Attribute("Effective").Value;
            string Remark = XElement.Attribute("Remark").Value;

            string cmdTxt = @"INSERT INTO [dbo].[LYV_Supplier]  
            (	        
            [LYV] , 
	        [Type] , 
	        [SupplierID] , 
	        [SupplierName],
            [CompanyAddress],
            [FactoryAddress],
            [Product],
            [Established],
            [License],
            [PersonInCharge],
            [ContactPerson],
            [Tel],
            [Fax],
            [Email],
            [Designated],
            [Cooperated],
            [C_Qualified],
            [New],
            [N_Qualified],
            [Price],
            [Effective],
            [Remark],
            [flowflag],
            [UserID],
            [UserDate]
            )   
                VALUES 
            (
            @LYV , 
	        @Type , 
	        @SupplierID , 
	        @SupplierName,  
	        @CompanyAddress , 
	        @FactoryAddress , 
	        @Product , 
	        @Established,  
	        @License , 
	        @PersonInCharge , 
	        @ContactPerson , 
	        @Tel,  
            @Fax , 
            @Email,
           " + (string.IsNullOrEmpty(Designated) ? "NULL" : "@Designated") + @",
           " + (string.IsNullOrEmpty(Cooperated) ? "NULL" : "@Cooperated") + @",
           " + (string.IsNullOrEmpty(C_Qualified) ? "NULL" : "@C_Qualified") + @",
           " + (string.IsNullOrEmpty(New) ? "NULL" : "@New") + @",
           " + (string.IsNullOrEmpty(N_Qualified) ? "NULL" : "@N_Qualified") + @",
           " + (string.IsNullOrEmpty(Price) ? "NULL" : "@Price") + @",
           " + (string.IsNullOrEmpty(Effective) ? "NULL" : "@Effective") + @",
            @Remark,
	        'N',  
            @USERID , 
	        GETDATE()
            )";

            this.m_db.AddParameter("@LYV", LYV);
            this.m_db.AddParameter("@Type", Type);
            this.m_db.AddParameter("@SupplierID", SupplierID);
            this.m_db.AddParameter("@SupplierName", SupplierName);
            this.m_db.AddParameter("@CompanyAddress", CompanyAddress);
            this.m_db.AddParameter("@FactoryAddress", FactoryAddress);
            this.m_db.AddParameter("@Product", Product);
            this.m_db.AddParameter("@Established", Established);
            this.m_db.AddParameter("@License", License);
            this.m_db.AddParameter("@PersonInCharge", PersonInCharge);
            this.m_db.AddParameter("@ContactPerson", ContactPerson);
            this.m_db.AddParameter("@Tel", Tel);
            this.m_db.AddParameter("@Fax", Fax);
            this.m_db.AddParameter("@Email", Email);
            this.m_db.AddParameter("@Designated", Designated);
            this.m_db.AddParameter("@Cooperated", Cooperated);
            this.m_db.AddParameter("@C_Qualified", C_Qualified);
            this.m_db.AddParameter("@New", New);
            this.m_db.AddParameter("@N_Qualified", N_Qualified);
            this.m_db.AddParameter("@Price", Price);
            this.m_db.AddParameter("@Effective", Effective);
            this.m_db.AddParameter("@Remark", Remark);
            this.m_db.AddParameter("@USERID", USERID);
            this.m_db.ExecuteNonQuery(cmdTxt);
            this.m_db.Dispose();
        }
        internal void UpdateFormStatus(ApplyTask applyTask)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string LYV = applyTask.FormNumber;
            string SiteCode = applyTask.SiteCode;
            string signStatus = applyTask.FormResult.ToString();
            string cmdflowflag = @"SELECT flowflag FROM LYV_Supplier WHERE LYV = @LYV";
            DataTable dt = new DataTable();
            this.m_db.AddParameter("@LYV", LYV);
            dt.Load(this.m_db.ExecuteReader(cmdflowflag));

            string flowflag = dt.Rows[0][0].ToString();
            if ((flowflag == "NP" || flowflag == "N") && SiteCode != "ReturnToApplicant")
            {
                XElement XElement = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Form"].FieldValue);
                string Type = XElement.Attribute("Type").Value;
                string SupplierID = XElement.Attribute("SupplierID").Value;
                string SupplierName = XElement.Attribute("SupplierName").Value;
                string CompanyAddress = XElement.Attribute("CompanyAddress").Value;
                string FactoryAddress = XElement.Attribute("FactoryAddress").Value;
                string Product = XElement.Attribute("Product").Value;
                string Established = XElement.Attribute("Established").Value;
                string License = XElement.Attribute("License").Value;
                string PersonInCharge = XElement.Attribute("PersonInCharge").Value;
                string ContactPerson = XElement.Attribute("ContactPerson").Value;
                string Tel = XElement.Attribute("Tel").Value;
                string Fax = XElement.Attribute("Fax").Value;
                string Email = XElement.Attribute("Email").Value;
                string Designated = XElement.Attribute("Designated").Value;
                string Cooperated = XElement.Attribute("Cooperated").Value;
                string C_Qualified = XElement.Attribute("C_Qualified").Value;
                string New = XElement.Attribute("New").Value;
                string N_Qualified = XElement.Attribute("N_Qualified").Value;
                string Price = XElement.Attribute("Price").Value;
                string Effective = XElement.Attribute("Effective").Value;
                string Remark = XElement.Attribute("Remark").Value;

                string cmdTxt = @"  UPDATE [dbo].[LYV_Supplier] SET 
	            [Type] = @Type, 
	            [SupplierID] = @SupplierID, 
	            [SupplierName] = @SupplierName,
                [CompanyAddress] = @CompanyAddress,
                [FactoryAddress] = @FactoryAddress,
                [Product] = @Product,
                [Established] = @Established,
                [License] = @License,
                [PersonInCharge] = @PersonInCharge,
                [ContactPerson] = @ContactPerson,
                [Tel] = @Tel,
                [Fax] = @Fax,
                [Email] = @Email,
                [Designated] =  " + (string.IsNullOrEmpty(Designated) ? "NULL" : "@Designated") + @",
                [Cooperated] =" + (string.IsNullOrEmpty(Cooperated) ? "NULL" : "@Cooperated") + @",
                [C_Qualified] = " + (string.IsNullOrEmpty(C_Qualified) ? "NULL" : "@C_Qualified") + @",
                [New] = " + (string.IsNullOrEmpty(New) ? "NULL" : "@New") + @",
                [N_Qualified] = " + (string.IsNullOrEmpty(N_Qualified) ? "NULL" : "@N_Qualified") + @",
                [Price] = " + (string.IsNullOrEmpty(Price) ? "NULL" : "@Price") + @",
                [Effective] =" + (string.IsNullOrEmpty(Effective) ? "NULL" : "@Effective") + @",
                [Remark] = @Remark,
                [flowflag] = 'N'";

                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.AddParameter("@Type", Type);
                this.m_db.AddParameter("@SupplierID", SupplierID);
                this.m_db.AddParameter("@SupplierName", SupplierName);
                this.m_db.AddParameter("@CompanyAddress", CompanyAddress);
                this.m_db.AddParameter("@FactoryAddress", FactoryAddress);
                this.m_db.AddParameter("@Product", Product);
                this.m_db.AddParameter("@Established", Established);
                this.m_db.AddParameter("@License", License);
                this.m_db.AddParameter("@PersonInCharge", PersonInCharge);
                this.m_db.AddParameter("@ContactPerson", ContactPerson);
                this.m_db.AddParameter("@Tel", Tel);
                this.m_db.AddParameter("@Fax", Fax);
                this.m_db.AddParameter("@Email", Email);
                this.m_db.AddParameter("@Designated", Designated);
                this.m_db.AddParameter("@Cooperated", Cooperated);
                this.m_db.AddParameter("@C_Qualified", C_Qualified);
                this.m_db.AddParameter("@New", New);
                this.m_db.AddParameter("@N_Qualified", N_Qualified);
                this.m_db.AddParameter("@Price", Price);
                this.m_db.AddParameter("@Effective", Effective);
                this.m_db.AddParameter("@Remark", Remark);

                this.m_db.ExecuteNonQuery(cmdTxt);

                if (!string.IsNullOrEmpty(SiteCode))
                {
                    string cmdTxt1 = "UPDATE LYV_Supplier SET flowflag = 'P' WHERE LYV = @LYV ";
                    this.m_db.AddParameter("@LYV", LYV);
                    this.m_db.ExecuteNonQuery(cmdTxt1);
                }
            }
            if (SiteCode == "ReturnToApplicant")
            {
                string cmdTxt = "UPDATE LYV_Supplier SET flowflag = 'NP' WHERE LYV = @LYV ";
                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else if (SiteCode == "SE" && signStatus == "Approve")
            {
                string cmdTxt = @"UPDATE LYV_Supplier SET flowflag='Z' WHERE LYV = @LYV AND flowflag IN ('N','P')";
                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else if (signStatus == "Disapprove")
            {
                string cmdTxt = @"UPDATE LYV_Supplier SET flowflag='X' WHERE LYV = @LYV";
                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            this.m_db.Dispose();
        }
        internal void UpdateFormResult(string LYV, string signStatus)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            if (signStatus == "Adopt")
            {
                string cmdTxt = @"UPDATE LYV_Supplier SET flowflag='Z' WHERE LYV = @LYV AND flowflag IN ('N','P')";
                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else if (signStatus == "Reject" || signStatus == "Cancel")
            {
                string cmdTxt = @"UPDATE LYV_Supplier SET flowflag='X' WHERE LYV = @LYV";
                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            this.m_db.Dispose();
        }
        internal DataTable getWSSignNextInfo(string docNbr, string UserGUID)
        {
            DataTable dt = new DataTable();
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmdTxt = @"SELECT TB_WKF_TASK.TASK_ID, TB_WKF_TASK_NODE.SITE_ID, TB_WKF_TASK_NODE.NODE_SEQ, TB_WKF_TASK_NODE.ORIGINAL_SIGNER
                            FROM TB_WKF_TASK INNER JOIN TB_WKF_TASK_NODE ON TB_WKF_TASK.TASK_ID = TB_WKF_TASK_NODE.TASK_ID
                            WHERE DOC_NBR=@DOC_NBR AND TB_WKF_TASK_NODE.ORIGINAL_SIGNER=@ORIGINAL_SIGNER AND TB_WKF_TASK_NODE.FINISH_TIME IS NULL";
            m_db.AddParameter("@DOC_NBR", docNbr);
            m_db.AddParameter("@ORIGINAL_SIGNER", UserGUID);

            dt.Load(m_db.ExecuteReader(cmdTxt));

            this.m_db.Dispose();

            return dt;
        }
        internal DataTable GetListSUP(string Type, string SupplierID, string SupplierName)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string where = " and flowflag = 'Z' ";
            if (Type != "ALL") where += " and LOWER(Type) like LOWER('%" + Type + "%') ";
            if (SupplierID != "") where += " and LOWER(SupplierID) like LOWER('%" + SupplierID + "%') ";
            if (SupplierName != "") where += " and LOWER(SupplierName) like LOWER('%" + SupplierName + "%') ";

            string SQL = @" SELECT LYV_Supplier.LYV, LYV_Supplier.Type, LYV_Supplier.SupplierID, LYV_Supplier.SupplierName, LYV_Supplier.CompanyAddress, LYV_Supplier.FactoryAddress, LYV_Supplier.Product, 
                                   convert(varchar,LYV_Supplier.Established,102) Established, LYV_Supplier.License, LYV_Supplier.PersonInCharge, LYV_Supplier.ContactPerson, LYV_Supplier.Tel, 
                                   LYV_Supplier.Fax, LYV_Supplier.Email, LYV_Supplier.UserID, convert(varchar,LYV_Supplier.UserDate,102) UserDate, TB_WKF_TASK.TASK_ID 
                            FROM LYV_Supplier LEFT JOIN TB_WKF_TASK on LYV_Supplier.LYV=TB_WKF_TASK.DOC_NBR 
                            WHERE 1=1 " + where + @" 
                            ORDER BY LYV_Supplier.LYV desc ";

            DataTable dt = new DataTable();
            dt.Load(this.m_db.ExecuteReader(SQL));

            this.m_db.Dispose();

            return dt;
        }
    }
}
