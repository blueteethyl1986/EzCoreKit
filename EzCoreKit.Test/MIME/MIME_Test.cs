using EzCoreKit.MIME;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EzCoreKit.Test.MIME{
    public class MIME_Test {
        [Fact(DisplayName = "MIME.GetMIMEByFileExt")]
        public void GetMIMEByFileExt() {
            Assert.Equal(DeclareMIME.GetMIMEByFileExt(".json"), DeclareMIME.JavaScript_Object_Notation_JSON);
        }

        [Fact(DisplayName = "MIME.GetMIMENameList")]
        public void GetMIMENameList() {
            Assert.NotEmpty(DeclareMIME.GetMIMENameList());
        }
    }
}
