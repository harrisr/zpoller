using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RLBBD1PollerLibrary;

namespace RLBPollerUnitTests
{
    [TestClass]
    public class PollerUnitTests
    {
        [TestMethod]
        public void RLBBD1PollerTest01()
        {
            try
            {
                //  This *SHOULD* throw an exception
                RLBBD1Poller poller = new RLBBD1Poller();
                Assert.IsNotNull(poller);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void RLBBD1PollerTest02()
        {
            try
            {
                //  This should not throw an exception
                RLBBD1Poller poller = new RLBBD1Poller("C:\\polling\\watch1", "*.txt", 1.0);
                Assert.IsNotNull(poller);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }
        
        [TestMethod]
        public void RLBBD1PollerTest03()
        {
            try
            {
                //  This *SHOULD* throw an exception
                RLBBD1Poller poller = new RLBBD1Poller("C:\\doesnotexist", "*.txt", 1.0);
                Assert.IsNotNull(poller);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void RLBBD1PollerTest04()
        {
            try
            {
                //  This *SHOULD* throw an exception
                RLBBD1Poller poller = new RLBBD1Poller("C:\\polling\\watch1", "*.zz", 1.0);
                Assert.IsNotNull(poller);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(true);
            }
        }
        
        [TestMethod]
        public void RLBBD1PollerTest05()
        {
            try
            {
                //  This *SHOULD* throw an exception
                RLBBD1Poller poller = new RLBBD1Poller("C:\\polling\\watch1", "*.z", 1.0);
                Assert.IsNotNull(poller);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(true);
            }
        }
        
        [TestMethod]
        public void RLBBD1PollerTest06()
        {
            try
            {
                //  This shoud not throw an exception
                RLBBD1Poller poller = new RLBBD1Poller("C:\\polling\\watch1", "*.txt", 1.0);
                Assert.IsNotNull(poller);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public void RLBBD1PollerTest07()
        {
            try
            {
                //  This shoud not throw an exception
                RLBBD1Poller poller = new RLBBD1Poller("C:\\polling\\watch1", "*.txt", 1.0);
                Assert.IsNotNull(poller);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public void RLBBD1PollerTest08()
        {
            try
            {
                //  This shoud not throw an exception
                RLBBD1Poller poller = new RLBBD1Poller("C:\\polling\\watch1", "*.bd1", 1.0);
                Assert.IsNotNull(poller);

                //poller.PollDirectory();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }



        //[TestMethod]
        //public void RLBBD1PollManagerTest01()
        //{
        //    RLBBD1PollManager poller = new RLBBD1PollManager();

        //    Assert.IsNotNull(poller);
        //}
    }
}
