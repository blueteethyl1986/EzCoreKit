using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EzCoreKit.Jobs {
    /// <summary>
    /// 工作管理者
    /// </summary>
    public class JobManager {
        /// <summary>
        /// 工作清單
        /// </summary>
        private static List<Job> jobList = new List<Job>();

        /// <summary>
        /// 工作清單
        /// </summary>
        public static IReadOnlyList<Job> Jobs => jobList.AsReadOnly();

        /// <summary>
        /// 當有工作被停止
        /// </summary>
        public static event EventHandler Exited;

        /// <summary>
        /// 加入新工作
        /// </summary>
        /// <param name="job"></param>
        public static void Add(Job job) {
            jobList.Add(job);
        }

        /// <summary>
        /// 執行工作
        /// </summary>
        /// <param name="id">唯一識別號</param>
        public static void Run(Guid id) {
            var job = Get(id);
            var process = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = job.Command,
                    Arguments = job.Arguments,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            job.process = process;
            job.process.Exited += Process_Exited;
            lock (jobList) {
                jobList.Add(job);
                process.Start();
                job.standardOutput = process.StandardOutput;
                job.standardInput = process.StandardInput;
            }
            Task.Run(() => {
                while (!process.StandardOutput.EndOfStream) {
                    job.OnOutput(process.StandardOutput.ReadLine());
                }
            }, CancellationToken.None);
        }

        /// <summary>
        /// 加入並執行新工作
        /// </summary>
        /// <param name="job">工作</param>
        public static void AddAndRun(Job job) {
            Add(job);
            Run(job.Id);
        }

        /// <summary>
        /// 取消執行中的工作
        /// </summary>
        /// <param name="id">唯一識別號</param>
        public static void Cancel(Guid id) {
            var target = Get(id);
            if (target.HasExited) target.process.Kill();
        }

        /// <summary>
        /// 取消執行中的工作並從工作清單移除
        /// </summary>
        /// <param name="id">唯一識別號</param>
        public static void RemoveAndCancel(Guid id) {
            var target = Get(id);
            if (target.HasExited) target.process.Kill();
            jobList.Remove(target);
        }

        /// <summary>
        /// 取得指定工作實例
        /// </summary>
        /// <param name="id">唯一識別號</param>
        /// <returns>工作實例</returns>
        public static Job Get(Guid id) {
            return Jobs.First(x => x.Id == id);
        }

        private static void Process_Exited(object sender, EventArgs e) {
            JobManager.Exited?.Invoke(sender, e);
        }
    }
}
