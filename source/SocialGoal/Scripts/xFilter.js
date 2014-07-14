/*!
* xFilter Library v1.0.0
*
* Copyright 2010, Stefan Pirvu
* Dual licensed under the MIT or GPL Version 2 licenses.
* http://jquery.org/license
*
*
* Date: Sep 09, 2010
* 
*
* The framwork extends the jqGrid (http://www.trirand.com/blog/?page_id=5) filter functionality.
* The filter uses JSON entities to hold filter rules and groups. Here is an example of a filter:

{ "groupOp": "and",
      "groups" : [ 
        { "groupOp": "or",
            "rules": [
                { "field": "name", "op": "eq", "data": "England" }, 
                { "field": "id", "op": "le", "data": "5"}
             ]
        } 
      ],
      "rules": [
        { "field": "name", "op": "eq", "data": "Romania" }, 
        { "field": "id", "op": "le", "data": "1"}
      ]
}
*/

xFilter = function (container, opts) {
    this.opts = opts || {};

    this.filter = this.opts.filter;
    this.columns = this.opts.columns;
    this.domContainer = container;
    this.onchange = this.opts.onchange;

    this.ops = [
	                { "name": "eq", "description": "==" },
	                { "name": "ne", "description": "!=" },
	                { "name": "bw", "description": "begins with" },
	                { "name": "nb", "description": "does not begin with" },
	                { "name": "lt", "description": "<" },
	                { "name": "le", "description": "<=" },
	                { "name": "gt", "description": ">" },
	                { "name": "ge", "description": ">=" },
	                { "name": "ew", "description": "ends with" },
	                { "name": "nw", "description": "does not end with" },
	                { "name": "cn", "description": "contains" },
	                { "name": "nc", "description": "does not contain" },
	                { "name": "nu", "description": "is null" },
	                { "name": "nn", "description": "is not null" }
    ];

    this.groupOps = ["and", "or"];

    // there must be at least one group defined for the filter to work. If there
    // none, the code will autogenerate a default one. This condition applies
    // only for the groups, not for the rules
    if (this.filter == null || this.filter == undefined) {
        this.filter = {
            groupOp: this.groupOps[0],
            rules: [],
            groups: []
        }
    }


    // generates the html elements
    this.reDraw();
}


xFilter.prototype.reDraw = function () {

    // clear dom container
    while (this.domContainer.hasChildNodes()) {
        this.domContainer.removeChild(this.domContainer.lastChild);
    }

    // generates the html table that will hold
    // the xFilter. Because the "filter" property
    // is the base filter entity, we well use it
    // as a start point
    var t = this.createTableForGroup(this.filter, null);

    this.domContainer.appendChild(t);

}


xFilter.prototype.createTableForGroup = function (group, parentgroup) {
    // save current entity in a variable so that it could
    // be referenced in anonimous method calls
    var that = this;

    // this table will hold all the group (tables) and rules (rows)
    var table = document.createElement("table");
    table.setAttribute("class", "group");

    var tr = document.createElement("tr");
    table.appendChild(tr);

    // this header will hold the group operator type and group action buttons for
    // creating subgroup "+ {", creating rule "+" or deleting the group "-"
    var th = document.createElement("th");
    th.setAttribute("colspan", "5");
    tr.appendChild(th);


    // dropdown for: choosing group operator type
    var groupOpSelect = document.createElement("select");
    th.appendChild(groupOpSelect);
    groupOpSelect.onchange = function () {
        group.groupOp = groupOpSelect.options[groupOpSelect.selectedIndex].value;
        that.onchange(); // signals that the filter has changed
    };

    // populate dropdown with all posible group operators: or, and
    for (var i = 0; i < this.groupOps.length; i++) {
        var o = document.createElement("option");
        o.setAttribute("value", this.groupOps[i]);

        if (group.groupOp == this.groupOps[i])
            o.setAttribute("selected", "selected");

        o.appendChild(document.createTextNode(this.groupOps[i]));
        groupOpSelect.appendChild(o);
    }


    // button for adding a new subgroup
    var inputAddSubgroup = document.createElement("input");
    inputAddSubgroup.setAttribute("type", "button");
    inputAddSubgroup.setAttribute("value", "+ {");
    inputAddSubgroup.setAttribute("title", "Add subgroup");
    inputAddSubgroup.setAttribute("class", "add-group");
    inputAddSubgroup.onclick = function () {
        if (group.groups == undefined)
            group.groups = [];

        group.groups.push({
            groupOp: that.groupOps[0],
            rules: [],
            groups: []
        }); // adding a new group

        that.reDraw(); // the html has changed, force reDraw

        that.onchange(); // signals that the filter has changed
    }
    th.appendChild(inputAddSubgroup);

    // button for adding a new rule
    var inputAddRule = document.createElement("input");
    inputAddRule.setAttribute("type", "button");
    inputAddRule.setAttribute("value", "+");
    inputAddRule.setAttribute("title", "Add rule");
    inputAddRule.setAttribute("class", "add-rule");
    inputAddRule.onclick = function () {
        if (group.rules == undefined)
            group.rules = [];

        group.rules.push({
            field: that.columns[0].name,
            op: that.ops[0].name,
            data: ""
        }); // adding a new rule

        that.reDraw(); // the html has changed, force reDraw

        // for the moment no change have been made to the rule, so
        // this will not trigger onchange event
    }
    th.appendChild(inputAddRule);

    // button for delete the group
    if (parentgroup != null) { // ignore the first group
        var inputDeleteGroup = document.createElement("input");
        inputDeleteGroup.setAttribute("type", "button");
        inputDeleteGroup.setAttribute("value", "-");
        inputDeleteGroup.setAttribute("title", "Delete group");
        inputDeleteGroup.setAttribute("class", "delete-group");
        th.appendChild(inputDeleteGroup);
        inputDeleteGroup.onclick = function () {
            // remove group from parent
            for (var i = 0; i < parentgroup.groups.length; i++) {
                if (parentgroup.groups[i] == group) {
                    parentgroup.groups.splice(i, 1);
                    break;
                }
            }

            that.reDraw(); // the html has changed, force reDraw

            that.onchange(); // signals that the filter has changed
        }
    }


    // append subgroup rows
    if (group.groups != undefined) {
        for (var i = 0; i < group.groups.length; i++) {
            var trHolderForSubgroup = document.createElement("tr");
            table.appendChild(trHolderForSubgroup);

            var tdFirstHolderForSubgroup = document.createElement("td");
            tdFirstHolderForSubgroup.setAttribute("class", "first");
            trHolderForSubgroup.appendChild(tdFirstHolderForSubgroup);

            var tdMainHolderForSubgroup = document.createElement("td");
            tdMainHolderForSubgroup.setAttribute("colspan", "4");
            tdMainHolderForSubgroup.appendChild(this.createTableForGroup(group.groups[i], group));
            trHolderForSubgroup.appendChild(tdMainHolderForSubgroup);
        }
    }


    // append rules rows
    if (group.rules != undefined) {
        for (var i = 0; i < group.rules.length; i++) {
            table.appendChild(
                       this.createTableRowForRule(group.rules[i], group)
                );
        }
    }

    return table;
}
xFilter.prototype.createTableRowForRule = function (rule, group) {
    // save current entity in a variable so that it could
    // be referenced in anonimous method calls    
    var that = this;
    var tr = document.createElement("tr");

    // first column used for padding
    var tdFirstHolderForRule = document.createElement("td");
    tdFirstHolderForRule.setAttribute("class", "first");
    tr.appendChild(tdFirstHolderForRule);


    // create field container
    var ruleFieldTd = document.createElement("td");
    ruleFieldTd.setAttribute("class", "columns");
    tr.appendChild(ruleFieldTd);

    // dropdown for: choosing field
    var ruleFieldSelect = document.createElement("select");
    ruleFieldTd.appendChild(ruleFieldSelect);
    ruleFieldSelect.onchange = function () {
        rule.field = ruleFieldSelect.options[ruleFieldSelect.selectedIndex].value;
        that.onchange();  // signals that the filter has changed
    }

    // populate drop down with user provided column definitions
    for (var i = 0; i < this.columns.length; i++) {
        var o = document.createElement("option");
        o.setAttribute("value", this.columns[i].name);

        if (rule.field == this.columns[i].name)
            o.setAttribute("selected", "selected");

        o.appendChild(document.createTextNode(this.columns[i].lable));
        ruleFieldSelect.appendChild(o);
    }



    // create operator container
    var ruleOperatorTd = document.createElement("td");
    ruleOperatorTd.setAttribute("class", "operators");
    tr.appendChild(ruleOperatorTd);

    // create it here so it can be referentiated in the onchange event
    var ruleDataInput = document.createElement("input");

    // dropdown for: choosing operator
    var ruleOperatorSelect = document.createElement("select");
    ruleOperatorTd.appendChild(ruleOperatorSelect);
    ruleOperatorSelect.onchange = function () {
        rule.op = ruleOperatorSelect.options[ruleOperatorSelect.selectedIndex].value;
        if (rule.op == "nu" || rule.op == "nn") { // disable for operator "is null" and "is not null"
            ruleDataInput.setAttribute("readonly", "true");
            ruleDataInput.setAttribute("disabled", "true");
            ruleDataInput.value = "";
        }

        that.onchange();  // signals that the filter has changed
    }

    // populate drop down with all available operators
    for (var i = 0; i < this.ops.length; i++) {
        var o = document.createElement("option");
        o.setAttribute("value", this.ops[i].name);

        if (rule.op == this.ops[i].name)
            o.setAttribute("selected", "selected");

        o.appendChild(document.createTextNode(this.ops[i].description));
        ruleOperatorSelect.appendChild(o);
    }

    // create data container
    var ruleDataTd = document.createElement("td");
    ruleDataTd.setAttribute("class", "data");
    tr.appendChild(ruleDataTd);

    // textbox for: data
    // is created previously
    ruleDataInput.setAttribute("type", "text");
    ruleDataInput.setAttribute("value", rule.data);
    ruleDataTd.appendChild(ruleDataInput);
    ruleDataInput.onchange = function () {
        rule.data = ruleDataInput.value;

        that.onchange(); // signals that the filter has changed
    }

    // create action container
    var ruleDeleteTd = document.createElement("td");
    tr.appendChild(ruleDeleteTd);

    // create button for: delete rule
    var ruleDeleteInput = document.createElement("input");
    ruleDeleteInput.setAttribute("type", "button");
    ruleDeleteInput.setAttribute("value", "-");
    ruleDeleteInput.setAttribute("title", "Delete rule");
    ruleDeleteInput.setAttribute("class", "delete-rule");
    ruleDeleteTd.appendChild(ruleDeleteInput);
    ruleDeleteInput.onclick = function () {
        // remove rule from group
        for (var i = 0; i < group.rules.length; i++) {
            if (group.rules[i] == rule) {
                group.rules.splice(i, 1);
                break;
            }
        }

        that.reDraw(); // the html has changed, force reDraw

        that.onchange(); // signals that the filter has changed
    }

    return tr;
}
xFilter.prototype.toString = function () {
    // this will obtain a string that can be used to match an item.
    // This is used by the Apply method to check if an item matches

    function getStringForGroup(group) {
        var s = "(";

        if (group.groups != undefined) {
            for (var index = 0; index < group.groups.length; index++) {
                if (s.length > 1) {
                    if (group.groupOp == "or")
                        s += " || ";
                    else
                        s += " && ";
                }
                s += getStringForGroup(group.groups[index]);
            }
        }

        if (group.rules != undefined) {
            for (var index = 0; index < group.rules.length; index++) {
                if (s.length > 1) {
                    if (group.groupOp == "or")
                        s += " || ";
                    else
                        s += " && ";
                }
                s += getStringForRule(group.rules[index]);
            }
        }

        s += ")";

        if (s == "()")
            return ""; // ignore groups that don't have rules
        else
            return s;
    }
    function getStringForRule(rule) {
        return rule.op + "(item." + rule.field + ",'" + rule.data + "')";
    }

    return getStringForGroup(this.filter);
}
xFilter.prototype.toUserFriendlyString = function () {
    return this.getUserFriendlyStringForGroup(this.filter);
}
xFilter.prototype.getUserFriendlyStringForGroup = function (group) {
    var s = "(";
    if (group.groups != undefined) {
        for (var index = 0; index < group.groups.length; index++) {
            if (s.length > 1)
                s += " " + group.groupOp + " ";

            try {
                s += this.getUserFriendlyStringForGroup(group.groups[index]);
            } catch (e) { alert(e); }
        }
    }

    if (group.rules != undefined) {
        try {
            for (var index = 0; index < group.rules.length; index++) {
                if (s.length > 1)
                    s += " " + group.groupOp + " ";
                s += this.getUserFriendlyStringForRule(group.rules[index]);
            }
        } catch (e) { alert(e); }
    }

    s += ")";

    if (s == "()")
        return ""; // ignore groups that don't have rules
    else
        return s;
}
xFilter.prototype.getUserFriendlyStringForRule = function (rule) {
    var opUF = "";
    for (var i = 0; i < this.ops.length; i++) {
        if (this.ops[i].name == rule.op) {
            opUF = this.ops[i].description;
            break;
        }
    }
    return rule.field + " " + opUF + " \"" + rule.data + "\"";
}
xFilter.prototype.Apply = function (jsonObj) {
    // filters the JSON based on the current filter
    if (jsonObj == null || jsonObj == undefined)
        return [];

    var filterString = this.toString();
    if (filterString.replace("(", "").replace(")", "") == "")
        return jsonObj;

    var newJsonObj = [];
    for (var index = 0; index < jsonObj.length; index++) {
        var item = jsonObj[index];

        if (eval(filterString) == true)
            newJsonObj.push(item);
    }

    return newJsonObj;
}

// functions used for client-side filtering by the Apply method
function eq(d1, d2) { return d1 == d2; }
function ne(d1, d2) { return d1 != d2; }
function lt(d1, d2) { return d1 < d2; }
function le(d1, d2) { return d1 <= d2; }
function gt(d1, d2) { return d1 > d2; }
function ge(d1, d2) { return d1 >= d2; }
function bw(d1, d2) { return d1.match("^" + d2); }
function nb(d1, d2) { return !d1.match("^" + d2); }
function ew(d1, d2) { return d1.match(d2 + "$"); }
function nw(d1, d2) { return !d1.match(d2 + "$"); }
function cn(d1, d2) { return d2.indexOf(d1) != -1; }
function nc(d1, d2) { return d2.indexOf(d1) == -1; }
function nu(d1, d2) { return d1 == null; }
function nn(d1, d2) { return d1 != null; }