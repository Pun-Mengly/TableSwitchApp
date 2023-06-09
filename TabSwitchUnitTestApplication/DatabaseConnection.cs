using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableSwitchWebApplication.Cores;

namespace TabSwitchUnitTestApplication
{
    public class DatabaseConnection
    {

        [Test]
        public async Task Test_AdoOperation()
        {
            try 
            {
                var result=await AdoOperation.ReturnDT1NullableObj("SELECT 1");
                var data = result.Rows.Count;
                Assert.That(data, Is.EqualTo(1), "DB Was not Connected");
            }
            catch (Exception ex)
            { 
                throw new Exception(ex.Message); 
            }
        }
    }
}
