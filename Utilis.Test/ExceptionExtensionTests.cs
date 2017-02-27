using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utilis.Extensions;

namespace Utilis.Test
{
    [TestClass]
    public class ExceptionExtensionTests
    {
        [TestMethod]
        public void TestMagicNumberShowsUpInToFullString ( )
        {
            Exception ex = new Exception ( "This is an exception." ).AddMagicNumber ( 123456789 );
            string exToFullString = ex.ToFullString ( );
            Assert.IsTrue ( exToFullString.Contains ( "123456789" ) );
        }

        [TestMethod]
        public void TesToFullStringWithoutMagicNumber ( )
        {
            Exception ex = new Exception ( "This is an exception.", new Exception ( "This is an INNER exception!", new Exception ( "This is an INNER INNER exception!" ) ) );
            string exToFullString = ex.ToFullString ( );
            Assert.IsTrue ( exToFullString.Length > 0 );
        }

        [TestMethod]
        public void TestMagicNumberShowsUpInToFullStringOfSubException ( )
        {
            Exception ex = new Exception ( "This is an exception.", new Exception ( "This is an INNER exception!" ).AddMagicNumber ( 123456789 ) );
            string exToFullString = ex.ToFullString ( );
            Assert.IsTrue ( exToFullString.Contains ( "123456789" ), "Text does not contain magic number!: \r\n" + exToFullString );
        }
    }
}
