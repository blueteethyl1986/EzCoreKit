﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzCoreKit.Extensions {
    public static class ChineseNumberExtension {
        private static long[] SplitByMod(this long Value, long Mod = 10) {
            Stack<long> result = new Stack<long>();
            do {
                result.Push(Value % Mod);
                Value = (long)Math.Floor(Value / (double)Mod);
            } while (Value > 0);
            return result.ToArray();
        }

        #region 中文單位對應字典
        /// <summary>
        /// 中文數字集合
        /// </summary>
        public static string[] ChineseNumber => "零壹貳參肆伍陸柒捌玖".Select(x => x.ToString()).ToArray();

        /// <summary>
        /// 第一級中文數字單位
        /// </summary>
        public static string[] ChineseUnitLevel0 = new string[] { "", "拾", "佰", "仟" };

        /// <summary>
        /// 第二集中文數字單位
        /// </summary>
        public static string[] ChineseUnitLevel1 = new string[] { "", "萬", "億", "兆", "京" };

        /// <summary>
        /// 中文數字單位
        /// </summary>
        public static string[][] ChineseUnit => new string[][] { ChineseUnitLevel0, ChineseUnitLevel1 };
        #endregion

        /// <summary>
        /// 將數字轉換為中文表示
        /// </summary>
        /// <param name="Value">數值</param>
        /// <returns>數字中文表示字串</returns>
        public static string ToChineseNumber(this long Value) {
            Stack<string> OutputValues = new Stack<string>();
            Stack<long> InputValues;
            int LevelIndex = 0;
            if (Value / 10 < 1) {
                return ChineseNumber[Value];
            } else if (Value / Math.Pow(10, ChineseUnitLevel0.Length) < 1) {//小於高級單位門檻
                InputValues = new Stack<long>(Value.SplitByMod());
            } else {//滿足高級單位門檻
                InputValues = new Stack<long>(Value.SplitByMod((long)Math.Pow(10, ChineseUnitLevel0.Length)));
                LevelIndex = 1;
            }

            long Previous = 0;
            for (int i = 0; InputValues.Count > 0; i++) {
                long UnitValue = InputValues.Pop();

                if (UnitValue == 0) {
                    if (Previous != 0) {
                        OutputValues.Push(UnitValue.ToChineseNumber());
                    }
                    Previous = UnitValue;
                } else {
                    if (ChineseUnit[LevelIndex].Length <= i) {//使用較低等的高級單位來補足
                        int iCopy = i; Stack<string> Units = new Stack<string>();
                        while (iCopy > 0) {
                            int UnitTarget = Math.Min(iCopy, ChineseUnit[LevelIndex].Length - 1);
                            Units.Push(ChineseUnit[LevelIndex][UnitTarget]);
                            iCopy -= UnitTarget;
                        }
                        OutputValues.Push(UnitValue.ToChineseNumber() + string.Join("", Units));
                    } else {
                        OutputValues.Push(UnitValue.ToChineseNumber() + ChineseUnit[LevelIndex][i]);
                    }
                    #region 補0
                    if (
                       LevelIndex == 1 &&
                       InputValues.Count > 0 &&
                       UnitValue / Math.Pow(10, ChineseUnitLevel0.Length - 1) < 1
                    ) {
                        OutputValues.Push(ChineseNumber[0]);
                        Previous = 0;
                    } else {
                        Previous = UnitValue;
                    }
                    #endregion
                }

            }
            return string.Join("", OutputValues.ToArray());
        }
    }
}
