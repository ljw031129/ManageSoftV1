using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProtocolsManage.Common
{
    public class EvaluateExpression11
    {
        /// <summary>
        /// 替换表达式中的指定字符
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="chr1">替换的字符</param>
        /// <param name="chr2">替换为字符串</param>
        /// <returns></returns>
        public static string ReplaceChar(string str, char chr1, string chr2)
        {
            if (str == null || str == string.Empty)
            {
                throw new Exception("String input can not be null");
            }

            string result = "";

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != chr1)
                {
                    result += str[i];
                }
                else
                {
                    result += chr2;
                }
            }

            return result;
        }
        /// <summary>
        /// 根据表3.1，判断两符号的优先关系
        /// </summary>
        /// <param name="Q1">操作栈栈顶运算符</param>
        /// <param name="Q2">当前从表达式读取到的运算符</param>
        /// <returns>返回Q1和Q2两个运算符之间的优先关系</returns>
        private static string Precede(string Q1, string Q2)
        {
            string f = string.Empty;
            switch (Q2)
            {
                case "+":
                case "-":
                    if (Q1 == "(" || Q1 == "#")
                        f = "<";
                    else
                        f = ">";
                    break;

                case "*":
                case "/":
                    if (Q1 == "*" || Q1 == "/" || Q1 == ")")
                        f = ">";
                    else
                        f = "<";
                    break;
                case "(":
                    if (Q1 == ")")
                        throw new ArgumentOutOfRangeException("表达式错误！");
                    else
                        f = "<";
                    break;
                case ")":
                    switch (Q1)
                    {
                        case "(": f = "="; break;
                        case "#": throw new ArgumentOutOfRangeException("表达式错误！");
                        default: f = ">"; break;
                    }
                    break;
                case "#":
                    switch (Q1)
                    {
                        case "#": f = "="; break;
                        case "(": throw new ArgumentOutOfRangeException("表达式错误！");
                        default: f = ">"; break;
                    }
                    break;
            }
            return f;
        }

        /// <summary>
        /// 判断c是否为操作符
        /// </summary>
        /// <param name="c">需要判断的运算符</param>
        /// <returns></returns>
        private static bool IsKeyword(string c)
        {
            switch (c)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                case "(":
                case ")": return true;
                default: return false;
            }
        }

        /// <summary>
        /// 判断字符串是否为实数
        /// </summary>
        /// <param name="input">需要判断的字符串</param>
        /// <returns></returns>
        private static bool IsNumeric(string input)
        {
            bool flag = true;
            string pattern = (@"^(-|\+)?\d+(\.\d+)?$");
            Regex validate = new Regex(pattern);
            if (!validate.IsMatch(input))
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 对传入的参数opnd1和opnd2进行oper四则运算
        /// </summary>
        /// <param name="opnd1">操作数1</param>
        /// <param name="oper">运算符</param>
        /// <param name="opnd2">操作数2</param>
        /// <returns>返回实数结果</returns>
        private static double Operate(double opnd1, string oper, double opnd2)
        {
            double result = 0;
            switch (oper)
            {
                case "+": result = opnd1 + opnd2; break;
                case "-": result = opnd1 - opnd2; break;
                case "*": result = opnd1 * opnd2; break;
                case "/":
                    {
                        if (opnd2 == 0) throw new ArgumentOutOfRangeException("除数为0!");
                        result = opnd1 / opnd2; break;
                    }
                default: throw new ArgumentOutOfRangeException(string.Format("操作符{0}错误！", oper));
            }
            return result;
        }

        /// <summary>
        /// 检查传入的是否为运算符
        /// </summary>
        /// <param name="op">要检查的数组元素</param>
        /// <returns></returns>
        private static bool IsOperator(string op)
        {
            bool flag = false;
            switch (op)
            {
                case "+": flag = true; break;
                case "-": flag = true; break;
                case "*": flag = true; break;
                case "/": flag = true; break;
            }
            return flag;
        }

        /// <summary>
        /// 对表达式的每一部分进行检查是否符合表达式的规范
        /// </summary>
        /// <param name="expr">表达式数组</param>
        /// <param name="idx">要检查的部分</param>
        /// <returns></returns>
        private static bool CheckEveryExpr(string[] expr, int idx)
        {
            int len = expr.Length;
            if (len == 0) return false;
            if (idx == len - 1) return true;
            int p = 0;  //previous
            int n = 0;  //next
            if (idx == 0)  //表达式只能以数字或者(开头
            {
                if (IsNumeric(expr[idx]))  //数字 expr[n]可为[+,-,*,/,#]
                {
                    n = idx + 1;
                    if (n >= len) return false;
                    else
                    {
                        if (expr[n] == "#") return true;
                        else
                        {
                            if (IsOperator(expr[n])) return true;
                            else return false;
                        }
                    }
                }
                else if (expr[idx] == "(")  //( expr[n]可为[数字]
                {
                    n = idx + 1;
                    if (n >= len) return false;
                    else
                    {
                        if (IsNumeric(expr[n])) return true;
                        else return false;
                    }
                }
                else return false;
            }
            else if (idx == len - 2)  //表达式只能以数字或者)结尾
            {
                if (IsNumeric(expr[idx])) return true;
                else if (expr[idx] == ")") return true;
                else return false;
            }
            else  //表达式中间部分分成4种进行判断
            {
                n = idx + 1;
                p = idx - 1;
                if (IsNumeric(expr[idx]))  //数字 expr[p]可为[(,+,-,*,/] expr[n]可为[),+,-,*,/]
                {
                    if (IsKeyword(expr[p]) && expr[p] != ")") return true;
                    else return false;
                }
                else if (IsKeyword(expr[idx]))  //操作符 +,-,*,/ 
                {
                    if (IsOperator(expr[idx]))  //+,-,*,/操作操作符 expr[p]可为[数字,)] expr[n]可为[数字,(]
                    {
                        if ((IsNumeric(expr[p]) || expr[p] == ")")
                            && (IsNumeric(expr[n]) || expr[n] == "(")) return true;
                        else return false;
                    }
                    else //操作符 ( )
                    {
                        if (expr[idx] == "(") //( expr[p]可为[+,-,*,/,(] expr[n]可为[数字,(]
                        {
                            if ((IsOperator(expr[p]) || expr[p] == "(") &&
                                (IsNumeric(expr[n]) || expr[n] == "(")) return true;
                            else return false;
                        }
                        else if (expr[idx] == ")") //) expr[p]可为[数字,)] expr[n]可为[+,-,*,/,)] 
                        {
                            if ((IsNumeric(expr[p]) || expr[p] == ")") &&
                                (IsOperator(expr[n]) || expr[n] == ")")) return true;
                            else return false;
                        }
                        else return false;
                    }
                }
                else return false;
            }
        }

        /// <summary>
        /// 把表达式拆分成字符串数组，用于后面的检查和求值
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>从左到右返回从下标0开始的字符串数组</returns>
        private static string[] SplitExpression(string expression)
        {
            string[] exprs = null;
            List<string> lstItem = new List<string>();
            expression = expression.Trim();
            int length = expression.Length;
            string item = string.Empty;
            string ch = string.Empty;
            while (length != 0)
            {
                ch = expression.Substring(expression.Length - length, 1);
                if (!IsKeyword(ch)) item += ch;
                else
                {
                    item = item.Trim();
                    if (item != string.Empty) lstItem.Add(item);
                    item = string.Empty;
                    lstItem.Add(ch);
                }
                length--;
            }
            item = item.Trim();
            if (item != string.Empty) lstItem.Add(item);

            exprs = new string[lstItem.Count + 1];
            for (int i = 0; i < lstItem.Count; i++) exprs[i] = lstItem[i];
            exprs[lstItem.Count] = "#";

            return exprs;
        }

        /// <summary>
        /// 对表达式进行语法校验
        /// </summary>
        /// <param name="expression">要校验的表达式</param>
        /// <returns></returns>
        public static bool CheckExpression(string expression)
        {
            bool flag = true;
            string op = string.Empty;
            string operand1 = string.Empty;
            string operand2 = string.Empty;
            Stack optr = new Stack();
            optr.Push("#");
            Stack opnd = new Stack();

            string[] expr = SplitExpression(expression);
            int idx = 0;
            while (idx < expr.Length && (expr[idx] != "#" || optr.Peek().ToString() != "#"))
            {
                if (!CheckEveryExpr(expr, idx)) return false;
                if (!IsKeyword(expr[idx]))
                {
                    if (!IsNumeric(expr[idx]))
                    {
                        if (expr[idx] == "#")
                        {
                            if (optr.Peek().ToString() != "#")
                            {
                                if (opnd.Count < 2) return false;
                                op = optr.Pop().ToString();
                                operand1 = opnd.Pop().ToString();
                                operand2 = opnd.Pop().ToString();
                                if (IsOperator(op)) opnd.Push(operand1);
                                else return false;
                            }
                        }
                        else return false;
                    }
                    else
                    {
                        opnd.Push(expr[idx]);
                        idx++;
                    }
                }
                else
                {
                    switch (Precede(optr.Peek().ToString(), expr[idx]))
                    {
                        case "<":         //栈顶元素优先权低
                            optr.Push(expr[idx]);
                            idx++;
                            break;
                        case "=":       //脱括号并接收下一个字符
                            optr.Pop();
                            idx++;
                            break;
                        case ">":    //退栈并将运算结果入栈
                            if (opnd.Count < 2) return false;
                            op = optr.Pop().ToString();
                            operand1 = opnd.Pop().ToString();
                            operand2 = opnd.Pop().ToString();
                            if (IsOperator(op)) opnd.Push(operand1);
                            else return false;
                            break;
                    }
                }
            }
            if (opnd.Count != 1) flag = false;
            return flag;
        }

        /// <summary>
        /// 对表达式进行求值，求值之前会先进行语法校验
        /// </summary>
        /// <param name="expression">要求值的表达式</param>
        /// <returns>求值结果</returns>
        public static double Calculate(string expression)
        {

            if (!CheckExpression(expression)) {
                
                throw new ArgumentOutOfRangeException("表达式错误！");
            }

            string op = string.Empty;
            string operand1 = string.Empty;
            string operand2 = string.Empty;
            Stack optr = new Stack();
            optr.Push("#");
            Stack opnd = new Stack();

            string[] expr = SplitExpression(expression);
            int idx = 0;
            while (idx < expr.Length && (expr[idx] != "#" || optr.Peek().ToString() != "#"))
            {
                if (IsNumeric(expr[idx]))
                {
                    opnd.Push(expr[idx]);
                    idx++;
                }
                else
                {
                    switch (Precede(optr.Peek().ToString(), expr[idx]))
                    {
                        case "<":         //栈顶元素优先权低
                            optr.Push(expr[idx]);
                            idx++;
                            break;
                        case "=":       //脱括号并接收下一个字符
                            optr.Pop();
                            idx++;
                            break;
                        case ">":    //退栈并将运算结果入栈
                            if (opnd.Count < 2) return 0;
                            op = optr.Pop().ToString();
                            operand2 = opnd.Pop().ToString();
                            operand1 = opnd.Pop().ToString();
                            opnd.Push(Operate(Convert.ToDouble(operand1), op, Convert.ToDouble(operand2)).ToString());
                            break;
                    }
                }
            }

            return Convert.ToDouble(opnd.Peek());
        }
    }
}
