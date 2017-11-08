using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzCoreKit.Threading {
    /// <summary>
    /// Promise執行程序實體
    /// </summary>
    /// <typeparam name="T">回傳結果類型</typeparam>
    /// <param name="resolve">實現回呼函數</param>
    /// <param name="reject">拒絕回呼函數</param>
    public delegate void PromiseExecutor<T>(Action<T> resolve, Action<Exception> reject);

    /// <summary>
    /// Promise代表一個將完成（或失敗）的一個非同步操作，所產生的值。
    /// </summary>
    /// <typeparam name="T">回傳結果類型</typeparam>
    public class Promise<T> {
        private event Action OnSuccess;
        private event Action OnError;
        private PromiseStatus status = PromiseStatus.Pending;

        /// <summary>
        /// 狀態
        /// </summary>
        public PromiseStatus Status {
            get {
                return status;
            }
            private set {
                status = value;
                if (status == PromiseStatus.Fulfilled) {
                    OnSuccess?.Invoke();
                } else if (status == PromiseStatus.Rejected) {
                    OnError?.Invoke();
                }
            }
        }

        /// <summary>
        /// 任務實例
        /// </summary>
        public Task<T> Task { get; set; }

        /// <summary>
        /// 執行結果
        /// </summary>
        public T Result { get; private set; }

        /// <summary>
        /// 執行例外
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// 初始化一個Promsie實例
        /// </summary>
        /// <param name="executor">Promise執行程序主體</param>
        public Promise(PromiseExecutor<T> executor) {
            this.Task = Task<T>.Run<T>(() => {
                executor((T r) => {
                    Result = r;
                    Status = PromiseStatus.Fulfilled;
                }, (Exception e) => {
                    Exception = e;
                    Status = PromiseStatus.Rejected;
                });

                if (Exception == null) {
                    return Result;
                } else {
                    throw Exception;
                }
            });
        }

        /// <summary>
        /// 取得並處理Promise運行結果
        /// </summary>
        /// <typeparam name="T2">運行結果處理類型</typeparam>
        /// <param name="func">運行結果處理方法</param>
        /// <returns>Promise</returns>
        public Promise<T2> Then<T2>(Func<T, T2> func) {
            return new Promise<T2>((res, rej) => {
                try {
                    res(func(this.Task.GetAwaiter().GetResult()));
                } catch (Exception e) {
                    rej(e);
                }
            });
        }

        /// <summary>
        /// 捕捉例外並進行例外處理
        /// </summary>
        /// <param name="func">例外處理方法</param>
        /// <returns>Promise</returns>
        public Promise<T> Catch(Func<Exception, T> func) {
            return new Promise<T>((res, rej) => {
                try {
                    res(this.Task.GetAwaiter().GetResult());
                } catch (Exception e) {
                    try {
                        res(func(e));
                    } catch (Exception e2) {
                        rej(e2);
                    }
                }
            });
        }

        /// <summary>
        /// 當在引數iterable中所有Promise都被實現時被實現或在引數iterable中有一個Promise被拒絕時立刻被拒絕
        /// </summary>
        /// <param name="iterable">Promise集合</param>
        /// <returns>Promise</returns>
        public static Promise<object[]> All(IEnumerable<Promise<object>> iterable) {
            return new Promise<object[]>((res, rej) => {
                object[] result = new object[iterable.Count()];
                Parallel.ForEach(iterable, (x, status, index) => {
                    try {
                        result[index] = x.Task.GetAwaiter().GetResult();
                    } catch (Exception e) {
                        rej(e);
                        status.Stop();
                    }
                });
                res(result);
            });
        }

        /// <summary>
        /// 也可稱為Any，此Promise物件會於iterable引數中任一個Promise轉為resolve或rejected時立即轉變成resolve或rejected，並且接收其成功值或失敗訊息
        /// </summary>
        /// <param name="iterable">Promise集合</param>
        /// <returns>Promise</returns>
        public static Promise<object> Race(IEnumerable<Promise<object>> iterable) {
            bool locked = false;
            return new Promise<object>((res, rej) => {
                Parallel.ForEach(iterable, (x, status, index) => {
                    try {
                        var result = x.Task.GetAwaiter().GetResult();
                        if (locked) return;
                        locked = true;
                        res(result);
                        status.Stop();
                    } catch (Exception e) {
                        rej(e);
                        status.Stop();
                    }
                });
            });
        }


        public static explicit operator Promise<object>(Promise<T> obj){
            return new Promise<object>((res, rej) => {
                obj.Then<T>(x=> { res(x); return x; })
                .Catch(x => { rej(x); return default(T); })
                .Task.ToSync();
            });
        }
    }
}
