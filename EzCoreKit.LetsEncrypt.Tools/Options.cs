using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.LetsEncrypt.Tools {
    public class Options {
        [Option('d', "domain", Required = true,
            HelpText = "Web伺服器DNS域名")]
        public IEnumerable<string> Domains { get; set; }
        
        [Option('e',"email",Default = "admin@example.com",
            HelpText = "管理員電子郵件")]
        public string Email { get; set; }

        [Option('o', "output", Required = true,
            HelpText = "輸出PFX檔案路徑(ex. C:\\text.pfx)")]
        public string OutputFile { get; set; }

        [Option('p', "password", Default = "",
            HelpText = "輸出PFX檔案密碼")]
        public string OutputFilePassword { get; set; }
    }
}
