using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Utilis.Extensions;

namespace Utilis.Test
{
    [TestFixture]
    public class ExceptionExtensionTests
    {
        [Test]
        public void TestMagicNumberShowsUpInToFullString ( )
        {
            Exception ex = new Exception ( "This is an exception." ).AddMagicNumber ( 123456789 );
            string exToFullString = ex.ToFullString ( );
            Assert.IsTrue ( exToFullString.Contains ( "123456789" ) );
        }

        [Test]
        public void TesToFullStringWithoutMagicNumber ( )
        {
            Exception ex = new Exception ( "This is an exception.", new Exception ( "This is an INNER exception!", new Exception ( "This is an INNER INNER exception!" ) ) );
            string exToFullString = ex.ToFullString ( );
            Assert.IsTrue ( exToFullString.Length > 0 );
        }

        [Test]
        public void TestMagicNumberShowsUpInToFullStringOfSubException ( )
        {
            Exception ex = new Exception ( "This is an exception.", new Exception ( "This is an INNER exception!" ).AddMagicNumber ( 123456789 ) );
            string exToFullString = ex.ToFullString ( );
            Assert.IsTrue ( exToFullString.Contains ( "123456789" ), "Text does not contain magic number!: \r\n" + exToFullString );
        }
    }
}
