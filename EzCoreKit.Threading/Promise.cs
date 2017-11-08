using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzCoreKit.Threading {
    /// <summary>
    /// Promise代表一個將完成（或失敗）的一個非同步操作，所產生的值。
    /// </summary>
    /// <typeparam name="T">回傳結果類型</typeparam>
    public class Promise<T> {
        private event Action OnSuccess;
        private event Action OnError;
        private PromiseStatus status = PromiseStatus.Pending;

        public PromiseStatus Status {
            get {
                return status;
            }
            private set {
                status = value;
                if(status == PromiseStatus.Fulfilled) {
                    OnSuccess?.Invoke();
                }else if(status == PromiseStatus.Rejected) {
                    OnError?.Invoke();
                }
            }
        }

        public Task<T> Task { get; set; }

        public T Result { get; private set; }

        public Exception Exception { get; private set; }

        public Promise(Action<Action<T>,Action<Exception>> executor) {
            this.Task = Task<T>.Run<T>(() => {
                executor((T r) => {
                    Result = r;
                    Status = PromiseStatus.Fulfilled;
                }, (Exception e) => {
                    Exception = e;
                    Status = PromiseStatus.Rejected;
                });
                
                if(Exception == null) {
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
        public Promise<T2> Then<T2>(Func<T,T2> func) {
            return new Promise<T2>((res, rej) => {
                try {
                    res(func(this.Task.GetAwaiter().GetResult()));
                } catch (Exception e){
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
            return new Promise<T>((res,rej)=> {
                try {
                    res(this.Task.GetAwaiter().GetResult());
                } catch(Exception e) {
                    try {
                        res(func(e));
                    }catch(Exception e2) {
                        rej(e2);
                    }
                }
            });
        }        
    }
}
