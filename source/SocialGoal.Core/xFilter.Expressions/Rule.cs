using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace xFilter.Expressions
{
    public class Rule
    {
        public Rule() { }

        public string Field { get; set; }

        public RuleOperator Operator { get; set; }

        public string Data { get; set; }



        /// <summary>
        /// Converts a rule into an expression. The rule doesn't have any
        /// notion about the input parameter, so we must set these
        /// </summary>
        /// <param name="parameterType"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Expression ToExpression(Type parameterType, ParameterExpression param) {
            // get the property that will be evaluated
            PropertyInfo pi = null;
            MemberExpression member = null;

            if (this.Field.Contains(".")) // check for subproperties
            {
                foreach (string f in this.Field.Split(".".ToCharArray()))
                {
                    if (pi == null)
                    {
                        pi = parameterType.GetProperty(f);
                        member = Expression.PropertyOrField(param, f);
                    }
                    else
                    {
                        pi = pi.PropertyType.GetProperty(f);
                        member = Expression.PropertyOrField(member, f);
                    }
                }
            }
            else {
                pi = parameterType.GetProperty(this.Field);
                member = Expression.PropertyOrField(param, this.Field);
            }


            ConstantExpression constant = this.Operator == RuleOperator.IsNull || this.Operator == RuleOperator.NotNull
                    ? Expression.Constant(null, pi.PropertyType)
                    : Expression.Constant(this.CastDataAs(pi.PropertyType), pi.PropertyType);

            switch(this.Operator) {
                case RuleOperator.IsNull: // it's the same for null
                case RuleOperator.Equals:
                     return Expression.Equal(member, constant);
                case RuleOperator.NotNull: // it's the same for not null
                case RuleOperator.NotEqual:
                     return Expression.Not(Expression.Equal(member, constant));
                case RuleOperator.LessThan:
                    return Expression.LessThan(member, constant);
                case RuleOperator.LessOrEqual:
                    return Expression.LessThanOrEqual(member, constant);
                case RuleOperator.GraterThan:
                    return Expression.GreaterThan(member, constant);
                case RuleOperator.GraterOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);
                case RuleOperator.StartsWith: // available only for string fields
                    return Expression.Call(member, typeof(string).GetMethod("StartsWith", new Type[] {typeof(string)} ), new Expression[] { constant });
                case RuleOperator.EndsWith: // available only for string fields
                    return Expression.Call(member, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), new Expression[] { constant });
                case RuleOperator.Contains: // available only for string fields
                    return Expression.Call(member, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), new Expression[] { constant });
            }

            return null;
        }

        /// <summary>
        /// This method is used to cast the Data field to a specifical item type.
        /// As data will only hold numbers, strings or date, we don't need a big
        /// method to reallize this cast
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        protected object CastDataAs(Type t) {
            // ignore invalid casts
            if (t == typeof(string))
                return this.Data;

            if (t == typeof(int))
                return int.Parse(this.Data);

            if (t == typeof(float))
                return float.Parse(this.Data);

            if (t == typeof(decimal))
                return decimal.Parse(this.Data);

            if (t == typeof(DateTime))
                return DateTime.Parse(this.Data);

            return this.Data;
        }
    }

    public enum RuleOperator { 
        /// <summary>
        /// ==
        /// </summary>
        Equals, 

        /// <summary>
        /// !=
        /// </summary>
	    NotEqual,

        /// <summary>
        /// begins with
        /// </summary>
	    StartsWith,

        /// <summary>
        /// <
        /// </summary>
	    LessThan, 

        /// <summary>
        /// <=
        /// </summary>
	    LessOrEqual,

        /// <summary>
        /// >
        /// </summary>
	    GraterThan,

        /// <summary>
        /// >=
        /// </summary>
	    GraterOrEqual,

        /// <summary>
        /// ends with
        /// </summary>
	    EndsWith, 

        /// <summary>
        /// contains
        /// </summary>
	    Contains, 

        /// <summary>
        /// is null
        /// </summary>
	    IsNull, 

        /// <summary>
        /// is not null
        /// </summary>
	    NotNull, 
    }
}