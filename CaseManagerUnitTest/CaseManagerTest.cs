using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VPCaseManager;
using VPCaseManager.Model;

namespace CaseManagerUnitTest
{
    [TestClass]
    public class CaseManagerTest
    {
        [TestInitialize] // Method runs before each and every test method
        public void Initialize()
        {
            // Initialization code (e.g., creating an instance of the class to test)
            SQLitePCL.Batteries.Init();
            //SQLitePCL.raw.SetProvider(new SQLitePCL.ISQLite3Provider());
        }


        [TestMethod]
        public void TestUsers()
        {
            CaseManager cm = new CaseManager(@"C:\Users\rickj\source");
            List<User> users = cm.GetUserList();
            foreach (User user in users)
            {
                Debug.WriteLine(user.UserName);
                Debug.WriteLine(user.UserKey);
            }
        }

        [TestMethod]
        public void TestProducts()
        {
            CaseManager cm = new CaseManager(@"C:\Users\rickj\source");
            List<Product> list = cm.GetProducts();
            foreach (Product p in list)
            {
                Debug.WriteLine(p.Description);
                Debug.WriteLine(p.ServerID);
                foreach (Plan plan in p.Plans)
                {
                    Debug.WriteLine(plan.Description);
                }
            }
        }
        [TestMethod]
        public void TestCases()
        {
            CaseManager cm = new CaseManager(@"C:\Users\rickj\source");
            Client client = cm.GetClient(1);
            Debug.WriteLine(client.FirstName + " " + client.LastName);
            List<Case> cases = cm.GetCaseList(client.ClientID);
            foreach (Case c in cases)
            {
                Debug.WriteLine(c.CaseName);
                Debug.WriteLine(c.CaseNotes);

                List<CaseDetail> details = cm.GetCaseDetailByCaseID(c.Key);
                foreach (CaseDetail detail in details)
                {
                    Debug.WriteLine(detail.Topic_Name);
                }
            }
        }


    }
}
