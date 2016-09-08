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
    public class HasAttributeTest
    {
        [TestMethod]
        public void TestClass ( )
        {
            Assert.IsTrue ( typeof ( Foo ).HasAttribute<System.Runtime.Serialization.DataContractAttribute> ( ) );
        }

        [TestMethod]
        public void TestField ( )
        {
            Assert.IsTrue ( typeof ( Foo ).GetField ( "Bar" ).HasAttribute<NonSerializedAttribute> ( ) );
        }

        [TestMethod]
        public void TestProperty ( )
        {
            Assert.IsTrue ( typeof ( Foo ).GetProperty ( "Blah" ).HasAttribute<System.Runtime.Serialization.DataMemberAttribute> ( ) );
        }
    }

    [System.Runtime.Serialization.DataContract]
    public class Foo
    {
        [System.Runtime.Serialization.DataMember]
        [System.ComponentModel.DataAnnotations.MaxLength ( 200 )]
        public int [] Blah { get; set; }

        [NonSerialized]
        public string Bar;
    }
}
