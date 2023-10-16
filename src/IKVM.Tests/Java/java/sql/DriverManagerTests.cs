using System;
using System.Collections.Generic;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.sql
{

    [TestClass]
    public class DriverManagerTests
    {

        class TestDriver : global::java.sql.Driver
        {

            static TestDriver()
            {
                try
                {
                    global::java.sql.DriverManager.registerDriver(new TestDriver());
                }
                catch (global::java.sql.SQLException ex)
                {
                    throw new global::java.lang.ExceptionInInitializerError(ex);
                }
            }

            public bool acceptsURL(string url)
            {
                throw new NotImplementedException();
            }

            public global::java.sql.Connection connect(string url, global::java.util.Properties info)
            {
                throw new NotImplementedException();
            }

            public int getMajorVersion()
            {
                throw new NotImplementedException();
            }

            public int getMinorVersion()
            {
                throw new NotImplementedException();
            }

            public global::java.util.logging.Logger getParentLogger()
            {
                throw new NotImplementedException();
            }

            public global::java.sql.DriverPropertyInfo[] getPropertyInfo(string url, global::java.util.Properties info)
            {
                throw new NotImplementedException();
            }

            public bool jdbcCompliant()
            {
                throw new NotImplementedException();
            }

        }

        [TestMethod]
        public void Can_find_odbc()
        {
            var l = new List<global::java.sql.Driver>();
            var e = global::java.sql.DriverManager.getDrivers();
            while (e.hasMoreElements())
                l.Add((global::java.sql.Driver)e.nextElement());

            l.Should().HaveCountGreaterThan(0);
            l.Should().Contain(i => i.GetType().FullName == "sun.jdbc.odbc.JdbcOdbcDriver");
        }

        [TestMethod]
        public void Can_register_driver()
        {
            global::java.lang.Class.forName("cli.IKVM.Tests.Java.java.sql.DriverManagerTests$TestDriver");

            var l = new List<global::java.sql.Driver>();
            var e = global::java.sql.DriverManager.getDrivers();
            while (e.hasMoreElements())
                l.Add((global::java.sql.Driver)e.nextElement());

            l.Should().HaveCountGreaterThan(0);
            l.Should().Contain(i => i.GetType().FullName == "IKVM.Tests.Java.java.sql.DriverManagerTests+TestDriver");
        }

    }

}
