using System;
using EzCoreKit.Extensions;
using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace EzCoreKit.Test.Extensions {
    public class ArrayExtensionTest {
        [Fact(DisplayName = "Extensions.Array.Full")]
        public void Array_Full_Test() {
            for (int i = 0; i < 16; i++) {
                var ary = new int[i];
                ary.Full(1);

                Assert.Equal(ary.Sum(), i);
            }
        }

        [Fact(DisplayName = "Extensions.Array.BoxingToArray")]
        public void Array_BoxingToArray() {
            var elements = new object[] { true, false, 'a', "S", 1, 0.1 };

            foreach (var element in elements) {
                var ary = element.BoxingToArray();
                var ary_ans = Array.CreateInstance(element.GetType(), 1);
                ary_ans.SetValue(element, 0);

                Assert.Equal(ary, ary_ans);
            }
        }

        [Fact(DisplayName = "Extensions.Array.GetLengths")]
        public void Array_GetLengths() {
            var arys = new Array[] {
                new int[]{ 1, 2, 3 },
                new int[,]{
                    { 1,2,3 },
                    { 4,5,6 }
                }
            };

            var lens = new int[][] {
                new int[]{ 3 },
                new int[]{ 2 , 3 }
            };

            for (int i = 0; i < arys.Length; i++) {
                Assert.Equal(arys[i].GetLengths(), lens[i]);
            }
        }

        [Fact(DisplayName = "Extensions.Array.GetAllIndexes")]
        public void Array_GetAllIndexes() {
            var arys =
                new int[,]{
                    { 1,2,3 },
                    { 4,5,6 }
                };

            var lens = new List<List<int>> {
                new List<int>(new int[]{ 0,0 }),
                new List<int>(new int[]{ 0,1 }),
                new List<int>(new int[]{ 0,2 }),
                new List<int>(new int[]{ 1,0 }),
                new List<int>(new int[]{ 1,1 }),
                new List<int>(new int[]{ 1,2 }),

            };

            Assert.Equal(arys.GetAllIndexes(), lens);
        }

        [Fact(DisplayName = "Extensions.Array.SetAndGetValue")]
        public void Array_SetAndGetValue() {
            var arys =
                new int[,]{
                    { 1,2,3 },
                    { 4,5,6 }
                };

            var index = new List<int>(new int[] { 0, 0 });
            for (int i = 0; i < 16; i++) {
                arys.SetValue(i, index);
                Assert.Equal(arys.GetValue(index), i);
            }
        }

        [Fact(DisplayName = "Extensions.Array.SpanSlice")]
        public void Array_SpanSlice() {
            int[] ary = Enumerable.Range(0, 10).ToArray();

            ary.SpanSlice(0, 2).Fill(123);// 將前兩項設為 123

            Assert.Equal(ary[0], 123);
        }
    }
}