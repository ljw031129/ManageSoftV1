// 创建一个模型
var shoppingModule = angular.module('MenuDataModule', []);
// 设置好服务工厂，用来创建我们的Items 接口，以便访问服务端数据库
shoppingModule.factory('Items', function () {
    var items = {};
    items.query = function () {
       
        return [
    {
        "Id": 0,
        "Icon": "icon-folder-open-alt",
        "MenuName": "一级菜单",
        "MenuUrl": "www.baidu.com",
        "Stutas": "in",
        "ChildMenu": [
            {
                "Id": 1,
                "Icon": "icon-folder-open-alt",
                "MenuName": "二级菜单01",
                "MenuUrl": "www.baidu.com",
                "Stutas": "",
                "ChildMenu": null
            },
            {
                "Id": 2,
                "Icon": "icon-folder-open-alt",
                "MenuName": "二级菜单02",
                "MenuUrl": "www.baidu.com",
                "Stutas": "",
                "ChildMenu": [
                    {
                        "Id": 1,
                        "Icon": "icon-folder-open-alt",
                        "MenuName": "二级菜单02-01",
                        "MenuUrl": "www.baidu.com",
                        "Stutas": "",
                        "ChildMenu": null
                    }]
            },
            {
                "Id": 2,
                "Icon": "icon-folder-open-alt",
                "MenuName": "二级菜单03",
                "MenuUrl": "www.baidu.com",
                "Stutas": "in",
                "ChildMenu": [
                    {
                        "Id": 1,
                        "Icon": "icon-folder-open-alt",
                        "MenuName": "二级菜单03-01",
                        "MenuUrl": "www.baidu.com",
                        "Stutas": "",
                        "ChildMenu": null
                    }
                ]
            }
        ]
    }
];
    };
    return items;
});