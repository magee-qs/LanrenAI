$.extend($.fn.validatebox.defaults.rules, {
    NotNull: { // 非空字符串验证 
        validator: function (value, param) {
            obj = $.trim(value);
            if (obj.length == 0 || obj == null || obj == undefined) {
                return false;
            }
            else
                return true;
        },
        message: '不能为空'
    },
    Num: {
        validator: function (value, param) {
            reg = /^[-+]?\d+$/;
            if (!reg.test(value)) {
                return false;
            } else {
                return true;
            }
        },
        message:'必须为数字'
    },
    
    Date: {
        validator: function (value, param) {
            if (value.length != 0) {
                reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/;
                if (!reg.test(value)) {
                    return false;
                }
                else {
                    return true;
                }
            }
        },
        message:'必须是日期类型'
    },
    
    Double: {
        validator: function (value, param) {
            if (value.length != 0) {
                reg = /^[-\+]?\d+(\.\d+)?$/;
                if (!reg.test(value)) {
                    return false;
                }
                else {
                    return true;
                }
            }
        },
        message: '必须是数值类型'
    }, 
});  

 