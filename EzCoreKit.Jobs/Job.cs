using System;
using System.Diagnostics;
using System.IO;

namespace EzCoreKit.Jobs {
    /// <summary>
    /// 工作
    /// </summary>
    public class Job {
        /// <summary>
        /// 標準輸入
        /// </summary>
        internal StreamWriter standardInput;

        /// <summary>
        /// 標準輸出
        /// </summary>
        internal StreamReader standardOutput;

        /// <summary>
        /// Process實例
        /// </summary>
        internal Process process;

        /// <summary>
        /// 唯一識別號
        /// </summary>
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// 命令
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// 命令參數
        /// </summary>
        public string Arguments { get; set; }

        /// <summary>
        /// 工作進度，0~100
        /// </summary>
        public int Processing { get; protected set; }

        /// <summary>
        /// 工作是否已結束
        /// </summary>
        public bool HasExited => process.HasExited;

        /// <summary>
        /// 執行結果，0表示正常結束。
        /// </summary>
        public int ExitCode => process.ExitCode;



        public void Input(string text) {
            standardInput.Write(text);
        }

        public void InputLine(string text) {
            standardInput.WriteLine(text);
        }

        public virtual void OnOutput(string line) { }
    }
}