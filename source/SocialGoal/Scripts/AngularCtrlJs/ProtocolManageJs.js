app.controller('ProCtrl', function ($scope, $filter, $http) {
    /******
   start-------解析协议数据加载
    ******/
    $scope.DataBodys = null;
    $scope.SelectpmFInterpreter = null;
    $scope.SelectReceiveData = null;
    $http.get('/ProtocolManage/GetPropertyInfoArrayByReceiveDataLast').success(function (data) {
        $scope.SelectReceiveData = data;
    });
    //加载基本信息数据
    $http.get('/ProtocolManage/GetAll').success(function (data) {
        $scope.PmFInterpreters = data;
    });
    $scope.selectPmInterpreter = function (pmFInterpreter) {
        $scope.SelectpmFInterpreter = pmFInterpreter;
    }
    $scope.loadDataBody = function (pmFInterpreter) {
        $http.get('/ProtocolManage/GetPmDataBodyById',
            { params: { pmId: pmFInterpreter.PmFInterpreterId } }).success(function (data) {
                $scope.DataBodys = data;
            });
    }
    $scope.pmDataBodySave = function (data, pmid) {
        //  angular.extend(data, { id: pmid });
        var guidStr = getGuidGenerator();
        var data = $filter('filter')($scope.DataBodys, { PmDataBodyId: pmid });
        if (data[0].DataType == "1" && data[0].PmDataByte.PmDataByteId == null) {
            data[0].PmDataByte.PmDataByteId = guidStr;
            var idx = $scope.DataBodys.indexOf(data[0]);
            $scope.DataBodys[idx].PmDataByte.PmDataByteId = guidStr;
        }
        //协议类型ID
        data[0].PmFInterpreterId = $scope.SelectpmFInterpreter.PmFInterpreterId;
        return $http.post('/ProtocolManage/UpdateDataBody', data[0]).success(function (reData) {

        });
    };
    $scope.DataTypes = [
      { value: 1, text: '字节' },
      { value: 2, text: 'BIT位' },
      { value: 3, text: '状态位' }

    ];
    $scope.Representations = [
     { value: 1, text: '整数' },
     { value: 2, text: '字符串' },
     { value: 3, text: '时间' },
     { value: 4, text: '不处理' },
     { value: 5, text: '温度' },
     { value: 6, text: '电压' }
    ];

    $scope.boolData = [
    { value: true, text: '是' },
    { value: false, text: '否' }
    ];
    $scope.showTypes = function (currentType) {

        var selected = $filter('filter')($scope.DataTypes, { value: currentType });
        return (currentType && selected.length) ? selected[0].text : '未设置';
    };
    $scope.showRepresentations = function (currentType) {

        var selected = $filter('filter')($scope.Representations, { value: currentType });
        return (selected.length) ? selected[0].text : '未设置';
    };
    $scope.showBool = function (curentData) {
        var selected = $filter('filter')($scope.boolData, { value: curentData });
        return (selected.length) ? selected[0].text : '未设置';
    }
    $scope.addBitDatas = function (data) {
        var idx = $scope.DataBodys.indexOf(data);
        $scope.insertData = {
            "PmDataBitId": getGuidGenerator(),
            "Representation": 0,
            "ByteCounts": 0,
            "IsBigEndian": false,
            "BitType": 0,
            "ChildStartPostion": 1,
            "ChildDataLength": 2,
            "DefaultValue": null,
            "DictionaryKey": "A2"
        };

        $scope.DataBodys[idx].PmDataBits.push($scope.insertData);
    }
    $scope.delBitDatas = function (dataBody, cIndex, bitData) {

        var idx = $scope.DataBodys.indexOf(dataBody);
        var st = $scope.DataBodys[idx];
        $scope.DataBodys[idx].PmDataBits.splice(cIndex, 1);
        //执行后台删除操作
        return $http.post('/ProtocolManage/DeletePmDataBits', bitData).success(function (reData) {

        });
    }
    // remove user
    $scope.removePro = function (index) {
        var dataBodys = $scope.DataBodys[index];
        dataBodys.PmFInterpreterId = $scope.SelectpmFInterpreter.PmFInterpreterId;

        $scope.DataBodys.splice(index, 1);
        return $http.post('/ProtocolManage/DeletePmDataBody', dataBodys).success(function (reData) {

        });

    };
    $scope.addDataBody = function () {
        $scope.inserted = {
            PmDataBodyId: getGuidGenerator(),
            InfoTypeNumber: "1",
            StartPosition: 0,
            DataLength: 1,
            DataType: 2,
            PmDataByte: {
                "PmDataByteId": getGuidGenerator(),
                "Representation": 2,
                "IsBigEndian": true,
                "IsUnsigned": false,
                "Formula": "($*0.1)",
                "DefaultValue": "默认值",
                "DictionaryKey": "A1"
            },
            PmDataBits: [
                    {
                        "PmDataBitId": getGuidGenerator(),
                        "Representation": 0,
                        "ByteCounts": 0,
                        "IsBigEndian": false,
                        "BitType": 0,
                        "ChildStartPostion": 1,
                        "ChildDataLength": 2,
                        "DefaultValue": null,
                        "DictionaryKey": "A2"
                    }
            ]
        };
        $scope.DataBodys.push($scope.inserted);
    };
    /******
  end------------- 解析协议数据加载
   ******/
    $scope.FormatTypes = [
      { value: 0, text: '不设置' },
      { value: 1, text: '状态' },
      { value: 2, text: '数值' },
       { value:3, text: '时间' }
    ];
    $scope.SetFormatTypes = function (currentType) {
        var selected = $filter('filter')($scope.FormatTypes, { value: currentType });
        return (currentType && selected.length) ? selected[0].text : '未设置';
    };
    $scope.ReceiveDataDisplays = null;
    $scope.loadReceiveDataDisplay = function (pmFInterpreter) {
        $http.get('/ProtocolManage/GetReceiveDataDisplayByPmFInterpreterId',
            { params: { pmId: pmFInterpreter.PmFInterpreterId } }).success(function (data) {
                $scope.ReceiveDataDisplays = data;
            });
    }

    $scope.ReceiveDataSave = function (data, rid) {
        //  angular.extend(data, { id: pmid });     
        var data = $filter('filter')($scope.ReceiveDataDisplays, { ReceiveDataDisplayId: rid });
        //协议类型ID
        data[0].PmFInterpreterId = $scope.SelectpmFInterpreter.PmFInterpreterId;
        return $http.post('/ProtocolManage/UpdateReceiveData', data[0]).success(function (reData) {

        });
    };
    $scope.addReDataDisplayFormat = function (data) {
        var idx = $scope.ReceiveDataDisplays.indexOf(data);
        $scope.insertData = {
            "ReDataDisplayFormatId": getGuidGenerator(),
            "FormatType": 1,
            "FormatExpression": "",
            "FormatValue": "",
            "FormatColor": "",
            "ReceiveDataDisplayId": data.ReceiveDataDisplayId
        };

        $scope.ReceiveDataDisplays[idx].ReDataDisplayFormats.push($scope.insertData);
    }
    $scope.delReDataDisplayFormat = function (dataBody, cIndex, reDataDisplayFormat) {
        var idx = $scope.ReceiveDataDisplays.indexOf(dataBody);
        $scope.ReceiveDataDisplays[idx].ReDataDisplayFormats.splice(cIndex, 1);
        //执行后台删除操作
        return $http.post('/ProtocolManage/DeleteReDataDisplayFormat', reDataDisplayFormat).success(function (reData) {

        });
    }
    $scope.addReceiveDataDisplay = function () {
        var ReceiveDataDisplayId = getGuidGenerator();
        $scope.inserted = {
            ReceiveDataDisplayId: ReceiveDataDisplayId,
            DictionaryKey: "0",
            ShowType: 1,
            Alignment:'',
            ShowIcon: "",
            ShowPostion: "",
            ShowOrder: "",
            ShowUnit: "",
            ShowCommon: true,
            PmFInterpreterId: "",
            ReDataDisplayFormats: [
                    {
                        "ReDataDisplayFormatId": getGuidGenerator(),
                        "FormatType": "",
                        "FormatExpression": "",
                        "FormatValue": "",
                        "FormatColor": "",
                        "ReceiveDataDisplayId": ReceiveDataDisplayId,

                    }
            ]
        };
        $scope.ReceiveDataDisplays.push($scope.inserted);

    }
    $scope.removeReceiveDataDisplays = function (index) {
        var dataBodys = $scope.ReceiveDataDisplays[index];
        dataBodys.PmFInterpreterId = $scope.SelectpmFInterpreter.PmFInterpreterId;

        $scope.ReceiveDataDisplays.splice(index, 1);
        return $http.post('/ProtocolManage/DeleteReceiveDataDisplay', dataBodys).success(function (reData) {

        });
    }
    //协议测试部分
    $scope.sendData = "";
    $scope.resultData = null;
    $scope.SubTest = function () {       
        $http.get('/ProtocolManage/TestProtocol',
           { params: { pmId: $scope.SelectpmFInterpreter.PmFInterpreterId, sendData: $scope.sendData } }).success(function (data) {
               $scope.resultData = data;
           });
    }
    $scope.IsNull = function () {
        if ($scope.sendData != "") {
            return true;
        } else {
            return false;
        }       
    }
    $scope.rest = function () {
        $scope.sendData = "";
    }

});

app.controller('TerminalCtrl', function ($scope, $filter, $http) {
    
    var terminalId = "C2340008";
    $scope.ReceiveDataLasts = null;
    $http.get('/Terminal/GetreceiveDataLast',
            { params: { terminalNum: terminalId } }).success(function (data) {
                $scope.ReceiveDataLasts = data;
            });
});
//生成随机的GUID
function getGuidGenerator() {
    var S4 = function () {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    };
    return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
}

