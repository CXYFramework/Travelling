using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Travelling.Services;

namespace Travelling.Service.Test
{
    [TestClass]
    public class TicketServiceTest
    {
        [TestMethod]
        public void GetTicketBySearchTestMethod()
        {
            TicketService service = new TicketService();
            string ticketXml = service.GetTicketBySearch(1, "");
            System.IO.File.AppendAllText("d:\\jjj.xml", ticketXml);

            Assert.IsNotNull(ticketXml);
        }
    }
}
