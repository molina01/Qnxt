using System.Collections.Generic;
using AutomationFramework.Helpers;
using NUnit.Framework;
using MolinaHealthCareApplicationPOM.ApplicationCommon.Common;
using static MolinaHealthCareApplicationPOM.ApplicationCommon.Common.Settings;
using System.Collections;
using System.Data;

namespace MolinaHealthcareApplication
{
    [TestFixture]
    public class DBQuery:HookInitailize
    {
        [SetUp]
        public override void TestSetUp()
        {
            //Read Url From App.Config
            if (ReportingHelpers.ExtentReportsInstance == null)
                return;
        }

        /// <summary>
        /// Close the Browser 
        /// </summary>
        [TearDown]
        public override void TestTearDown()
        {           

        }

        public static IEnumerable TestPage
        {
            get
            {
                TestInitializationHook.TestInitializeSettings(ApplicationType.DesktopAdmin);
                return EnvironmentReader.ReadDataFromDataSheet(string.Concat(Settings.DatasheetPath, "DDTF_AdminPortal.xlsx"), "Global", "TestDBQuery");
            }
        }

       [Test]
       [TestCaseSource(typeof(DBQuery), "TestPage")]
       public void TestDBQuery(Dictionary<string, string> InputData)
        {
            ReadInputData(InputData);
            TestInitializationHook.ExtentTestInstance = TestInitializationHook.ExtentReportsInstance.CreateTest(TestContext.CurrentContext.Test.MethodName + "_" + Settings.State,"Admin");
            LogHelpers.Write(" INFO :: Test Method : Execution Started TestDBQuery");
            Assert.AreNotEqual(null, Database.FetchDataFromDB(Settings.DBQuery.ToString(), "user_id"));
            LogHelpers.Write(" INFO :: Test Method : Execution completed TestDBQuery " + System.Environment.NewLine);
        }

        [Test]
        public void ReturnTableDBQuery()
        {
            Settings.DBQuery = "select top(1)  p.FedID,p.ProvID,p.NPi,En.FirstName,En.LastName from EPORTAL_PLANDATA..provider p inner Join EPORTAL_PLANDATA..Entity En on En.EntID = P.EntityID where p.Npi<>''and p.FedId<>'' and en.FirstName<>''and p.StateCode ='CA'";
            TestInitializationHook.ExtentTestInstance = TestInitializationHook.ExtentReportsInstance.CreateTest(TestContext.CurrentContext.Test.MethodName, "Admin");
            LogHelpers.Write(" INFO :: Test Method : Execution Started ReturnTableDBQuery");
            DataTable aDataTable = Database.FetchDataFromDB(Settings.DBQuery.ToString());
            Assert.AreNotEqual(null, aDataTable);
            string aFedID = aDataTable.Rows[0]["FedID"].ToString();
            string aProvID = aDataTable.Rows[0]["ProvID"].ToString();
            string aNPi = aDataTable.Rows[0]["NPi"].ToString();
            string aFirstName = aDataTable.Rows[0]["FirstName"].ToString();
            string aLastName = aDataTable.Rows[0]["LastName"].ToString();
            LogHelpers.Write($" INFO :: FedID :{aFedID}, ProvID: { aProvID}, NPi :{aNPi},FirstName : {aFirstName}, LastName: {aLastName}");
            LogHelpers.Write(" INFO :: Test Method : Execution completed ReturnTableDBQuery " + System.Environment.NewLine);

        }

    }
}
