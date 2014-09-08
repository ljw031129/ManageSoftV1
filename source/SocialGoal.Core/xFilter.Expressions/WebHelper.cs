using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace xFilter.Expressions
{
    public class WebHelper
    {
        public static Expression<Func<T, bool>> JSONToExpressionTree<T>(string jsonString)
        {
            try
            {
                return DeserializeGroupFromJSON(jsonString).ToExpressionTree<T>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Serializes a group entity
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static string SerializeGroupToJSON(Group g) { 
        
            string json = "{ \"groupOp\": \"" + g.Operator.ToString().ToLower() + "\"";
            
            if(g.Groups != null && g.Groups.Count > 0)
            {
                json += ",\"groups\" : [";

                for(int i = 0; i < g.Groups.Count; i++) {
                    if(i > 0) 
                        json += ",";

                    json += SerializeGroupToJSON(g.Groups[i]);
                }

                json += "]";
            }

            if (g.Rules != null && g.Rules.Count > 0)
            {
                json += ",\"rules\" : [";

                for(int i = 0; i < g.Rules.Count; i++) {
                    if(i > 0) 
                        json += ",";

                    Rule r= g.Rules[i];
                    
                    string opString = "eq";
                    
                    switch (r.Operator) {
                        case RuleOperator.Contains: opString = "cn"; break;
                        case RuleOperator.EndsWith: opString = "ew"; break;
                        case RuleOperator.Equals: opString = "eq"; break;
                        case RuleOperator.GraterOrEqual: opString = "ge"; break;
                        case RuleOperator.GraterThan: opString = "gt"; break;
                        case RuleOperator.IsNull: opString = "nu"; break;
                        case RuleOperator.LessOrEqual: opString = "le"; break;
                        case RuleOperator.LessThan: opString = "lt"; break;
                        case RuleOperator.NotEqual: opString = "ne"; break;
                        case RuleOperator.NotNull: opString = "nn"; break;
                        case RuleOperator.StartsWith: opString = "bw"; break;
                    }

                    json += "{ \"field\": \"" +r.Field+ "\", \"op\": \"" + opString + "\", \"data\": \"" + r.Data + "\" }";
                }

                json += "]";
            }

            return json + "}";
        }

        /// <summary>
        /// Deserializes a JSON into a group
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static Group DeserializeGroupFromJSON(JToken container)
        {
            // JObject container = JObject.Parse(jsonString);

            Group g = DeserializeGroupFromJSON(container);

            return g;
        }

        public static Group DeserializeGroupFromJSON(JObject value)
        {
            Group g = new Group();
            g.Operator = ((string)value["groupOp"]).ToLower() == "and"
                ? GroupOperator.And : GroupOperator.Or;

            if(value["groups"] != null)
                foreach (JToken token in (JArray)value["groups"])
                    g.Groups.Add(DeserializeGroupFromJSON(token));

            if (value["rules"] != null)
                foreach (JToken token in (JArray)value["rules"])
                    g.Rules.Add(DeserializeRuleFromJSON(token));

            return g;
        }

        public static Rule DeserializeRuleFromJSON(JToken value)
        {
            Rule r = new Rule();

            r.Field = (string)value["field"];
            r.Data = (string)value["data"];
            switch ((string)value["op"]) {
                case "cn": r.Operator = RuleOperator.Contains; break;
                case "ew": r.Operator =  RuleOperator.EndsWith; break;
                case "eq": r.Operator =  RuleOperator.Equals; break;
                case "ge": r.Operator =  RuleOperator.GraterOrEqual; break;
                case "gt": r.Operator =  RuleOperator.GraterThan; break;
                case "nu": r.Operator =  RuleOperator.IsNull; break;
                case "le": r.Operator =  RuleOperator.LessOrEqual; break;
                case "lt": r.Operator =  RuleOperator.LessThan; break;
                case "ne": r.Operator =  RuleOperator.NotEqual; break;
                case "nn": r.Operator =  RuleOperator.NotNull; break;
                case "bw": r.Operator =  RuleOperator.StartsWith; break;
            }

            return r;
        }
    }
}
