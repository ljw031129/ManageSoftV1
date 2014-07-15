using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace xFilter.Expressions
{
    public class Group
    {
        public Group() {
            this.Groups = new List<Group>();
            this.Rules = new List<Rule>();
        }

        public GroupOperator Operator { get; set; }

        public List<Rule> Rules { get; set; }

        public List<Group> Groups { get; set; }


        /// <summary>
        /// Returns an expression tree from the Group / Rules tree
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Expression<Func<T, bool>> ToExpressionTree<T>(){

            Type t = typeof(T);

            // create a parameter expression that can be passed to the rules
            ParameterExpression param = Expression.Parameter(t, "p");

            // get the expression body. This consists of all subgroups and rules
            Expression body = GetExpressionFromSubgroup(this, t, param);
            
            if (body == null)
                return null;
            else
                return Expression.Lambda<Func<T, bool>>(
                                body,
                                new ParameterExpression[] { param }
                           );

               

        }


        public Expression<Func<T, string>> ToExpressionTreeOrder<T>()
        {
            Type t = typeof(T);
            // create a parameter expression that can be passed to the rules
            ParameterExpression param = Expression.Parameter(t, "p");
            // get the expression body. This consists of all subgroups and rules  
            Expression body = Expression.PropertyOrField(param, "EquipmentCreatTime");
            try
            {
                if (body == null)
                    return null;
                else                   
                    return Expression.Lambda<Func<T, string>>(
                                   body,
                                   new ParameterExpression[] { param }
                              );
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        protected Expression GetExpressionFromSubgroup(Group subgroup, Type parameterType,ParameterExpression param) {

            Expression body = null;

            // get expressions from subgroup
            foreach (Group g in subgroup.Groups) {
                // make a recurrent call to make sure that we get all the expressions
                Expression subgroupExpression = GetExpressionFromSubgroup(g, parameterType, param);

                if (subgroupExpression == null)
                    continue; // ignore groups that don't have rules

                if (body == null)
                    body = subgroupExpression;
                else {
                    if (subgroup.Operator == GroupOperator.And)
                        body = Expression.And(body, subgroupExpression);
                    else
                        body = Expression.Or(body, subgroupExpression);
                }
            }

            // get expressions from rules
            foreach (Rule r in subgroup.Rules) { 
                Expression ruleExpression = r.ToExpression(parameterType, param);

                if (ruleExpression == null)
                    continue; // ignore broken rules

                if (body == null)
                    body = ruleExpression;
                else
                {
                    if (subgroup.Operator == GroupOperator.And)
                        body = Expression.And(body, ruleExpression);
                    else
                        body = Expression.Or(body, ruleExpression);
                }
            }

            return body;
        }

    }

    public enum GroupOperator { 
        And = 0,
        Or = 1
    }
}
