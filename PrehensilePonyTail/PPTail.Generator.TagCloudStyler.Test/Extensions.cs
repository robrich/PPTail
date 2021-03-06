﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;

namespace PPTail.Generator.TagCloudStyler.Test
{
    public static class Extensions
    {
        public static IEnumerable<string> GetTagList(this IEnumerable<string> ignore)
        {
            return ignore.GetTagList(50.GetRandom(30));
        }

        public static IEnumerable<string> GetTagList(this IEnumerable<string> ignore, int count)
        {
            var result = new List<string>();
            for (int i = 0; i < count; i++)
                result.Add(string.Empty.GetRandom());
            return result;
        }
    }
}
