using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTTools
{
    class DisposableBaseInfo : IDisposable
    {
        /*
         * finalize释放非托管资源
         * dispose释放托管和非托管资源
         * 两者不冲突，可以重复调用
         * 
         * */

        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private bool _IsDisposed = false;

        /// <summary>
        /// 垃圾回收调用，释放非托管资源
        /// </summary>
        ~DisposableBaseInfo()
        {
            Dispose(false);
        }

        /// <summary>
        /// 类的使用者，显示调用，释放资源
        /// 并且移除GC链表中该对象，避免释放两次
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 参数为true表示释放托管和非托管资源
        /// 参数为false 释放非托管资源
        /// 子类有自己的非托管资源，可以重载此方法，添加自身的非托管资源的
        /// 重在时候需要调用base.Dispose,保证基类的释放
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool isDisposing)
        {
            if(!this._IsDisposed)
            {
                if(isDisposing)
                {
                    //如果对象没有被释放，释放托管资源
                }
            }
            this._IsDisposed = true;  //标识此对象已经被释放
        }
    }
}
