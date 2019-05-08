using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Tubumu.Core.Extensions
{
    public static class TaskExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)] // 造成编译器优化调用
        public static void NoWarning(this Task task)
        {

        }
    }
}
