﻿using EzCoreKit.Test.TestModels;
using System;
using System.Collections.Generic;
using System.Text;
using EzCoreKit.Linq;
using System.Linq;
using Xunit;
using EzCoreKit.Extensions;

namespace EzCoreKit.Test.Linq {
    public class OrderByAndGroupByExtension_Test {
        public List<Student> InitList() {
            List<Student> result = new List<Student>();
            result.Add(new Student() {
                Id = 6,
                Class = "B",
                Name = "User06"
            });
            result.Add(new Student() {
                Id = 0,
                Class = "A",
                Name = "User01"
            });
            result.Add(new Student() {
                Id = 4,
                Class = "B",
                Name = "User04"
            });
            result.Add(new Student() {
                Id = 3,
                Class = "A",
                Name = "User03"
            });
            result.Add(new Student() {
                Id = 1,
                Class = "A",
                Name = "User02"
            });
            result.Add(new Student() {
                Id = 5,
                Class = "B",
                Name = "User05"
            });

            return result;
        }

        [Fact(DisplayName = "Linq.OrderBy")]
        public void OrderBy_Test() {
            var list = InitList();
            var orderedList = list.OrderBy(new(bool isDec, string name)[] {
                (isDec : true,name: "Class"),
                (isDec : false,name: "Id"),
            });

            Assert.Equal(orderedList, list.OrderByDescending(x => x.Class).ThenBy(x => x.Id));
        }

        [Fact(DisplayName = "Linq.GroupBy")]
        public void GroupBy_Test() {
            var list = InitList();
            var groupedList = list.GroupBy("Class".BoxingToArray());

            Assert.Equal(groupedList, list.GroupBy(x => x.Class));
        }
    }
}
